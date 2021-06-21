// Decompiled with JetBrains decompiler
// Type: GameLoop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
  public int currentDay = -1;
  private Vector2 maxCheckMobUpdateInterval = new Vector2(3f, 10f);
  private Vector2 checkMobUpdateInterval = new Vector2(3f, 10f);
  public GameLoop.MobSpawn[] mobs;
  private int activeMobs;
  private int maxMobCap = 999;
  private float totalWeight;
  public LayerMask whatIsSpawnable;
  [Header("Boss Stuff")]
  public MobType[] bosses;
  public static GameLoop Instance;
  private bool nightStarted;
  private bool bossNight;

  public static int currentMobCap { get; set; } = 999;

  private void Update()
  {
    if (!LocalClient.serverOwner || GameManager.state != GameManager.GameState.Playing || GameManager.gameSettings.gameMode == GameSettings.GameMode.Creative)
      return;
    this.DayLoop();
  }

  private void Awake() => GameLoop.Instance = this;

  public void StartLoop()
  {
    if (!LocalClient.serverOwner)
      UnityEngine.Object.Destroy((UnityEngine.Object) this);
    foreach (Client client in Server.clients.Values)
      client?.player?.PingPlayer();
    this.InvokeRepeating("TimeoutPlayers", 2f, 2f);
  }

  private void NewDay(int day)
  {
    if (GameManager.instance.GetPlayersAlive() <= 0)
      return;
    this.bossNight = false;
    this.nightStarted = false;
    this.currentDay = day;
    this.CancelInvoke("CheckMobSpawns");
    ServerSend.NewDay(day);
    GameManager.instance.UpdateDay(day);
    this.totalWeight = this.CalculateSpawnWeights(this.currentDay);
    int num = GameManager.instance.GetPlayersInLobby();
    if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
      num = GameManager.instance.GetPlayersAlive();
    GameLoop.currentMobCap = (int) (3 + GameManager.gameSettings.difficulty + (int) ((double) (num - 1) * 0.200000002980232));
    GameLoop.currentMobCap = (int) ((double) GameLoop.currentMobCap + (double) (GameLoop.currentMobCap * this.currentDay) * 0.5);
    if (GameLoop.currentMobCap > this.maxMobCap)
      GameLoop.currentMobCap = this.maxMobCap;
    if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
      GameLoop.currentMobCap = (int) ((double) num + (double) this.currentDay * 0.5);
    this.checkMobUpdateInterval = this.maxCheckMobUpdateInterval * (1f - PowerupInventory.CumulativeDistribution(this.currentDay, 0.05f, 0.5f));
    MusicController.Instance.PlaySong(MusicController.SongType.Day);
  }

  private void DayLoop()
  {
    int day = Mathf.FloorToInt(DayCycle.totalTime);
    if (day > this.currentDay)
    {
      this.NewDay(day);
    }
    else
    {
      if (this.nightStarted || (double) DayCycle.time <= 0.5 || (double) DayCycle.time >= 0.949999988079071)
        return;
      this.nightStarted = true;
      this.StartNight();
    }
  }

  private void StartNight()
  {
    if (this.currentDay != 0 && this.currentDay % GameManager.gameSettings.BossDay() == 0 && GameManager.gameSettings.gameMode == GameSettings.GameMode.Survival)
    {
      this.StartBoss(this.bosses[0]);
      MusicController.Instance.PlaySong(MusicController.SongType.Boss, false);
    }
    else
      MusicController.Instance.PlaySong(MusicController.SongType.Night);
    this.Invoke("CheckMobSpawns", UnityEngine.Random.Range(this.checkMobUpdateInterval.x, this.checkMobUpdateInterval.y));
  }

  public void StartBoss(MobType bossMob)
  {
    float bossMultiplier = (float) (0.899999976158142 + 0.100000001490116 * (double) GameManager.instance.GetPlayersAlive());
    this.SpawnMob(bossMob, this.FindBossPosition(), bossMultiplier: bossMultiplier);
  }

  private void CheckMobSpawns()
  {
    if (this.bossNight || GameManager.gameSettings.gameMode == GameSettings.GameMode.Creative)
      return;
    float num1 = (float) GameManager.instance.GetPlayersAlive() / 2f;
    this.Invoke(nameof (CheckMobSpawns), UnityEngine.Random.Range(this.checkMobUpdateInterval.x / num1, this.checkMobUpdateInterval.y / num1));
    this.activeMobs = MobManager.Instance.GetActiveEnemies();
    if (GameManager.state != GameManager.GameState.Playing || (double) DayCycle.time < 0.5 || (this.activeMobs > this.maxMobCap || this.activeMobs > GameLoop.currentMobCap))
      return;
    int randomAlivePlayer = this.FindRandomAlivePlayer();
    if (randomAlivePlayer == -1)
      return;
    MobType spawn = this.SelectMobToSpawn();
    int num2 = Mathf.Clamp(UnityEngine.Random.Range(1, 3), 1, GameLoop.currentMobCap - this.activeMobs);
    for (int index = 0; index < num2; ++index)
      this.SpawnMob(spawn, this.FindPositionAroundPlayer(randomAlivePlayer));
  }

  private int FindRandomAlivePlayer()
  {
    List<int> intList = new List<int>();
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((bool) (UnityEngine.Object) playerManager && !playerManager.dead)
        intList.Add(playerManager.id);
    }
    return intList.Count < 1 ? -1 : intList[UnityEngine.Random.Range(0, intList.Count)];
  }

  public MobType SelectMobToSpawn(bool shrine = false)
  {
    float num1 = UnityEngine.Random.Range(0.0f, 1f);
    float num2 = 0.0f;
    float num3 = this.totalWeight;
    if (shrine)
      num3 = this.CalculateSpawnWeights(this.currentDay + 2);
    MonoBehaviour.print((object) ("total weight: " + (object) num3));
    for (int index = 0; index < this.mobs.Length; ++index)
    {
      num2 += this.mobs[index].currentWeight;
      if ((double) num1 < (double) num2 / (double) num3)
        return this.mobs[index].mob;
    }
    MonoBehaviour.print((object) "fouind nothing");
    return this.mobs[0].mob;
  }

  private Vector3 FindPositionAroundPlayer(int selectedPlayerId)
  {
    if (!(bool) (UnityEngine.Object) GameManager.players[selectedPlayerId])
      return Vector3.zero;
    Vector3 position = GameManager.players[selectedPlayerId].transform.position;
    Vector2 vector2 = UnityEngine.Random.insideUnitCircle * 60f;
    Vector3 vector3_1 = new Vector3(vector2.x, 0.0f, vector2.y);
    MonoBehaviour.print((object) ("offset: " + (object) vector3_1));
    Vector3 vector3_2 = Vector3.up * 20f;
    RaycastHit hitInfo;
    if (Physics.Raycast(position + vector3_2 + vector3_1, Vector3.down, out hitInfo, 5000f, (int) this.whatIsSpawnable))
      return hitInfo.point;
    Debug.LogError((object) "Failed to spawn");
    return Vector3.zero;
  }

  private Vector3 FindBossPosition() => this.FindPositionAroundPlayer(this.FindRandomAlivePlayer());

  private int SpawnMob(MobType mob, Vector3 pos, float multiplier = 1f, float bossMultiplier = 1f)
  {
    float num = 0.01f + Mathf.Clamp((float) this.currentDay * 0.01f, 0.05f, 0.3f);
    if ((double) UnityEngine.Random.Range(0.0f, 1f) < (double) num)
      multiplier = 1.5f;
    if (this.activeMobs > this.maxMobCap || this.activeMobs > GameLoop.currentMobCap)
      return -1;
    int nextId = MobManager.Instance.GetNextId();
    MobSpawner.Instance.ServerSpawnNewMob(nextId, mob.id, pos, multiplier, bossMultiplier);
    return nextId;
  }

  private float CalculateSpawnWeights(int day)
  {
    float num1 = 0.0f;
    foreach (GameLoop.MobSpawn mob in this.mobs)
    {
      if (day >= mob.dayStart)
      {
        int num2 = Mathf.Clamp(day, mob.dayStart, mob.dayPeak);
        int num3 = mob.dayPeak - mob.dayStart + 1;
        int dayStart = mob.dayStart;
        int num4 = num2 - dayStart + 1;
        mob.currentWeight = num3 != 0 ? (float) num4 / (float) num3 : mob.maxWeight;
        num1 += mob.currentWeight;
      }
    }
    return num1;
  }

  public void TimeoutPlayers()
  {
    foreach (Client client in Server.clients.Values)
    {
      if (client != null && client.player != null)
      {
        int num = 60;
        if ((double) Time.time - (double) client.player.lastPingTime > (double) num)
        {
          Debug.Log((object) ("Kicking player: " + client.player.username + " with id " + (object) client.player.id));
          SteamNetworking.CloseP2PSessionWithUser((SteamId) client.player.steamId.Value);
          ServerHandle.DisconnectPlayer(client.player.id);
          break;
        }
      }
    }
  }

  [Serializable]
  public class MobSpawn
  {
    public MobType mob;
    public int dayStart;
    public int dayPeak;
    public float maxWeight;
    public float currentWeight;
  }
}
