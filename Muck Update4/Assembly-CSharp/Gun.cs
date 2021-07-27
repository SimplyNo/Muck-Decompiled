// Decompiled with JetBrains decompiler
// Type: Gun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
  private float xBob = 0.12f;
  private float yBob = 0.08f;
  private float zBob = 0.1f;
  private float bobSpeed = 0.45f;
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
  private float gunDrag = 0.2f;
  public float currentGunDragMultiplier = 1f;
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
    this.startPos = this.transform.localPosition;
    this.rb = PlayerMovement.Instance.GetRb();
    this.playerCam = PlayerMovement.Instance.playerCam;
  }

  private void Update()
  {
    if (!(bool) (UnityEngine.Object) PlayerMovement.Instance)
      return;
    this.MovementBob();
    this.ReloadGun();
    this.RecoilGun();
    this.SpeedBob();
    float b1 = 0.0f;
    float b2 = 0.0f;
    if (!InventoryUI.Instance.gameObject.activeInHierarchy)
    {
      b1 = -Input.GetAxis("Mouse X") * this.gunDrag * this.currentGunDragMultiplier;
      b2 = -Input.GetAxis("Mouse Y") * this.gunDrag * this.currentGunDragMultiplier;
    }
    this.desX = Mathf.Lerp(this.desX, b1, Time.unscaledDeltaTime * 10f);
    this.desY = Mathf.Lerp(this.desY, b2, Time.unscaledDeltaTime * 10f);
    this.Rotation(new Vector2(this.desX, this.desY));
    this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, this.startPos + new Vector3(this.desX, this.desY, 0.0f) + this.desiredBob + this.recoilOffset + new Vector3(0.0f, -this.reloadPosOffset, 0.0f) + this.speedBob, Time.unscaledDeltaTime * 15f);
  }

  private void Rotation(Vector2 offset)
  {
    float num = offset.magnitude * 0.03f;
    if ((double) offset.x < 0.0)
      num = -num;
    Vector3 euler = new Vector3((float) ((double) offset.y * 80.0) + this.reloadRotation, -offset.x * 40f, num * 50f) + this.recoilRotation;
    try
    {
      if ((double) Time.deltaTime <= 0.0)
        return;
      this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(euler), Time.deltaTime * 20f);
    }
    catch (Exception ex)
    {
    }
  }

  private void MovementBob()
  {
    if (!(bool) (UnityEngine.Object) this.rb)
      return;
    if ((double) Mathf.Abs(this.rb.velocity.magnitude) < 4.0 || !PlayerMovement.Instance.grounded || PlayerMovement.Instance.IsCrouching())
      this.desiredBob = Vector3.zero;
    else
      this.desiredBob = new Vector3(Mathf.PingPong(Time.time * this.bobSpeed, this.xBob) - this.xBob / 2f, Mathf.PingPong(Time.time * this.bobSpeed, this.yBob) - this.yBob / 2f, Mathf.PingPong(Time.time * this.bobSpeed, this.zBob) - this.zBob / 2f);
  }

  private void SpeedBob()
  {
    Vector2 velRelativeToLook = PlayerMovement.Instance.FindVelRelativeToLook();
    this.speedBob = Vector3.Lerp(this.speedBob, Vector3.ClampMagnitude(new Vector3(velRelativeToLook.x, PlayerMovement.Instance.GetVelocity().y, velRelativeToLook.y) * -0.01f, 0.6f), Time.deltaTime * 10f);
  }

  private void RecoilGun()
  {
    this.recoilOffset = Vector3.SmoothDamp(this.recoilOffset, Vector3.zero, ref this.recoilOffsetVel, 0.05f);
    this.recoilRotation = Vector3.SmoothDamp(this.recoilRotation, Vector3.zero, ref this.recoilRotVel, 0.07f);
  }

  public void Build() => this.recoilOffset += Vector3.down;

  private void ReloadGun()
  {
    this.reloadProgress += Time.deltaTime;
    this.reloadRotation = Mathf.Lerp(0.0f, this.desiredReloadRotation, this.reloadProgress / this.reloadTime);
    this.reloadPosOffset = Mathf.SmoothDamp(this.reloadPosOffset, 0.0f, ref this.rPVel, this.reloadTime * 0.2f);
    if ((double) this.reloadRotation / 360.0 <= (double) this.spins)
      return;
    ++this.spins;
  }
}
