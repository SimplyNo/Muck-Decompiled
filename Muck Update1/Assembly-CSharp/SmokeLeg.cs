// Decompiled with JetBrains decompiler
// Type: SmokeLeg
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
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
