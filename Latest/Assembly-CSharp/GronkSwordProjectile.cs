// Decompiled with JetBrains decompiler
// Type: GronkSwordProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class GronkSwordProjectile : MonoBehaviour
{
  private void Start()
  {
    ((Component) this).get_transform().get_forward();
    Quaternion rotation = ((Component) this).get_transform().get_rotation();
    Vector3 vector3 = Vector3.op_Addition(((Quaternion) ref rotation).get_eulerAngles(), new Vector3(0.0f, 0.0f, -90f));
    ((Component) this).get_transform().set_rotation(Quaternion.Euler(vector3));
    M0 component = ((Component) this).GetComponent<Rigidbody>();
    ((Rigidbody) component).set_maxAngularVelocity(9999f);
    ((Rigidbody) component).AddRelativeTorque(Vector3.op_Multiply(((Rigidbody) component).get_angularVelocity(), 2000f));
    ((Rigidbody) component).set_angularVelocity(Vector3.get_zero());
  }

  public GronkSwordProjectile() => base.\u002Ector();
}
