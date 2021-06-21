// Decompiled with JetBrains decompiler
// Type: HitParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
      ParticleSystem.MainModule main = particle.get_main();
      ((ParticleSystem.MainModule) ref main).set_startColor(ParticleSystem.MinMaxGradient.op_Implicit(HitEffectExtension.GetColor(effect)));
      ParticleSystem.EmissionModule emission = particle.get_emission();
      ParticleSystem.Burst burst = ((ParticleSystem.EmissionModule) ref emission).GetBurst(0);
      ParticleSystem.MinMaxCurve count = ((ParticleSystem.Burst) ref burst).get_count();
      ref ParticleSystem.MinMaxCurve local = ref count;
      ((ParticleSystem.MinMaxCurve) ref local).set_constant(((ParticleSystem.MinMaxCurve) ref local).get_constant() * 2f);
      ((ParticleSystem.Burst) ref burst).set_count(count);
      ((ParticleSystem.EmissionModule) ref emission).SetBurst(0, burst);
    }
    this.audioDone = true;
    this.audio.sounds = this.critHit;
    this.audio.Randomize(0.0f);
  }

  private void Start()
  {
    if (this.audioDone)
      return;
    this.audio.Randomize(0.0f);
  }

  public HitParticles() => base.\u002Ector();
}
