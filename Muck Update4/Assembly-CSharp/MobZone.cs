// Decompiled with JetBrains decompiler
// Type: MobZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MobZone : SpawnZone
{
  public MobType mobType;

  public override void ServerSpawnEntity()
  {
    MonoBehaviour.print((object) ("spawning mob from id: " + (object) this.id));
    MonoBehaviour.print((object) ("queue: " + (object) this.entityQueue));
    Vector3 randomPos = this.FindRandomPos();
    if (randomPos == Vector3.zero)
      return;
    --this.entityBuffer;
    int nextId = MobManager.Instance.GetNextId();
    int id = this.mobType.id;
    GameObject gameObject = this.LocalSpawnEntity(randomPos, id, nextId, this.id);
    MobServerNeutral component = gameObject.GetComponent<MobServerNeutral>();
    if ((bool) (Object) component)
      component.mobZoneId = this.id;
    ServerSend.MobZoneSpawn(randomPos, id, nextId, this.id);
    if (this.mobType.name == "Woodman")
    {
      gameObject.GetComponent<WoodmanBehaviour>().mobZoneId = this.id;
    }
    else
    {
      if (this.mobType.behaviour == MobType.MobBehaviour.Neutral)
        return;
      gameObject.AddComponent<DontAttackUntilPlayerSpotted>().mobZoneId = this.id;
    }
  }

  public override GameObject LocalSpawnEntity(
    Vector3 pos,
    int mobType,
    int mobId,
    int zoneId)
  {
    Mob component = Object.Instantiate<GameObject>(MobSpawner.Instance.allMobs[mobType].mobPrefab, pos, Quaternion.identity).GetComponent<Mob>();
    component.tag = "DontCount";
    MobManager.Instance.AddMob(component, mobId);
    this.entities.Add(component.gameObject);
    return component.gameObject;
  }
}
