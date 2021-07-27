// Decompiled with JetBrains decompiler
// Type: FurnaceUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class FurnaceUI : InventoryExtensions
{
  public InventoryCell metalCell;
  public InventoryCell fuelCell;
  public InventoryCell resultCell;
  public InventoryItem.ProcessType processType;
  private bool processing;
  private float currentProcessTime;
  private float totalProcessTime;
  private float timeToProcess;
  public RawImage processBar;
  private InventoryItem currentFuel;
  private InventoryItem currentMetal;
  public static FurnaceUI Instance;
  private float closedTime;
  private float closedProgress;
  public InventoryCell[] synchedCells;

  private void Awake()
  {
    FurnaceUI.Instance = this;
    this.gameObject.SetActive(false);
  }

  public override void UpdateCraftables()
  {
  }

  private void StartProcessing()
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

  public void CopyChest(Chest c)
  {
    if (!this.gameObject.activeInHierarchy)
      return;
    InventoryItem[] cells = OtherInput.Instance.currentChest.cells;
    for (int index = 0; index < cells.Length; ++index)
    {
      if (c.locked[index])
        this.synchedCells[index].enabled = false;
      else
        this.synchedCells[index].enabled = true;
      if (index < this.synchedCells.Length)
      {
        this.synchedCells[index].currentItem = !((Object) cells[index] != (Object) null) ? (InventoryItem) null : Object.Instantiate<InventoryItem>(cells[index]);
        this.synchedCells[index].UpdateCell();
      }
    }
    this.processBar.transform.localScale = new Vector3(((FurnaceSync) OtherInput.Instance.currentChest).ProgressRatio(), 1f, 1f);
  }

  private void AddMaterial(InventoryCell cell, int processedItemId)
  {
  }

  public bool CanProcess() => false;
}
