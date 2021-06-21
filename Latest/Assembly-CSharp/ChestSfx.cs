// Decompiled with JetBrains decompiler
// Type: ChestSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ChestSfx : MonoBehaviour
{
  public AudioClip open;
  public AudioClip close;
  private AudioSource audio;

  private void Awake() => this.audio = this.GetComponent<AudioSource>();

  public void OpenChest()
  {
    this.audio.clip = this.open;
    this.audio.Play();
  }

  public void CloseChest()
  {
    this.audio.clip = this.close;
    this.audio.Play();
  }
}
