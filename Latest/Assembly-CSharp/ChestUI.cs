// Decompiled with JetBrains decompiler
// Type: ChestUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ChestUI : InventoryExtensions
{
  public InventoryCell[] cells;

  private void Awake()
  {
  }

  public void CopyChest(Chest c)
  {
    InventoryItem[] cells = c.cells;
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

  public override void UpdateCraftables()
  {
  }
}
