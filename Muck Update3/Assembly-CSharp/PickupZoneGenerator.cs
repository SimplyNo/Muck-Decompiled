// Decompiled with JetBrains decompiler
// Type: PickupZoneGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
