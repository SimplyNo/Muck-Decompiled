// Decompiled with JetBrains decompiler
// Type: ContinousHitbox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ContinousHitbox : MonoBehaviour
{
  public HitboxDamage hitbox;
  public float resetTime = 0.1f;

  private void Awake() => this.InvokeRepeating("ResetHitbox", this.resetTime, this.resetTime);

  private void ResetHitbox() => this.hitbox.Reset();
}
