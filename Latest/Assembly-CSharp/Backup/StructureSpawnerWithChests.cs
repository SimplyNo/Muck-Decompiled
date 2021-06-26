// Decompiled with JetBrains decompiler
// Type: StructureSpawnerWithChests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class StructureSpawnerWithChests : StructureSpawner
{
  public override void Process(GameObject newStructure, RaycastHit hit)
  {
    newStructure.transform.rotation = Quaternion.LookRotation(hit.normal);
    SpawnChestsInLocations componentInChildren1 = newStructure.GetComponentInChildren<SpawnChestsInLocations>();
    if ((bool) (Object) componentInChildren1)
      componentInChildren1.SetChests(this.randomGen);
    SpawnPowerupsInLocations componentInChildren2 = newStructure.GetComponentInChildren<SpawnPowerupsInLocations>();
    if (!(bool) (Object) componentInChildren2)
      return;
    componentInChildren2.SetChests(this.randomGen);
  }
}
