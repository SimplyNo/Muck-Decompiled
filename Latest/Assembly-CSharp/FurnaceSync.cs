// Decompiled with JetBrains decompiler
// Type: FurnaceSync
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class FurnaceSync : Chest
{
  public InventoryItem.ProcessType processType;
  private bool processing;
  private float currentProcessTime;
  private float totalProcessTime;
  private float timeToProcess = 1f;
  private InventoryItem currentFuel;
  private InventoryItem currentMetal;

  public float ProgressRatio() => this.currentProcessTime / this.timeToProcess;

  public override void UpdateCraftables()
  {
    if (this.processing && this.CanProcess() && (this.currentFuel.id != this.cells[0].id || this.currentMetal.id != this.cells[1].id))
      this.StartProcessing();
    if (!this.processing && this.CanProcess())
      this.StartProcessing();
    if (!Object.op_Inequality((Object) FurnaceUI.Instance, (Object) null) || !Object.op_Equality((Object) OtherInput.Instance.currentChest, (Object) this))
      return;
    FurnaceUI.Instance.CopyChest(OtherInput.Instance.currentChest);
    ((Component) FurnaceUI.Instance.processBar).get_transform().set_localScale(new Vector3(this.currentProcessTime / this.timeToProcess, 1f, 1f));
  }

  private void StartProcessing()
  {
    this.currentFuel = this.cells[0];
    this.currentMetal = this.cells[1];
    ItemFuel fuel = this.currentFuel.fuel;
    this.totalProcessTime = 0.0f;
    this.currentProcessTime = 0.0f;
    this.timeToProcess = this.currentMetal.processTime / fuel.speedMultiplier;
    this.processing = true;
  }

  private void Update()
  {
    if (!this.processing)
      return;
    if (!this.CanProcess())
    {
      this.StopProcessing();
      MonoBehaviour.print((object) "stopped due to one of these conditions");
    }
    else
    {
      this.currentProcessTime += Time.get_deltaTime();
      this.totalProcessTime += Time.get_deltaTime();
      if (Object.op_Implicit((Object) FurnaceUI.Instance) && Object.op_Equality((Object) OtherInput.Instance.currentChest, (Object) this))
        ((Component) FurnaceUI.Instance.processBar).get_transform().set_localScale(new Vector3(this.currentProcessTime / this.timeToProcess, 1f, 1f));
      if ((double) this.currentProcessTime < (double) this.timeToProcess)
        return;
      this.ProcessItem();
      this.currentProcessTime = 0.0f;
    }
  }

  private void StopProcessing()
  {
    this.processing = false;
    if (!Object.op_Implicit((Object) FurnaceUI.Instance))
      return;
    ((Component) FurnaceUI.Instance.processBar).get_transform().set_localScale(Vector3.get_zero());
  }

  public void ProcessItem()
  {
    if (!LocalClient.serverOwner)
      return;
    int id = this.cells[1].processedItem.id;
    this.UseMaterial(this.cells[1]);
    this.UseFuel(this.cells[0]);
    this.AddMaterial(this.cells[2], id);
    this.UpdateCraftables();
  }

  private void UseMaterial(InventoryItem materialItem)
  {
    --materialItem.amount;
    if (materialItem.amount <= 0)
    {
      materialItem = (InventoryItem) null;
      ClientSend.ChestUpdate(this.id, 1, -1, 0);
    }
    else
      ClientSend.ChestUpdate(this.id, 1, materialItem.id, materialItem.amount);
  }

  private void UseFuel(InventoryItem fuelItem)
  {
    ItemFuel fuel = fuelItem.fuel;
    --fuel.currentUses;
    if (fuel.currentUses <= 0)
    {
      --fuelItem.amount;
      fuel.currentUses = fuel.maxUses;
      ClientSend.ChestUpdate(this.id, 0, fuelItem.id, fuelItem.amount);
    }
    if (fuelItem.amount > 0)
      return;
    fuelItem = (InventoryItem) null;
    ClientSend.ChestUpdate(this.id, 0, -1, 0);
  }

  private void AddMaterial(InventoryItem item, int processedItemId)
  {
    if (Object.op_Equality((Object) this.cells[2], (Object) null))
    {
      this.cells[2] = (InventoryItem) Object.Instantiate<InventoryItem>((M0) ItemManager.Instance.allItems[processedItemId]);
      this.cells[2].amount = 1;
    }
    else
      ++this.cells[2].amount;
    ClientSend.ChestUpdate(this.id, 2, processedItemId, this.cells[2].amount);
  }

  public bool CanProcess() => Object.op_Implicit((Object) this.cells[1]) && Object.op_Implicit((Object) this.cells[0]) && (!Object.op_Inequality((Object) this.cells[2], (Object) null) || !Object.op_Equality((Object) this.cells[1].processedItem, (Object) null) && this.cells[1].processedItem.id == this.cells[2].id && this.cells[2].amount < this.cells[2].max) && (this.cells[1].processable && this.cells[1].processType == this.processType && this.cells[0].tag == InventoryItem.ItemTag.Fuel);
}
