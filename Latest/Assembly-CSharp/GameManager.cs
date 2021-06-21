// Decompiled with JetBrains decompiler
// Type: GameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
  public GameObject lobbyCamera;
  public GameObject testGame;
  public GameObject zone;
  private bool winnerSent;
  public GameObject gravePrefab;
  public int winnerId;
  private float mapRadius;
  public LayerMask whatIsGround;
  public LayerMask whatIsGroundAndObject;
  private List<Vector3> spawnPositions;

  public static GameSettings gameSettings { get; set; }

  private void Awake()
  {
    if (Object.op_Equality((Object) GameManager.instance, (Object) null))
      GameManager.instance = this;
    else if (Object.op_Inequality((Object) GameManager.instance, (Object) this))
      Object.Destroy((Object) this);
    GameManager.players = new Dictionary<int, PlayerManager>();
    this.currentDay = 0;
  }

  private void Start()
  {
    if (GameManager.gameSettings == null && Object.op_Equality((Object) NetworkController.Instance, (Object) null))
    {
      Debug.LogError((object) "testing spawn");
      GameManager.gameSettings = new GameSettings(44430);
      GameManager.gameSettings.gameMode = GameSettings.GameMode.Survival;
      GameManager.gameSettings.difficulty = GameSettings.Difficulty.Normal;
      if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
        Object.Instantiate<GameObject>((M0) this.zone, Vector3.get_zero(), Quaternion.get_identity());
      Server.InitializeServerPackets();
      LocalClient.InitializeClientData();
      Object.Instantiate<GameObject>((M0) this.testGame);
      LocalClient.instance.serverHost = SteamId.op_Implicit((ulong) SteamManager.Instance.PlayerSteamId.Value);
      SteamLobby.steamIdToClientId.Add((ulong) SteamManager.Instance.PlayerSteamId.Value, 0);
      Server.clients.Add(0, new Client(0));
      Server.clients[0].player = new Player(0, "Dani", Color.get_black(), SteamId.op_Implicit((ulong) SteamManager.Instance.PlayerSteamId.Value));
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
      Object.Instantiate<GameObject>((M0) gameManager.zone, Vector3.get_zero(), Quaternion.get_identity());
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
    LoadingScreen.Instance.SetText("Generating navmesh", 0.75f);
    gameManager.generateNavmesh.GenerateNavMesh();
    yield return (object) 60;
    LoadingScreen.Instance.SetText("Finished loading", 1f);
    ClientSend.PlayerFinishedLoading();
    LoadingScreen.Instance.FinishLoading();
  }

  private void GenerateWorld()
  {
    MonoBehaviour.print((object) "generating world");
    this.mapGenerator.GenerateMap(GameManager.GetSeed());
    MonoBehaviour.print((object) "generating resources");
    this.resourceGen.SetActive(true);
    MonoBehaviour.print((object) "generating navmesh");
    this.generateNavmesh.GenerateNavMesh();
  }

  public int currentDay { get; set; }

  public void UpdateDay(int day)
  {
    this.currentDay = day;
    this.dayUi.SetDay(day);
    this.extraUi.UpdateDay(this.currentDay);
    if (GameManager.gameSettings.gameMode != GameSettings.GameMode.Versus)
      return;
    ZoneController.Instance.NextDay(this.currentDay);
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
      Object.Instantiate<GameObject>((M0) this.localPlayerPrefab, position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
      component = (PlayerManager) ((Component) PlayerMovement.Instance).get_gameObject().GetComponent<PlayerManager>();
    }
    else
    {
      component = (PlayerManager) ((GameObject) Object.Instantiate<GameObject>((M0) this.playerPrefab, position, Quaternion.Euler(0.0f, orientationY, 0.0f))).GetComponent<PlayerManager>();
      component.SetDesiredPosition(position);
    }
    component.SetDesiredPosition(position);
    component.id = id;
    component.username = username;
    component.color = color;
    GameManager.players.Add(id, component);
    if ((GameManager.gameSettings.gameMode != GameSettings.GameMode.Versus || id != LocalClient.instance.myId) && GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
      return;
    this.extraUi.InitPlayerStatus(id, username);
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
    float num3 = 1.6f;
    if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Easy)
    {
      num1 = 0.35f;
      num2 = 0.2f;
      num3 = 1.4f;
    }
    else if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Gamer)
    {
      num1 = 1.75f;
      num2 = 0.25f;
      num3 = 2.3f;
    }
    return num1 + Mathf.Pow(num2 * (float) this.currentDay, num3);
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
    float num2 = 0.23f;
    float num3 = 1.54f;
    if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Easy)
    {
      num1 = 0.9f;
      num2 = 0.2f;
      num3 = 1.3f;
    }
    else if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Gamer)
    {
      num1 = 1.3f;
      num2 = 0.28f;
      num3 = 1.65f;
    }
    return num1 + Mathf.Pow(num2 * (float) this.currentDay, num3);
  }

  private void SlowUpdate()
  {
    if (this.winnerSent || GameManager.gameSettings.gameMode != GameSettings.GameMode.Versus || this.GetPlayersAlive() > 1)
      return;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (Object.op_Inequality((Object) playerManager, (Object) null) && !playerManager.dead)
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
    ((Component) player).get_gameObject().SetActive(false);
    pos = ((Component) player).get_transform().get_position();
    ((PlayerRagdoll) ((GameObject) Object.Instantiate<GameObject>((M0) this.playerRagdoll, pos, ((Component) player).get_transform().get_rotation())).GetComponent<PlayerRagdoll>()).SetRagdoll(id, Vector3.op_UnaryNegation(((Component) player).get_transform().get_forward()));
  }

  public Vector3 GetGravePosition(int playerId)
  {
    try
    {
      Vector3 vector3 = ((Component) GameManager.players[playerId]).get_transform().get_position();
      if (vector3.y < -100.0)
        vector3 = Vector3.get_zero();
      RaycastHit raycastHit;
      if (Physics.Raycast(Vector3.op_Addition(vector3, Vector3.op_Multiply(Vector3.get_up(), 3000f)), Vector3.get_down(), ref raycastHit, 8000f, LayerMask.op_Implicit(this.whatIsGround)))
        return ((RaycastHit) ref raycastHit).get_point();
    }
    catch (Exception ex)
    {
      return Vector3.get_zero();
    }
    return Vector3.get_zero();
  }

  public void SpawnGrave(Vector3 gravePos, int playerId, int graveObjectId)
  {
    PlayerManager player = GameManager.players[playerId];
    ++player.deaths;
    GraveInteract componentInChildren = (GraveInteract) ((GameObject) Object.Instantiate<GameObject>((M0) this.gravePrefab, gravePos, Quaternion.get_identity())).GetComponentInChildren<GraveInteract>();
    componentInChildren.username = player.username.Substring(0, Mathf.Clamp(15, 0, player.username.Length));
    componentInChildren.playerId = player.id;
    componentInChildren.SetId(graveObjectId);
    ResourceManager.Instance.AddObject(graveObjectId, ((Component) ((Component) componentInChildren).get_transform().get_parent()).get_gameObject());
    player.graveId = graveObjectId;
    float time = Mathf.Clamp((float) ((double) componentInChildren.timeLeft * (double) (GameManager.players[playerId].deaths - 1) * 2.0), 30f, 300f);
    componentInChildren.SetTime(time);
    ((GravePing) ((Component) ((Component) componentInChildren).get_transform().get_root()).GetComponentInChildren<GravePing>()).SetPing(player.username);
  }

  public void RespawnPlayer(int id, Vector3 zero)
  {
    if (!GameManager.players.ContainsKey(id))
      return;
    GameManager.players[id].dead = false;
    Vector3 position = ResourceManager.Instance.list[GameManager.players[id].graveId].get_transform().get_position();
    if (GameManager.players[id].graveId != -1)
      GameManager.players[id].RemoveGrave();
    if (LocalClient.instance.myId == id)
    {
      ((Component) PlayerMovement.Instance).get_transform().set_position(Vector3.op_Addition(position, Vector3.op_Multiply(Vector3.get_up(), 3f)));
      ((Component) PlayerMovement.Instance).get_gameObject().SetActive(true);
      PlayerStatus.Instance.Respawn();
    }
    else
      ((Component) GameManager.players[id]).get_gameObject().SetActive(true);
  }

  public void StartGame()
  {
    LoadingScreen.Instance.Hide();
    this.lobbyCamera.SetActive(false);
    GameManager.state = GameManager.GameState.Playing;
    if (LocalClient.serverOwner)
      GameLoop.Instance.StartLoop();
    Hotbar.Instance.UpdateHotbar();
  }

  public void DisconnectPlayer(int id)
  {
    if (Object.op_Inequality((Object) GameManager.players[id], (Object) null) && Object.op_Inequality((Object) ((Component) GameManager.players[id]).get_gameObject(), (Object) null))
    {
      Object.Destroy((Object) ((Component) GameManager.players[id]).get_gameObject());
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
      if (Object.op_Implicit((Object) playerManager) && !playerManager.dead)
        ++num;
    }
    MonoBehaviour.print((object) ("players alive:  " + (object) num));
    return num;
  }

  public int GetPlayersInLobby()
  {
    int num = 0;
    foreach (Object @object in GameManager.players.Values)
    {
      if (Object.op_Implicit(@object))
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

  public void GameOver(int winnerId)
  {
    this.winnerId = winnerId;
    this.Invoke("ShowEndScreen", 4f);
    MusicController.Instance.StopSong();
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
    LocalClient.instance.serverHost = (SteamId) null;
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
    Cursor.set_visible(true);
    Cursor.set_lockState((CursorLockMode) 0);
  }

  public void ReturnToMenu() => SceneManager.LoadScene("TestSteamLobby");

  public List<Vector3> FindSurvivalSpawnPositions(int nPlayers)
  {
    Vector3 vector3 = Vector3.get_zero();
    List<Vector3> vector3List = new List<Vector3>();
    for (int index = 0; index < 100; ++index)
    {
      Random.InitState(GameManager.GetSeed());
      Vector2 vector2 = Vector2.op_Multiply(Random.get_insideUnitCircle(), this.mapRadius);
      RaycastHit raycastHit;
      if (Physics.Raycast(new Vector3((float) vector2.x, 200f, (float) vector2.y), Vector3.get_down(), ref raycastHit, 500f, LayerMask.op_Implicit(this.whatIsGround)) && WorldUtility.WorldHeightToBiome((float) ((RaycastHit) ref raycastHit).get_point().y) != TextureData.TerrainType.Water)
      {
        vector3 = ((RaycastHit) ref raycastHit).get_point();
        break;
      }
    }
    for (int index = 0; index < 100; ++index)
    {
      Vector2 vector2 = Vector2.op_Multiply(Random.get_insideUnitCircle(), 50f);
      RaycastHit raycastHit;
      if (Physics.Raycast(Vector3.op_Addition(vector3, new Vector3((float) vector2.x, 200f, (float) vector2.y)), Vector3.get_down(), ref raycastHit, 500f, LayerMask.op_Implicit(this.whatIsGround)) && WorldUtility.WorldHeightToBiome((float) ((RaycastHit) ref raycastHit).get_point().y) != TextureData.TerrainType.Water)
        vector3List.Add(Vector3.op_Addition(((RaycastHit) ref raycastHit).get_point(), Vector3.get_up()));
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
      Vector2 vector2 = Vector2.op_Multiply(Random.get_insideUnitCircle(), this.mapRadius);
      RaycastHit raycastHit;
      if (Physics.Raycast(new Vector3((float) vector2.x, 200f, (float) vector2.y), Vector3.get_down(), ref raycastHit, 500f, LayerMask.op_Implicit(this.whatIsGround)) && WorldUtility.WorldHeightToBiome((float) ((RaycastHit) ref raycastHit).get_point().y) != TextureData.TerrainType.Water)
        vector3List.Add(Vector3.op_Addition(((RaycastHit) ref raycastHit).get_point(), Vector3.get_up()));
    }
    while (vector3List.Count <= nPlayers)
    {
      Debug.LogError((object) "Couldnt find spawn positions");
      vector3List.Add(new Vector3(0.0f, 50f, 0.0f));
    }
    return vector3List;
  }

  private void OnApplicationQuit() => ClientSend.PlayerDisconnect();

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
            ServerSend.SpawnPlayer(client1.id, client2.player, Vector3.op_Addition(this.spawnPositions[index], Vector3.get_up()));
            ++index;
          }
        }
      }
    }
  }

  public GameManager() => base.\u002Ector();

  public enum GameState
  {
    Loading,
    Playing,
    GameOver,
  }
}
