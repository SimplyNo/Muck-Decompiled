// Decompiled with JetBrains decompiler
// Type: HandheldCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using MilkShake;
using UnityEngine;

public class HandheldCamera : MonoBehaviour
{
  public ShakePreset cameraShake;
  private Shaker shaker;

  private void Start()
  {
    this.shaker = this.GetComponent<Shaker>();
    this.shaker.Shake((IShakeParameters) this.cameraShake);
  }
}
