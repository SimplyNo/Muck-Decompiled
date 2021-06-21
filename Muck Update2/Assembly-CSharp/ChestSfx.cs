// Decompiled with JetBrains decompiler
// Type: ChestSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class ChestSfx : MonoBehaviour
{
  public AudioClip open;
  public AudioClip close;
  private AudioSource audio;

  private void Awake() => this.audio = (AudioSource) ((Component) this).GetComponent<AudioSource>();

  public void OpenChest()
  {
    this.audio.set_clip(this.open);
    this.audio.Play();
  }

  public void CloseChest()
  {
    this.audio.set_clip(this.close);
    this.audio.Play();
  }

  public ChestSfx() => base.\u002Ector();
}
