// Decompiled with JetBrains decompiler
// Type: SmokeLeg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SmokeLeg : MonoBehaviour
{
  public GameObject smokeFx;
  public float cooldown;
  private bool ready;

  private void OnTriggerEnter(Collider other)
  {
    if (!this.ready || ((Component) other).get_gameObject().get_layer() != LayerMask.NameToLayer("Ground"))
      return;
    this.ready = false;
    this.Invoke("GetReady", this.cooldown);
    Object.Instantiate<GameObject>((M0) this.smokeFx, ((Component) this).get_transform().get_position(), this.smokeFx.get_transform().get_rotation());
  }

  private void GetReady() => this.ready = true;

  public SmokeLeg() => base.\u002Ector();
}
