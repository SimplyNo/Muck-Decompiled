// Decompiled with JetBrains decompiler
// Type: UiSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    this.audio.clip = !open ? this.closeInventory : this.openInventory;
    this.audio.volume = 0.1f;
    this.audio.pitch = Random.Range(0.8f, 1.2f);
    this.audio.Play();
  }

  public void PlayPickup()
  {
    this.audio.clip = this.pickupItem;
    this.audio.volume = 0.3f;
    this.audio.pitch = Random.Range(0.8f, 1.2f);
    this.audio.Play();
  }

  public void PlayArmor()
  {
    this.audio.clip = this.armor;
    this.audio.volume = 0.55f;
    this.audio.pitch = Random.Range(0.8f, 1.2f);
    this.audio.Play();
  }

  public void PlayClick()
  {
    this.audio.clip = this.hover;
    this.audio.volume = 0.65f;
    this.audio.pitch = Random.Range(0.6f, 0.7f);
    this.audio.Play();
  }

  public void PlayTaskComplete()
  {
    this.audio.clip = this.tutorialTask;
    this.audio.volume = 0.35f;
    this.audio.pitch = Random.Range(0.9f, 1.2f);
    this.audio.Play();
  }
}
