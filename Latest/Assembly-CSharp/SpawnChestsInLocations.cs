// Decompiled with JetBrains decompiler
// Type: SpawnChestsInLocations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChestsInLocations : MonoBehaviour
{
  public Transform[] positions;
  public SpawnChestsInLocations.WeightedTables[] lootTables;
  public GameObject chest;
  private float totalWeight;

  public void SetChests(ConsistentRandom rand)
  {
    foreach (SpawnChestsInLocations.WeightedTables lootTable in this.lootTables)
      this.totalWeight += lootTable.weight;
    int num = rand.Next(0, this.positions.Length) + 1;
    List<int> intList = new List<int>();
    for (int index = 0; index < this.positions.Length; ++index)
      intList.Add(index);
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = rand.Next(0, intList.Count);
      rand.Next(0, this.lootTables.Length);
      int index3 = intList[index2];
      intList.Remove(index3);
      Vector3 position = this.positions[index3].get_position();
      Quaternion rotation = this.positions[index3].get_rotation();
      List<InventoryItem> loot = this.FindLootTable(this.lootTables, this.totalWeight, rand).GetLoot(rand);
      Chest componentInChildren = (Chest) ((GameObject) Object.Instantiate<GameObject>((M0) this.chest, position, rotation)).GetComponentInChildren<Chest>();
      int nextId = ChestManager.Instance.GetNextId();
      ChestManager.Instance.AddChest(componentInChildren, nextId);
      componentInChildren.InitChest(loot, rand);
    }
  }

  public LootDrop FindLootTable(
    SpawnChestsInLocations.WeightedTables[] structurePrefabs,
    float totalWeight,
    ConsistentRandom rand)
  {
    float num1 = (float) rand.NextDouble();
    float num2 = 0.0f;
    for (int index = 0; index < structurePrefabs.Length; ++index)
    {
      num2 += structurePrefabs[index].weight;
      if ((double) num1 < (double) num2 / (double) totalWeight)
        return structurePrefabs[index].table;
    }
    MonoBehaviour.print((object) "couldnt find, just returning 0");
    return structurePrefabs[0].table;
  }

  public SpawnChestsInLocations() => base.\u002Ector();

  [Serializable]
  public class WeightedTables
  {
    public LootDrop table;
    public float weight;
  }
}
