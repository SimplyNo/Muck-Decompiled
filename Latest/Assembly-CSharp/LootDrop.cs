// Decompiled with JetBrains decompiler
// Type: LootDrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LootDrop : ScriptableObject
{
  [Header("Loot Table")]
  public LootDrop.LootItems[] loot;
  public bool dropOne;

  public int id { get; set; }

  public List<InventoryItem> GetLoot()
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    foreach (LootDrop.LootItems lootItems in this.loot)
    {
      if ((double) Random.Range(0.0f, 1f) < (double) lootItems.dropChance)
      {
        int amount = Random.Range(lootItems.amountMin, lootItems.amountMax + 1);
        InventoryItem instance = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
        instance.Copy(lootItems.item, amount);
        inventoryItemList.Add(instance);
      }
    }
    return inventoryItemList;
  }

  public List<InventoryItem> GetLoot(ConsistentRandom rand)
  {
    List<InventoryItem> inventoryItemList = new List<InventoryItem>();
    foreach (LootDrop.LootItems lootItems in this.loot)
    {
      if (rand.NextDouble() < (double) lootItems.dropChance)
      {
        int amount = Random.Range(lootItems.amountMin, lootItems.amountMax + 1);
        InventoryItem instance = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
        instance.Copy(lootItems.item, amount);
        inventoryItemList.Add(instance);
      }
    }
    return inventoryItemList;
  }

  public LootDrop() => base.\u002Ector();

  [Serializable]
  public class LootItems
  {
    public InventoryItem item;
    public int amountMin;
    public int amountMax;
    public float dropChance;
  }
}
