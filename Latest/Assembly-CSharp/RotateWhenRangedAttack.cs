// Decompiled with JetBrains decompiler
// Type: RotateWhenRangedAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class RotateWhenRangedAttack : MonoBehaviour
{
  private Mob mob;

  private void Awake() => this.mob = (Mob) ((Component) this).GetComponent<Mob>();

  public void LateUpdate()
  {
    if (!Object.op_Implicit((Object) this.mob.target) || !this.mob.IsRangedAttacking())
      return;
    ((Component) this).get_transform().set_rotation(Quaternion.LookRotation(VectorExtensions.XZVector(Vector3.op_Subtraction(((Component) this.mob.target).get_transform().get_position(), ((Component) this).get_transform().get_position()))));
  }

  public RotateWhenRangedAttack() => base.\u002Ector();
}
