// Decompiled with JetBrains decompiler
// Type: FootStep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class FootStep : MonoBehaviour
{
  public LayerMask whatIsGround;
  public RandomSfx randomSfx;
  public AudioClip[] woodSfx;

  private void Start() => this.FindGroundType();

  private void FindGroundType()
  {
    RaycastHit raycastHit;
    if (Physics.Raycast(((Component) this).get_transform().get_position(), Vector3.get_down(), ref raycastHit, 5f, LayerMask.op_Implicit(this.whatIsGround)) && ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().CompareTag("Build"))
      this.randomSfx.sounds = this.woodSfx;
    this.randomSfx.Randomize(0.0f);
  }

  public FootStep() => base.\u002Ector();
}
