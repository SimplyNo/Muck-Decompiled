// Decompiled with JetBrains decompiler
// Type: SpawnPowerupsInLocations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
      Vector3 position = this.positions[index3].get_position();
      Quaternion rotation = this.positions[index3].get_rotation();
      GameObject o = (GameObject) Object.Instantiate<GameObject>((M0) this.FindObjectToSpawn(this.powerupChests, totalWeight, rand), position, rotation);
      int nextId = ResourceManager.Instance.GetNextId();
      M0 componentInChildren = o.GetComponentInChildren<LootContainerInteract>();
      ((LootContainerInteract) componentInChildren).price = 0;
      ((LootContainerInteract) componentInChildren).SetId(nextId);
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

  public SpawnPowerupsInLocations() => base.\u002Ector();
}
