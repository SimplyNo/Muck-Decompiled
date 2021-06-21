// Decompiled with JetBrains decompiler
// Type: ShakeCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
  public float maxDistance = 50f;
  public float shakeM;

  private void Start()
  {
    float num = Vector3.Distance(this.transform.position, PlayerMovement.Instance.playerCam.position);
    if ((double) num > (double) this.maxDistance)
      return;
    float shakeRatio = this.shakeM * (float) (1.0 - (double) num / (double) this.maxDistance);
    CameraShaker.Instance.StepShake(shakeRatio);
  }
}
