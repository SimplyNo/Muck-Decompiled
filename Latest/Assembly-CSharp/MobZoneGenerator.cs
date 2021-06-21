// Decompiled with JetBrains decompiler
// Type: MobZoneGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

public class MobZoneGenerator : SpawnZoneGenerator<MobType>
{
  public override void AddEntitiesToZone() => MobZoneManager.Instance.AddZones(this.zones);

  public override SpawnZone ProcessZone(SpawnZone zone)
  {
    ((MobZone) zone).mobType = this.FindObjectToSpawn(this.entities, this.totalWeight);
    return zone;
  }
}
