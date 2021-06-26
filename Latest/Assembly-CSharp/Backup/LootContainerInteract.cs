// Decompiled with JetBrains decompiler
// Type: LootContainerInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LootContainerInteract : MonoBehaviour, Interactable, SharedObject
{
  public LootDrop lootTable;
  public int price;
  private int basePrice;
  private int id;
  private static int totalId = 69420;
  private bool ready = true;
  private bool opened;
  public Animator animator;
  public float white;
  public float blue;
  public float gold;
  public bool testPowerup;
  public Powerup powerupToTest;

  private void Start()
  {
    if (this.testPowerup)
      this.TestSpawn();
    this.ready = true;
    this.basePrice = this.price;
  }

  private void OnEnable()
  {
    if (!this.opened)
      return;
    this.OpenContainer();
  }

  private void TestSpawn()
  {
    this.id = LootContainerInteract.totalId++;
    ResourceManager.Instance.AddObject(this.id, this.gameObject);
  }

  public void Interact()
  {
    if (InventoryUI.Instance.GetMoney() < this.price || !this.ready)
      return;
    this.ready = false;
    InventoryUI.Instance.UseMoney(this.price);
    ClientSend.PickupInteract(this.id);
  }

  private void GetReady() => this.ready = true;

  public void LocalExecute()
  {
  }

  public void AllExecute() => this.OpenContainer();

  public void ServerExecute(int fromClient)
  {
    if (!LocalClient.serverOwner)
      return;
    Powerup powerup = ItemManager.Instance.GetRandomPowerup(this.white, this.blue, this.gold);
    if (this.testPowerup && (Object) this.powerupToTest != (Object) null)
      powerup = this.powerupToTest;
    int nextId = ItemManager.Instance.GetNextId();
    ItemManager.Instance.DropPowerupAtPosition(powerup.id, this.transform.position, nextId);
    ServerSend.DropPowerupAtPosition(powerup.id, nextId, this.transform.position);
  }

  public void RemoveObject()
  {
  }

  public void OpenContainer()
  {
    this.opened = true;
    if (!this.gameObject.activeInHierarchy)
      return;
    this.animator.Play("OpenChest");
    Object.Destroy((Object) this.gameObject);
  }

  public string GetName()
  {
    this.price = (int) ((double) this.basePrice * (double) GameManager.instance.ChestPriceMultiplier());
    return this.price < 1 ? "Open chest" : string.Format("{0} Gold\n<size=75%>open chest", (object) this.price);
  }

  public bool IsStarted() => false;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;
}
