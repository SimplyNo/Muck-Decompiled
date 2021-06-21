// Decompiled with JetBrains decompiler
// Type: RotateTowardsPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
  public Mob mob;

  private void Update()
  {
    if (!Object.op_Implicit((Object) this.mob.target))
      return;
    Vector3 velocity = this.mob.agent.get_velocity();
    if ((double) ((Vector3) ref velocity).get_magnitude() >= 0.05 || this.mob.IsAttacking())
      return;
    Quaternion quaternion = Quaternion.LookRotation(VectorExtensions.XZVector(Vector3.op_Subtraction(((Component) this.mob.target).get_transform().get_position(), ((Component) this).get_transform().get_position())));
    ((Component) this).get_transform().set_rotation(Quaternion.Lerp(((Component) this).get_transform().get_rotation(), quaternion, Time.get_deltaTime() * 5f));
  }

  public RotateTowardsPlayer() => base.\u002Ector();
}
