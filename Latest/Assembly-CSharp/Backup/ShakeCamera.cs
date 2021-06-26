// Decompiled with JetBrains decompiler
// Type: ShakeCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using MilkShake;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
  public ShakePreset customShake;
  public float maxDistance = 50f;
  public float shakeM;

  private void Start()
  {
    if ((bool) (Object) this.customShake)
    {
      CameraShaker.Instance.ShakeWithPreset(this.customShake);
    }
    else
    {
      float num = Vector3.Distance(this.transform.position, PlayerMovement.Instance.playerCam.position);
      if ((double) num > (double) this.maxDistance)
        return;
      float shakeRatio = this.shakeM * (float) (1.0 - (double) num / (double) this.maxDistance);
      CameraShaker.Instance.StepShake(shakeRatio);
    }
  }
}
