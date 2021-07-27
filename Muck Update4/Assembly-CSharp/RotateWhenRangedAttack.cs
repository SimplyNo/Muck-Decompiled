// Decompiled with JetBrains decompiler
// Type: RotateWhenRangedAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RotateWhenRangedAttack : MonoBehaviour
{
  private Mob mob;

  private void Awake() => this.mob = this.GetComponent<Mob>();

  public void LateUpdate()
  {
    if (!(bool) (Object) this.mob.target || !this.mob.IsRangedAttacking())
      return;
    this.transform.rotation = Quaternion.LookRotation(VectorExtensions.XZVector(this.mob.target.transform.position - this.transform.position));
  }
}
