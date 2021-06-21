// Decompiled with JetBrains decompiler
// Type: UiSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class UiSfx : MonoBehaviour
{
  public AudioSource audio;
  public AudioClip openInventory;
  public AudioClip closeInventory;
  public AudioClip pickupItem;
  public AudioClip armor;
  public AudioClip hover;
  public AudioClip click;
  public AudioClip tutorialTask;
  public static UiSfx Instance;

  private void Awake() => UiSfx.Instance = this;

  public void PlayInventory(bool open)
  {
    if (open)
      this.audio.set_clip(this.openInventory);
    else
      this.audio.set_clip(this.closeInventory);
    this.audio.set_volume(0.1f);
    this.audio.set_pitch(Random.Range(0.8f, 1.2f));
    this.audio.Play();
  }

  public void PlayPickup()
  {
    this.audio.set_clip(this.pickupItem);
    this.audio.set_volume(0.3f);
    this.audio.set_pitch(Random.Range(0.8f, 1.2f));
    this.audio.Play();
  }

  public void PlayArmor()
  {
    this.audio.set_clip(this.armor);
    this.audio.set_volume(0.55f);
    this.audio.set_pitch(Random.Range(0.8f, 1.2f));
    this.audio.Play();
  }

  public void PlayClick()
  {
    this.audio.set_clip(this.hover);
    this.audio.set_volume(0.65f);
    this.audio.set_pitch(Random.Range(0.6f, 0.7f));
    this.audio.Play();
  }

  public void PlayTaskComplete()
  {
    this.audio.set_clip(this.tutorialTask);
    this.audio.set_volume(0.35f);
    this.audio.set_pitch(Random.Range(0.9f, 1.2f));
    this.audio.Play();
  }

  public UiSfx() => base.\u002Ector();
}
