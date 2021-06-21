// Decompiled with JetBrains decompiler
// Type: SpawnZoneGenerator`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnZoneGenerator<T> : MonoBehaviour
{
  public GameObject spawnZone;
  private int mapChunkSize;
  private float worldEdgeBuffer;
  public int nZones;
  protected ConsistentRandom randomGen;
  public LayerMask whatIsTerrain;
  protected List<SpawnZone> zones;
  public int seedOffset;
  private Vector3[] shrines;
  protected float totalWeight;
  public T[] entities;
  public float[] weights;

  public float worldScale { get; set; }

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
      Vector3 vector3 = Vector3.op_Multiply(new Vector3((float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0), 0.0f, (float) ((this.randomGen.NextDouble() * 2.0 - 1.0) * (double) this.mapChunkSize / 2.0)), this.worldScale);
      vector3.y = (__Null) 200.0;
      RaycastHit raycastHit;
      if (Physics.Raycast(vector3, Vector3.get_down(), ref raycastHit, 500f, LayerMask.op_Implicit(this.whatIsTerrain)) && WorldUtility.WorldHeightToBiome((float) ((RaycastHit) ref raycastHit).get_point().y) == TextureData.TerrainType.Grass)
      {
        this.shrines[index] = ((RaycastHit) ref raycastHit).get_point();
        ++num;
        GameObject spawnZone = this.spawnZone;
        SpawnZone component = (SpawnZone) ((GameObject) Object.Instantiate<GameObject>((M0) spawnZone, ((RaycastHit) ref raycastHit).get_point(), spawnZone.get_transform().get_rotation())).GetComponent<SpawnZone>();
        ((SharedObject) ((Component) component).GetComponentInChildren<SharedObject>()).SetId(ResourceManager.Instance.GetNextId());
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

  protected SpawnZoneGenerator() => base.\u002Ector();
}
