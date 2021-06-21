// Decompiled with JetBrains decompiler
// Type: MobZoneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MobZoneManager : MonoBehaviour
{
  public Dictionary<int, SpawnZone> zones;
  private static int zoneId;
  public bool attatchDebug;
  public GameObject debug;
  public static MobZoneManager Instance;

  private void Awake()
  {
    MobZoneManager.zoneId = 0;
    MobZoneManager.Instance = this;
    this.zones = new Dictionary<int, SpawnZone>();
  }

  public void AddZones(List<SpawnZone> zones)
  {
    foreach (SpawnZone zone in zones)
      this.AddZone(zone, zone.id);
  }

  public void AddZone(SpawnZone mz, int id)
  {
    mz.SetId(id);
    this.zones.Add(id, mz);
    if (!this.attatchDebug)
      return;
    ((DebugObject) ((GameObject) Object.Instantiate<GameObject>((M0) this.debug, ((Component) mz).get_transform())).GetComponentInChildren<DebugObject>()).text = nameof (id) + (object) id;
  }

  public int GetNextId() => MobZoneManager.zoneId++;

  public void RemoveZone(int mobId) => this.zones.Remove(mobId);

  public MobZoneManager() => base.\u002Ector();
}
