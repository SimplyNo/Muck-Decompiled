// Decompiled with JetBrains decompiler
// Type: GrowableFoodZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    if (randomPos == Vector3.zero)
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
    GameObject o = Object.Instantiate<GameObject>(ItemManager.Instance.allItems[entityId].prefab, pos, Quaternion.identity);
    o.GetComponentInChildren<SharedObject>().SetId(objectId);
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
