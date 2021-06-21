// Decompiled with JetBrains decompiler
// Type: FurnaceUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    ((Component) this).get_gameObject().SetActive(false);
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
    if (!((Component) this).get_gameObject().get_activeInHierarchy())
      return;
    InventoryItem[] cells = OtherInput.Instance.currentChest.cells;
    for (int index = 0; index < cells.Length; ++index)
    {
      if (c.locked[index])
        ((Behaviour) this.synchedCells[index]).set_enabled(false);
      else
        ((Behaviour) this.synchedCells[index]).set_enabled(true);
      if (index < this.synchedCells.Length)
      {
        this.synchedCells[index].currentItem = !Object.op_Inequality((Object) cells[index], (Object) null) ? (InventoryItem) null : (InventoryItem) Object.Instantiate<InventoryItem>((M0) cells[index]);
        this.synchedCells[index].UpdateCell();
      }
    }
    ((Component) this.processBar).get_transform().set_localScale(new Vector3(((FurnaceSync) OtherInput.Instance.currentChest).ProgressRatio(), 1f, 1f));
  }

  private void AddMaterial(InventoryCell cell, int processedItemId)
  {
  }

  public bool CanProcess() => false;
}
