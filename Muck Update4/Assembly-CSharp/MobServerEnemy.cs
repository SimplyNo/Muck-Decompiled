// Decompiled with JetBrains decompiler
// Type: MobServerEnemy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MobServerEnemy : MobServer
{
  public LayerMask groundMask;
  protected bool serverReadyToAttack = true;

  protected void Start()
  {
    this.StartRoutines();
    this.groundMask = (LayerMask) (1 << LayerMask.NameToLayer("Ground"));
  }

  protected override void Behaviour() => this.TryAttack();

  public override void TookDamage()
  {
  }

  private void TryAttack()
  {
    if (!(bool) (Object) this.mob.target || this.mob.IsAttacking() || !this.serverReadyToAttack)
      return;
    if (this.mob.targetPlayerId != -1 && GameManager.players[this.mob.targetPlayerId].dead)
      this.mob.target = (Transform) null;
    else
      this.AttackBehaviour();
  }

  protected virtual void AttackBehaviour()
  {
    if ((double) Vector3.Distance(this.mob.target.position, this.transform.position) > (double) this.mob.mobType.startAttackDistance || (double) Mathf.Abs(Vector3.SignedAngle(this.transform.forward, VectorExtensions.XZVector(this.mob.target.position) - VectorExtensions.XZVector(this.transform.position), Vector3.up)) > (double) this.mob.mobType.minAttackAngle)
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
    if ((Object) this.mob.target != (Object) null)
      num1 = Vector3.Distance(this.mob.transform.position, this.mob.target.transform.position);
    if ((double) num1 < 10.0 * (double) this.mob.mobType.followPlayerDistance)
      this.Invoke("SyncFindNextPosition", this.findPositionInterval[0]);
    else if ((double) num1 < 25.0 * (double) this.mob.mobType.followPlayerDistance)
      this.Invoke("SyncFindNextPosition", this.findPositionInterval[1]);
    else
      this.Invoke("SyncFindNextPosition", this.findPositionInterval[2]);
    if (this.mob.IsAttacking() && this.mob.stopOnAttack || (this.mob.knocked || !this.mob.ready))
      return Vector3.zero;
    Vector3 vector3_1 = Vector3.zero;
    Transform transform = (Transform) null;
    int num2 = -1;
    float num3 = float.PositiveInfinity;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((bool) (Object) playerManager && !playerManager.dead)
      {
        float num4 = Vector3.Distance(playerManager.transform.position, this.transform.position);
        if ((double) num4 < (double) num3)
        {
          num3 = num4;
          Vector3 position = playerManager.transform.position;
          Vector3 vector3_2 = Vector3.zero;
          if ((double) num4 > 12.0)
          {
            vector3_2 = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
            vector3_2 *= num4 * (1f - this.mob.mobType.followPlayerAccuracy);
          }
          Vector3 vector3_3 = vector3_2;
          vector3_1 = position + vector3_3;
          transform = playerManager.transform;
          num2 = playerManager.id;
        }
      }
    }
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((bool) (Object) playerManager && !playerManager.dead)
      {
        float num4 = Vector3.Distance(playerManager.transform.position, this.transform.position);
        if ((double) num4 < (double) num3)
        {
          num3 = num4;
          Vector3 position = playerManager.transform.position;
          Vector3 vector3_2 = Vector3.zero;
          if ((double) num4 > 12.0)
          {
            vector3_2 = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f));
            vector3_2 *= num4 * (1f - this.mob.mobType.followPlayerAccuracy);
          }
          Vector3 vector3_3 = vector3_2;
          vector3_1 = position + vector3_3;
          transform = playerManager.transform;
          num2 = playerManager.id;
        }
      }
    }
    bool flag = false;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      RaycastHit hitInfo;
      if ((bool) (Object) playerManager && !playerManager.dead && (Physics.Raycast(this.mob.transform.position, (playerManager.transform.position - this.mob.transform.position).normalized, out hitInfo, 2000f, (int) MobManager.Instance.whatIsRaycastable) && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player")))
      {
        flag = true;
        break;
      }
    }
    if (!flag && !this.mob.mobType.ignoreBuilds)
    {
      foreach (GameObject gameObject in ResourceManager.Instance.builds.Values)
      {
        if (!((Object) gameObject == (Object) null) && (double) Vector3.Distance(this.transform.position, gameObject.transform.position) < (double) num3)
        {
          vector3_1 = gameObject.transform.position;
          transform = gameObject.transform;
          num2 = -1;
          break;
        }
      }
    }
    if (!(bool) (Object) transform)
      return Vector3.zero;
    double num5 = (double) Vector3.Distance(this.mob.agent.destination, transform.position);
    double num6 = (double) Vector3.Distance(transform.position, this.mob.transform.position);
    if (vector3_1 == Vector3.zero)
    {
      this.mob.target = (Transform) null;
      return Vector3.zero;
    }
    this.mob.target = transform;
    this.mob.targetPlayerId = num2;
    return vector3_1;
  }
}
