// Decompiled with JetBrains decompiler
// Type: CauldronSync
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CauldronSync : Chest
{
  private int fuelCellId;
  private int resultCellId = 5;
  private int[] ingredientCells = new int[4]{ 1, 2, 3, 4 };
  private bool processing;
  private float currentProcessTime;
  private float totalProcessTime;
  private float timeToProcess = 1f;

  public float ProgressRatio() => this.currentProcessTime / this.timeToProcess;

  public override void UpdateCraftables()
  {
    InventoryItem inventoryItem = this.CanProcess();
    if (!this.processing && (Object) inventoryItem != (Object) null)
    {
      ItemFuel fuel = this.cells[this.fuelCellId].fuel;
      this.totalProcessTime = 0.0f;
      this.currentProcessTime = 0.0f;
      this.timeToProcess = inventoryItem.processTime / fuel.speedMultiplier;
      this.processing = true;
    }
    if (!(bool) (Object) CauldronUI.Instance || !((Object) OtherInput.Instance.currentChest == (Object) this))
      return;
    CauldronUI.Instance.CopyChest(OtherInput.Instance.currentChest);
    CauldronUI.Instance.processBar.transform.localScale = new Vector3(this.currentProcessTime / this.timeToProcess, 1f, 1f);
  }

  private void Update()
  {
    if (!this.processing)
      return;
    if (!(bool) (Object) this.CanProcess())
    {
      this.StopProcessing();
    }
    else
    {
      this.currentProcessTime += Time.deltaTime;
      this.totalProcessTime += Time.deltaTime;
      if ((bool) (Object) CauldronUI.Instance && (Object) OtherInput.Instance.currentChest == (Object) this)
        CauldronUI.Instance.processBar.transform.localScale = new Vector3(this.currentProcessTime / this.timeToProcess, 1f, 1f);
      if ((double) this.currentProcessTime < (double) this.timeToProcess)
        return;
      this.ProcessItem();
      this.currentProcessTime = 0.0f;
    }
  }

  private void StopProcessing()
  {
    this.processing = false;
    if (!(bool) (Object) CauldronUI.Instance)
      return;
    CauldronUI.Instance.processBar.transform.localScale = Vector3.zero;
  }

  public void ProcessItem()
  {
    if (!LocalClient.serverOwner)
      return;
    InventoryItem inventoryItem = this.CanProcess();
    foreach (int ingredientCell in this.ingredientCells)
    {
      if (!((Object) this.cells[ingredientCell] == (Object) null))
      {
        foreach (InventoryItem.CraftRequirement requirement in inventoryItem.requirements)
        {
          if (requirement.item.id == this.cells[ingredientCell].id)
          {
            this.UseMaterial(this.cells[ingredientCell], ingredientCell);
            break;
          }
        }
      }
    }
    this.UseFuel(this.cells[this.fuelCellId]);
    this.AddMaterial(this.cells[this.resultCellId], inventoryItem.id);
    this.UpdateCraftables();
    if (!(bool) (Object) CauldronUI.Instance || !((Object) OtherInput.Instance.currentChest == (Object) this))
      return;
    CauldronUI.Instance.CopyChest(OtherInput.Instance.currentChest);
  }

  private void UseMaterial(InventoryItem materialItem, int cellId)
  {
    --materialItem.amount;
    if (materialItem.amount <= 0)
    {
      materialItem = (InventoryItem) null;
      ClientSend.ChestUpdate(this.id, cellId, -1, 0);
    }
    else
      ClientSend.ChestUpdate(this.id, cellId, materialItem.id, materialItem.amount);
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
    if ((Object) this.cells[this.resultCellId] == (Object) null)
    {
      this.cells[this.resultCellId] = Object.Instantiate<InventoryItem>(ItemManager.Instance.allItems[processedItemId]);
      this.cells[this.resultCellId].amount = 1;
    }
    else
      ++this.cells[this.resultCellId].amount;
    ClientSend.ChestUpdate(this.id, this.resultCellId, processedItemId, this.cells[this.resultCellId].amount);
  }

  public InventoryItem CanProcess()
  {
    if (this.NoIngredients() || !(bool) (Object) this.cells[this.fuelCellId])
      return (InventoryItem) null;
    InventoryItem itemByIngredients = this.FindItemByIngredients(this.ingredientCells);
    if ((Object) itemByIngredients == (Object) null)
      return (InventoryItem) null;
    if ((Object) this.cells[this.resultCellId] != (Object) null)
    {
      if (itemByIngredients.id != this.cells[this.resultCellId].id)
        return (InventoryItem) null;
      if (this.cells[this.resultCellId].amount + itemByIngredients.craftAmount > this.cells[this.resultCellId].max)
        return (InventoryItem) null;
    }
    return this.cells[this.fuelCellId].tag == InventoryItem.ItemTag.Fuel ? itemByIngredients : (InventoryItem) null;
  }

  public InventoryItem FindItemByIngredients(int[] iCells)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    foreach (int iCell in iCells)
    {
      if ((Object) this.cells[iCell] != (Object) null)
        inventoryItemList.Add(this.cells[iCell]);
    }
    foreach (InventoryItem inventoryItem1 in CauldronUI.Instance.processableFood)
    {
      int count = inventoryItemList.Count;
      int num = 0;
      if (inventoryItem1.requirements.Length == count)
      {
        bool flag = false;
        foreach (InventoryItem.CraftRequirement requirement in inventoryItem1.requirements)
        {
          foreach (InventoryItem inventoryItem2 in inventoryItemList)
          {
            if (inventoryItem2.id == requirement.item.id)
            {
              if (inventoryItem2.amount < requirement.amount)
              {
                flag = true;
                break;
              }
              ++num;
              break;
            }
          }
        }
        if (!flag && num == count)
          return inventoryItem1;
      }
    }
    return (InventoryItem) null;
  }

  private bool NoIngredients()
  {
    foreach (int ingredientCell in this.ingredientCells)
    {
      if ((Object) this.cells[ingredientCell] != (Object) null)
        return false;
    }
    return true;
  }
}
