// Decompiled with JetBrains decompiler
// Type: MobSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    Mob.BossType bossType = Mob.BossType.None,
    int guardianType = -1)
  {
    this.SpawnMob(pos, mobType, mobId, multiplier, bossMultiplier, bossType, guardianType);
    ServerSend.MobSpawn(pos, mobType, mobId, multiplier, bossMultiplier, guardianType);
  }

  public void SpawnMob(
    Vector3 pos,
    int mobType,
    int mobId,
    float multiplier,
    float bossMultiplier,
    Mob.BossType bossType = Mob.BossType.None,
    int guardianType = -1)
  {
    Mob component = Object.Instantiate<GameObject>(this.allMobs[mobType].mobPrefab, pos, Quaternion.identity).GetComponent<Mob>();
    MobManager.Instance.AddMob(component, mobId);
    component.multiplier = multiplier;
    component.bossMultiplier = bossMultiplier;
    if (component.bossType != Mob.BossType.BossShrine || bossType != Mob.BossType.None)
      component.bossType = bossType;
    if (guardianType != -1)
      component.GetComponent<Guardian>().type = (Guardian.GuardianType) guardianType;
    MonoBehaviour.print((object) ("spawned new mob with id: " + (object) mobId));
  }
}
