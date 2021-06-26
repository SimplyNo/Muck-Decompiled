// Decompiled with JetBrains decompiler
// Type: ShakeOnEnable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
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
    this.sfx.Play();
    CameraShaker.Instance.ShakeWithPreset(this.preset);
    if (!(bool) (Object) this.hitbox)
      return;
    this.hitbox.Reset();
  }
}
