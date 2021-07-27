// Decompiled with JetBrains decompiler
// Type: BillySpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class BillySpawner : MonoBehaviour
{
  public BillySpawner.WeightedSpawn[] structurePrefabs;
  private int mapChunkSize;
  private float worldEdgeBuffer = 0.6f;
  public int maxCaves = 50;
  public int minCaves = 3;
  protected ConsistentRandom randomGen;
  public LayerMask whatIsTerrain;
  private List<GameObject> structures;
  public bool dontAddToResourceManager;
  private Vector3[] shrines;
  private float totalWeight;

  public float worldScale { get; set; } = 12f;

  private void Start()
  {
    this.structures = new List<GameObject>();
    this.randomGen = new ConsistentRandom(GameManager.GetSeed() + ResourceManager.GetNextGenOffset());
    this.shrines = new Vector3[this.maxCaves];
    this.mapChunkSize = MapGenerator.mapChunkSize;
    this.worldScale *= this.worldEdgeBuffer;
    foreach (BillySpawner.WeightedSpawn structurePrefab in this.structurePrefabs)
      this.totalWeight += structurePrefab.weight;
    int index = 0;
    int num = 0;
    while (index < this.maxCaves)
    {
      ++num;
      Vector3 vector3 = new Vector3((float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0), 0.0f, (float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0)) * this.worldScale;
      vector3.y = 200f;
      Debug.DrawLine(vector3, vector3 + Vector3.down * 500f, Color.cyan, 50f);
      RaycastHit hitInfo;
      if (Physics.Raycast(vector3, Vector3.down, out hitInfo, 500f, (int) this.whatIsTerrain))
      {
        if (WorldUtility.WorldHeightToBiome(hitInfo.point.y) == TextureData.TerrainType.Grass && (double) Mathf.Abs(Vector3.Angle(Vector3.up, hitInfo.normal)) <= 15.0)
        {
          this.shrines[index] = hitInfo.point;
          ++index;
          GameObject objectToSpawn = this.FindObjectToSpawn(this.structurePrefabs, this.totalWeight);
          GameObject newStructure = UnityEngine.Object.Instantiate<GameObject>(objectToSpawn, hitInfo.point, objectToSpawn.transform.rotation);
          if (!this.dontAddToResourceManager)
            newStructure.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
          this.structures.Add(newStructure);
          this.Process(newStructure, hitInfo);
        }
        else
          continue;
      }
      if (num > this.maxCaves * 2 && index >= this.minCaves || num > this.maxCaves * 10)
        break;
    }
    if (this.dontAddToResourceManager)
      return;
    ResourceManager.Instance.AddResources(this.structures);
  }

  public virtual void Process(GameObject newStructure, RaycastHit hit) => newStructure.transform.rotation = Quaternion.LookRotation(hit.normal);

  private void OnDrawGizmos()
  {
  }

  public GameObject FindObjectToSpawn(
    BillySpawner.WeightedSpawn[] structurePrefabs,
    float totalWeight)
  {
    float num1 = (float) this.randomGen.NextDouble();
    float num2 = 0.0f;
    for (int index = 0; index < structurePrefabs.Length; ++index)
    {
      num2 += structurePrefabs[index].weight;
      if ((double) num1 < (double) num2 / (double) totalWeight)
        return structurePrefabs[index].prefab;
    }
    return structurePrefabs[0].prefab;
  }

  [Serializable]
  public class WeightedSpawn
  {
    public GameObject prefab;
    public float weight;
  }
}
