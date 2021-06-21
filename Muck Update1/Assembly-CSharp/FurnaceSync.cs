// Decompiled with JetBrains decompiler
// Type: FurnaceSync
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    if (!((Object) FurnaceUI.Instance != (Object) null) || !((Object) OtherInput.Instance.currentChest == (Object) this))
      return;
    FurnaceUI.Instance.CopyChest(OtherInput.Instance.currentChest);
    FurnaceUI.Instance.processBar.transform.localScale = new Vector3(this.currentProcessTime / this.timeToProcess, 1f, 1f);
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
      this.currentProcessTime += Time.deltaTime;
      this.totalProcessTime += Time.deltaTime;
      if ((bool) (Object) FurnaceUI.Instance && (Object) OtherInput.Instance.currentChest == (Object) this)
        FurnaceUI.Instance.processBar.transform.localScale = new Vector3(this.currentProcessTime / this.timeToProcess, 1f, 1f);
      if ((double) this.currentProcessTime < (double) this.timeToProcess)
        return;
      this.ProcessItem();
      this.currentProcessTime = 0.0f;
    }
  }

  private void StopProcessing()
  {
    this.processing = false;
    if (!(bool) (Object) FurnaceUI.Instance)
      return;
    FurnaceUI.Instance.processBar.transform.localScale = Vector3.zero;
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
    if ((Object) this.cells[2] == (Object) null)
    {
      this.cells[2] = Object.Instantiate<InventoryItem>(ItemManager.Instance.allItems[processedItemId]);
      this.cells[2].amount = 1;
    }
    else
      ++this.cells[2].amount;
    ClientSend.ChestUpdate(this.id, 2, processedItemId, this.cells[2].amount);
  }

  public bool CanProcess() => (bool) (Object) this.cells[1] && (bool) (Object) this.cells[0] && (!((Object) this.cells[2] != (Object) null) || !((Object) this.cells[1].processedItem == (Object) null) && this.cells[1].processedItem.id == this.cells[2].id && this.cells[2].amount < this.cells[2].max) && (this.cells[1].processable && this.cells[1].processType == this.processType && this.cells[0].tag == InventoryItem.ItemTag.Fuel);
}
