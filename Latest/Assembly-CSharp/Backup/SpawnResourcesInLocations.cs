// Decompiled with JetBrains decompiler
// Type: SpawnResourcesInLocations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnResourcesInLocations : MonoBehaviour
{
  public Transform[] positions;
  public SpawnResourcesInLocations.WeightedTables[] lootTables;
  public int minResources;
  public bool randomRotation = true;
  private float totalWeight;

  public void SetResources(ConsistentRandom rand)
  {
    foreach (SpawnResourcesInLocations.WeightedTables lootTable in this.lootTables)
      this.totalWeight += lootTable.weight;
    int num = rand.Next(this.minResources, this.positions.Length);
    List<int> intList = new List<int>();
    for (int index = 0; index < this.positions.Length; ++index)
      intList.Add(index);
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = rand.Next(0, intList.Count);
      rand.Next(0, this.lootTables.Length);
      int index3 = intList[index2];
      intList.Remove(index3);
      Vector3 position = this.positions[index3].position;
      Quaternion rotation = Quaternion.Euler(!this.randomRotation ? this.positions[index3].rotation.eulerAngles : new Vector3((float) rand.NextDouble() * 360f, (float) rand.NextDouble() * 360f, (float) rand.NextDouble() * 360f));
      GameObject o = UnityEngine.Object.Instantiate<GameObject>(this.FindResource(this.lootTables, this.totalWeight, rand), position, rotation);
      int nextId = ResourceManager.Instance.GetNextId();
      o.GetComponent<Hitable>().SetId(nextId);
      ResourceManager.Instance.AddObject(nextId, o);
    }
  }

  public GameObject FindResource(
    SpawnResourcesInLocations.WeightedTables[] structurePrefabs,
    float totalWeight,
    ConsistentRandom rand)
  {
    float num1 = (float) rand.NextDouble();
    float num2 = 0.0f;
    for (int index = 0; index < structurePrefabs.Length; ++index)
    {
      num2 += structurePrefabs[index].weight;
      if ((double) num1 < (double) num2 / (double) totalWeight)
        return structurePrefabs[index].resource;
    }
    return structurePrefabs[0].resource;
  }

  [Serializable]
  public class WeightedTables
  {
    public GameObject resource;
    public float weight;
  }
}
