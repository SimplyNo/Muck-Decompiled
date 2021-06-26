// Decompiled with JetBrains decompiler
// Type: BobMob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BobMob : Mob
{
  private int landingNode;
  private float t;
  private float speed = 50f;

  public BobMob.DragonState state { get; set; }

  public Vector3 desiredPos { get; set; }

  public ProjectileAttackNoGravity projectileController { get; set; }

  private void Awake()
  {
    this.projectileController = this.GetComponent<ProjectileAttackNoGravity>();
    this.state = BobMob.DragonState.Flying;
    this.hitable = this.GetComponent<Hitable>();
    this.animator = this.GetComponent<Animator>();
    if (LocalClient.serverOwner)
    {
      if (this.mobType.behaviour == MobType.MobBehaviour.Enemy)
        this.gameObject.AddComponent<MobServerEnemy>();
      else if (this.mobType.behaviour == MobType.MobBehaviour.Neutral)
        this.gameObject.AddComponent<MobServerNeutral>();
      else if (this.mobType.behaviour == MobType.MobBehaviour.EnemyMeleeAndRanged)
        this.gameObject.AddComponent<MobServerEnemyMeleeAndRanged>();
      else if (this.mobType.behaviour == MobType.MobBehaviour.Dragon)
        this.gameObject.AddComponent<MobServerDragon>();
    }
    this.attackTimes = new float[this.attackAnimations.Length];
    for (int index = 0; index < this.attackAnimations.Length; ++index)
      this.attackTimes[index] = this.attackAnimations[index].length;
  }

  protected override void Animate()
  {
  }

  public override void SetTarget(int targetId)
  {
    this.targetPlayerId = targetId;
    this.target = GameManager.players[this.targetPlayerId].transform;
  }

  public void StartLanding()
  {
    this.landingNode = 0;
    this.state = BobMob.DragonState.Landing;
  }

  public void GroundedToFlight()
  {
    this.state = BobMob.DragonState.Flying;
    this.animator.SetBool("Landed", false);
  }

  public void DragonUpdate(BobMob.DragonState state)
  {
    if (state != BobMob.DragonState.Flying)
    {
      if (state != BobMob.DragonState.Landing)
        return;
      this.StartLanding();
    }
    else
      this.GroundedToFlight();
  }

  public override void ExtraUpdate()
  {
    switch (this.state)
    {
      case BobMob.DragonState.Flying:
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation((this.desiredPos - this.transform.position).normalized), Time.deltaTime * 0.6f);
        this.transform.position += this.transform.forward * this.speed * Time.deltaTime;
        break;
      case BobMob.DragonState.Landing:
        if (this.landingNode >= 2)
          break;
        Vector3 position = Boat.Instance.landingNodes[this.landingNode].position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation((position - this.transform.position).normalized), Time.deltaTime * 6f);
        this.transform.position += this.transform.forward * this.speed * 1.3f * Time.deltaTime;
        if ((double) Vector3.Distance(this.transform.position, position) < 10.0)
          ++this.landingNode;
        if (this.landingNode <= 1)
          break;
        this.state = BobMob.DragonState.Grounded;
        CameraShaker.Instance.StepShake(1f);
        break;
      case BobMob.DragonState.Grounded:
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(Boat.Instance.dragonLandingPosition.forward.normalized), Time.deltaTime * 2f);
        this.transform.position = Vector3.Lerp(this.transform.position, this.desiredPos, Time.deltaTime * 2f);
        this.animator.SetBool("Landed", true);
        break;
    }
  }

  private void LateUpdate()
  {
  }

  public override void Attack(int targetPlayerId, int attackAnimationIndex)
  {
    this.Invoke("FinishAttacking", this.attackTimes[attackAnimationIndex]);
    this.animator.Play(this.attackAnimations[attackAnimationIndex].name);
    this.targetPlayerId = targetPlayerId;
  }

  protected override void FinishAttacking()
  {
  }

  public override void SetDestination(Vector3 dest) => this.desiredPos = dest;

  public enum DragonState
  {
    Flying,
    Landing,
    Grounded,
  }
}
