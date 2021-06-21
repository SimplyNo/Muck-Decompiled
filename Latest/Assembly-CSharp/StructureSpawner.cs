// Decompiled with JetBrains decompiler
// Type: StructureSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{
  public StructureSpawner.WeightedSpawn[] structurePrefabs;
  private int mapChunkSize;
  private float worldEdgeBuffer;
  public int nShrines;
  protected ConsistentRandom randomGen;
  public LayerMask whatIsTerrain;
  private List<GameObject> structures;
  public bool dontAddToResourceManager;
  private Vector3[] shrines;
  private float totalWeight;

  public float worldScale { get; set; }

  private void Start()
  {
    this.structures = new List<GameObject>();
    this.randomGen = new ConsistentRandom(GameManager.GetSeed() + ResourceManager.GetNextGenOffset());
    this.shrines = new Vector3[this.nShrines];
    this.mapChunkSize = MapGenerator.mapChunkSize;
    this.worldScale *= this.worldEdgeBuffer;
    foreach (StructureSpawner.WeightedSpawn structurePrefab in this.structurePrefabs)
      this.totalWeight += structurePrefab.weight;
    int num = 0;
    for (int index = 0; index < this.nShrines; ++index)
    {
      Vector3 vector3 = Vector3.op_Multiply(new Vector3((float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0), 0.0f, (float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0)), this.worldScale);
      vector3.y = (__Null) 200.0;
      Debug.DrawLine(vector3, Vector3.op_Addition(vector3, Vector3.op_Multiply(Vector3.get_down(), 500f)), Color.get_cyan(), 50f);
      RaycastHit hit;
      if (Physics.Raycast(vector3, Vector3.get_down(), ref hit, 500f, LayerMask.op_Implicit(this.whatIsTerrain)) && WorldUtility.WorldHeightToBiome((float) ((RaycastHit) ref hit).get_point().y) == TextureData.TerrainType.Grass)
      {
        this.shrines[index] = ((RaycastHit) ref hit).get_point();
        ++num;
        GameObject objectToSpawn = this.FindObjectToSpawn(this.structurePrefabs, this.totalWeight);
        GameObject newStructure = (GameObject) Object.Instantiate<GameObject>((M0) objectToSpawn, ((RaycastHit) ref hit).get_point(), objectToSpawn.get_transform().get_rotation());
        if (!this.dontAddToResourceManager)
          ((SharedObject) newStructure.GetComponentInChildren<SharedObject>()).SetId(ResourceManager.Instance.GetNextId());
        this.structures.Add(newStructure);
        this.Process(newStructure, hit);
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

  public GameObject FindObjectToSpawn(
    StructureSpawner.WeightedSpawn[] structurePrefabs,
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

  public StructureSpawner() => base.\u002Ector();

  [Serializable]
  public class WeightedSpawn
  {
    public GameObject prefab;
    public float weight;
  }
}
