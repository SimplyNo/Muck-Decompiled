// Decompiled with JetBrains decompiler
// Type: PlayerMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public GameObject playerJumpSmokeFx;
  public GameObject footstepFx;
  public Transform playerCam;
  public Transform orientation;
  private Rigidbody rb;
  public bool dead;
  private float moveSpeed;
  private float maxWalkSpeed;
  private float maxRunSpeed;
  private float maxSpeed;
  public bool grounded;
  public LayerMask whatIsGround;
  public float extraGravity;
  private Vector3 crouchScale;
  private Vector3 playerScale;
  private float slideForce;
  private float slideCounterMovement;
  private bool readyToJump;
  private float jumpCooldown;
  private float jumpForce;
  private int jumps;
  private float x;
  private float y;
  private float mouseDeltaX;
  private float mouseDeltaY;
  private bool jumping;
  private bool sliding;
  private bool crouching;
  private Vector3 normalVector;
  public ParticleSystem ps;
  private ParticleSystem.EmissionModule psEmission;
  private Collider playerCollider;
  private float fallSpeed;
  public GameObject playerSmokeFx;
  private PlayerStatus playerStatus;
  private float distance;
  private bool onRamp;
  private int extraJumps;
  private int resetJumpCounter;
  private int jumpCounterResetTime;
  private float counterMovement;
  private float threshold;
  private int readyToCounterX;
  private int readyToCounterY;
  private bool cancelling;
  private float maxSlopeAngle;
  private bool airborne;
  private bool onGround;
  private bool surfing;
  private bool cancellingGrounded;
  private bool cancellingSurf;
  private float delay;
  private int groundCancel;
  private int wallCancel;
  private int surfCancel;
  public LayerMask whatIsHittable;
  private float vel;

  public bool sprinting { get; private set; }

  public static PlayerMovement Instance { get; private set; }

  private void Awake()
  {
    PlayerMovement.Instance = this;
    this.rb = (Rigidbody) ((Component) this).GetComponent<Rigidbody>();
    this.playerStatus = (PlayerStatus) ((Component) this).GetComponent<PlayerStatus>();
  }

  private void Start()
  {
    this.playerScale = ((Component) this).get_transform().get_localScale();
    this.playerCollider = (Collider) ((Component) this).GetComponent<Collider>();
    Cursor.set_lockState((CursorLockMode) 1);
    Cursor.set_visible(false);
  }

  private void Update()
  {
    if (this.dead)
      return;
    this.FootSteps();
    this.fallSpeed = (float) this.rb.get_velocity().y;
  }

  public void SetInput(Vector2 dir, bool crouching, bool jumping, bool sprinting)
  {
    this.x = (float) dir.x;
    this.y = (float) dir.y;
    this.crouching = crouching;
    this.jumping = jumping;
    this.sprinting = sprinting;
  }

  private void CheckInput()
  {
    if (this.crouching && !this.sliding)
      this.StartCrouch();
    if (!this.crouching && this.sliding)
      this.StopCrouch();
    if (this.sprinting && this.playerStatus.CanRun())
      this.maxSpeed = this.maxRunSpeed;
    else
      this.maxSpeed = this.maxWalkSpeed;
  }

  public void StartCrouch()
  {
    if (this.sliding)
      return;
    this.sliding = true;
    ((Component) this).get_transform().set_localScale(this.crouchScale);
    ((Component) this).get_transform().set_position(new Vector3((float) ((Component) this).get_transform().get_position().x, (float) (((Component) this).get_transform().get_position().y - 0.649999976158142), (float) ((Component) this).get_transform().get_position().z));
    Vector3 velocity = this.rb.get_velocity();
    if ((double) ((Vector3) ref velocity).get_magnitude() <= 0.5 || !this.grounded)
      return;
    this.rb.AddForce(Vector3.op_Multiply(((Component) this.orientation).get_transform().get_forward(), this.slideForce));
  }

  public void StopCrouch()
  {
    this.sliding = false;
    ((Component) this).get_transform().set_localScale(this.playerScale);
    ((Component) this).get_transform().set_position(new Vector3((float) ((Component) this).get_transform().get_position().x, (float) (((Component) this).get_transform().get_position().y + 0.649999976158142), (float) ((Component) this).get_transform().get_position().z));
  }

  private void FootSteps()
  {
    if (this.crouching || this.dead || !this.grounded)
      return;
    float num1 = 1f;
    Vector3 velocity = this.rb.get_velocity();
    float num2 = ((Vector3) ref velocity).get_magnitude();
    if ((double) num2 > 20.0)
      num2 = 20f;
    this.distance += (float) ((double) num2 * (double) Time.get_deltaTime() * 50.0);
    if ((double) this.distance <= 300.0 / (double) num1)
      return;
    Object.Instantiate<GameObject>((M0) this.footstepFx, ((Component) this).get_transform().get_position(), Quaternion.get_identity());
    this.distance = 0.0f;
  }

  public void Movement(float x, float y)
  {
    this.UpdateCollisionChecks();
    this.x = x;
    this.y = y;
    if (this.dead)
      return;
    this.CheckInput();
    if (WorldUtility.WorldHeightToBiome((float) (((Component) this).get_transform().get_position().y + 1.60000002384186)) == TextureData.TerrainType.Water)
      this.maxSpeed *= 0.4f;
    if (!this.grounded)
      this.rb.AddForce(Vector3.op_Multiply(Vector3.get_down(), this.extraGravity));
    Vector2 velRelativeToLook = this.FindVelRelativeToLook();
    float x1 = (float) velRelativeToLook.x;
    float y1 = (float) velRelativeToLook.y;
    this.CounterMovement(x, y, velRelativeToLook);
    this.RampMovement(velRelativeToLook);
    if (this.readyToJump && this.jumping && this.grounded)
      this.Jump();
    if (this.crouching && this.grounded && this.readyToJump)
    {
      this.rb.AddForce(Vector3.op_Multiply(Vector3.get_down(), 60f));
    }
    else
    {
      float num1 = x;
      float num2 = y;
      float num3 = this.maxSpeed * PowerupInventory.Instance.GetSpeedMultiplier((int[]) null);
      if ((double) x > 0.0 && (double) x1 > (double) num3)
        num1 = 0.0f;
      if ((double) x < 0.0 && (double) x1 < -(double) num3)
        num1 = 0.0f;
      if ((double) y > 0.0 && (double) y1 > (double) num3)
        num2 = 0.0f;
      if ((double) y < 0.0 && (double) y1 < -(double) num3)
        num2 = 0.0f;
      float num4 = 1f;
      float num5 = 1f;
      if (!this.grounded)
      {
        num4 = 0.2f;
        num5 = 0.2f;
        if (this.IsHoldingAgainstVerticalVel(velRelativeToLook))
        {
          float num6 = Mathf.Abs((float) (velRelativeToLook.y * 0.025000000372529));
          if ((double) num6 < 0.5)
            num6 = 0.5f;
          num5 = Mathf.Abs(num6);
        }
      }
      if (this.grounded && this.crouching)
        num5 = 0.0f;
      if (this.surfing)
      {
        num4 = 0.6f;
        num5 = 0.3f;
      }
      float num7 = 0.01f;
      this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.orientation.get_forward(), num2), this.moveSpeed), 0.02f), num5));
      this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.orientation.get_right(), num1), this.moveSpeed), 0.02f), num4));
      if (!this.grounded)
      {
        if ((double) num1 != 0.0)
          this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.orientation.get_forward()), (float) velRelativeToLook.y), this.moveSpeed), 0.02f), num7));
        if ((double) num2 != 0.0)
          this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(this.orientation.get_right()), (float) velRelativeToLook.x), this.moveSpeed), 0.02f), num7));
      }
      if (this.readyToJump)
        return;
      ++this.resetJumpCounter;
      if (this.resetJumpCounter < this.jumpCounterResetTime)
        return;
      this.ResetJump();
    }
  }

  private void RampMovement(Vector2 mag)
  {
    if (this.grounded && this.onRamp && (!this.surfing && !this.crouching) && (!this.jumping && this.resetJumpCounter >= this.jumpCounterResetTime && ((double) Math.Abs(this.x) < 0.0500000007450581 && (double) Math.Abs(this.y) < 0.0500000007450581)))
    {
      this.rb.set_useGravity(false);
      if (this.rb.get_velocity().y > 0.0)
      {
        this.rb.set_velocity(new Vector3((float) this.rb.get_velocity().x, 0.0f, (float) this.rb.get_velocity().z));
      }
      else
      {
        if (this.rb.get_velocity().y > 0.0 || (double) Math.Abs(((Vector2) ref mag).get_magnitude()) >= 1.0)
          return;
        this.rb.set_velocity(Vector3.get_zero());
      }
    }
    else
      this.rb.set_useGravity(true);
  }

  private void ResetJump()
  {
    this.readyToJump = true;
    this.CancelInvoke("JumpCooldown");
  }

  public void Jump()
  {
    if (!this.grounded && !this.surfing && (this.grounded || this.jumps <= 0) || (!this.readyToJump || !this.playerStatus.CanJump()))
      return;
    if (this.grounded)
      this.jumps = PowerupInventory.Instance.GetExtraJumps();
    this.rb.set_isKinematic(false);
    if (!this.grounded)
      --this.jumps;
    this.readyToJump = false;
    this.CancelInvoke("JumpCooldown");
    this.Invoke("JumpCooldown", 0.25f);
    this.resetJumpCounter = 0;
    float num = this.jumpForce * PowerupInventory.Instance.GetJumpMultiplier();
    this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_up(), num), 1.5f), (ForceMode) 1);
    this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(this.normalVector, num), 0.5f), (ForceMode) 1);
    Vector3 velocity = this.rb.get_velocity();
    if (this.rb.get_velocity().y < 0.5)
      this.rb.set_velocity(new Vector3((float) velocity.x, 0.0f, (float) velocity.z));
    else if (this.rb.get_velocity().y > 0.0)
      this.rb.set_velocity(new Vector3((float) velocity.x, 0.0f, (float) velocity.z));
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = ((ParticleSystem) ((GameObject) Object.Instantiate<GameObject>((M0) this.playerJumpSmokeFx, ((Component) this).get_transform().get_position(), Quaternion.LookRotation(Vector3.get_up()))).GetComponent<ParticleSystem>()).get_velocityOverLifetime();
    ((ParticleSystem.VelocityOverLifetimeModule) ref velocityOverLifetime).set_x(ParticleSystem.MinMaxCurve.op_Implicit((float) (this.rb.get_velocity().x * 2.0)));
    ((ParticleSystem.VelocityOverLifetimeModule) ref velocityOverLifetime).set_z(ParticleSystem.MinMaxCurve.op_Implicit((float) (this.rb.get_velocity().z * 2.0)));
    this.playerStatus.Jump();
  }

  private void JumpCooldown() => this.readyToJump = true;

  private void CounterMovement(float x, float y, Vector2 mag)
  {
    if ((double) x == 0.0 && (double) y == 0.0)
    {
      Vector3 velocity = this.rb.get_velocity();
      if ((double) ((Vector3) ref velocity).get_magnitude() < 1.0 && this.grounded && (!this.jumping && this.playerStatus.CanJump()))
      {
        this.rb.set_isKinematic(true);
        goto label_4;
      }
    }
    this.rb.set_isKinematic(false);
label_4:
    if (!this.grounded || this.jumping && this.playerStatus.CanJump())
      return;
    if (this.crouching)
    {
      Rigidbody rb = this.rb;
      double num = (double) this.moveSpeed * 0.0199999995529652;
      Vector3 velocity = this.rb.get_velocity();
      Vector3 vector3_1 = Vector3.op_UnaryNegation(((Vector3) ref velocity).get_normalized());
      Vector3 vector3_2 = Vector3.op_Multiply(Vector3.op_Multiply((float) num, vector3_1), this.slideCounterMovement);
      rb.AddForce(vector3_2);
    }
    else
    {
      if ((double) Math.Abs((float) mag.x) > (double) this.threshold && (double) Math.Abs(x) < 0.0500000007450581 && this.readyToCounterX > 1)
        this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.moveSpeed, ((Component) this.orientation).get_transform().get_right()), 0.02f), (float) -mag.x), this.counterMovement));
      if ((double) Math.Abs((float) mag.y) > (double) this.threshold && (double) Math.Abs(y) < 0.0500000007450581 && this.readyToCounterY > 1)
        this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.moveSpeed, ((Component) this.orientation).get_transform().get_forward()), 0.02f), (float) -mag.y), this.counterMovement));
      if (this.IsHoldingAgainstHorizontalVel(mag))
        this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.moveSpeed, ((Component) this.orientation).get_transform().get_right()), 0.02f), (float) -mag.x), this.counterMovement), 2f));
      if (this.IsHoldingAgainstVerticalVel(mag))
        this.rb.AddForce(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(this.moveSpeed, ((Component) this.orientation).get_transform().get_forward()), 0.02f), (float) -mag.y), this.counterMovement), 2f));
      if ((double) Mathf.Sqrt(Mathf.Pow((float) this.rb.get_velocity().x, 2f) + Mathf.Pow((float) this.rb.get_velocity().z, 2f)) > (double) this.maxSpeed * (double) PowerupInventory.Instance.GetSpeedMultiplier((int[]) null))
      {
        float y1 = (float) this.rb.get_velocity().y;
        Vector3 velocity = this.rb.get_velocity();
        Vector3 vector3 = Vector3.op_Multiply(Vector3.op_Multiply(((Vector3) ref velocity).get_normalized(), this.maxSpeed), PowerupInventory.Instance.GetSpeedMultiplier((int[]) null));
        this.rb.set_velocity(new Vector3((float) vector3.x, y1, (float) vector3.z));
      }
      if ((double) Math.Abs(x) < 0.0500000007450581)
        ++this.readyToCounterX;
      else
        this.readyToCounterX = 0;
      if ((double) Math.Abs(y) < 0.0500000007450581)
        ++this.readyToCounterY;
      else
        this.readyToCounterY = 0;
    }
  }

  private bool IsHoldingAgainstHorizontalVel(Vector2 vel)
  {
    if (vel.x < -(double) this.threshold && (double) this.x > 0.0)
      return true;
    return vel.x > (double) this.threshold && (double) this.x < 0.0;
  }

  private bool IsHoldingAgainstVerticalVel(Vector2 vel)
  {
    if (vel.y < -(double) this.threshold && (double) this.y > 0.0)
      return true;
    return vel.y > (double) this.threshold && (double) this.y < 0.0;
  }

  public Vector2 FindVelRelativeToLook()
  {
    float num1 = Mathf.DeltaAngle((float) ((Component) this.orientation).get_transform().get_eulerAngles().y, Mathf.Atan2((float) this.rb.get_velocity().x, (float) this.rb.get_velocity().z) * 57.29578f);
    float num2 = 90f - num1;
    Vector2 vector2 = new Vector2((float) this.rb.get_velocity().x, (float) this.rb.get_velocity().z);
    double magnitude = (double) ((Vector2) ref vector2).get_magnitude();
    float num3 = (float) magnitude * Mathf.Cos(num1 * ((float) Math.PI / 180f));
    return new Vector2((float) magnitude * Mathf.Cos(num2 * ((float) Math.PI / 180f)), num3);
  }

  private bool IsFloor(Vector3 v) => (double) Vector3.Angle(Vector3.get_up(), v) < (double) this.maxSlopeAngle;

  private bool IsSurf(Vector3 v)
  {
    float num = Vector3.Angle(Vector3.get_up(), v);
    return (double) num < 89.0 && (double) num > (double) this.maxSlopeAngle;
  }

  private bool IsWall(Vector3 v) => (double) Math.Abs(90f - Vector3.Angle(Vector3.get_up(), v)) < 0.100000001490116;

  private bool IsRoof(Vector3 v) => v.y == -1.0;

  private void OnCollisionEnter(Collision other)
  {
    int layer = other.get_gameObject().get_layer();
    Vector3 normal = ((ContactPoint) ref other.get_contacts()[0]).get_normal();
    if (LayerMask.op_Implicit(this.whatIsGround) != (LayerMask.op_Implicit(this.whatIsGround) | 1 << layer) || !this.IsFloor(normal) || (double) this.fallSpeed >= -12.0)
      return;
    MoveCamera.Instance.BobOnce(new Vector3(0.0f, this.fallSpeed, 0.0f));
    Vector3 point = ((ContactPoint) ref other.get_contacts()[0]).get_point();
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = ((ParticleSystem) ((GameObject) Object.Instantiate<GameObject>((M0) this.playerSmokeFx, point, Quaternion.LookRotation(Vector3.op_Subtraction(((Component) this).get_transform().get_position(), point)))).GetComponent<ParticleSystem>()).get_velocityOverLifetime();
    ((ParticleSystem.VelocityOverLifetimeModule) ref velocityOverLifetime).set_x(ParticleSystem.MinMaxCurve.op_Implicit((float) (this.rb.get_velocity().x * 2.0)));
    ((ParticleSystem.VelocityOverLifetimeModule) ref velocityOverLifetime).set_z(ParticleSystem.MinMaxCurve.op_Implicit((float) (this.rb.get_velocity().z * 2.0)));
  }

  private void OnCollisionStay(Collision other)
  {
    int layer = other.get_gameObject().get_layer();
    if (LayerMask.op_Implicit(this.whatIsGround) != (LayerMask.op_Implicit(this.whatIsGround) | 1 << layer))
      return;
    for (int index = 0; index < other.get_contactCount(); ++index)
    {
      Vector3 normal = ((ContactPoint) ref other.get_contacts()[index]).get_normal();
      if (this.IsFloor(normal))
      {
        if (!this.grounded)
        {
          int num = this.crouching ? 1 : 0;
        }
        this.onRamp = (double) Vector3.Angle(Vector3.get_up(), normal) > 1.0;
        this.grounded = true;
        this.normalVector = normal;
        this.cancellingGrounded = false;
        this.groundCancel = 0;
      }
      if (this.IsSurf(normal))
      {
        this.surfing = true;
        this.cancellingSurf = false;
        this.surfCancel = 0;
      }
    }
  }

  private void UpdateCollisionChecks()
  {
    if (!this.cancellingGrounded)
    {
      this.cancellingGrounded = true;
    }
    else
    {
      ++this.groundCancel;
      if ((double) this.groundCancel > (double) this.delay)
        this.StopGrounded();
    }
    if (!this.cancellingSurf)
    {
      this.cancellingSurf = true;
      this.surfCancel = 1;
    }
    else
    {
      ++this.surfCancel;
      if ((double) this.surfCancel <= (double) this.delay)
        return;
      this.StopSurf();
    }
  }

  private void StopGrounded() => this.grounded = false;

  private void StopSurf() => this.surfing = false;

  public Vector3 GetVelocity() => this.rb.get_velocity();

  public float GetFallSpeed() => (float) this.rb.get_velocity().y;

  public Collider GetPlayerCollider() => this.playerCollider;

  public Transform GetPlayerCamTransform() => ((Component) this.playerCam).get_transform();

  public Vector3 HitPoint()
  {
    RaycastHit[] raycastHitArray = Physics.RaycastAll(((Component) this.playerCam).get_transform().get_position(), ((Component) this.playerCam).get_transform().get_forward(), 100f, LayerMask.op_Implicit(this.whatIsHittable));
    if (raycastHitArray.Length < 1)
      return Vector3.op_Addition(((Component) this.playerCam).get_transform().get_position(), Vector3.op_Multiply(((Component) this.playerCam).get_transform().get_forward(), 100f));
    if (raycastHitArray.Length > 1)
    {
      for (int index = 0; index < raycastHitArray.Length; ++index)
      {
        if (((Component) ((RaycastHit) ref raycastHitArray[index]).get_transform()).get_gameObject().get_layer() == LayerMask.NameToLayer("Enemy") || ((Component) ((RaycastHit) ref raycastHitArray[index]).get_transform()).get_gameObject().get_layer() == LayerMask.NameToLayer("Object") || ((Component) ((RaycastHit) ref raycastHitArray[index]).get_transform()).get_gameObject().get_layer() == LayerMask.NameToLayer("Ground"))
          return ((RaycastHit) ref raycastHitArray[index]).get_point();
      }
    }
    return ((RaycastHit) ref raycastHitArray[0]).get_point();
  }

  public bool IsCrouching() => this.crouching;

  public bool IsDead() => this.dead;

  public Rigidbody GetRb() => this.rb;

  public PlayerMovement() => base.\u002Ector();
}
