// Decompiled with JetBrains decompiler
// Type: HitableChest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class HitableChest : HitableResource
{
  public override void OnKill(Vector3 dir) => ChestManager.Instance.RemoveChest(this.GetId());
}
