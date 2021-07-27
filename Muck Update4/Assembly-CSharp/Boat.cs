// Decompiled with JetBrains decompiler
// Type: Boat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
  public Boat.BoatStatus status;
  public static Boat Instance;
  public InventoryItem mapItem;
  public InventoryItem gemMap;
  public GameObject objectivePing;
  public ObjectivePing boatPing;
  private ConsistentRandom rand;
  public SpawnChestsInLocations chestSpawner;
  public GameObject[] holes;
  public Texture gemTexture;
  public Texture boatTexture;
  private bool gemsDiscovered;
  public List<ShrineGuardian> guardians;
  public CountPlayersOnBoat countPlayers;
  private float heightUnderWater = 3f;
  private Rigidbody rb;
  public Transform dragonSpawnPos;
  public Camera cinematicCamera;
  public MobType dragonBoss;
  public Transform rbTransform;
  public GameObject waterSfx;
  public Transform dragonLandingPosition;
  public Transform[] landingNodes;
  public GameObject wheel;
  private bool sinking;
  private float amp = 20f;
  private FinishGameInteract wheelInteract;
  public ObjectivePing wheelPing;
  private Component[] repairs;
  public Map.MapMarker boatMapMarker;

  public float waterHeight { get; set; }

  private void Start()
  {
    this.rb = this.GetComponentInChildren<Rigidbody>();
    this.guardians = new List<ShrineGuardian>();
    this.rand = new ConsistentRandom(GameManager.GetSeed());
    Boat.Instance = this;
    this.InvokeRepeating("CheckFound", 0.5f, 1f);
    this.boatPing = Object.Instantiate<GameObject>(this.objectivePing, this.transform.position, Quaternion.identity).GetComponent<ObjectivePing>();
    this.boatPing.SetText("?");
    this.boatPing.gameObject.SetActive(false);
    for (int index = 0; index < this.holes.Length; ++index)
    {
      if (this.rand.NextDouble() > 0.5)
      {
        Object.Destroy((Object) this.holes[index]);
      }
      else
      {
        Vector3 position = this.holes[index].transform.position;
        float y = position.y;
        RaycastHit hitInfo;
        if (Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out hitInfo, 50f, (int) GameManager.instance.whatIsGround) && (double) hitInfo.point.y > (double) y)
          Object.Destroy((Object) this.holes[index]);
      }
    }
    this.repairs = this.gameObject.GetComponentsInChildren(typeof (RepairInteract), true);
    foreach (RepairInteract repair in this.repairs)
    {
      int nextId = ResourceManager.Instance.GetNextId();
      repair.SetId(nextId);
      ResourceManager.Instance.AddObject(nextId, repair.gameObject);
    }
    if (LocalClient.serverOwner)
      this.InvokeRepeating("SlowUpdate", 1f, 1f);
    foreach (RepairInteract repair in this.repairs)
      ;
    this.gameObject.name = nameof (Boat);
  }

  private void SlowUpdate()
  {
    if (!this.CheckBoatFullyRepaired())
      return;
    this.SendBoatFinished();
    this.CancelInvoke(nameof (SlowUpdate));
  }

  private void SendMarkShip()
  {
    this.MarkShip();
    ClientSend.SendShipStatus(Boat.BoatPackets.MarkShip);
  }

  private void SendShipFound()
  {
    Debug.LogError((object) "Found ship. Not sending");
    this.FindShip();
    ClientSend.SendShipStatus(Boat.BoatPackets.FindShip);
  }

  private void SendMarkGems()
  {
    this.MarkGems();
    ClientSend.SendShipStatus(Boat.BoatPackets.MarkGems);
  }

  private void SendBoatFinished()
  {
    int nextId = ResourceManager.Instance.GetNextId();
    this.BoatFinished(nextId);
    ClientSend.SendShipStatus(Boat.BoatPackets.FinishBoat, nextId);
  }

  public void UpdateShipStatus(Boat.BoatPackets p, int interactId)
  {
    switch (p)
    {
      case Boat.BoatPackets.MarkShip:
        this.MarkShip();
        break;
      case Boat.BoatPackets.FindShip:
        this.FindShip();
        break;
      case Boat.BoatPackets.MarkGems:
        this.MarkGems();
        break;
      case Boat.BoatPackets.FinishBoat:
        this.BoatFinished(interactId);
        break;
    }
  }

  public void LeaveIsland()
  {
    if (this.status == Boat.BoatStatus.LeftIsland)
      return;
    this.status = Boat.BoatStatus.LeftIsland;
    GameManager.instance.boatLeft = true;
    this.sinking = true;
    Object.Destroy((Object) this.wheelInteract.gameObject);
    Object.Destroy((Object) this.wheelPing.gameObject);
    PlayerStatus.Instance.EnterOcean();
    AchievementManager.Instance.LeaveMuck();
  }

  private void FixedUpdate()
  {
    if (!this.sinking)
      return;
    this.MoveBoat();
  }

  private void MoveBoat()
  {
    Vector3 vector3 = Vector3.up * 2f * Time.deltaTime;
    World.Instance.water.position += vector3;
    float y = World.Instance.water.position.y;
    if ((double) this.rb.position.y < (double) y - (double) this.heightUnderWater)
    {
      if (!this.waterSfx.activeInHierarchy)
        this.waterSfx.SetActive(true);
      this.rb.MovePosition(new Vector3(this.transform.position.x, y - this.heightUnderWater, this.transform.position.z));
    }
    if ((double) y <= 85.0)
      return;
    this.sinking = false;
    if (!LocalClient.serverOwner)
      return;
    float bossMultiplier = (float) (0.850000023841858 + 0.150000005960464 * (double) GameManager.instance.GetPlayersAlive());
    int nextId = MobManager.Instance.GetNextId();
    MobSpawner.Instance.ServerSpawnNewMob(nextId, this.dragonBoss.id, this.dragonSpawnPos.position, 1f, bossMultiplier);
    List<Mob> mobList = new List<Mob>();
    foreach (Mob mob in MobManager.Instance.mobs.Values)
      mobList.Add(mob);
    for (int index = 0; index < mobList.Count; ++index)
      mobList[index].hitable.Hit(mobList[index].hitable.maxHp, 1f, 2, mobList[index].transform.position, -1);
  }

  public void BoatFinished(int interactId)
  {
    this.wheel.SetActive(true);
    this.wheelPing = Object.Instantiate<GameObject>(this.objectivePing, this.wheel.transform.position, Quaternion.identity).GetComponent<ObjectivePing>();
    this.wheelPing.SetText("");
    this.wheelInteract = this.wheel.AddComponent<FinishGameInteract>();
    this.wheelInteract.SetId(interactId);
    ResourceManager.Instance.AddObject(interactId, this.wheelInteract.gameObject);
  }

  public bool CheckBoatFullyRepaired()
  {
    foreach (Object repair in this.repairs)
    {
      if (!(repair == (Object) null))
        return false;
    }
    return true;
  }

  public void CheckForMap()
  {
    if (this.status == Boat.BoatStatus.Hidden)
    {
      foreach (InventoryCell cell in InventoryUI.Instance.cells)
      {
        if (!((Object) cell.currentItem == (Object) null) && cell.currentItem.id == this.mapItem.id)
          this.SendMarkShip();
      }
    }
    if (this.gemsDiscovered)
      return;
    foreach (InventoryCell cell in InventoryUI.Instance.cells)
    {
      if (!((Object) cell.currentItem == (Object) null) && cell.currentItem.id == this.gemMap.id)
        this.SendMarkGems();
    }
  }

  private void MarkGems()
  {
    this.gemsDiscovered = true;
    foreach (ShrineGuardian guardian in this.guardians)
    {
      if ((Object) guardian != (Object) null)
      {
        Map.Instance.AddMarker(guardian.transform, Map.MarkerType.Gem, this.gemTexture, Guardian.TypeToColor(guardian.type), "?");
        Map.Instance.AddMarker(guardian.transform, Map.MarkerType.Gem, this.gemTexture, Guardian.TypeToColor(guardian.type), "?");
      }
    }
    ChatBox.Instance.AppendMessage(-1, string.Format("<color=orange>Guardians <color=white>have been located  (\"{0}\" to open map)", (object) InputManager.map), "");
  }

  private void CheckFound()
  {
    if (this.status != Boat.BoatStatus.Hidden && this.status != Boat.BoatStatus.Marked || (!(bool) (Object) PlayerMovement.Instance || (double) Vector3.Distance(PlayerMovement.Instance.transform.position, this.transform.position) >= 40.0))
      return;
    this.SendShipFound();
  }

  public void FindShip()
  {
    this.status = Boat.BoatStatus.Found;
    Object.Destroy((Object) this.boatPing.gameObject);
    Map.Instance.AddMarker(this.transform, Map.MarkerType.Other, this.boatTexture, Color.white, "Shipwreck");
    ChatBox.Instance.AppendMessage(-1, string.Format("<color=orange>Broken Ship <color=white>has been located (\"{0}\" to open map)", (object) InputManager.map), "");
  }

  public void MarkShip()
  {
    this.status = Boat.BoatStatus.Marked;
    this.boatPing.gameObject.SetActive(true);
    ChatBox.Instance.AppendMessage(-1, string.Format("Something has been marked on your map...  (\"{0}\" to open map)", (object) InputManager.map), "");
    Map.Instance.AddMarker(this.transform, Map.MarkerType.Other, (Texture) null, Color.white);
  }

  public enum BoatStatus
  {
    Hidden,
    Marked,
    Found,
    LeftIsland,
  }

  public enum BoatPackets
  {
    MarkShip,
    FindShip,
    MarkGems,
    FinishBoat,
  }
}
