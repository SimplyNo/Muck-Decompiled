// Decompiled with JetBrains decompiler
// Type: CinematicCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
  public Transform target;
  public float speed;

  private void Update()
  {
    ((Component) this).get_transform().LookAt(this.target);
    ((Component) this).get_transform().RotateAround(this.target.get_position(), Vector3.get_up(), this.speed);
  }

  public CinematicCamera() => base.\u002Ector();
}
