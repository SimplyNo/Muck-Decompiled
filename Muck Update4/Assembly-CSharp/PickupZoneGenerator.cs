// Decompiled with JetBrains decompiler
// Type: PickupZoneGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
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
