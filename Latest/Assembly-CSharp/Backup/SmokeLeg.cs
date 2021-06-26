// Decompiled with JetBrains decompiler
// Type: SmokeLeg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SmokeLeg : MonoBehaviour
{
  public GameObject smokeFx;
  public float cooldown;
  private bool ready = true;

  private void OnTriggerEnter(Collider other)
  {
    if (!this.ready || other.gameObject.layer != LayerMask.NameToLayer("Ground"))
      return;
    this.ready = false;
    this.Invoke("GetReady", this.cooldown);
    Object.Instantiate<GameObject>(this.smokeFx, this.transform.position, this.smokeFx.transform.rotation);
  }

  private void GetReady() => this.ready = true;
}
