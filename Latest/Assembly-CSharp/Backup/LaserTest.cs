// Decompiled with JetBrains decompiler
// Type: LaserTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LaserTest : MonoBehaviour
{
  public ParticleSystem ps;
  public ParticleSystem psSwirl;
  public LineRenderer lr;
  public Transform targetParticles;
  public LayerMask whatIsHittable;
  public Hitable hitable;
  public Transform hitParticles;
  private float damageUpdateRate = 0.1f;
  public GameObject damageFx;
  public GameObject sfx;
  private ParticleSystem.ShapeModule shape;
  private ParticleSystem.VelocityOverLifetimeModule vel;
  private Mob mob;
  public Transform target;
  private Vector3 currentPos;
  private bool hitSomething;

  private void Awake()
  {
    this.lr.positionCount = 2;
    this.shape = this.ps.shape;
    this.vel = this.psSwirl.velocityOverLifetime;
    this.mob = this.transform.root.GetComponent<Mob>();
  }

  private void OnEnable()
  {
    this.currentPos = this.transform.position;
    this.target = this.mob.target;
    if ((Object) this.target == (Object) null)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.CancelInvoke("StopLaser");
      this.Invoke("StopLaser", 2.1f);
      this.hitParticles.gameObject.SetActive(true);
      this.InvokeRepeating("DamageEffect", this.damageUpdateRate, this.damageUpdateRate);
    }
  }

  private void StopLaser()
  {
    this.target = this.transform;
    this.hitParticles.gameObject.SetActive(false);
    this.CancelInvoke("DamageEffect");
  }

  private void LateUpdate()
  {
    if ((Object) this.hitable == (Object) null)
    {
      Debug.LogError((object) "Stopping");
      this.gameObject.SetActive(false);
      this.sfx.SetActive(false);
      this.StopLaser();
    }
    else
    {
      this.currentPos = Vector3.Lerp(this.currentPos, this.target.position, Time.deltaTime * 15f);
      float maxDistance = Vector3.Distance(this.transform.position, this.currentPos);
      RaycastHit hitInfo;
      if (Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, maxDistance, (int) this.whatIsHittable))
      {
        this.currentPos = hitInfo.point - this.transform.forward * 0.2f;
        this.hitSomething = true;
      }
      this.targetParticles.transform.position = this.currentPos;
      this.transform.LookAt(this.target);
      float z = Vector3.Distance(this.transform.position, this.currentPos);
      this.shape.position = new Vector3(this.shape.position.x, this.shape.position.y, z / 2f);
      this.shape.scale = new Vector3(this.shape.scale.x, this.shape.scale.y, z);
      this.vel.z = (ParticleSystem.MinMaxCurve) (z / 2f);
      this.lr.SetPosition(0, Vector3.zero);
      this.lr.SetPosition(1, Vector3.forward * z / this.transform.root.localScale.x);
      this.hitParticles.transform.position = this.currentPos;
      this.hitParticles.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
    }
  }

  private void DamageEffect() => Object.Instantiate<GameObject>(this.damageFx, this.hitParticles.transform.position, this.hitParticles.transform.rotation);
}
