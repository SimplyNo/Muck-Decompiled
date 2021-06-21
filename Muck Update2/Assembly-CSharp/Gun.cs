// Decompiled with JetBrains decompiler
// Type: Gun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
  private Rigidbody rb;
  private Transform playerCam;
  private Vector3 startPos;
  private List<Vector3> velHistory;
  private Vector3 desiredBob;
  private float xBob;
  private float yBob;
  private float zBob;
  private float bobSpeed;
  private Vector3 recoilOffset;
  private Vector3 recoilRotation;
  private Vector3 recoilOffsetVel;
  private Vector3 recoilRotVel;
  private float reloadRotation;
  private float desiredReloadRotation;
  private float reloadTime;
  private float rVel;
  private float reloadPosOffset;
  private float rPVel;
  private float gunDrag;
  public float currentGunDragMultiplier;
  private float desX;
  private float desY;
  private Vector3 speedBob;
  private float reloadProgress;
  private int spins;

  public static Gun Instance { get; set; }

  private void Start()
  {
    Gun.Instance = this;
    this.velHistory = new List<Vector3>();
    this.startPos = ((Component) this).get_transform().get_localPosition();
    this.rb = PlayerMovement.Instance.GetRb();
    this.playerCam = PlayerMovement.Instance.playerCam;
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) PlayerMovement.Instance))
      return;
    this.MovementBob();
    this.ReloadGun();
    this.RecoilGun();
    this.SpeedBob();
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (!((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy())
    {
      num1 = -Input.GetAxis("Mouse X") * this.gunDrag * this.currentGunDragMultiplier;
      num2 = -Input.GetAxis("Mouse Y") * this.gunDrag * this.currentGunDragMultiplier;
    }
    this.desX = Mathf.Lerp(this.desX, num1, Time.get_unscaledDeltaTime() * 10f);
    this.desY = Mathf.Lerp(this.desY, num2, Time.get_unscaledDeltaTime() * 10f);
    this.Rotation(new Vector2(this.desX, this.desY));
    Vector3 vector3 = Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(this.startPos, new Vector3(this.desX, this.desY, 0.0f)), this.desiredBob), this.recoilOffset), new Vector3(0.0f, -this.reloadPosOffset, 0.0f)), this.speedBob);
    ((Component) this).get_transform().set_localPosition(Vector3.Lerp(((Component) this).get_transform().get_localPosition(), vector3, Time.get_unscaledDeltaTime() * 15f));
  }

  private void Rotation(Vector2 offset)
  {
    float num = ((Vector2) ref offset).get_magnitude() * 0.03f;
    if (offset.x < 0.0)
      num = -num;
    Vector3 vector3 = Vector3.op_Addition(new Vector3((float) (offset.y * 80.0) + this.reloadRotation, (float) -offset.x * 40f, num * 50f), this.recoilRotation);
    try
    {
      if ((double) Time.get_deltaTime() <= 0.0)
        return;
      ((Component) this).get_transform().set_localRotation(Quaternion.Lerp(((Component) this).get_transform().get_localRotation(), Quaternion.Euler(vector3), Time.get_deltaTime() * 20f));
    }
    catch (Exception ex)
    {
    }
  }

  private void MovementBob()
  {
    if (!Object.op_Implicit((Object) this.rb))
      return;
    Vector3 velocity = this.rb.get_velocity();
    if ((double) Mathf.Abs(((Vector3) ref velocity).get_magnitude()) < 4.0 || !PlayerMovement.Instance.grounded || PlayerMovement.Instance.IsCrouching())
      this.desiredBob = Vector3.get_zero();
    else
      this.desiredBob = new Vector3(Mathf.PingPong(Time.get_time() * this.bobSpeed, this.xBob) - this.xBob / 2f, Mathf.PingPong(Time.get_time() * this.bobSpeed, this.yBob) - this.yBob / 2f, Mathf.PingPong(Time.get_time() * this.bobSpeed, this.zBob) - this.zBob / 2f);
  }

  private void SpeedBob()
  {
    Vector2 velRelativeToLook = PlayerMovement.Instance.FindVelRelativeToLook();
    Vector3 vector3;
    ((Vector3) ref vector3).\u002Ector((float) velRelativeToLook.x, (float) PlayerMovement.Instance.GetVelocity().y, (float) velRelativeToLook.y);
    this.speedBob = Vector3.Lerp(this.speedBob, Vector3.ClampMagnitude(Vector3.op_Multiply(vector3, -0.01f), 0.6f), Time.get_deltaTime() * 10f);
  }

  private void RecoilGun()
  {
    this.recoilOffset = Vector3.SmoothDamp(this.recoilOffset, Vector3.get_zero(), ref this.recoilOffsetVel, 0.05f);
    this.recoilRotation = Vector3.SmoothDamp(this.recoilRotation, Vector3.get_zero(), ref this.recoilRotVel, 0.07f);
  }

  public void Build() => this.recoilOffset = Vector3.op_Addition(this.recoilOffset, Vector3.get_down());

  private void ReloadGun()
  {
    this.reloadProgress += Time.get_deltaTime();
    this.reloadRotation = Mathf.Lerp(0.0f, this.desiredReloadRotation, this.reloadProgress / this.reloadTime);
    this.reloadPosOffset = Mathf.SmoothDamp(this.reloadPosOffset, 0.0f, ref this.rPVel, this.reloadTime * 0.2f);
    if ((double) this.reloadRotation / 360.0 <= (double) this.spins)
      return;
    ++this.spins;
  }

  public Gun() => base.\u002Ector();
}
