// Decompiled with JetBrains decompiler
// Type: ShakeOnEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using MilkShake;
using UnityEngine;

public class ShakeOnEnable : MonoBehaviour
{
  public AudioSource sfx;
  public ShakePreset preset;
  public HitboxDamage hitbox;

  private void OnEnable()
  {
    if ((bool) (Object) this.sfx)
      this.sfx.Play();
    CameraShaker.Instance.ShakeWithPreset(this.preset);
    if (!(bool) (Object) this.hitbox)
      return;
    this.hitbox.Reset();
  }
}
