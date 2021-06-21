// Decompiled with JetBrains decompiler
// Type: MoveCameraTowards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MoveCameraTowards : MonoBehaviour
{
  public float speed;
  public Transform target;
  private bool ready;

  private void Awake() => this.Invoke("SetReady", 1f);

  private void SetReady() => this.ready = true;

  private void Update()
  {
    if (!this.ready)
      return;
    ((Component) this).get_transform().set_position(Vector3.Lerp(((Component) this).get_transform().get_position(), this.target.get_position(), Time.get_deltaTime() * this.speed));
  }

  public MoveCameraTowards() => base.\u002Ector();
}
