// Decompiled with JetBrains decompiler
// Type: PickupZoneGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

public class PickupZoneGenerator : SpawnZoneGenerator<InventoryItem>
{
  public override void AddEntitiesToZone() => MobZoneManager.Instance.AddZones(this.zones);

  public override SpawnZone ProcessZone(SpawnZone zone)
  {
    GrowableFoodZone growableFoodZone = (GrowableFoodZone) zone;
    growableFoodZone.spawnItems = this.entities;
    growableFoodZone.spawnChance = this.weights;
    float num = 0.0f;
    foreach (float weight in this.weights)
      num += weight;
    growableFoodZone.totalWeight = num;
    return (SpawnZone) growableFoodZone;
  }
}
