// Decompiled with JetBrains decompiler
// Type: ChestUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
        ((Component) this.cells[index]).get_gameObject().SetActive(false);
      else
        ((Component) this.cells[index]).get_gameObject().SetActive(true);
      this.cells[index].currentItem = !Object.op_Inequality((Object) cells[index], (Object) null) ? (InventoryItem) null : (InventoryItem) Object.Instantiate<InventoryItem>((M0) cells[index]);
      this.cells[index].UpdateCell();
    }
  }

  public override void UpdateCraftables()
  {
  }
}
