// Decompiled with JetBrains decompiler
// Type: LookAtTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class LookAtTarget : MonoBehaviour
{
  public Transform target;
  public Transform head;
  public float lookDistance;
  public bool yAxis;
  private Mob mob;

  private void Awake() => this.mob = (Mob) ((Component) ((Component) this).get_transform().get_root()).GetComponent<Mob>();

  private void LateUpdate()
  {
    if (Object.op_Equality((Object) this.mob.target, (Object) null) || (double) Vector3.Distance(this.mob.target.get_position(), ((Component) this).get_transform().get_position()) > (double) this.lookDistance)
      return;
    float num = Mathf.Clamp(Vector3.SignedAngle(((Component) this).get_transform().get_forward(), Vector3.op_Subtraction(VectorExtensions.XZVector(this.mob.target.get_position()), VectorExtensions.XZVector(((Component) this).get_transform().get_position())), Vector3.get_up()), -130f, 130f);
    Quaternion localRotation = ((Component) this.head).get_transform().get_localRotation();
    Vector3 eulerAngles = ((Quaternion) ref localRotation).get_eulerAngles();
    if (!this.yAxis)
      ((Component) this.head).get_transform().set_localRotation(Quaternion.Euler((float) eulerAngles.x, (float) eulerAngles.y, num));
    else
      ((Component) this.head).get_transform().set_localRotation(Quaternion.Euler((float) eulerAngles.x, num, (float) eulerAngles.z));
  }

  public LookAtTarget() => base.\u002Ector();
}
