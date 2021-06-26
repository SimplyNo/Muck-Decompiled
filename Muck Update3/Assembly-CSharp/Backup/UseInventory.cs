// Decompiled with JetBrains decompiler
// Type: UseInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class UseInventory : MonoBehaviour
{
  public static UseInventory Instance;
  public HitBox hitBox;
  public Animator animator;
  public TrailRenderer swingTrail;
  public RandomSfx swingSfx;
  public AudioSource chargeSfx;
  public AudioSource eatSfx;
  public ParticleSystem eatingParticles;
  private ParticleSystem.EmissionModule eatingEmission;
  private ParticleSystem.VelocityOverLifetimeModule velocity;
  private float eatTime;
  private float attackTime;
  private float chargeTime;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;
  public Transform renderTransform;
  private InventoryItem currentItem;

  private void Awake()
  {
    UseInventory.Instance = this;
    foreach (AnimationClip animationClip in this.animator.runtimeAnimatorController.animationClips)
    {
      string name = animationClip.name;
      if (!(name == "Attack"))
      {
        if (!(name == "Eat"))
        {
          if (name == "Charge")
            this.chargeTime = animationClip.length;
        }
        else
          this.eatTime = animationClip.length;
      }
      else
        this.attackTime = animationClip.length;
    }
    this.eatingEmission = this.eatingParticles.emission;
    this.eatingEmission.enabled = false;
    this.velocity = this.eatingParticles.velocityOverLifetime;
    this.SetWeapon((InventoryItem) null);
  }

  public void SetWeapon(InventoryItem item)
  {
    this.StopUse();
    this.currentItem = item;
    if ((Object) item == (Object) null)
    {
      this.meshRenderer.material = (Material) null;
      this.meshFilter.mesh = (Mesh) null;
    }
    else
    {
      if (item.swingFx)
        this.swingTrail.gameObject.SetActive(true);
      else
        this.swingTrail.gameObject.SetActive(false);
      this.renderTransform.localRotation = Quaternion.Euler(item.rotationOffset);
      this.renderTransform.localScale = Vector3.one * item.scale;
      this.renderTransform.localPosition = item.positionOffset;
      this.meshRenderer.material = item.material;
      this.meshFilter.mesh = item.mesh;
      this.hitBox.transform.parent.localScale = item.attackRange;
      this.animator.SetFloat("AttackSpeed", this.currentItem.attackSpeed);
      this.animator.Play("Equip", -1, 0.0f);
    }
  }

  private void StopUse()
  {
    if (this.IsAnimationPlaying("Eat"))
    {
      this.eatSfx.Stop();
      this.eatingEmission.enabled = false;
    }
    this.CancelInvoke();
    this.animator.Play("Idle");
    CooldownBar.Instance.HideBar();
    this.eatingEmission.enabled = false;
  }

  private void Update()
  {
    if (!this.IsAnimationPlaying("Eat"))
      return;
    Vector3 velocity = PlayerMovement.Instance.GetVelocity();
    this.velocity.x = (ParticleSystem.MinMaxCurve) velocity.x;
    this.velocity.y = (ParticleSystem.MinMaxCurve) velocity.y;
    this.velocity.z = (ParticleSystem.MinMaxCurve) velocity.z;
  }

  public void Use()
  {
    if ((Object) this.currentItem == (Object) null || OtherInput.Instance.IsAnyMenuOpen() || (this.IsAnimationPlaying("Attack") || this.IsAnimationPlaying("Equip")) || (this.IsAnimationPlaying("Eat") || this.IsAnimationPlaying("Charge") || (this.IsAnimationPlaying("ChargeHold") || this.IsAnimationPlaying("Shoot"))))
      return;
    float attackTime = this.attackTime;
    float num = this.currentItem.attackSpeed * PowerupInventory.Instance.GetAttackSpeedMultiplier((int[]) null);
    float time = attackTime / num;
    bool stayOnScreen = false;
    string stateName;
    if (this.currentItem.tag == InventoryItem.ItemTag.Food)
    {
      time = this.eatTime / num;
      stateName = "Eat";
      this.eatSfx.Stop();
      this.CancelInvoke("FinishEating");
      this.eatSfx.PlayDelayed(0.3f / num);
      ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Eat, true);
      this.Invoke("FinishEating", time * 0.95f);
      this.Invoke("StartParticles", time * 0.25f);
    }
    else if (this.currentItem.type == InventoryItem.ItemType.Bow)
    {
      float robinMultiplier = PowerupInventory.Instance.GetRobinMultiplier((int[]) null);
      time = this.chargeTime / (num * robinMultiplier);
      stateName = "Charge";
      ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Charge, true);
      this.chargeSfx.Play();
      this.chargeSfx.pitch = this.currentItem.attackSpeed;
      stayOnScreen = true;
    }
    else
    {
      this.swingSfx.Randomize(0.15f / num);
      stateName = "Attack" + (object) Random.Range(1, 4);
      ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Attack, true);
    }
    this.animator.Play(stateName);
    this.animator.SetFloat("AttackSpeed", num);
    CooldownBar.Instance.ResetCooldownTime(time, stayOnScreen);
  }

  private void StartParticles() => this.eatingEmission.enabled = true;

  public void UseButtonUp()
  {
    if (this.IsAnimationPlaying("Eat"))
    {
      this.animator.Play("Idle");
      this.eatingEmission.enabled = false;
      CooldownBar.Instance.HideBar();
      this.eatSfx.Stop();
      ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Eat, false);
      this.CancelInvoke();
    }
    if (!this.IsAnimationPlaying("Charge") && !this.IsAnimationPlaying("ChargeHold"))
      return;
    this.chargeSfx.Stop();
    ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Charge, false);
    this.ReleaseWeapon();
  }

  private void ReleaseWeapon()
  {
    float shakeRatio;
    if (this.IsAnimationPlaying("ChargeHold"))
    {
      shakeRatio = 1f;
    }
    else
    {
      shakeRatio = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
      MonoBehaviour.print((object) ("charge: " + (object) shakeRatio));
    }
    ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Charge, false);
    this.animator.Play("Shoot", -1, 0.0f);
    CooldownBar.Instance.HideBar();
    if ((Object) InventoryUI.Instance.arrows.currentItem == (Object) null)
      return;
    InventoryItem currentItem1 = Hotbar.Instance.currentItem;
    InventoryItem currentItem2 = InventoryUI.Instance.arrows.currentItem;
    List<Collider> colliderList = new List<Collider>();
    for (int index = 0; index < currentItem1.bowComponent.nArrows && !((Object) InventoryUI.Instance.arrows.currentItem == (Object) null); ++index)
    {
      --currentItem2.amount;
      if (currentItem2.amount <= 0)
        InventoryUI.Instance.arrows.currentItem = (InventoryItem) null;
      Vector3 pos = PlayerMovement.Instance.playerCam.position + Vector3.down * 0.5f;
      Vector3 forward = PlayerMovement.Instance.playerCam.forward;
      float angleDelta = (float) currentItem1.bowComponent.angleDelta;
      Vector3 rot = Quaternion.AngleAxis((float) (-((double) angleDelta * (double) (currentItem1.bowComponent.nArrows - 1)) / 2.0 + (double) angleDelta * (double) index), PlayerMovement.Instance.playerCam.up) * forward;
      GameObject gameObject = Object.Instantiate<GameObject>(currentItem2.prefab);
      gameObject.GetComponent<Renderer>().material = currentItem2.material;
      gameObject.transform.position = pos;
      gameObject.transform.rotation = this.transform.rotation;
      float attackDamage1 = (float) Hotbar.Instance.currentItem.attackDamage;
      float attackDamage2 = (float) currentItem2.attackDamage;
      float projectileSpeed = currentItem1.bowComponent.projectileSpeed;
      Rigidbody component1 = gameObject.GetComponent<Rigidbody>();
      float force1 = 100f * shakeRatio * projectileSpeed * PowerupInventory.Instance.GetRobinMultiplier((int[]) null);
      Vector3 force2 = rot * force1;
      component1.AddForce(force2);
      Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), PlayerMovement.Instance.GetPlayerCollider(), true);
      float num = attackDamage2 * attackDamage1 * shakeRatio;
      Arrow component2 = gameObject.GetComponent<Arrow>();
      component2.damage = (int) ((double) num * (double) PowerupInventory.Instance.GetRobinMultiplier((int[]) null));
      component2.fallingWhileShooting = !PlayerMovement.Instance.grounded && (double) PlayerMovement.Instance.GetVelocity().y < 0.0;
      component2.speedWhileShooting = PlayerMovement.Instance.GetVelocity().magnitude;
      component2.item = currentItem2;
      ClientSend.ShootArrow(pos, rot, force1, currentItem2.id);
      colliderList.Add(component2.GetComponent<Collider>());
    }
    foreach (Collider collider1 in colliderList)
    {
      foreach (Collider collider2 in colliderList)
        Physics.IgnoreCollision(collider1, collider2, true);
    }
    InventoryUI.Instance.arrows.UpdateCell();
    CameraShaker.Instance.ChargeShake(shakeRatio);
  }

  private void FinishEating()
  {
    this.eatSfx.Stop();
    this.eatingEmission.enabled = false;
    PlayerStatus.Instance.Eat(this.currentItem);
    ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Eat, false);
    Hotbar.Instance.UseItem(1);
  }

  private bool IsAnimationPlaying(string animationName)
  {
    if (this.animator.GetCurrentAnimatorClipInfo(0).Length == 0)
      return false;
    string name = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    return name.Contains(animationName) || animationName == name;
  }
}
