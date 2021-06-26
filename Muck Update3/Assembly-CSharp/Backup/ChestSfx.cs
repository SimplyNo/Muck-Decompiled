// Decompiled with JetBrains decompiler
// Type: ChestSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
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
