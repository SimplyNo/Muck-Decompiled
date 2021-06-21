// Decompiled with JetBrains decompiler
// Type: ResourceGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
  public DrawChunks drawChunks;
  public StructureSpawner.WeightedSpawn[] resourcePrefabs;
  private float totalWeight;
  public MapGenerator mapGenerator;
  private int density;
  public float spawnThreshold;
  public AnimationCurve noiseDistribution;
  public AnimationCurve heightDistribution;
  public List<GameObject>[] resources;
  private ConsistentRandom randomGen;
  public NoiseData noiseData;
  [Header("Variety")]
  public Vector3 randomRotation;
  public Vector2 randomScale;
  public int randPos;
  private int totalResources;
  public int forceSeedOffset;
  public float minSpawnHeight;
  public float maxSpawnHeight;
  public int width;
  public int height;
  public bool useFalloffMap;
  public ResourceGenerator.SpawnType type;

  public float worldScale { get; set; }

  private void Start()
  {
    this.randomGen = new ConsistentRandom(GameManager.GetSeed());
    foreach (StructureSpawner.WeightedSpawn resourcePrefab in this.resourcePrefabs)
      this.totalWeight += resourcePrefab.weight;
    this.GenerateForest();
    if (!Object.op_Implicit((Object) ResourceManager.Instance))
      return;
    ResourceManager.Instance.AddResources(this.resources);
  }

  public float topLeftX { get; private set; }

  public float topLeftZ { get; private set; }

  private void GenerateForest()
  {
    int num1 = this.forceSeedOffset == -1 ? ResourceManager.GetNextGenOffset() : this.forceSeedOffset;
    float[,] perlinNoiseMap1 = this.mapGenerator.GeneratePerlinNoiseMap(GameManager.GetSeed());
    float[,] perlinNoiseMap2 = this.mapGenerator.GeneratePerlinNoiseMap(this.noiseData, GameManager.GetSeed() + num1, this.useFalloffMap);
    this.width = perlinNoiseMap1.GetLength(0);
    this.height = perlinNoiseMap1.GetLength(1);
    this.topLeftX = (float) (this.width - 1) / -2f;
    this.topLeftZ = (float) (this.height - 1) / 2f;
    this.resources = new List<GameObject>[this.drawChunks.nChunks];
    for (int index = 0; index < this.resources.Length; ++index)
      this.resources[index] = new List<GameObject>();
    int density = this.density;
    int num2 = 0;
    int num3 = 0;
    for (int y = 0; y < this.height; y += density)
    {
      for (int x = 0; x < this.width; x += density)
      {
        if ((double) perlinNoiseMap1[x, y] >= (double) this.minSpawnHeight && (double) perlinNoiseMap1[x, y] <= (double) this.maxSpawnHeight)
        {
          float num4 = perlinNoiseMap2[x, y];
          if ((double) num4 >= (double) this.spawnThreshold)
          {
            float num5 = (float) (((double) num4 - (double) this.spawnThreshold) / (1.0 - (double) this.spawnThreshold));
            double num6 = (double) this.noiseDistribution.Evaluate((float) this.randomGen.NextDouble());
            ++num2;
            double num7 = 1.0 - (double) num5;
            if (num6 > num7)
            {
              Vector3 pos = Vector3.op_Addition(Vector3.op_Multiply(new Vector3(this.topLeftX + (float) x, 100f, this.topLeftZ - (float) y), this.worldScale), new Vector3((float) this.randomGen.Next(-this.randPos, this.randPos), 0.0f, (float) this.randomGen.Next(-this.randPos, this.randPos)));
              RaycastHit raycastHit;
              if (Physics.Raycast(pos, Vector3.get_down(), ref raycastHit, 1200f))
              {
                pos.y = ((RaycastHit) ref raycastHit).get_point().y;
                ++num3;
                int index = this.drawChunks.FindChunk(x, y);
                if (index >= this.drawChunks.nChunks)
                  index = this.drawChunks.nChunks - 1;
                this.resources[index].Add(this.SpawnTree(pos));
              }
            }
          }
        }
      }
    }
    this.totalResources = num3;
    this.drawChunks.InitChunks(this.resources);
    this.drawChunks.totalTrees = this.totalResources;
  }

  private GameObject SpawnTree(Vector3 pos)
  {
    Quaternion quaternion = Quaternion.Euler((float) ((this.randomGen.NextDouble() - 0.5) * this.randomRotation.x * 2.0), (float) ((this.randomGen.NextDouble() - 0.5) * this.randomRotation.y * 2.0), (float) ((this.randomGen.NextDouble() - 0.5) * this.randomRotation.z * 2.0));
    float num = (float) this.randomGen.NextDouble() * (float) (this.randomScale.y - this.randomScale.x);
    Vector3 scale = Vector3.op_Multiply(Vector3.get_one(), (float) this.randomScale.x + num);
    M0 m0 = Object.Instantiate<GameObject>((M0) this.FindObjectToSpawn(this.resourcePrefabs, this.totalWeight), pos, quaternion);
    ((GameObject) m0).get_transform().set_localScale(scale);
    ((SharedObject) ((GameObject) m0).GetComponentInChildren<SharedObject>()).SetId(ResourceManager.Instance.GetNextId());
    HitableResource component = (HitableResource) ((GameObject) m0).GetComponent<HitableResource>();
    if (Object.op_Implicit((Object) component))
    {
      component.SetDefaultScale(scale);
      component.PopIn();
    }
    ((GameObject) m0).SetActive(false);
    return (GameObject) m0;
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

  public ResourceGenerator() => base.\u002Ector();

  public enum SpawnType
  {
    Static,
    Pickup,
  }
}
