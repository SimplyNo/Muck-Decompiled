// Decompiled with JetBrains decompiler
// Type: StructureSpawnerWithChests
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class StructureSpawnerWithChests : StructureSpawner
{
  public override void Process(GameObject newStructure, RaycastHit hit)
  {
    newStructure.get_transform().set_rotation(Quaternion.LookRotation(((RaycastHit) ref hit).get_normal()));
    SpawnChestsInLocations componentInChildren1 = (SpawnChestsInLocations) newStructure.GetComponentInChildren<SpawnChestsInLocations>();
    if (Object.op_Implicit((Object) componentInChildren1))
      componentInChildren1.SetChests(this.randomGen);
    SpawnPowerupsInLocations componentInChildren2 = (SpawnPowerupsInLocations) newStructure.GetComponentInChildren<SpawnPowerupsInLocations>();
    if (!Object.op_Implicit((Object) componentInChildren2))
      return;
    componentInChildren2.SetChests(this.randomGen);
  }
}
