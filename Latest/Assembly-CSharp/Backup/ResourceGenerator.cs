// Decompiled with JetBrains decompiler
// Type: ResourceGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
  public DrawChunks drawChunks;
  public StructureSpawner.WeightedSpawn[] resourcePrefabs;
  private float totalWeight;
  public MapGenerator mapGenerator;
  private int density = 1;
  public float spawnThreshold = 0.45f;
  public AnimationCurve noiseDistribution;
  public AnimationCurve heightDistribution;
  public List<GameObject>[] resources;
  private ConsistentRandom randomGen;
  public NoiseData noiseData;
  [Header("Variety")]
  public Vector3 randomRotation;
  public Vector2 randomScale;
  public int randPos = 12;
  private int totalResources;
  public int forceSeedOffset = -1;
  public float minSpawnHeight = 0.4f;
  public float maxSpawnHeight = 0.35f;
  public int width;
  public int height;
  public bool useFalloffMap = true;
  public int minSpawnAmount = 10;
  public ResourceGenerator.SpawnType type;

  public float worldScale { get; set; } = 12f;

  private void Start()
  {
    this.randomGen = new ConsistentRandom(GameManager.GetSeed());
    foreach (StructureSpawner.WeightedSpawn resourcePrefab in this.resourcePrefabs)
      this.totalWeight += resourcePrefab.weight;
    this.GenerateForest();
    if (!(bool) (Object) ResourceManager.Instance)
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
    int num4 = 0;
    while (num3 < this.minSpawnAmount && num4 < 100)
    {
      ++num4;
      for (int y = 0; y < this.height; y += density)
      {
        for (int x = 0; x < this.width; x += density)
        {
          if ((double) perlinNoiseMap1[x, y] >= (double) this.minSpawnHeight && (double) perlinNoiseMap1[x, y] <= (double) this.maxSpawnHeight)
          {
            float num5 = perlinNoiseMap2[x, y];
            if ((double) num5 >= (double) this.spawnThreshold)
            {
              float num6 = (float) (((double) num5 - (double) this.spawnThreshold) / (1.0 - (double) this.spawnThreshold));
              double num7 = (double) this.noiseDistribution.Evaluate((float) this.randomGen.NextDouble());
              ++num2;
              double num8 = 1.0 - (double) num6;
              if (num7 > num8)
              {
                Vector3 vector3 = new Vector3(this.topLeftX + (float) x, 100f, this.topLeftZ - (float) y) * this.worldScale + new Vector3((float) this.randomGen.Next(-this.randPos, this.randPos), 0.0f, (float) this.randomGen.Next(-this.randPos, this.randPos));
                RaycastHit hitInfo;
                if (Physics.Raycast(vector3, Vector3.down, out hitInfo, 1200f))
                {
                  vector3.y = hitInfo.point.y;
                  ++num3;
                  int index = this.drawChunks.FindChunk(x, y);
                  if (index >= this.drawChunks.nChunks)
                    index = this.drawChunks.nChunks - 1;
                  this.resources[index].Add(this.SpawnTree(vector3));
                }
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
    Quaternion rotation = Quaternion.Euler((float) ((this.randomGen.NextDouble() - 0.5) * (double) this.randomRotation.x * 2.0), (float) ((this.randomGen.NextDouble() - 0.5) * (double) this.randomRotation.y * 2.0), (float) ((this.randomGen.NextDouble() - 0.5) * (double) this.randomRotation.z * 2.0));
    Vector3 scale = Vector3.one * (this.randomScale.x + (float) this.randomGen.NextDouble() * (this.randomScale.y - this.randomScale.x));
    GameObject gameObject = Object.Instantiate<GameObject>(this.FindObjectToSpawn(this.resourcePrefabs, this.totalWeight), pos, rotation);
    gameObject.transform.localScale = scale;
    gameObject.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
    HitableResource component = gameObject.GetComponent<HitableResource>();
    if ((bool) (Object) component)
    {
      component.SetDefaultScale(scale);
      component.PopIn();
    }
    gameObject.SetActive(false);
    return gameObject;
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

  public enum SpawnType
  {
    Static,
    Pickup,
  }
}
