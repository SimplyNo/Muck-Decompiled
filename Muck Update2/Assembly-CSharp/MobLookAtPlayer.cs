// Decompiled with JetBrains decompiler
// Type: MobLookAtPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MobLookAtPlayer : MonoBehaviour
{
  public bool lookAtPlayer;
  public Transform torso;
  public Transform head;
  private Mob mob;
  private Vector3 defaultHeadRotation;
  private Vector3 defaultTorsoRotation;
  public Vector3 maxTorsoRotation;
  public Vector3 maxHeadRotation;

  private void Awake()
  {
    this.defaultHeadRotation = ((Component) this.head).get_transform().get_eulerAngles();
    this.defaultTorsoRotation = ((Component) this.torso).get_transform().get_eulerAngles();
  }

  private void LateUpdate() => this.LookAtPlayer();

  private void LookAtPlayer()
  {
    if (!this.lookAtPlayer || !Object.op_Implicit((Object) this.mob.target))
      return;
    Vector3 vector3 = VectorExtensions.XZVector(Vector3.op_Subtraction(this.mob.target.get_position(), ((Component) this).get_transform().get_position()));
    double num = (double) Vector3.SignedAngle(VectorExtensions.XZVector(((Component) this).get_transform().get_forward()), vector3, Vector3.get_up());
  }

  public MobLookAtPlayer() => base.\u002Ector();
}
