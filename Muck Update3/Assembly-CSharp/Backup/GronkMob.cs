// Decompiled with JetBrains decompiler
// Type: GronkMob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GronkMob : Mob
{
  public override void ExtraUpdate()
  {
    if (!(bool) (Object) this.target || !this.IsRangedAttacking() || !this.IsAttacking())
      return;
    this.transform.rotation = Quaternion.LookRotation(VectorExtensions.XZVector(this.target.transform.position - this.transform.position));
  }
}
