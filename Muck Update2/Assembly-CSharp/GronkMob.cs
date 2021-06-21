// Decompiled with JetBrains decompiler
// Type: GronkMob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class GronkMob : Mob
{
  public override void ExtraUpdate()
  {
    if (!Object.op_Implicit((Object) this.target) || !this.IsRangedAttacking() || !this.IsAttacking())
      return;
    ((Component) this).get_transform().set_rotation(Quaternion.LookRotation(VectorExtensions.XZVector(Vector3.op_Subtraction(((Component) this.target).get_transform().get_position(), ((Component) this).get_transform().get_position()))));
  }
}
