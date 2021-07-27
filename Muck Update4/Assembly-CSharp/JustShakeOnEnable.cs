// Decompiled with JetBrains decompiler
// Type: JustShakeOnEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using MilkShake;
using UnityEngine;

public class JustShakeOnEnable : MonoBehaviour
{
  public ShakePreset customShake;
  public float maxDistance = 50f;
  public float shakeM;
  public bool customAndDist;

  private void OnEnable()
  {
    if ((bool) (Object) this.customShake && !this.customAndDist)
    {
      CameraShaker.Instance.ShakeWithPreset(this.customShake);
    }
    else
    {
      float num = Vector3.Distance(this.transform.position, PlayerMovement.Instance.playerCam.position);
      if ((double) num > (double) this.maxDistance)
        return;
      float shakeRatio = this.shakeM * (float) (1.0 - (double) num / (double) this.maxDistance);
      if (this.customAndDist)
        CameraShaker.Instance.ShakeWithPresetAndRatio(this.customShake, shakeRatio);
      CameraShaker.Instance.StepShake(shakeRatio);
    }
  }
}
