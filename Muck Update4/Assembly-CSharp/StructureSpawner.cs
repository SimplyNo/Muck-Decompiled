// Decompiled with JetBrains decompiler
// Type: StructureSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
  public StructureSpawner.WeightedSpawn[] structurePrefabs;
  private int mapChunkSize;
  private float worldEdgeBuffer = 0.6f;
  public int nShrines = 50;
  protected ConsistentRandom randomGen;
  public LayerMask whatIsTerrain;
  private List<GameObject> structures;
  public bool dontAddToResourceManager;
  private Vector3[] shrines;

  public float worldScale { get; set; } = 12f;

  public void CalculateWeight()
  {
    this.totalWeight = 0.0f;
    foreach (StructureSpawner.WeightedSpawn structurePrefab in this.structurePrefabs)
      this.totalWeight += structurePrefab.weight;
  }

  private void Start()
  {
    this.structures = new List<GameObject>();
    this.randomGen = new ConsistentRandom(GameManager.GetSeed() + ResourceManager.GetNextGenOffset());
    this.shrines = new Vector3[this.nShrines];
    this.mapChunkSize = MapGenerator.mapChunkSize;
    this.worldScale *= this.worldEdgeBuffer;
    this.CalculateWeight();
    int num = 0;
    for (int index = 0; index < this.nShrines; ++index)
    {
      Vector3 vector3 = new Vector3((float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0), 0.0f, (float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0)) * this.worldScale;
      vector3.y = 200f;
      Debug.DrawLine(vector3, vector3 + Vector3.down * 500f, Color.cyan, 50f);
      RaycastHit hitInfo;
      if (Physics.Raycast(vector3, Vector3.down, out hitInfo, 500f, (int) this.whatIsTerrain) && WorldUtility.WorldHeightToBiome(hitInfo.point.y) == TextureData.TerrainType.Grass)
      {
        this.shrines[index] = hitInfo.point;
        ++num;
        GameObject objectToSpawn = this.FindObjectToSpawn(this.structurePrefabs, this.totalWeight, this.randomGen);
        GameObject newStructure = UnityEngine.Object.Instantiate<GameObject>(objectToSpawn, hitInfo.point, objectToSpawn.transform.rotation);
        if (!this.dontAddToResourceManager)
          newStructure.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
        this.structures.Add(newStructure);
        this.Process(newStructure, hitInfo);
      }
    }
    if (!this.dontAddToResourceManager)
      ResourceManager.Instance.AddResources(this.structures);
    MonoBehaviour.print((object) ("spawned: " + (object) this.structures.Count));
  }

  public virtual void Process(GameObject newStructure, RaycastHit hit)
  {
  }

  private void OnDrawGizmos()
  {
  }

  public float totalWeight { get; set; }

  public GameObject FindObjectToSpawn(
    StructureSpawner.WeightedSpawn[] structurePrefabs,
    float totalWeight,
    ConsistentRandom rand)
  {
    float num1 = (float) rand.NextDouble();
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
