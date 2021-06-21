// Decompiled with JetBrains decompiler
// Type: LootDrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
      if ((double) UnityEngine.Random.Range(0.0f, 1f) < (double) lootItems.dropChance)
      {
        int amount = UnityEngine.Random.Range(lootItems.amountMin, lootItems.amountMax + 1);
        InventoryItem instance = ScriptableObject.CreateInstance<InventoryItem>();
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
        int amount = UnityEngine.Random.Range(lootItems.amountMin, lootItems.amountMax + 1);
        InventoryItem instance = ScriptableObject.CreateInstance<InventoryItem>();
        instance.Copy(lootItems.item, amount);
        inventoryItemList.Add(instance);
      }
    }
    return inventoryItemList;
  }

  [Serializable]
  public class LootItems
  {
    public InventoryItem item;
    public int amountMin;
    public int amountMax;
    public float dropChance;
  }
}
