// Decompiled with JetBrains decompiler
// Type: ShakeCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
  public float maxDistance;
  public float shakeM;

  private void Start()
  {
    float num = Vector3.Distance(((Component) this).get_transform().get_position(), PlayerMovement.Instance.playerCam.get_position());
    if ((double) num > (double) this.maxDistance)
      return;
    float shakeRatio = this.shakeM * (float) (1.0 - (double) num / (double) this.maxDistance);
    CameraShaker.Instance.StepShake(shakeRatio);
  }

  public ShakeCamera() => base.\u002Ector();
}
