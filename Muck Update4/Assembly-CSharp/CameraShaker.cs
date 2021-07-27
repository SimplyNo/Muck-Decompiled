// Decompiled with JetBrains decompiler
// Type: CameraShaker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    this.shaker = this.GetComponent<Shaker>();
  }

  public void DamageShake(float shakeRatio)
  {
    if (!CurrentSettings.cameraShake)
      return;
    shakeRatio *= 2f;
    shakeRatio = Mathf.Clamp(shakeRatio, 0.2f, 1f);
    this.shaker.Shake((IShakeParameters) this.damagePreset).StrengthScale = shakeRatio;
  }

  public void StepShake(float shakeRatio)
  {
    if (!CurrentSettings.cameraShake)
      return;
    this.shaker.Shake((IShakeParameters) this.stepShakePreset).StrengthScale = shakeRatio;
  }

  public void ChargeShake(float shakeRatio)
  {
    if (!CurrentSettings.cameraShake)
      return;
    shakeRatio = Mathf.Clamp(shakeRatio, 0.2f, 1f);
    this.shaker.Shake((IShakeParameters) this.chargePreset).StrengthScale = shakeRatio;
  }

  public void ShakeWithPreset(ShakePreset preset)
  {
    if (!CurrentSettings.cameraShake)
      return;
    this.shaker.Shake((IShakeParameters) preset);
  }

  public void ShakeWithPresetAndRatio(ShakePreset preset, float shakeRatio)
  {
    if (!CurrentSettings.cameraShake)
      return;
    shakeRatio *= 2f;
    shakeRatio = Mathf.Clamp(shakeRatio, 0.2f, 1f);
    this.shaker.Shake((IShakeParameters) preset).StrengthScale = shakeRatio;
  }
}
