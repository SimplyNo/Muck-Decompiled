// Decompiled with JetBrains decompiler
// Type: CameraLookAt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
  public Transform target;

  private void Update()
  {
    Quaternion quaternion = Quaternion.LookRotation(Vector3.op_Subtraction(this.target.get_position(), ((Component) this).get_transform().get_position()));
    ((Component) this).get_transform().set_rotation(Quaternion.Lerp(((Component) this).get_transform().get_rotation(), quaternion, Time.get_deltaTime() * 6.4f));
  }

  public CameraLookAt() => base.\u002Ector();
}
