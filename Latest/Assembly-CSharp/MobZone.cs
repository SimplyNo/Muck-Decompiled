// Decompiled with JetBrains decompiler
// Type: MobZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MobZone : SpawnZone
{
  public MobType mobType;

  public override void ServerSpawnEntity()
  {
    MonoBehaviour.print((object) ("spawning mob from id: " + (object) this.id));
    MonoBehaviour.print((object) ("queue: " + (object) this.entityQueue));
    Vector3 randomPos = this.FindRandomPos();
    if (Vector3.op_Equality(randomPos, Vector3.get_zero()))
      return;
    --this.entityBuffer;
    int nextId = MobManager.Instance.GetNextId();
    int id = this.mobType.id;
    GameObject gameObject = this.LocalSpawnEntity(randomPos, id, nextId, this.id);
    MobServerNeutral component = (MobServerNeutral) gameObject.GetComponent<MobServerNeutral>();
    if (Object.op_Implicit((Object) component))
      component.mobZoneId = this.id;
    ServerSend.MobZoneSpawn(randomPos, id, nextId, this.id);
    if (this.mobType.behaviour == MobType.MobBehaviour.Neutral)
      return;
    ((DontAttackUntilPlayerSpotted) gameObject.AddComponent<DontAttackUntilPlayerSpotted>()).mobZoneId = this.id;
  }

  public override GameObject LocalSpawnEntity(
    Vector3 pos,
    int mobType,
    int mobId,
    int zoneId)
  {
    Mob component = (Mob) ((GameObject) Object.Instantiate<GameObject>((M0) MobSpawner.Instance.allMobs[mobType].mobPrefab, pos, Quaternion.get_identity())).GetComponent<Mob>();
    ((Component) component).set_tag("DontCount");
    MobManager.Instance.AddMob(component, mobId);
    this.entities.Add(((Component) component).get_gameObject());
    return ((Component) component).get_gameObject();
  }
}
