// Decompiled with JetBrains decompiler
// Type: Mob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour, SharedObject
{
  public MobType mobType;
  public float attackCooldown;
  public int id;
  public bool stopOnAttack;
  private bool attacking;
  public Mob.BossType bossType;
  public AnimationClip[] attackAnimations;
  public GameObject footstepFx;
  private float distance;
  public float footstepFrequency = 1f;
  public bool knocked;
  private float defaulAngularSpeed;
  private float oldAccel;
  private float oldAngularSpeed;
  public int nRangedAttacks;
  private Mob.AttackType currentAttackType;
  private Vector3 offsetPosition;
  private Vector3 offsetDir;
  private float syncSpeed = 5f;

  public Transform target { get; set; }

  public int targetPlayerId { get; set; }

  public bool ready { get; set; } = true;

  public float multiplier { get; set; } = 1f;

  public float bossMultiplier { get; set; } = 1f;

  public Animator animator { get; protected set; }

  public NavMeshAgent agent { get; private set; }

  public Hitable hitable { get; protected set; }

  private void TestSpawn()
  {
    this.id = MobManager.Instance.GetNextId();
    MobManager.Instance.AddMob(this, this.id);
  }

  public bool IsBuff() => (double) this.multiplier > 1.0;

  private void Start()
  {
    if (!this.IsBuff())
      return;
    this.transform.localScale *= 1.4f;
  }

  public void SetSpeed(float multiplier) => this.agent.speed = this.mobType.speed * multiplier;

  private void Awake()
  {
    this.hitable = this.GetComponent<Hitable>();
    this.agent = this.GetComponent<NavMeshAgent>();
    this.agent.speed = this.mobType.speed;
    this.animator = this.GetComponent<Animator>();
    this.defaulAngularSpeed = this.agent.angularSpeed;
    if (LocalClient.serverOwner)
    {
      if (this.mobType.behaviour == MobType.MobBehaviour.Enemy)
        this.gameObject.AddComponent<MobServerEnemy>();
      else if (this.mobType.behaviour == MobType.MobBehaviour.Neutral)
        this.gameObject.AddComponent<MobServerNeutral>();
      else if (this.mobType.behaviour == MobType.MobBehaviour.EnemyMeleeAndRanged)
        this.gameObject.AddComponent<MobServerEnemyMeleeAndRanged>();
    }
    this.attackTimes = new float[this.attackAnimations.Length];
    for (int index = 0; index < this.attackAnimations.Length; ++index)
      this.attackTimes[index] = this.attackAnimations[index].length;
  }

  public float[] attackTimes { get; set; }

  private void Update()
  {
    this.Animate();
    this.ExtraUpdate();
    this.FootSteps();
    this.UpdateOffsetPosition();
  }

  private void FootSteps()
  {
    if (!(bool) (Object) this.footstepFx || (double) Vector3.Distance(PlayerMovement.Instance.playerCam.transform.position, this.transform.position) > 50.0)
      return;
    float num = this.agent.velocity.magnitude * 3f;
    if ((double) num > 20.0)
      num = 20f;
    this.distance += (float) ((double) num * (double) Time.deltaTime * 50.0) * this.footstepFrequency;
    if ((double) this.distance <= 300.0 / (double) this.footstepFrequency)
      return;
    Object.Instantiate<GameObject>(this.footstepFx, this.transform.position, Quaternion.identity);
    this.distance = 0.0f;
  }

  public virtual void ExtraUpdate()
  {
  }

  public void Knockback(Vector3 dir)
  {
    this.CancelInvoke("StopKnockback");
    this.oldAngularSpeed = this.agent.angularSpeed;
    this.agent.destination = this.transform.position + dir * 6f;
    this.animator.SetBool(nameof (Knockback), true);
    this.knocked = true;
    this.agent.velocity += dir * 10f;
    this.agent.angularSpeed = 0.0f;
    this.agent.updateRotation = false;
    this.Invoke("StopKnockback", 0.75f);
  }

  private void StopKnockback()
  {
    this.animator.SetBool("Knockback", false);
    this.agent.velocity = Vector3.zero;
    this.knocked = false;
    this.agent.angularSpeed = this.defaulAngularSpeed;
    this.agent.updateRotation = true;
  }

  private void LateUpdate()
  {
    if (!(bool) (Object) this.target)
      return;
    float num = Vector3.Distance(this.transform.position, this.target.position);
    if (this.attacking || (double) num >= (double) this.agent.stoppingDistance)
      return;
    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(VectorExtensions.XZVector(this.target.transform.position - this.transform.position)), Time.deltaTime * 6f);
  }

  public bool IsAttacking() => this.attacking;

  public bool IsRangedAttacking() => this.currentAttackType == Mob.AttackType.Ranged;

  public virtual void Attack(int targetPlayerId, int attackAnimationIndex)
  {
    MonoBehaviour.print((object) ("attacking. stoponattack: " + this.stopOnAttack.ToString()));
    if (this.stopOnAttack)
    {
      this.agent.isStopped = true;
      this.attacking = true;
    }
    this.currentAttackType = attackAnimationIndex < this.attackAnimations.Length - this.nRangedAttacks ? Mob.AttackType.Melee : Mob.AttackType.Ranged;
    this.Invoke("FinishAttacking", this.attackTimes[attackAnimationIndex]);
    this.animator.Play(this.attackAnimations[attackAnimationIndex].name);
    this.targetPlayerId = targetPlayerId;
  }

  protected virtual void FinishAttacking()
  {
    this.attacking = false;
    this.currentAttackType = Mob.AttackType.Melee;
    if (!this.agent.isOnNavMesh)
      return;
    this.agent.isStopped = false;
  }

  protected virtual void Animate()
  {
    if (!(bool) (Object) this.animator)
      return;
    this.animator.SetFloat("Speed", this.agent.velocity.magnitude / this.agent.speed);
  }

  public virtual void SetDestination(Vector3 dest)
  {
    if (!this.agent.isOnNavMesh)
      return;
    this.agent.destination = dest;
    this.agent.isStopped = false;
  }

  public virtual void SetTarget(int targetId)
  {
    if (!this.agent.isOnNavMesh)
      return;
    this.targetPlayerId = targetId;
    this.target = GameManager.players[this.targetPlayerId].transform;
  }

  public void SetPosition(Vector3 nextPosition)
  {
    Debug.DrawLine(this.transform.position, nextPosition, Color.red, 10f);
    this.offsetPosition = nextPosition - this.transform.position;
    this.offsetDir = this.offsetPosition.normalized;
  }

  private void UpdateOffsetPosition()
  {
    if ((double) this.offsetPosition.x <= 0.0)
      return;
    Vector3 vector3 = this.offsetDir * this.syncSpeed * Time.deltaTime;
    this.offsetPosition -= vector3;
    if ((double) this.offsetPosition.x < 0.0)
      vector3 -= this.offsetPosition;
    this.transform.position += vector3;
  }

  public void SetId(int id)
  {
    this.id = id;
    this.hitable.SetId(id);
  }

  public int GetId() => this.id;

  public enum AttackType
  {
    Melee,
    Ranged,
  }

  public enum BossType
  {
    None,
    BossNight,
    BossShrine,
  }
}
