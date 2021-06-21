// Decompiled with JetBrains decompiler
// Type: SpectateCameraTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SpectateCameraTest : MonoBehaviour
{
  public Transform target;
  private Vector3 desiredSpectateRotation;

  private void Start()
  {
    ((Component) this).get_transform().set_parent(this.target);
    ((Component) this).get_transform().set_localRotation(Quaternion.get_identity());
    ((Component) this).get_transform().set_localPosition(new Vector3(0.0f, 0.0f, -6f));
  }

  private void Update()
  {
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    this.desiredSpectateRotation = Vector3.op_Addition(this.desiredSpectateRotation, Vector3.op_Multiply(new Vector3((float) vector2.y, (float) -vector2.x, 0.0f), 1.5f));
    this.target.set_rotation(Quaternion.Lerp(((Component) this).get_transform().get_rotation(), Quaternion.Euler(this.desiredSpectateRotation), Time.get_deltaTime() * 10f));
  }

  public SpectateCameraTest() => base.\u002Ector();
}
