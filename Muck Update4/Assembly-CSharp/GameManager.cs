// Decompiled with JetBrains decompiler
// Type: GameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  public static bool connected;
  public static bool started;
  public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
  public GameObject localPlayerPrefab;
  public GameObject playerPrefab;
  public GameObject playerRagdoll;
  public MapGenerator mapGenerator;
  public GenerateNavmesh generateNavmesh;
  public GameObject resourceGen;
  private bool gameOver;
  public DayUi dayUi;
  public GameObject gameoverUi;
  public ExtraUI extraUi;
  public static GameManager.GameState state;
  public bool boatLeft;
  public GameObject lobbyCamera;
  public GameObject testGame;
  public GameObject zone;
  private bool winnerSent;
  public GameObject gravePrefab;
  public int winnerId;
  private float mapRadius = 1100f;
  public LayerMask whatIsGround;
  public LayerMask whatIsGroundAndObject;
  private List<Vector3> spawnPositions;
  public bool powerupsPickedup;
  public bool damageTaken;
  public bool onlyRock = true;
  public Dictionary<string, int>[] stats;
  public int nStatsPlayers;

  public static GameSettings gameSettings { get; set; }

  private void Awake()
  {
    if ((UnityEngine.Object) GameManager.instance == (UnityEngine.Object) null)
      GameManager.instance = this;
    else if ((UnityEngine.Object) GameManager.instance != (UnityEngine.Object) this)
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    GameManager.players = new Dictionary<int, PlayerManager>();
    this.currentDay = 0;
  }

  private void Start()
  {
    if (GameManager.gameSettings == null && (UnityEngine.Object) NetworkController.Instance == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "testing spawn");
      GameManager.gameSettings = new GameSettings(44430);
      GameManager.gameSettings.gameMode = GameSettings.GameMode.Survival;
      GameManager.gameSettings.difficulty = GameSettings.Difficulty.Normal;
      if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
        UnityEngine.Object.Instantiate<GameObject>(this.zone, Vector3.zero, Quaternion.identity);
      Server.InitializeServerPackets();
      LocalClient.InitializeClientData();
      UnityEngine.Object.Instantiate<GameObject>(this.testGame);
      LocalClient.instance.serverHost = (SteamId) SteamManager.Instance.PlayerSteamId.Value;
      SteamLobby.steamIdToClientId.Add(SteamManager.Instance.PlayerSteamId.Value, 0);
      Server.clients.Add(0, new Client(0));
      Server.clients[0].player = new Player(0, "Dani", Color.black, (SteamId) SteamManager.Instance.PlayerSteamId.Value);
      LocalClient.serverOwner = true;
      ClientSend.PlayerFinishedLoading();
      DayCycle.dayDuration = 10f;
    }
    else
    {
      if (NetworkController.Instance.networkType != NetworkController.NetworkType.Steam)
        return;
      this.StartCoroutine(this.GenerateWorldRoutine());
      LoadingScreen.Instance.Show(0.0f);
      DayCycle.dayDuration = (float) GameManager.gameSettings.DayLength();
    }
  }

  public static int GetSeed() => GameManager.gameSettings.Seed;

  private IEnumerator GenerateWorldRoutine()
  {
    GameManager gameManager = this;
    if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
    {
      UnityEngine.Object.Instantiate<GameObject>(gameManager.zone, Vector3.zero, Quaternion.identity);
      if (LocalClient.serverOwner)
        gameManager.InvokeRepeating("SlowUpdate", 0.5f, 0.5f);
    }
    yield return (object) 3f;
    LoadingScreen.Instance.SetText("Generating World Mesh", 0.25f);
    gameManager.mapGenerator.GenerateMap(GameManager.GetSeed());
    Map.Instance.GenerateMap();
    yield return (object) 30;
    LoadingScreen.Instance.SetText("Generating resources", 0.5f);
    gameManager.resourceGen.SetActive(true);
    yield return (object) 30;
    LoadingScreen.Instance.SetText("Generating resources", 0.5f);
    gameManager.resourceGen.SetActive(true);
    yield return (object) 30;
    LoadingScreen.Instance.SetText("Generating navmesh", 0.75f);
    gameManager.generateNavmesh.GenerateNavMesh();
    yield return (object) 60;
    LoadingScreen.Instance.SetText("Finished loading", 1f);
    ClientSend.PlayerFinishedLoading();
    LoadingScreen.Instance.FinishLoading();
  }

  public int currentDay { get; set; }

  public void UpdateDay(int day)
  {
    this.currentDay = day;
    this.dayUi.SetDay(day);
    this.extraUi.UpdateDay(this.currentDay);
    if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
      ZoneController.Instance.NextDay(this.currentDay);
    AchievementManager.Instance.NewDay(this.currentDay);
  }

  public void SpawnPlayer(
    int id,
    string username,
    Color color,
    Vector3 position,
    float orientationY)
  {
    if (GameManager.players.ContainsKey(id))
      return;
    MonoBehaviour.print((object) "Spawning player");
    PlayerManager component;
    if (id == LocalClient.instance.myId)
    {
      UnityEngine.Object.Instantiate<GameObject>(this.localPlayerPrefab, position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
      component = PlayerMovement.Instance.gameObject.GetComponent<PlayerManager>();
    }
    else
    {
      component = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, position, Quaternion.Euler(0.0f, orientationY, 0.0f)).GetComponent<PlayerManager>();
      component.SetDesiredPosition(position);
    }
    component.SetDesiredPosition(position);
    component.id = id;
    component.username = username;
    component.color = color;
    GameManager.players.Add(id, component);
    if ((GameManager.gameSettings.gameMode != GameSettings.GameMode.Versus || id != LocalClient.instance.myId) && GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
      return;
    this.extraUi.InitPlayerStatus(id, username, component);
  }

  public int CalculateDamage(float damage, float armor, float sharpness, float hardness)
  {
    armor = (float) ((100.0 - (double) Mathf.Clamp(armor, 0.0f, 100f)) / 100.0);
    if ((double) armor < 0.0199999995529652)
      armor = 0.02f;
    return (int) ((double) damage * (double) armor);
  }

  public float MobDamageMultiplier()
  {
    float num1 = 0.9f;
    float num2 = 0.26f;
    float p = 1.55f;
    if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Easy)
    {
      num1 = 0.35f;
      num2 = 0.2f;
      p = 1.33f;
    }
    else if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Gamer)
    {
      num1 = 1.75f;
      num2 = 0.25f;
      p = 2.3f;
    }
    return num1 + Mathf.Pow(num2 * (float) this.currentDay, p);
  }

  public float ChestPriceMultiplier()
  {
    if (GameManager.gameSettings.difficulty != GameSettings.Difficulty.Gamer)
    {
      int difficulty = (int) GameManager.gameSettings.difficulty;
    }
    double num = 1.0;
    return Mathf.Clamp((float) (num * (1.0 + (double) (this.currentDay - 3) / (double) GameManager.gameSettings.GetChestPriceMultiplier())), (float) num, 100f);
  }

  public float MobHpMultiplier()
  {
    float num1 = 1.05f;
    float num2 = 0.24f;
    float p = 1.5f;
    if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Easy)
    {
      num1 = 0.9f;
      num2 = 0.2f;
      p = 1.32f;
    }
    else if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Gamer)
    {
      num1 = 1.3f;
      num2 = 0.3f;
      p = 1.8f;
    }
    return num1 + Mathf.Pow(num2 * (float) this.currentDay, p);
  }

  private void SlowUpdate()
  {
    if (this.winnerSent || GameManager.gameSettings.gameMode != GameSettings.GameMode.Versus || this.GetPlayersAlive() > 1)
      return;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((UnityEngine.Object) playerManager != (UnityEngine.Object) null && !playerManager.dead)
      {
        int id = playerManager.id;
        string str = "Nobody";
        if (GameManager.players.ContainsKey(id))
          str = GameManager.players[id].username;
        Debug.Log((object) ("Winner is: " + (object) id + " | with name: " + str));
        ServerSend.GameOver(id);
        this.GameOver(id);
        this.winnerSent = true;
        break;
      }
    }
  }

  public void KillPlayer(int id, Vector3 pos)
  {
    PlayerManager player = GameManager.players[id];
    player.dead = true;
    player.gameObject.SetActive(false);
    pos = player.transform.position;
    UnityEngine.Object.Instantiate<GameObject>(this.playerRagdoll, pos, player.transform.rotation).GetComponent<PlayerRagdoll>().SetRagdoll(id, -player.transform.forward);
  }

  public Vector3 GetGravePosition(int playerId)
  {
    try
    {
      Vector3 vector3 = GameManager.players[playerId].transform.position;
      if ((double) vector3.y < -100.0)
        vector3 = Vector3.zero;
      RaycastHit hitInfo;
      if (Physics.Raycast(vector3 + Vector3.up * 3000f, Vector3.down, out hitInfo, 8000f, (int) this.whatIsGround))
        return hitInfo.point;
    }
    catch (Exception ex)
    {
      return Vector3.zero;
    }
    return Vector3.zero;
  }

  public void SpawnGrave(Vector3 gravePos, int playerId, int graveObjectId)
  {
    PlayerManager player = GameManager.players[playerId];
    ++player.deaths;
    GraveInteract componentInChildren = UnityEngine.Object.Instantiate<GameObject>(this.gravePrefab, gravePos, Quaternion.identity).GetComponentInChildren<GraveInteract>();
    componentInChildren.username = player.username.Substring(0, Mathf.Clamp(15, 0, player.username.Length));
    componentInChildren.playerId = player.id;
    componentInChildren.SetId(graveObjectId);
    ResourceManager.Instance.AddObject(graveObjectId, componentInChildren.transform.parent.gameObject);
    player.graveId = graveObjectId;
    float time = Mathf.Clamp((float) ((double) componentInChildren.timeLeft * (double) (GameManager.players[playerId].deaths - 1) * 2.0), 30f, 300f);
    componentInChildren.SetTime(time);
    componentInChildren.transform.root.GetComponentInChildren<GravePing>().SetPing(player.username);
  }

  public void RespawnPlayer(int id, Vector3 zero)
  {
    if (!GameManager.players.ContainsKey(id))
      return;
    GameManager.players[id].dead = false;
    Vector3 position = ResourceManager.Instance.list[GameManager.players[id].graveId].transform.position;
    if (GameManager.players[id].graveId != -1)
      GameManager.players[id].RemoveGrave();
    if (LocalClient.instance.myId == id)
    {
      PlayerMovement.Instance.transform.position = position + Vector3.up * 3f;
      PlayerMovement.Instance.gameObject.SetActive(true);
      PlayerStatus.Instance.Respawn();
    }
    else
      GameManager.players[id].gameObject.SetActive(true);
  }

  public void StartGame()
  {
    LoadingScreen.Instance.Hide();
    this.lobbyCamera.SetActive(false);
    GameManager.state = GameManager.GameState.Playing;
    if (LocalClient.serverOwner)
      GameLoop.Instance.StartLoop();
    Hotbar.Instance.UpdateHotbar();
    AchievementManager.Instance.StartGame(GameManager.gameSettings.difficulty);
  }

  public void DisconnectPlayer(int id)
  {
    if ((UnityEngine.Object) GameManager.players[id] != (UnityEngine.Object) null && (UnityEngine.Object) GameManager.players[id].gameObject != (UnityEngine.Object) null)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) GameManager.players[id].gameObject);
      GameManager.players[id].dead = true;
      GameManager.players[id].disconnected = true;
    }
    GameManager.players?.Remove(id);
  }

  public int GetPlayersAlive()
  {
    int num = 0;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((bool) (UnityEngine.Object) playerManager && !playerManager.dead)
        ++num;
    }
    MonoBehaviour.print((object) ("players alive:  " + (object) num));
    return num;
  }

  public int GetPlayersInLobby()
  {
    int num = 0;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((bool) (UnityEngine.Object) playerManager && !playerManager.disconnected)
        ++num;
    }
    return num;
  }

  public void CheckIfGameOver()
  {
    if (this.GetPlayersAlive() > 0)
      return;
    this.GameOver(-2);
    ServerSend.GameOver();
  }

  public void GameOver()
  {
    MusicController.Instance.StopSong();
    this.Invoke("ShowEndScreen", 4f);
  }

  public void GameOver(int winnerId, float time = 4f)
  {
    Debug.LogError((object) "game over");
    this.winnerId = winnerId;
    this.Invoke("ShowEndScreen", time);
    MusicController.Instance.StopSong();
    AchievementManager.Instance.CheckGameOverAchievements(winnerId);
  }

  public void LeaveGame()
  {
    if (LocalClient.serverOwner)
    {
      Debug.LogError((object) "Host left game");
      this.HostLeftGame();
    }
    else
      ClientSend.PlayerDisconnect();
    SteamManager.Instance.leaveLobby();
    SceneManager.LoadScene("Menu");
    LocalClient.instance.serverHost = new SteamId();
    LocalClient.serverOwner = false;
  }

  private void HostLeftGame()
  {
    foreach (Client client in Server.clients.Values)
    {
      if (client != null && client.player != null && client.player.id != LocalClient.instance.myId)
      {
        ServerSend.DisconnectPlayer(client.player.id);
        MonoBehaviour.print((object) "sending disconnect to all players");
      }
    }
  }

  private void ShowEndScreen()
  {
    GameManager.state = GameManager.GameState.GameOver;
    this.gameoverUi.SetActive(true);
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
  }

  public void ReturnToMenu() => SceneManager.LoadScene("TestSteamLobby");

  public List<Vector3> FindSurvivalSpawnPositions(int nPlayers)
  {
    Vector3 vector3 = Vector3.zero;
    List<Vector3> vector3List = new List<Vector3>();
    for (int index = 0; index < 100; ++index)
    {
      UnityEngine.Random.InitState(GameManager.GetSeed());
      Vector2 vector2 = UnityEngine.Random.insideUnitCircle * this.mapRadius;
      RaycastHit hitInfo;
      if (Physics.Raycast(new Vector3(vector2.x, 200f, vector2.y), Vector3.down, out hitInfo, 500f, (int) this.whatIsGround) && WorldUtility.WorldHeightToBiome(hitInfo.point.y) != TextureData.TerrainType.Water)
      {
        vector3 = hitInfo.point;
        break;
      }
    }
    for (int index = 0; index < 100; ++index)
    {
      Vector2 vector2 = UnityEngine.Random.insideUnitCircle * 50f;
      RaycastHit hitInfo;
      if (Physics.Raycast(vector3 + new Vector3(vector2.x, 200f, vector2.y), Vector3.down, out hitInfo, 500f, (int) this.whatIsGround) && WorldUtility.WorldHeightToBiome(hitInfo.point.y) != TextureData.TerrainType.Water)
        vector3List.Add(hitInfo.point + Vector3.up);
    }
    while (vector3List.Count < nPlayers)
      vector3List.Add(new Vector3(0.0f, 50f, 0.0f));
    return vector3List;
  }

  public List<Vector3> FindVersusSpawnPositions(int nPlayers)
  {
    List<Vector3> vector3List = new List<Vector3>();
    for (int index = 0; index < 100; ++index)
    {
      Vector2 vector2 = UnityEngine.Random.insideUnitCircle * this.mapRadius;
      RaycastHit hitInfo;
      if (Physics.Raycast(new Vector3(vector2.x, 200f, vector2.y), Vector3.down, out hitInfo, 500f, (int) this.whatIsGround) && WorldUtility.WorldHeightToBiome(hitInfo.point.y) != TextureData.TerrainType.Water)
        vector3List.Add(hitInfo.point + Vector3.up);
    }
    while (vector3List.Count <= nPlayers)
    {
      Debug.LogError((object) "Couldnt find spawn positions");
      vector3List.Add(new Vector3(0.0f, 50f, 0.0f));
    }
    return vector3List;
  }

  private void OnApplicationQuit() => ClientSend.PlayerDisconnect();

  public bool KickPlayer(string username)
  {
    foreach (Client client in Server.clients.Values)
    {
      if (client != null && client.player != null && (client.player.id != LocalClient.instance.myId && client.player.username.ToLower() == username.ToLower()))
      {
        ServerHandle.KickPlayer(client.player.id);
        SteamNetworking.CloseP2PSessionWithUser((SteamId) client.player.steamId.Value);
        Debug.Log((object) ("Kicking player: " + client.player.username + " with id " + (object) client.player.id));
        return true;
      }
    }
    return false;
  }

  public void SendPlayersIntoGame(List<Vector3> spawnPositions)
  {
    this.spawnPositions = spawnPositions;
    this.Invoke("SendPlayersIntoGameNow", 2f);
  }

  private void SendPlayersIntoGameNow()
  {
    int index = 0;
    foreach (Client client1 in Server.clients.Values)
    {
      if (client1?.player != null)
      {
        foreach (Client client2 in Server.clients.Values)
        {
          if (client2?.player != null)
          {
            ServerSend.SpawnPlayer(client1.id, client2.player, this.spawnPositions[index] + Vector3.up);
            ++index;
          }
        }
      }
    }
  }

  public void MakeStats(Packet packet)
  {
    this.stats = new Dictionary<string, int>[NetworkController.maxPlayers];
    int num = packet.ReadInt();
    this.nStatsPlayers = num;
    for (int index = 0; index < num; ++index)
    {
      this.stats[index] = new Dictionary<string, int>();
      this.stats[index].Add("Id", packet.ReadInt());
      foreach (string allStat in Player.allStats)
        this.stats[index].Add(allStat, packet.ReadInt());
    }
  }

  public enum GameState
  {
    Loading,
    Playing,
    GameOver,
  }
}
