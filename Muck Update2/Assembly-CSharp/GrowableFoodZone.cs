// Decompiled with JetBrains decompiler
// Type: GrowableFoodZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class GrowableFoodZone : SpawnZone
{
  public InventoryItem[] spawnItems;
  public float[] spawnChance;
  public float totalWeight;

  public override void ServerSpawnEntity()
  {
    MonoBehaviour.print((object) ("spawning food from id: " + (object) this.id));
    Vector3 randomPos = this.FindRandomPos();
    if (Vector3.op_Equality(randomPos, Vector3.get_zero()))
      return;
    --this.entityBuffer;
    int nextId = ResourceManager.Instance.GetNextId();
    int itemToSpawn = this.FindItemToSpawn();
    this.LocalSpawnEntity(randomPos, itemToSpawn, nextId, this.id);
    ServerSend.PickupZoneSpawn(randomPos, itemToSpawn, nextId, this.id);
  }

  public override GameObject LocalSpawnEntity(
    Vector3 pos,
    int entityId,
    int objectId,
    int zoneId)
  {
    GameObject o = (GameObject) Object.Instantiate<GameObject>((M0) ItemManager.Instance.allItems[entityId].prefab, pos, Quaternion.get_identity());
    ((SharedObject) o.GetComponentInChildren<SharedObject>()).SetId(objectId);
    ResourceManager.Instance.AddObject(objectId, o);
    this.entities.Add(o);
    return o;
  }

  public int FindItemToSpawn()
  {
    float num1 = Random.Range(0.0f, 1f);
    float num2 = 0.0f;
    for (int index = 0; index < this.spawnItems.Length; ++index)
    {
      num2 += this.spawnChance[index];
      if ((double) num1 < (double) num2 / (double) this.totalWeight)
        return this.spawnItems[index].id;
    }
    return this.spawnItems[0].id;
  }
}
