// Decompiled with JetBrains decompiler
// Type: RotateTowardsPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RotateTowardsPlayer : MonoBehaviour
{
  public Mob mob;

  private void Update()
  {
    if (!(bool) (Object) this.mob.target || ((double) this.mob.agent.velocity.magnitude >= 0.05 || this.mob.IsAttacking()))
      return;
    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(VectorExtensions.XZVector(this.mob.target.transform.position - this.transform.position)), Time.deltaTime * 5f);
  }
}
