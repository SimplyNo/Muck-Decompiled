// Decompiled with JetBrains decompiler
// Type: CameraShaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using MilkShake;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
  public ShakePreset damagePreset;
  public ShakePreset chargePreset;
  public ShakePreset stepShakePreset;
  private Shaker shaker;
  public static CameraShaker Instance;

  private void Awake()
  {
    CameraShaker.Instance = this;
    this.shaker = (Shaker) ((Component) this).GetComponent<Shaker>();
  }

  public void DamageShake(float shakeRatio)
  {
    if (!CurrentSettings.cameraShake)
      return;
    shakeRatio *= 2f;
    shakeRatio = Mathf.Clamp(shakeRatio, 0.2f, 1f);
    this.shaker.Shake((IShakeParameters) this.damagePreset, new int?()).StrengthScale = (__Null) (double) shakeRatio;
  }

  public void StepShake(float shakeRatio)
  {
    if (!CurrentSettings.cameraShake)
      return;
    this.shaker.Shake((IShakeParameters) this.stepShakePreset, new int?()).StrengthScale = (__Null) (double) shakeRatio;
  }

  public void ChargeShake(float shakeRatio)
  {
    if (!CurrentSettings.cameraShake)
      return;
    shakeRatio = Mathf.Clamp(shakeRatio, 0.2f, 1f);
    this.shaker.Shake((IShakeParameters) this.chargePreset, new int?()).StrengthScale = (__Null) (double) shakeRatio;
  }

  public CameraShaker() => base.\u002Ector();
}
