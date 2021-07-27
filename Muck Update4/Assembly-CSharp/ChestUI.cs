// Decompiled with JetBrains decompiler
// Type: ChestUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ChestUI : InventoryExtensions
{
  public InventoryCell[] cells;

  private void Awake()
  {
  }

  public void CopyChest(Chest c, bool addMap = false)
  {
    InventoryItem[] cells = c.cells;
    if (addMap)
      cells = this.AddMapToCells(cells);
    Debug.Log((object) ("Checking loot, cells: " + (object) this.cells.Length));
    for (int index = 0; index < this.cells.Length; ++index)
    {
      if (c.locked[index])
        this.cells[index].gameObject.SetActive(false);
      else
        this.cells[index].gameObject.SetActive(true);
      this.cells[index].currentItem = !((Object) cells[index] != (Object) null) ? (InventoryItem) null : Object.Instantiate<InventoryItem>(cells[index]);
      this.cells[index].UpdateCell();
    }
  }

  private InventoryItem[] AddMapToCells(InventoryItem[] cells)
  {
    for (int index = 0; index < cells.Length; ++index)
    {
      if ((Object) cells[index] == (Object) null)
      {
        cells[index] = Boat.Instance.mapItem;
        break;
      }
    }
    return cells;
  }

  public override void UpdateCraftables()
  {
  }
}
