// Decompiled with JetBrains decompiler
// Type: MobZoneGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
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
