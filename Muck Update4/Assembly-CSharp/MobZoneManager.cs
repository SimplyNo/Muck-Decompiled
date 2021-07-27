// Decompiled with JetBrains decompiler
// Type: MobZoneManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    Object.Instantiate<GameObject>(this.debug, mz.transform).GetComponentInChildren<DebugObject>().text = nameof (id) + (object) id;
  }

  public int GetNextId() => MobZoneManager.zoneId++;

  public void RemoveZone(int mobId) => this.zones.Remove(mobId);
}
