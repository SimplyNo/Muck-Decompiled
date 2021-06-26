// Decompiled with JetBrains decompiler
// Type: PlayerMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
  private float moveSpeed = 3500f;
  private float maxWalkSpeed = 6.5f;
  private float maxRunSpeed = 13f;
  private float maxSpeed = 6.5f;
  public bool grounded;
  public LayerMask whatIsGround;
  public float extraGravity = 5f;
  private Vector3 crouchScale = new Vector3(1f, 1.05f, 1f);
  private Vector3 playerScale;
  private float slideForce = 800f;
  private float slideCounterMovement = 0.12f;
  private bool readyToJump = true;
  private float jumpCooldown = 0.25f;
  private float jumpForce = 12f;
  private int jumps = 1;
  private float x;
  private float y;
  private float mouseDeltaX;
  private float mouseDeltaY;
  private Vector3 normalVector;
  public ParticleSystem ps;
  private ParticleSystem.EmissionModule psEmission;
  private Collider playerCollider;
  private float fallSpeed;
  public GameObject playerSmokeFx;
  private PlayerStatus playerStatus;
  private float distance;
  private float swimSpeed = 50f;
  private bool pushed;
  private bool onRamp;
  private int extraJumps;
  private int resetJumpCounter;
  private int jumpCounterResetTime = 10;
  private float counterMovement = 0.14f;
  private float threshold = 0.01f;
  private int readyToCounterX;
  private int readyToCounterY;
  private bool cancelling;
  private float maxSlopeAngle = 50f;
  private bool airborne;
  private bool onGround;
  private bool surfing;
  private bool cancellingGrounded;
  private bool cancellingSurf;
  private float delay = 5f;
  private int groundCancel;
  private int wallCancel;
  private int surfCancel;
  public LayerMask whatIsHittable;
  private float vel;

  public bool jumping { get; set; }

  public bool sliding { get; set; }

  public bool crouching { get; set; }

  public bool sprinting { get; private set; }

  public static PlayerMovement Instance { get; private set; }

  private void Awake()
  {
    PlayerMovement.Instance = this;
    this.rb = this.GetComponent<Rigidbody>();
    this.playerStatus = this.GetComponent<PlayerStatus>();
  }

  private void Start()
  {
    this.playerScale = this.transform.localScale;
    this.playerCollider = this.GetComponent<Collider>();
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  private void Update()
  {
    if (this.dead)
      return;
    this.FootSteps();
    this.fallSpeed = this.rb.velocity.y;
  }

  public Vector2 GetInput() => new Vector2(this.x, this.y);

  public void SetInput(Vector2 dir, bool crouching, bool jumping, bool sprinting)
  {
    this.x = dir.x;
    this.y = dir.y;
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
    this.transform.localScale = this.crouchScale;
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.65f, this.transform.position.z);
    if ((double) this.rb.velocity.magnitude <= 0.5 || !this.grounded)
      return;
    this.rb.AddForce(this.orientation.transform.forward * this.slideForce);
  }

  public void StopCrouch()
  {
    this.sliding = false;
    this.transform.localScale = this.playerScale;
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.65f, this.transform.position.z);
  }

  private void FootSteps()
  {
    if (this.crouching || this.dead || !this.grounded)
      return;
    float num1 = 1f;
    float num2 = this.rb.velocity.magnitude;
    if ((double) num2 > 20.0)
      num2 = 20f;
    this.distance += (float) ((double) num2 * (double) Time.deltaTime * 50.0);
    if ((double) this.distance <= 300.0 / (double) num1)
      return;
    UnityEngine.Object.Instantiate<GameObject>(this.footstepFx, this.transform.position, Quaternion.identity);
    this.distance = 0.0f;
  }

  private void WaterMovement()
  {
    float num1 = 1f;
    if (this.jumping)
      num1 *= 2f;
    this.rb.AddForce(Vector3.up * this.rb.mass * -Physics.gravity.y * num1);
    float num2 = 1f;
    if ((double) PlayerStatus.Instance.stamina <= 0.0)
      num2 = 0.5f;
    this.rb.AddForce(this.playerCam.transform.forward * this.y * this.swimSpeed * num2);
    this.rb.AddForce(this.orientation.transform.right * this.x * this.swimSpeed * num2);
  }

  public bool IsUnderWater() => (double) this.transform.position.y < (double) World.Instance.water.position.y;

  public void Movement(float x, float y)
  {
    this.UpdateCollisionChecks();
    this.x = x;
    this.y = y;
    if (this.dead)
      return;
    this.CheckInput();
    if (WorldUtility.WorldHeightToBiome(this.transform.position.y + 3.2f) == TextureData.TerrainType.Water)
      this.maxSpeed *= 0.4f;
    if (this.IsUnderWater())
    {
      if ((double) this.rb.drag <= 0.0)
        this.rb.drag = 1f;
      this.WaterMovement();
    }
    else
    {
      if ((double) this.rb.drag > 0.0)
        this.rb.drag = 0.0f;
      if (!this.grounded)
        this.rb.AddForce(Vector3.down * this.extraGravity);
      Vector2 velRelativeToLook = this.FindVelRelativeToLook();
      float x1 = velRelativeToLook.x;
      float y1 = velRelativeToLook.y;
      this.CounterMovement(x, y, velRelativeToLook);
      this.RampMovement(velRelativeToLook);
      if (this.readyToJump && this.jumping && this.grounded)
        this.Jump();
      if (this.crouching && this.grounded && this.readyToJump)
      {
        this.rb.AddForce(Vector3.down * 60f);
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
            float f = Mathf.Abs(velRelativeToLook.y * 0.025f);
            if ((double) f < 0.5)
              f = 0.5f;
            num5 = Mathf.Abs(f);
          }
        }
        if (this.grounded && this.crouching)
          num5 = 0.0f;
        if (this.surfing)
        {
          num4 = 0.6f;
          num5 = 0.3f;
        }
        float num6 = 0.01f;
        this.rb.AddForce(this.orientation.forward * num2 * this.moveSpeed * 0.02f * num5);
        this.rb.AddForce(this.orientation.right * num1 * this.moveSpeed * 0.02f * num4);
        if (!this.grounded)
        {
          if ((double) num1 != 0.0)
            this.rb.AddForce(-this.orientation.forward * velRelativeToLook.y * this.moveSpeed * 0.02f * num6);
          if ((double) num2 != 0.0)
            this.rb.AddForce(-this.orientation.right * velRelativeToLook.x * this.moveSpeed * 0.02f * num6);
        }
        if (this.readyToJump)
          return;
        ++this.resetJumpCounter;
        if (this.resetJumpCounter < this.jumpCounterResetTime)
          return;
        this.ResetJump();
      }
    }
  }

  public void PushPlayer()
  {
    this.pushed = true;
    this.Invoke("ResetPush", 0.3f);
  }

  private void ResetPush() => this.pushed = false;

  private void RampMovement(Vector2 mag)
  {
    if (this.grounded && this.onRamp && (!this.surfing && !this.crouching) && (!this.jumping && this.resetJumpCounter >= this.jumpCounterResetTime && ((double) Math.Abs(this.x) < 0.0500000007450581 && (double) Math.Abs(this.y) < 0.0500000007450581)) && !this.pushed)
    {
      this.rb.useGravity = false;
      if ((double) this.rb.velocity.y > 0.0)
      {
        this.rb.velocity = new Vector3(this.rb.velocity.x, 0.0f, this.rb.velocity.z);
      }
      else
      {
        if ((double) this.rb.velocity.y > 0.0 || (double) Math.Abs(mag.magnitude) >= 1.0)
          return;
        this.rb.velocity = Vector3.zero;
      }
    }
    else
      this.rb.useGravity = true;
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
    this.rb.isKinematic = false;
    if (!this.grounded)
      --this.jumps;
    this.readyToJump = false;
    this.CancelInvoke("JumpCooldown");
    this.Invoke("JumpCooldown", 0.25f);
    this.resetJumpCounter = 0;
    float num = this.jumpForce * PowerupInventory.Instance.GetJumpMultiplier();
    this.rb.AddForce(Vector3.up * num * 1.5f, ForceMode.Impulse);
    this.rb.AddForce(this.normalVector * num * 0.5f, ForceMode.Impulse);
    Vector3 velocity = this.rb.velocity;
    if ((double) this.rb.velocity.y < 0.5)
      this.rb.velocity = new Vector3(velocity.x, 0.0f, velocity.z);
    else if ((double) this.rb.velocity.y > 0.0)
      this.rb.velocity = new Vector3(velocity.x, 0.0f, velocity.z);
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = UnityEngine.Object.Instantiate<GameObject>(this.playerJumpSmokeFx, this.transform.position, Quaternion.LookRotation(Vector3.up)).GetComponent<ParticleSystem>().velocityOverLifetime;
    velocityOverLifetime.x = (ParticleSystem.MinMaxCurve) (this.rb.velocity.x * 2f);
    velocityOverLifetime.z = (ParticleSystem.MinMaxCurve) (this.rb.velocity.z * 2f);
    this.playerStatus.Jump();
  }

  private void JumpCooldown() => this.readyToJump = true;

  private void CounterMovement(float x, float y, Vector2 mag)
  {
    this.rb.isKinematic = (double) x == 0.0 && (double) y == 0.0 && ((double) this.rb.velocity.magnitude < 1.0 && this.grounded && (!this.jumping && this.playerStatus.CanJump()));
    if (!this.grounded || this.jumping && this.playerStatus.CanJump())
      return;
    if (this.crouching)
    {
      this.rb.AddForce(this.moveSpeed * 0.02f * -this.rb.velocity.normalized * this.slideCounterMovement);
    }
    else
    {
      if ((double) Math.Abs(mag.x) > (double) this.threshold && (double) Math.Abs(x) < 0.0500000007450581 && this.readyToCounterX > 1)
        this.rb.AddForce(this.moveSpeed * this.orientation.transform.right * 0.02f * -mag.x * this.counterMovement);
      if ((double) Math.Abs(mag.y) > (double) this.threshold && (double) Math.Abs(y) < 0.0500000007450581 && this.readyToCounterY > 1)
        this.rb.AddForce(this.moveSpeed * this.orientation.transform.forward * 0.02f * -mag.y * this.counterMovement);
      if (this.IsHoldingAgainstHorizontalVel(mag))
        this.rb.AddForce(this.moveSpeed * this.orientation.transform.right * 0.02f * -mag.x * this.counterMovement * 2f);
      if (this.IsHoldingAgainstVerticalVel(mag))
        this.rb.AddForce(this.moveSpeed * this.orientation.transform.forward * 0.02f * -mag.y * this.counterMovement * 2f);
      if ((double) Mathf.Sqrt(Mathf.Pow(this.rb.velocity.x, 2f) + Mathf.Pow(this.rb.velocity.z, 2f)) > (double) this.maxSpeed * (double) PowerupInventory.Instance.GetSpeedMultiplier((int[]) null))
      {
        float y1 = this.rb.velocity.y;
        Vector3 vector3 = this.rb.velocity.normalized * this.maxSpeed * PowerupInventory.Instance.GetSpeedMultiplier((int[]) null);
        this.rb.velocity = new Vector3(vector3.x, y1, vector3.z);
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
    if ((double) vel.x < -(double) this.threshold && (double) this.x > 0.0)
      return true;
    return (double) vel.x > (double) this.threshold && (double) this.x < 0.0;
  }

  private bool IsHoldingAgainstVerticalVel(Vector2 vel)
  {
    if ((double) vel.y < -(double) this.threshold && (double) this.y > 0.0)
      return true;
    return (double) vel.y > (double) this.threshold && (double) this.y < 0.0;
  }

  public Vector2 FindVelRelativeToLook()
  {
    float num1 = Mathf.DeltaAngle(this.orientation.transform.eulerAngles.y, Mathf.Atan2(this.rb.velocity.x, this.rb.velocity.z) * 57.29578f);
    float num2 = 90f - num1;
    double magnitude = (double) new Vector2(this.rb.velocity.x, this.rb.velocity.z).magnitude;
    float y = (float) magnitude * Mathf.Cos(num1 * ((float) Math.PI / 180f));
    return new Vector2((float) magnitude * Mathf.Cos(num2 * ((float) Math.PI / 180f)), y);
  }

  private bool IsFloor(Vector3 v) => (double) Vector3.Angle(Vector3.up, v) < (double) this.maxSlopeAngle;

  private bool IsSurf(Vector3 v)
  {
    float num = Vector3.Angle(Vector3.up, v);
    return (double) num < 89.0 && (double) num > (double) this.maxSlopeAngle;
  }

  private bool IsWall(Vector3 v) => (double) Math.Abs(90f - Vector3.Angle(Vector3.up, v)) < 0.100000001490116;

  private bool IsRoof(Vector3 v) => (double) v.y == -1.0;

  private void OnCollisionEnter(Collision other)
  {
    int layer = other.gameObject.layer;
    Vector3 normal = other.contacts[0].normal;
    if ((int) this.whatIsGround != ((int) this.whatIsGround | 1 << layer) || !this.IsFloor(normal) || (double) this.fallSpeed >= -12.0)
      return;
    MoveCamera.Instance.BobOnce(new Vector3(0.0f, this.fallSpeed, 0.0f));
    Vector3 point = other.contacts[0].point;
    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = UnityEngine.Object.Instantiate<GameObject>(this.playerSmokeFx, point, Quaternion.LookRotation(this.transform.position - point)).GetComponent<ParticleSystem>().velocityOverLifetime;
    velocityOverLifetime.x = (ParticleSystem.MinMaxCurve) (this.rb.velocity.x * 2f);
    velocityOverLifetime.z = (ParticleSystem.MinMaxCurve) (this.rb.velocity.z * 2f);
  }

  private void OnCollisionStay(Collision other)
  {
    if ((int) this.whatIsGround != ((int) this.whatIsGround | 1 << other.gameObject.layer))
      return;
    for (int index = 0; index < other.contactCount; ++index)
    {
      Vector3 vector3 = other.contacts[index].normal;
      vector3 = new Vector3(vector3.x, Mathf.Abs(vector3.y), vector3.z);
      if (this.IsFloor(vector3))
      {
        if (!this.grounded)
        {
          int num = this.crouching ? 1 : 0;
        }
        this.onRamp = (double) Vector3.Angle(Vector3.up, vector3) > 1.0;
        this.grounded = true;
        this.normalVector = vector3;
        this.cancellingGrounded = false;
        this.groundCancel = 0;
      }
      if (this.IsSurf(vector3))
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

  public Vector3 GetVelocity() => this.rb.velocity;

  public float GetFallSpeed() => this.rb.velocity.y;

  public Collider GetPlayerCollider() => this.playerCollider;

  public Transform GetPlayerCamTransform() => this.playerCam.transform;

  public Vector3 HitPoint()
  {
    RaycastHit[] raycastHitArray = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, 100f, (int) this.whatIsHittable);
    if (raycastHitArray.Length < 1)
      return this.playerCam.transform.position + this.playerCam.transform.forward * 100f;
    if (raycastHitArray.Length > 1)
    {
      for (int index = 0; index < raycastHitArray.Length; ++index)
      {
        if (raycastHitArray[index].transform.gameObject.layer == LayerMask.NameToLayer("Enemy") || raycastHitArray[index].transform.gameObject.layer == LayerMask.NameToLayer("Object") || raycastHitArray[index].transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
          return raycastHitArray[index].point;
      }
    }
    return raycastHitArray[0].point;
  }

  public bool IsCrouching() => this.crouching;

  public bool IsDead() => this.dead;

  public Rigidbody GetRb() => this.rb;
}
