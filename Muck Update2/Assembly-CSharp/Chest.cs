// Decompiled with JetBrains decompiler
// Type: Chest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
  public InventoryItem[] cells;
  public int chestSize;
  public bool inUse;
  public bool[] locked;
  private Animator animator;

  public int id { get; set; }

  private void Start()
  {
    this.locked = new bool[this.chestSize];
    this.animator = (Animator) ((Component) this).GetComponent<Animator>();
    if (this.cells != null && this.cells.Length != 0)
      return;
    this.cells = new InventoryItem[this.chestSize];
  }

  public void Use(bool b)
  {
    if (Object.op_Implicit((Object) this.animator))
      this.animator.SetBool(nameof (Use), b);
    this.inUse = b;
  }

  public bool IsUsed() => this.inUse;

  public virtual void UpdateCraftables()
  {
  }

  public void InitChest(List<InventoryItem> items, ConsistentRandom rand)
  {
    this.cells = new InventoryItem[this.chestSize];
    List<int> intList = new List<int>();
    for (int index = 0; index < this.chestSize; ++index)
      intList.Add(index);
    foreach (InventoryItem inventoryItem in items)
    {
      int index = rand.Next(0, intList.Count);
      intList.Remove(index);
      this.cells[index] = inventoryItem;
    }
  }

  public Chest() => base.\u002Ector();
}
