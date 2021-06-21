// Decompiled with JetBrains decompiler
// Type: RandomSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class RandomSfx : MonoBehaviour
{
  public AudioClip[] sounds;
  [Range(0.0f, 2f)]
  public float maxPitch;
  [Range(0.0f, 2f)]
  public float minPitch;
  private AudioSource s;
  public bool playOnAwake;

  private void Awake()
  {
    this.s = (AudioSource) ((Component) this).GetComponent<AudioSource>();
    if (!this.playOnAwake)
      return;
    this.Randomize(0.0f);
  }

  public void Randomize(float delay)
  {
    this.s.set_clip(this.sounds[Random.Range(0, this.sounds.Length)]);
    this.s.set_pitch(Random.Range(this.minPitch, this.maxPitch));
    this.s.PlayDelayed(delay);
  }

  public RandomSfx() => base.\u002Ector();
}
