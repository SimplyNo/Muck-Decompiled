// Decompiled with JetBrains decompiler
// Type: MobFlying
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MobFlying : Mob
{
  private float defaultHeight = 5.6f;
  public LayerMask whatIsGround;

  public override void ExtraUpdate()
  {
    RaycastHit raycastHit;
    if (!Object.op_Implicit((Object) this.target) || !Physics.Raycast(((Component) this.target).get_transform().get_position(), Vector3.get_down(), ref raycastHit, 5000f, LayerMask.op_Implicit(this.whatIsGround)))
      return;
    float num = this.defaultHeight + ((RaycastHit) ref raycastHit).get_distance();
    this.agent.set_baseOffset(Mathf.Lerp(this.agent.get_baseOffset(), num, Time.get_deltaTime() * 0.3f));
  }
}
