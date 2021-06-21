// Decompiled with JetBrains decompiler
// Type: MobSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
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
    float bossMultiplier)
  {
    this.SpawnMob(pos, mobType, mobId, multiplier, bossMultiplier);
    ServerSend.MobSpawn(pos, mobType, mobId, multiplier, bossMultiplier);
  }

  public void SpawnMob(
    Vector3 pos,
    int mobType,
    int mobId,
    float multiplier,
    float bossMultiplier)
  {
    Mob component = Object.Instantiate<GameObject>(this.allMobs[mobType].mobPrefab, pos, Quaternion.identity).GetComponent<Mob>();
    MobManager.Instance.AddMob(component, mobId);
    component.multiplier = multiplier;
    component.bossMultiplier = bossMultiplier;
    MonoBehaviour.print((object) ("spawned new mob with id: " + (object) mobId));
  }
}
