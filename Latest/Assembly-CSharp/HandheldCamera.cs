// Decompiled with JetBrains decompiler
// Type: HandheldCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using MilkShake;
using UnityEngine;

public class HandheldCamera : MonoBehaviour
{
  public ShakePreset cameraShake;
  private Shaker shaker;

  private void Start()
  {
    this.shaker = (Shaker) ((Component) this).GetComponent<Shaker>();
    this.shaker.Shake((IShakeParameters) this.cameraShake, new int?());
  }

  public HandheldCamera() => base.\u002Ector();
}
