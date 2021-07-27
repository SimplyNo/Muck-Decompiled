// Decompiled with JetBrains decompiler
// Type: SpawnPowerupsInLocations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerupsInLocations : MonoBehaviour
{
  public Transform[] positions;
  public StructureSpawner.WeightedSpawn[] powerupChests;

  public void SetChests(ConsistentRandom rand)
  {
    int num = rand.Next(0, this.positions.Length) + 1;
    float totalWeight = 0.0f;
    foreach (StructureSpawner.WeightedSpawn powerupChest in this.powerupChests)
      totalWeight += powerupChest.weight;
    List<int> intList = new List<int>();
    for (int index = 0; index < this.positions.Length; ++index)
      intList.Add(index);
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = rand.Next(0, intList.Count);
      int index3 = intList[index2];
      intList.Remove(index3);
      Vector3 position = this.positions[index3].position;
      Quaternion rotation = this.positions[index3].rotation;
      GameObject o = Object.Instantiate<GameObject>(this.FindObjectToSpawn(this.powerupChests, totalWeight, rand), position, rotation);
      int nextId = ResourceManager.Instance.GetNextId();
      LootContainerInteract componentInChildren = o.GetComponentInChildren<LootContainerInteract>();
      componentInChildren.price = 0;
      componentInChildren.SetId(nextId);
      ResourceManager.Instance.AddObject(nextId, o);
    }
  }

  public GameObject FindObjectToSpawn(
    StructureSpawner.WeightedSpawn[] prefabs,
    float totalWeight,
    ConsistentRandom randomGen)
  {
    float num1 = (float) randomGen.NextDouble();
    float num2 = 0.0f;
    for (int index = 0; index < prefabs.Length; ++index)
    {
      num2 += prefabs[index].weight;
      if ((double) num1 < (double) num2 / (double) totalWeight)
        return prefabs[index].prefab;
    }
    return prefabs[0].prefab;
  }
}
