// Decompiled with JetBrains decompiler
// Type: ShipChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ShipChest : MonoBehaviour
{
  public InventoryItem[] spawnLoot;
  private ConsistentRandom rand;

  private void Start()
  {
    this.rand = new ConsistentRandom(GameManager.GetSeed());
    Chest componentInChildren = this.GetComponentInChildren<Chest>();
    List<InventoryItem> items = new List<InventoryItem>();
    foreach (InventoryItem inventoryItem in this.spawnLoot)
      items.Add(inventoryItem);
    componentInChildren.InitChest(items, this.rand);
  }
}
