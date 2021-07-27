// Decompiled with JetBrains decompiler
// Type: MobServerDragon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MobServerDragon : MobServer
{
  private List<Vector3> nodes;
  private bool serverReadyToAttack = true;
  private BobMob.DragonState previousState;
  private int hpOnLanding;
  private int startFlightHp;
  private int nAttacksBeforeFlight;
  private int currentAttacks;
  private float fireballCooldown = 2.25f;
  private int minFlyingNodes = 6;
  private int maxFlyingNodes = 40;
  private int currentNodes;
  private int damageTakenThisLanding;

  private void Start()
  {
    this.StartRoutines();
    this.nodes = new List<Vector3>();
    this.serverReadyToAttack = false;
    this.Invoke("GetReady", 4f);
  }

  protected override void Behaviour() => this.TryAttack();

  private void TryAttack()
  {
    if (!this.serverReadyToAttack)
      return;
    switch (((BobMob) this.mob).state)
    {
      case BobMob.DragonState.Flying:
        this.FlyingBehaviour();
        break;
      case BobMob.DragonState.Grounded:
        this.GroundedBehaviour();
        break;
    }
    this.previousState = ((BobMob) this.mob).state;
    if (!(bool) (Object) this.mob.target || this.mob.IsAttacking() || (!this.serverReadyToAttack || this.mob.targetPlayerId == -1) || !GameManager.players[this.mob.targetPlayerId].dead)
      return;
    this.mob.target = (Transform) null;
  }

  private void FlyingBehaviour()
  {
    if ((double) Vector3.Angle((VectorExtensions.XZVector(Boat.Instance.transform.position) - VectorExtensions.XZVector(this.transform.position)).normalized, this.mob.transform.forward) > 80.0)
      return;
    PlayerManager randomAlivePlayer = this.GetRandomAlivePlayer();
    if ((Object) randomAlivePlayer == (Object) null)
      return;
    this.mob.target = randomAlivePlayer.transform;
    float time = this.fireballCooldown - (1f - (float) this.mob.hitable.hp / (float) this.mob.hitable.maxHp);
    this.serverReadyToAttack = false;
    this.Invoke("GetReady", time);
    ((BobMob) this.mob).projectileController.SpawnProjectilePredictNextPosition();
  }

  private PlayerManager GetRandomAlivePlayer()
  {
    List<PlayerManager> playerManagerList = new List<PlayerManager>();
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((bool) (Object) playerManager && !playerManager.dead && !playerManager.disconnected)
        playerManagerList.Add(playerManager);
    }
    return playerManagerList.Count == 0 ? (PlayerManager) null : playerManagerList[Random.Range(0, playerManagerList.Count)];
  }

  private void GroundedToFlight()
  {
    ((BobMob) this.mob).GroundedToFlight();
    this.currentNodes = 0;
    this.serverReadyToAttack = false;
    this.Invoke("GetReady", 4f);
    this.SyncFindNextPosition();
    ServerSend.DragonUpdate(0);
  }

  private Vector3 FlyingToGrounded()
  {
    this.hpOnLanding = this.mob.hitable.hp;
    this.startFlightHp = this.hpOnLanding - (int) ((double) this.mob.hitable.maxHp * 0.330000013113022);
    Debug.LogError((object) ("flight hp: " + (object) this.startFlightHp));
    this.currentAttacks = 0;
    this.nAttacksBeforeFlight = Random.Range(6, 12);
    ((BobMob) this.mob).StartLanding();
    ServerSend.DragonUpdate(1);
    return Boat.Instance.dragonLandingPosition.position;
  }

  private void GroundedBehaviour()
  {
    if (this.mob.hitable.hp < this.startFlightHp)
    {
      this.GroundedToFlight();
      Debug.LogError((object) "Forcing flight since 30% taken");
    }
    else if (this.currentAttacks >= this.nAttacksBeforeFlight)
      this.GroundedToFlight();
    else if (this.previousState != BobMob.DragonState.Grounded)
    {
      this.serverReadyToAttack = false;
      this.Invoke("GetReady", Random.Range(3.5f, 4.5f));
      this.currentAttacks = 0;
    }
    else
    {
      this.mob.SetTarget(this.GetRandomAlivePlayer().id);
      int attackAnimationIndex = Random.Range(0, this.mob.attackAnimations.Length);
      this.mob.Attack(this.mob.targetPlayerId, attackAnimationIndex);
      ServerSend.MobAttack(this.mob.GetId(), this.mob.targetPlayerId, attackAnimationIndex);
      this.serverReadyToAttack = false;
      this.Invoke("GetReady", this.mob.attackTimes[attackAnimationIndex] + Random.Range(0.0f, this.mob.attackCooldown));
      ++this.currentAttacks;
    }
  }

  private void GetReady() => this.serverReadyToAttack = true;

  public override void TookDamage()
  {
  }

  private void FindNodes()
  {
    Vector3 vector3_1 = Boat.Instance.rbTransform.position + Vector3.up * 90f;
    ConsistentRandom consistentRandom = new ConsistentRandom();
    int num = 6;
    for (int index = 0; index < num; ++index)
    {
      Vector3 vector3_2 = Vector3.right * (float) (consistentRandom.NextDouble() * 2.0 - 1.0) * 130f;
      Vector3 vector3_3 = Vector3.forward * (float) (consistentRandom.NextDouble() * 2.0 - 1.0) * 130f;
      Vector3 vector3_4 = Vector3.up * (float) (consistentRandom.NextDouble() * 2.0 - 1.0) * 40f;
      this.nodes.Add(vector3_1 + vector3_2 + vector3_3 + vector3_4);
    }
  }

  private void OnDrawGizmos()
  {
    foreach (Vector3 node in this.nodes)
    {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(node, 10f);
    }
  }

  protected override Vector3 FindNextPosition()
  {
    this.CancelInvoke("SyncFindNextPosition");
    this.Invoke("SyncFindNextPosition", this.findPositionInterval[1]);
    if (this.nodes.Count < 1)
      this.FindNodes();
    switch (((BobMob) this.mob).state)
    {
      case BobMob.DragonState.Flying:
        if (this.currentNodes > this.minFlyingNodes && (double) Random.Range(0.0f, 1f) < 0.0900000035762787 || this.currentNodes > this.maxFlyingNodes)
          return this.FlyingToGrounded();
        ++this.currentNodes;
        Vector3 vector3 = ((BobMob) this.mob).desiredPos;
        int num = 0;
        while (vector3 == ((BobMob) this.mob).desiredPos)
        {
          vector3 = this.nodes[Random.Range(0, this.nodes.Count)];
          ++num;
          if (num > 100)
            break;
        }
        return vector3;
      default:
        return Vector3.zero;
    }
  }
}
