// Decompiled with JetBrains decompiler
// Type: GenerateCamp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GenerateCamp : MonoBehaviour
{
  public GameObject zonePrefab;
  public MobType mobType;
  private MobZone zone;
  private float campRadius = 80f;
  private int min = 6;
  private int max = 10;
  private bool rolesAssigned;
  private ConsistentRandom rand;
  public LayerMask whatIsGround;
  public GameObject chiefChest;
  public GameObject hut;
  public GameObject barrel;
  public GameObject log;
  public GameObject logPile;
  public GameObject rockPile;
  public GameObject fireplace;
  public StructureSpawner houseSpawner;
  public bool testing;

  private void Awake()
  {
  }

  private void GenerateZone(ConsistentRandom rand)
  {
    this.zone = this.gameObject.AddComponent<MobZone>();
    this.zone.mobType = this.mobType;
    this.zone.respawnTime = -1f;
    this.zone.roamDistance = this.campRadius;
    this.zone.renderDistance = this.campRadius * 3f;
    this.zone.entityCap = rand.Next(this.min, this.max);
    this.zone.whatIsGround = this.whatIsGround;
    int nextId = MobZoneManager.Instance.GetNextId();
    this.zone.SetId(nextId);
    MobZoneManager.Instance.AddZone((SpawnZone) this.zone, nextId);
  }

  public void MakeCamp(ConsistentRandom rand)
  {
    this.rand = rand;
    this.GenerateZone(rand);
    this.GenerateStructures(rand);
  }

  private void GenerateStructures(ConsistentRandom rand)
  {
    int amount1 = 1;
    int amount2 = rand.Next(2, 4);
    int amount3 = rand.Next(2, 3);
    int amount4 = rand.Next(2, 4);
    int amount5 = rand.Next(2, 7);
    int amount6 = rand.Next(2, 8);
    int amount7 = rand.Next(2, 5);
    int amount8 = rand.Next(2, 5);
    List<GameObject> gameObjectList1 = this.SpawnObjects(this.chiefChest, amount1, rand);
    foreach (GameObject gameObject in gameObjectList1)
      gameObject.GetComponentInChildren<ChiefChestInteract>().mobZoneId = this.zone.id;
    Debug.Log((object) string.Format("spawned {0} / {1}", (object) gameObjectList1.Count, (object) amount1));
    List<GameObject> gameObjectList2 = this.SpawnObjects(this.hut, amount2, rand);
    foreach (GameObject gameObject in gameObjectList2)
      gameObject.GetComponent<SpawnChestsInLocations>().SetChests(rand);
    Debug.Log((object) string.Format("spawned {0} / {1}", (object) gameObjectList2.Count, (object) amount2));
    Debug.Log((object) string.Format("spawned {0} / {1}", (object) this.SpawnObjects(this.fireplace, amount4, rand).Count, (object) amount4));
    Debug.Log((object) string.Format("spawned {0} / {1}", (object) this.SpawnObjects(this.barrel, amount5, rand).Count, (object) amount5));
    Debug.Log((object) string.Format("spawned {0} / {1}", (object) this.SpawnObjects(this.log, amount6, rand).Count, (object) amount6));
    Debug.Log((object) string.Format("spawned {0} / {1}", (object) this.SpawnObjects(this.logPile, amount7, rand).Count, (object) amount7));
    Debug.Log((object) string.Format("spawned {0} / {1}", (object) this.SpawnObjects(this.rockPile, amount8, rand).Count, (object) amount8));
    this.SpawnObjects(this.houseSpawner, amount3, rand);
  }

  private List<GameObject> SpawnObjects(
    GameObject obj,
    int amount,
    ConsistentRandom rand)
  {
    if ((Object) obj == (Object) null)
      return new List<GameObject>();
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < amount; ++index)
    {
      RaycastHit pos = this.FindPos(rand);
      if (!((Object) pos.collider == (Object) null))
      {
        GameObject o = Object.Instantiate<GameObject>(obj, pos.point, Quaternion.LookRotation(pos.normal));
        o.transform.Rotate(o.transform.right, 90f, Space.World);
        Hitable component = o.GetComponent<Hitable>();
        if ((bool) (Object) component)
        {
          int nextId = ResourceManager.Instance.GetNextId();
          component.SetId(nextId);
          ResourceManager.Instance.AddObject(nextId, o);
        }
        gameObjectList.Add(o);
      }
    }
    return gameObjectList;
  }

  private List<GameObject> SpawnObjects(
    StructureSpawner houses,
    int amount,
    ConsistentRandom rand)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    houses.CalculateWeight();
    for (int index = 0; index < amount; ++index)
    {
      GameObject objectToSpawn = houses.FindObjectToSpawn(houses.structurePrefabs, houses.totalWeight, rand);
      RaycastHit pos = this.FindPos(rand);
      if (!((Object) pos.collider == (Object) null))
      {
        GameObject o = Object.Instantiate<GameObject>(objectToSpawn, pos.point, Quaternion.LookRotation(pos.normal));
        int nextId1 = ResourceManager.Instance.GetNextId();
        o.GetComponent<Hitable>().SetId(nextId1);
        ResourceManager.Instance.AddObject(nextId1, o);
        SpawnChestsInLocations componentInChildren1 = o.GetComponentInChildren<SpawnChestsInLocations>();
        if ((bool) (Object) componentInChildren1)
          componentInChildren1.SetChests(rand);
        SpawnPowerupsInLocations componentInChildren2 = o.GetComponentInChildren<SpawnPowerupsInLocations>();
        if ((bool) (Object) componentInChildren2)
          componentInChildren2.SetChests(rand);
        Hitable component = o.GetComponent<Hitable>();
        if ((bool) (Object) component)
        {
          int nextId2 = ResourceManager.Instance.GetNextId();
          component.SetId(nextId2);
          ResourceManager.Instance.AddObject(nextId2, o);
        }
        gameObjectList.Add(o);
      }
    }
    return gameObjectList;
  }

  private RaycastHit FindPos(ConsistentRandom rand)
  {
    RaycastHit hitInfo = new RaycastHit();
    if (Physics.SphereCast(this.transform.position + Vector3.up * 200f + this.RandomSpherePos(rand) * this.campRadius, 1f, Vector3.down, out hitInfo, 400f, (int) this.whatIsGround))
    {
      if (hitInfo.collider.CompareTag("Camp"))
        hitInfo = new RaycastHit();
      if (WorldUtility.WorldHeightToBiome(hitInfo.point.y) == TextureData.TerrainType.Water)
        hitInfo = new RaycastHit();
    }
    return hitInfo;
  }

  private Vector3 RandomSpherePos(ConsistentRandom rand)
  {
    double num1 = rand.NextDouble() * 2.0 - 1.0;
    float num2 = (float) (rand.NextDouble() * 2.0 - 1.0);
    float num3 = (float) (rand.NextDouble() * 2.0 - 1.0);
    double num4 = (double) num2;
    double num5 = (double) num3;
    return new Vector3((float) num1, (float) num4, (float) num5).normalized;
  }

  public void AssignRoles()
  {
    if (this.rolesAssigned)
      return;
    this.rolesAssigned = true;
    List<GameObject> entities = this.zone.entities;
    int num1 = this.rand.Next(1, this.min);
    int num2 = 0;
    foreach (GameObject gameObject in entities)
    {
      gameObject.GetComponent<WoodmanBehaviour>().AssignRole(this.rand);
      ++num2;
      if (num2 >= num1)
        break;
    }
    foreach (GameObject gameObject in entities)
      gameObject.GetComponent<hahahayes>().Randomize(this.rand);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(this.transform.position, this.campRadius);
  }
}
