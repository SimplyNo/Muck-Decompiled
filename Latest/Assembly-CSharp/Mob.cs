// Decompiled with JetBrains decompiler
// Type: Mob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
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
  public AnimationClip[] attackAnimations;
  public GameObject footstepFx;
  private float distance;
  public float footstepFrequency = 1f;
  public bool knocked;
  private float oldAccel;
  private float oldAngularSpeed;

  public Transform target { get; set; }

  public int targetPlayerId { get; set; }

  public float multiplier { get; set; } = 1f;

  public float bossMultiplier { get; set; } = 1f;

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
    this.transform.localScale *= 1.4f;
  }

  public void SetSpeed(float multiplier) => this.agent.speed = this.mobType.speed * multiplier;

  private void Awake()
  {
    this.hitable = this.GetComponent<Hitable>();
    this.agent = this.GetComponent<NavMeshAgent>();
    this.agent.speed = this.mobType.speed;
    this.animator = this.GetComponent<Animator>();
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
  }

  private void FootSteps()
  {
    if (!(bool) (Object) this.footstepFx || (double) Vector3.Distance(PlayerMovement.Instance.transform.position, this.transform.position) > 50.0)
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
    this.knocked = false;
    this.agent.angularSpeed = this.oldAngularSpeed;
    this.agent.updateRotation = true;
  }

  private void LateUpdate()
  {
  }

  public bool IsAttacking() => this.attacking;

  public void Attack(int targetPlayerId, int attackAnimationIndex)
  {
    MonoBehaviour.print((object) ("attacking. stoponattack: " + this.stopOnAttack.ToString()));
    if (this.stopOnAttack)
    {
      this.agent.isStopped = true;
      this.attacking = true;
    }
    MonoBehaviour.print((object) ("ataacking, cooldowntime: " + (object) this.attackTimes[attackAnimationIndex]));
    this.Invoke("FinishAttacking", this.attackTimes[attackAnimationIndex]);
    this.animator.Play(this.attackAnimations[attackAnimationIndex].name);
    this.targetPlayerId = targetPlayerId;
  }

  private void FinishAttacking()
  {
    this.attacking = false;
    if (!this.agent.isOnNavMesh)
      return;
    this.agent.isStopped = false;
  }

  private void Animate()
  {
    if (!(bool) (Object) this.animator)
      return;
    this.animator.SetFloat("Speed", this.agent.velocity.magnitude / this.agent.speed);
  }

  public void SetDestination(Vector3 dest)
  {
    if (!this.agent.isOnNavMesh)
      return;
    this.agent.destination = dest;
    this.agent.isStopped = false;
  }

  public void SetPosition(Vector3 nextPosition)
  {
    Debug.DrawLine(this.transform.position, nextPosition, Color.red, 10f);
    this.transform.position = nextPosition;
  }

  public void SetId(int id)
  {
    this.id = id;
    this.hitable.SetId(id);
  }

  public int GetId() => this.id;
}
