// Decompiled with JetBrains decompiler
// Type: Mob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
  public float footstepFrequency;
  public bool knocked;
  private float defaulAngularSpeed;
  private float oldAccel;
  private float oldAngularSpeed;
  public int nRangedAttacks;
  private Mob.AttackType currentAttackType;
  private Vector3 offsetPosition;
  private Vector3 offsetDir;
  private float syncSpeed;

  public Transform target { get; set; }

  public int targetPlayerId { get; set; }

  public bool ready { get; set; }

  public float multiplier { get; set; }

  public float bossMultiplier { get; set; }

  public Animator animator { get; private set; }

  public NavMeshAgent agent { get; private set; }

  public Hitable hitable { get; private set; }

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
    MonoBehaviour.print((object) "dangerous mob oOoOOo spooky");
    Transform transform = ((Component) this).get_transform();
    transform.set_localScale(Vector3.op_Multiply(transform.get_localScale(), 1.4f));
  }

  public void SetSpeed(float multiplier) => this.agent.set_speed(this.mobType.speed * multiplier);

  private void Awake()
  {
    this.hitable = (Hitable) ((Component) this).GetComponent<Hitable>();
    this.agent = (NavMeshAgent) ((Component) this).GetComponent<NavMeshAgent>();
    this.agent.set_speed(this.mobType.speed);
    this.animator = (Animator) ((Component) this).GetComponent<Animator>();
    this.defaulAngularSpeed = this.agent.get_angularSpeed();
    if (LocalClient.serverOwner)
    {
      if (this.mobType.behaviour == MobType.MobBehaviour.Enemy)
        ((Component) this).get_gameObject().AddComponent<MobServerEnemy>();
      else if (this.mobType.behaviour == MobType.MobBehaviour.Neutral)
        ((Component) this).get_gameObject().AddComponent<MobServerNeutral>();
      else if (this.mobType.behaviour == MobType.MobBehaviour.EnemyMeleeAndRanged)
        ((Component) this).get_gameObject().AddComponent<MobServerEnemyMeleeAndRanged>();
    }
    this.attackTimes = new float[this.attackAnimations.Length];
    for (int index = 0; index < this.attackAnimations.Length; ++index)
      this.attackTimes[index] = this.attackAnimations[index].get_length();
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
    if (!Object.op_Implicit((Object) this.footstepFx) || (double) Vector3.Distance(((Component) PlayerMovement.Instance.playerCam).get_transform().get_position(), ((Component) this).get_transform().get_position()) > 50.0)
      return;
    Vector3 velocity = this.agent.get_velocity();
    float num = ((Vector3) ref velocity).get_magnitude() * 3f;
    if ((double) num > 20.0)
      num = 20f;
    this.distance += (float) ((double) num * (double) Time.get_deltaTime() * 50.0) * this.footstepFrequency;
    if ((double) this.distance <= 300.0 / (double) this.footstepFrequency)
      return;
    Object.Instantiate<GameObject>((M0) this.footstepFx, ((Component) this).get_transform().get_position(), Quaternion.get_identity());
    this.distance = 0.0f;
  }

  public virtual void ExtraUpdate()
  {
  }

  public void Knockback(Vector3 dir)
  {
    this.CancelInvoke("StopKnockback");
    this.oldAngularSpeed = this.agent.get_angularSpeed();
    this.agent.set_destination(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(dir, 6f)));
    this.animator.SetBool(nameof (Knockback), true);
    this.knocked = true;
    NavMeshAgent agent = this.agent;
    agent.set_velocity(Vector3.op_Addition(agent.get_velocity(), Vector3.op_Multiply(dir, 10f)));
    this.agent.set_angularSpeed(0.0f);
    this.agent.set_updateRotation(false);
    this.Invoke("StopKnockback", 0.75f);
  }

  private void StopKnockback()
  {
    this.animator.SetBool("Knockback", false);
    this.agent.set_velocity(Vector3.get_zero());
    this.knocked = false;
    this.agent.set_angularSpeed(this.defaulAngularSpeed);
    this.agent.set_updateRotation(true);
  }

  private void LateUpdate()
  {
    if (!Object.op_Implicit((Object) this.target))
      return;
    float num = Vector3.Distance(((Component) this).get_transform().get_position(), this.target.get_position());
    if (this.attacking || (double) num >= (double) this.agent.get_stoppingDistance())
      return;
    Quaternion quaternion = Quaternion.LookRotation(VectorExtensions.XZVector(Vector3.op_Subtraction(((Component) this.target).get_transform().get_position(), ((Component) this).get_transform().get_position())));
    ((Component) this).get_transform().set_rotation(Quaternion.Lerp(((Component) this).get_transform().get_rotation(), quaternion, Time.get_deltaTime() * 6f));
  }

  public bool IsAttacking() => this.attacking;

  public bool IsRangedAttacking() => this.currentAttackType == Mob.AttackType.Ranged;

  public void Attack(int targetPlayerId, int attackAnimationIndex)
  {
    MonoBehaviour.print((object) ("attacking. stoponattack: " + this.stopOnAttack.ToString()));
    if (this.stopOnAttack)
    {
      this.agent.set_isStopped(true);
      this.attacking = true;
    }
    this.currentAttackType = attackAnimationIndex < this.attackAnimations.Length - this.nRangedAttacks ? Mob.AttackType.Melee : Mob.AttackType.Ranged;
    this.Invoke("FinishAttacking", this.attackTimes[attackAnimationIndex]);
    this.animator.Play(((Object) this.attackAnimations[attackAnimationIndex]).get_name());
    this.targetPlayerId = targetPlayerId;
  }

  private void FinishAttacking()
  {
    this.attacking = false;
    this.currentAttackType = Mob.AttackType.Melee;
    if (!this.agent.get_isOnNavMesh())
      return;
    this.agent.set_isStopped(false);
  }

  private void Animate()
  {
    if (!Object.op_Implicit((Object) this.animator))
      return;
    Vector3 velocity = this.agent.get_velocity();
    this.animator.SetFloat("Speed", ((Vector3) ref velocity).get_magnitude() / this.agent.get_speed());
  }

  public void SetDestination(Vector3 dest)
  {
    if (!this.agent.get_isOnNavMesh())
      return;
    this.agent.set_destination(dest);
    this.agent.set_isStopped(false);
  }

  public void SetTarget(int targetId)
  {
    if (!this.agent.get_isOnNavMesh())
      return;
    this.targetPlayerId = targetId;
    this.target = ((Component) GameManager.players[this.targetPlayerId]).get_transform();
  }

  public void SetPosition(Vector3 nextPosition)
  {
    Debug.DrawLine(((Component) this).get_transform().get_position(), nextPosition, Color.get_red(), 10f);
    this.offsetPosition = Vector3.op_Subtraction(nextPosition, ((Component) this).get_transform().get_position());
    this.offsetDir = ((Vector3) ref this.offsetPosition).get_normalized();
  }

  private void UpdateOffsetPosition()
  {
    if (this.offsetPosition.x <= 0.0)
      return;
    Vector3 vector3 = Vector3.op_Multiply(Vector3.op_Multiply(this.offsetDir, this.syncSpeed), Time.get_deltaTime());
    this.offsetPosition = Vector3.op_Subtraction(this.offsetPosition, vector3);
    if (this.offsetPosition.x < 0.0)
      vector3 = Vector3.op_Subtraction(vector3, this.offsetPosition);
    Transform transform = ((Component) this).get_transform();
    transform.set_position(Vector3.op_Addition(transform.get_position(), vector3));
  }

  public void SetId(int id)
  {
    this.id = id;
    this.hitable.SetId(id);
  }

  public int GetId() => this.id;

  public Mob() => base.\u002Ector();

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
