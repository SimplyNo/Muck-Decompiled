// Decompiled with JetBrains decompiler
// Type: MobServerEnemyMeleeAndRanged
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MobServerEnemyMeleeAndRanged : MobServerEnemy
{
  public float rangedCooldown = 6f;
  public bool readyForRangedAttack;

  private new void Start()
  {
    base.Start();
    this.Invoke("GetReadyForRangedAttack", Random.Range(this.rangedCooldown * 0.5f, this.rangedCooldown * 1.5f));
  }

  protected override void AttackBehaviour()
  {
    this.rangedCooldown = this.mob.mobType.rangedCooldown;
    float num1 = Vector3.Distance(this.mob.target.position, this.transform.position);
    bool flag = true;
    if ((double) num1 <= (double) this.mob.mobType.startAttackDistance && (double) num1 >= (double) this.mob.mobType.startRangedAttackDistance)
      flag = (double) Random.Range(0.0f, 1f) < 0.5;
    if ((double) num1 <= (double) this.mob.mobType.startAttackDistance & flag)
    {
      if ((double) Mathf.Abs(Vector3.SignedAngle(this.transform.forward, VectorExtensions.XZVector(this.mob.target.position) - VectorExtensions.XZVector(this.transform.position), Vector3.up)) > (double) this.mob.mobType.minAttackAngle)
        return;
      int num2 = 0;
      if (this.mob.mobType.onlyRangedInRangedPattern)
        num2 = this.mob.nRangedAttacks;
      int attackAnimationIndex = Random.Range(0, this.mob.attackAnimations.Length - num2);
      this.mob.Attack(this.mob.targetPlayerId, attackAnimationIndex);
      ServerSend.MobAttack(this.mob.GetId(), this.mob.targetPlayerId, attackAnimationIndex);
      this.serverReadyToAttack = false;
      this.Invoke("GetReady", this.mob.attackTimes[attackAnimationIndex] + Random.Range(0.0f, this.mob.attackCooldown));
    }
    else
    {
      if ((double) num1 > (double) this.mob.mobType.maxAttackDistance || !this.readyForRangedAttack)
        return;
      int attackAnimationIndex = this.mob.attackAnimations.Length - 1 - Random.Range(0, this.mob.nRangedAttacks);
      this.mob.Attack(this.mob.targetPlayerId, attackAnimationIndex);
      ServerSend.MobAttack(this.mob.GetId(), this.mob.targetPlayerId, attackAnimationIndex);
      this.serverReadyToAttack = false;
      this.Invoke("GetReady", this.mob.attackTimes[attackAnimationIndex] + Random.Range(0.0f, this.mob.attackCooldown));
      this.readyForRangedAttack = false;
      this.Invoke("GetReadyForRangedAttack", Random.Range(this.rangedCooldown * 0.5f, this.rangedCooldown * 1.5f));
    }
  }

  private void GetReadyForRangedAttack() => this.readyForRangedAttack = true;
}
