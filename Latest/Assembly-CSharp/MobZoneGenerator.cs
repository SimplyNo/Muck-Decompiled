// Decompiled with JetBrains decompiler
// Type: MobZoneGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

public class MobZoneGenerator : SpawnZoneGenerator<MobType>
{
  public override void AddEntitiesToZone() => MobZoneManager.Instance.AddZones(this.zones);

  public override SpawnZone ProcessZone(SpawnZone zone)
  {
    ((MobZone) zone).mobType = this.FindObjectToSpawn(this.entities, this.totalWeight);
    return zone;
  }
}
