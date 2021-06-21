// Decompiled with JetBrains decompiler
// Type: LootContainerInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class LootContainerInteract : MonoBehaviour, Interactable, SharedObject
{
  public LootDrop lootTable;
  public int price;
  private int basePrice;
  private int id;
  private static int totalId = 69420;
  private bool ready;
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
    ResourceManager.Instance.AddObject(this.id, ((Component) this).get_gameObject());
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
    if (this.testPowerup && Object.op_Inequality((Object) this.powerupToTest, (Object) null))
      powerup = this.powerupToTest;
    int nextId = ItemManager.Instance.GetNextId();
    ItemManager.Instance.DropPowerupAtPosition(powerup.id, ((Component) this).get_transform().get_position(), nextId);
    ServerSend.DropPowerupAtPosition(powerup.id, nextId, ((Component) this).get_transform().get_position());
  }

  public void RemoveObject()
  {
  }

  public void OpenContainer()
  {
    this.opened = true;
    if (!((Component) this).get_gameObject().get_activeInHierarchy())
      return;
    this.animator.Play("OpenChest");
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }

  public string GetName()
  {
    this.price = (int) ((double) this.basePrice * (double) GameManager.instance.ChestPriceMultiplier());
    return this.price < 1 ? "Open chest" : string.Format("{0} Gold\n<size=75%>open chest", (object) this.price);
  }

  public bool IsStarted() => false;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  public LootContainerInteract() => base.\u002Ector();
}
