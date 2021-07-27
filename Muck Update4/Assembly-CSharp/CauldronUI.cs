// Decompiled with JetBrains decompiler
// Type: CauldronUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class CauldronUI : InventoryExtensions
{
  public InventoryCell[] ingredientCells;
  public InventoryCell fuelCell;
  public InventoryCell resultCell;
  public InventoryItem.ProcessType processType;
  public InventoryItem[] processableFood;
  private bool processing;
  private float currentProcessTime;
  private float totalProcessTime;
  private float timeToProcess;
  public RawImage processBar;
  public static CauldronUI Instance;
  public InventoryCell[] synchedCells;
  private float closedTime;
  private float closedProgress;

  private void Awake()
  {
    CauldronUI.Instance = this;
    this.gameObject.SetActive(false);
  }

  public void CopyChest(Chest c)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    InventoryItem[] cells = OtherInput.Instance.currentChest.cells;
    for (int index = 0; index < cells.Length; ++index)
    {
      if (index < this.synchedCells.Length)
      {
        if (c.locked[index])
          this.synchedCells[index].enabled = false;
        else
          this.synchedCells[index].enabled = true;
        this.synchedCells[index].currentItem = !((Object) cells[index] != (Object) null) ? (InventoryItem) null : Object.Instantiate<InventoryItem>(cells[index]);
        this.synchedCells[index].UpdateCell();
      }
    }
    this.processBar.transform.localScale = new Vector3(((CauldronSync) OtherInput.Instance.currentChest).ProgressRatio(), 1f, 1f);
  }

  public override void UpdateCraftables()
  {
  }

  private void OnDisable()
  {
  }

  private void OnEnable()
  {
  }

  private void Update()
  {
  }

  private void StopProcessing()
  {
  }

  public void ProcessItem()
  {
  }

  private void UseMaterial(InventoryCell cell)
  {
  }

  private void UseFuel(InventoryCell cell)
  {
  }

  private void AddMaterial(InventoryCell cell, InventoryItem processedItem)
  {
  }

  public InventoryItem CanProcess() => (InventoryItem) null;

  public InventoryItem FindItemByIngredients(InventoryCell[] iCells) => (InventoryItem) null;

  private bool NoIngredients() => false;
}
