// Decompiled with JetBrains decompiler
// Type: MobServerEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MobServerEnemy : MobServer
{
  public LayerMask groundMask;
  protected bool serverReadyToAttack = true;

  protected void Start()
  {
    this.StartRoutines();
    this.groundMask = LayerMask.op_Implicit(1 << LayerMask.NameToLayer("Ground"));
  }

  protected override void Behaviour() => this.TryAttack();

  public override void TookDamage()
  {
  }

  private void TryAttack()
  {
    if (!Object.op_Implicit((Object) this.mob.target) || this.mob.IsAttacking() || !this.serverReadyToAttack)
      return;
    if (this.mob.targetPlayerId != -1 && GameManager.players[this.mob.targetPlayerId].dead)
      this.mob.target = (Transform) null;
    else
      this.AttackBehaviour();
  }

  protected virtual void AttackBehaviour()
  {
    if ((double) Vector3.Distance(this.mob.target.get_position(), ((Component) this).get_transform().get_position()) > (double) this.mob.mobType.startAttackDistance || (double) Mathf.Abs(Vector3.SignedAngle(((Component) this).get_transform().get_forward(), Vector3.op_Subtraction(VectorExtensions.XZVector(this.mob.target.get_position()), VectorExtensions.XZVector(((Component) this).get_transform().get_position())), Vector3.get_up())) > (double) this.mob.mobType.minAttackAngle)
      return;
    int attackAnimationIndex = Random.Range(0, this.mob.attackAnimations.Length);
    this.mob.Attack(this.mob.targetPlayerId, attackAnimationIndex);
    ServerSend.MobAttack(this.mob.GetId(), this.mob.targetPlayerId, attackAnimationIndex);
    this.serverReadyToAttack = false;
    this.Invoke("GetReady", this.mob.attackTimes[attackAnimationIndex] + Random.Range(0.0f, this.mob.attackCooldown));
  }

  protected void GetReady() => this.serverReadyToAttack = true;

  protected override Vector3 FindNextPosition()
  {
    float num1 = 15f * this.mob.mobType.followPlayerDistance;
    if (Object.op_Inequality((Object) this.mob.target, (Object) null))
      num1 = Vector3.Distance(((Component) this.mob).get_transform().get_position(), ((Component) this.mob.target).get_transform().get_position());
    if ((double) num1 < 10.0 * (double) this.mob.mobType.followPlayerDistance)
      this.Invoke("SyncFindNextPosition", this.findPositionInterval[0]);
    else if ((double) num1 < 25.0 * (double) this.mob.mobType.followPlayerDistance)
      this.Invoke("SyncFindNextPosition", this.findPositionInterval[1]);
    else
      this.Invoke("SyncFindNextPosition", this.findPositionInterval[2]);
    if (this.mob.IsAttacking() && this.mob.stopOnAttack || (this.mob.knocked || !this.mob.ready))
      return Vector3.get_zero();
    Vector3 vector3_1 = Vector3.get_zero();
    Transform transform = (Transform) null;
    int num2 = -1;
    float num3 = float.PositiveInfinity;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (Object.op_Implicit((Object) playerManager) && !playerManager.dead)
      {
        float num4 = Vector3.Distance(((Component) playerManager).get_transform().get_position(), ((Component) this).get_transform().get_position());
        if ((double) num4 < (double) num3)
        {
          num3 = num4;
          Vector3 position = ((Component) playerManager).get_transform().get_position();
          Vector3 vector3_2 = Vector3.get_zero();
          if ((double) num4 > 12.0)
          {
            ((Vector3) ref vector3_2).\u002Ector(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
            vector3_2 = Vector3.op_Multiply(vector3_2, num4 * (1f - this.mob.mobType.followPlayerAccuracy));
          }
          Vector3 vector3_3 = vector3_2;
          vector3_1 = Vector3.op_Addition(position, vector3_3);
          transform = ((Component) playerManager).get_transform();
          num2 = playerManager.id;
        }
      }
    }
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (Object.op_Implicit((Object) playerManager) && !playerManager.dead)
      {
        float num4 = Vector3.Distance(((Component) playerManager).get_transform().get_position(), ((Component) this).get_transform().get_position());
        if ((double) num4 < (double) num3)
        {
          num3 = num4;
          Vector3 position = ((Component) playerManager).get_transform().get_position();
          Vector3 vector3_2 = Vector3.get_zero();
          if ((double) num4 > 12.0)
          {
            ((Vector3) ref vector3_2).\u002Ector(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
            vector3_2 = Vector3.op_Multiply(vector3_2, num4 * (1f - this.mob.mobType.followPlayerAccuracy));
          }
          Vector3 vector3_3 = vector3_2;
          vector3_1 = Vector3.op_Addition(position, vector3_3);
          transform = ((Component) playerManager).get_transform();
          num2 = playerManager.id;
        }
      }
    }
    bool flag = false;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (Object.op_Implicit((Object) playerManager) && !playerManager.dead)
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(((Component) playerManager).get_transform().get_position(), ((Component) this.mob).get_transform().get_position());
        Vector3 normalized = ((Vector3) ref vector3_2).get_normalized();
        RaycastHit raycastHit;
        if (Physics.Raycast(((Component) this.mob).get_transform().get_position(), normalized, ref raycastHit, 2000f, LayerMask.op_Implicit(MobManager.Instance.whatIsRaycastable)) && ((Component) ((RaycastHit) ref raycastHit).get_transform()).get_gameObject().get_layer() == LayerMask.NameToLayer("Player"))
        {
          flag = true;
          break;
        }
      }
    }
    if (!flag && !this.mob.mobType.ignoreBuilds)
    {
      using (Dictionary<int, GameObject>.ValueCollection.Enumerator enumerator = ResourceManager.Instance.builds.Values.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (!Object.op_Equality((Object) current, (Object) null) && (double) Vector3.Distance(((Component) this).get_transform().get_position(), current.get_transform().get_position()) < (double) num3)
          {
            vector3_1 = current.get_transform().get_position();
            transform = current.get_transform();
            num2 = -1;
            break;
          }
        }
      }
    }
    if (!Object.op_Implicit((Object) transform))
      return Vector3.get_zero();
    double num5 = (double) Vector3.Distance(this.mob.agent.get_destination(), transform.get_position());
    double num6 = (double) Vector3.Distance(transform.get_position(), ((Component) this.mob).get_transform().get_position());
    if (Vector3.op_Equality(vector3_1, Vector3.get_zero()))
    {
      this.mob.target = (Transform) null;
      return Vector3.get_zero();
    }
    this.mob.target = transform;
    this.mob.targetPlayerId = num2;
    return vector3_1;
  }
}
