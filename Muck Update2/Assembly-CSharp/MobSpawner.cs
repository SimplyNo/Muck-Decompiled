// Decompiled with JetBrains decompiler
// Type: MobSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MobSpawner : MonoBehaviour
{
  public MobType[] mobsInspector;
  public MobType[] allMobs;
  public static MobSpawner Instance;

  private void Awake()
  {
    MobSpawner.Instance = this;
    this.FillList();
  }

  private void FillList()
  {
    this.allMobs = new MobType[this.mobsInspector.Length];
    for (int index = 0; index < this.mobsInspector.Length; ++index)
    {
      this.allMobs[index] = this.mobsInspector[index];
      this.allMobs[index].id = index;
    }
  }

  public void ServerSpawnNewMob(
    int mobId,
    int mobType,
    Vector3 pos,
    float multiplier,
    float bossMultiplier,
    Mob.BossType bossType = Mob.BossType.None)
  {
    this.SpawnMob(pos, mobType, mobId, multiplier, bossMultiplier, bossType);
    ServerSend.MobSpawn(pos, mobType, mobId, multiplier, bossMultiplier);
  }

  public void SpawnMob(
    Vector3 pos,
    int mobType,
    int mobId,
    float multiplier,
    float bossMultiplier,
    Mob.BossType bossType = Mob.BossType.None)
  {
    Mob component = (Mob) ((GameObject) Object.Instantiate<GameObject>((M0) this.allMobs[mobType].mobPrefab, pos, Quaternion.get_identity())).GetComponent<Mob>();
    MobManager.Instance.AddMob(component, mobId);
    component.multiplier = multiplier;
    component.bossMultiplier = bossMultiplier;
    if (component.bossType != Mob.BossType.BossShrine || bossType != Mob.BossType.None)
      component.bossType = bossType;
    MonoBehaviour.print((object) ("spawned new mob with id: " + (object) mobId));
  }

  public MobSpawner() => base.\u002Ector();
}
