// Decompiled with JetBrains decompiler
// Type: SpawnZoneGenerator`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZoneGenerator<T> : MonoBehaviour
{
  public GameObject spawnZone;
  private int mapChunkSize;
  private float worldEdgeBuffer = 0.6f;
  public int nZones = 50;
  protected ConsistentRandom randomGen;
  public LayerMask whatIsTerrain;
  protected List<SpawnZone> zones;
  public int seedOffset;
  private Vector3[] shrines;
  protected float totalWeight;
  public T[] entities;
  public float[] weights;

  public float worldScale { get; set; } = 12f;

  private void Start()
  {
    this.zones = new List<SpawnZone>();
    this.randomGen = new ConsistentRandom(GameManager.GetSeed() + this.seedOffset);
    this.shrines = new Vector3[this.nZones];
    this.mapChunkSize = MapGenerator.mapChunkSize;
    this.worldScale *= this.worldEdgeBuffer;
    foreach (float weight in this.weights)
      this.totalWeight += weight;
    int num = 0;
    for (int index = 0; index < this.nZones; ++index)
    {
      Vector3 origin = new Vector3((float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0), 0.0f, (float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0)) * this.worldScale;
      origin.y = 200f;
      RaycastHit hitInfo;
      if (Physics.Raycast(origin, Vector3.down, out hitInfo, 500f, (int) this.whatIsTerrain) && WorldUtility.WorldHeightToBiome(hitInfo.point.y) == TextureData.TerrainType.Grass)
      {
        this.shrines[index] = hitInfo.point;
        ++num;
        GameObject spawnZone = this.spawnZone;
        SpawnZone component = Object.Instantiate<GameObject>(spawnZone, hitInfo.point, spawnZone.transform.rotation).GetComponent<SpawnZone>();
        component.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
        component.id = MobZoneManager.Instance.GetNextId();
        this.zones.Add(this.ProcessZone(component));
      }
    }
    this.AddEntitiesToZone();
    this.nZones = this.zones.Count;
  }

  public abstract void AddEntitiesToZone();

  public abstract SpawnZone ProcessZone(SpawnZone zone);

  private void OnDrawGizmos()
  {
  }

  public T FindObjectToSpawn(T[] entityTypes, float totalWeight)
  {
    float num1 = (float) this.randomGen.NextDouble();
    float num2 = 0.0f;
    for (int index = 0; index < entityTypes.Length; ++index)
    {
      num2 += this.weights[index];
      if ((double) num1 < (double) num2 / (double) totalWeight)
        return entityTypes[index];
    }
    return entityTypes[0];
  }
}
