// Decompiled with JetBrains decompiler
// Type: HitParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HitParticles : MonoBehaviour
{
  public ParticleSystem[] particles;
  public RandomSfx audio;
  public AudioClip[] normalHit;
  public AudioClip[] critHit;
  private bool audioDone;

  public void SetEffect(HitEffect effect)
  {
    foreach (ParticleSystem particle in this.particles)
    {
      particle.main.startColor = (ParticleSystem.MinMaxGradient) HitEffectExtension.GetColor(effect);
      ParticleSystem.EmissionModule emission = particle.emission;
      ParticleSystem.Burst burst = emission.GetBurst(0);
      ParticleSystem.MinMaxCurve count = burst.count;
      count.constant *= 2f;
      burst.count = count;
      emission.SetBurst(0, burst);
    }
    this.audioDone = true;
    this.audio.sounds = this.critHit;
    this.audio.Randomize(0.0f);
  }

  private void Start()
  {
    if (!((Object) this.audio != (Object) null) || this.audioDone)
      return;
    this.audio.Randomize(0.0f);
  }
}
