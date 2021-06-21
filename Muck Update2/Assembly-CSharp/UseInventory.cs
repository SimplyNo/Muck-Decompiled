// Decompiled with JetBrains decompiler
// Type: UseInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    foreach (AnimationClip animationClip in this.animator.get_runtimeAnimatorController().get_animationClips())
    {
      string name = ((Object) animationClip).get_name();
      if (!(name == "Attack"))
      {
        if (!(name == "Eat"))
        {
          if (name == "Charge")
            this.chargeTime = animationClip.get_length();
        }
        else
          this.eatTime = animationClip.get_length();
      }
      else
        this.attackTime = animationClip.get_length();
    }
    this.eatingEmission = this.eatingParticles.get_emission();
    ((ParticleSystem.EmissionModule) ref this.eatingEmission).set_enabled(false);
    this.velocity = this.eatingParticles.get_velocityOverLifetime();
    this.SetWeapon((InventoryItem) null);
  }

  public void SetWeapon(InventoryItem item)
  {
    this.StopUse();
    this.currentItem = item;
    if (Object.op_Equality((Object) item, (Object) null))
    {
      ((Renderer) this.meshRenderer).set_material((Material) null);
      this.meshFilter.set_mesh((Mesh) null);
    }
    else
    {
      if (item.swingFx)
        ((Component) this.swingTrail).get_gameObject().SetActive(true);
      else
        ((Component) this.swingTrail).get_gameObject().SetActive(false);
      this.renderTransform.set_localRotation(Quaternion.Euler(item.rotationOffset));
      this.renderTransform.set_localScale(Vector3.op_Multiply(Vector3.get_one(), item.scale));
      this.renderTransform.set_localPosition(item.positionOffset);
      ((Renderer) this.meshRenderer).set_material(item.material);
      this.meshFilter.set_mesh(item.mesh);
      ((Component) this.hitBox).get_transform().get_parent().set_localScale(item.attackRange);
      this.animator.SetFloat("AttackSpeed", this.currentItem.attackSpeed);
      this.animator.Play("Equip", -1, 0.0f);
    }
  }

  private void StopUse()
  {
    if (this.IsAnimationPlaying("Eat"))
    {
      this.eatSfx.Stop();
      ((ParticleSystem.EmissionModule) ref this.eatingEmission).set_enabled(false);
    }
    this.CancelInvoke();
    this.animator.Play("Idle");
    CooldownBar.Instance.HideBar();
    ((ParticleSystem.EmissionModule) ref this.eatingEmission).set_enabled(false);
  }

  private void Update()
  {
    if (!this.IsAnimationPlaying("Eat"))
      return;
    Vector3 velocity = PlayerMovement.Instance.GetVelocity();
    ((ParticleSystem.VelocityOverLifetimeModule) ref this.velocity).set_x(ParticleSystem.MinMaxCurve.op_Implicit((float) velocity.x));
    ((ParticleSystem.VelocityOverLifetimeModule) ref this.velocity).set_y(ParticleSystem.MinMaxCurve.op_Implicit((float) velocity.y));
    ((ParticleSystem.VelocityOverLifetimeModule) ref this.velocity).set_z(ParticleSystem.MinMaxCurve.op_Implicit((float) velocity.z));
  }

  public void Use()
  {
    if (Object.op_Equality((Object) this.currentItem, (Object) null) || OtherInput.Instance.IsAnyMenuOpen() || (this.IsAnimationPlaying("Attack") || this.IsAnimationPlaying("Equip")) || (this.IsAnimationPlaying("Eat") || this.IsAnimationPlaying("Charge") || (this.IsAnimationPlaying("ChargeHold") || this.IsAnimationPlaying("Shoot"))))
      return;
    float attackTime = this.attackTime;
    float num = this.currentItem.attackSpeed * PowerupInventory.Instance.GetAttackSpeedMultiplier((int[]) null);
    float time = attackTime / num;
    bool stayOnScreen = false;
    string str;
    if (this.currentItem.tag == InventoryItem.ItemTag.Food)
    {
      time = this.eatTime / num;
      str = "Eat";
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
      str = "Charge";
      ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Charge, true);
      this.chargeSfx.Play();
      this.chargeSfx.set_pitch(this.currentItem.attackSpeed);
      stayOnScreen = true;
    }
    else
    {
      this.swingSfx.Randomize(0.15f / num);
      str = "Attack" + (object) Random.Range(1, 4);
      ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Attack, true);
    }
    this.animator.Play(str);
    this.animator.SetFloat("AttackSpeed", num);
    CooldownBar.Instance.ResetCooldownTime(time, stayOnScreen);
  }

  private void StartParticles() => ((ParticleSystem.EmissionModule) ref this.eatingEmission).set_enabled(true);

  public void UseButtonUp()
  {
    if (this.IsAnimationPlaying("Eat"))
    {
      this.animator.Play("Idle");
      ((ParticleSystem.EmissionModule) ref this.eatingEmission).set_enabled(false);
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
      AnimatorStateInfo animatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
      shakeRatio = ((AnimatorStateInfo) ref animatorStateInfo).get_normalizedTime();
      MonoBehaviour.print((object) ("charge: " + (object) shakeRatio));
    }
    ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Charge, false);
    this.animator.Play("Shoot", -1, 0.0f);
    CooldownBar.Instance.HideBar();
    if (Object.op_Equality((Object) InventoryUI.Instance.arrows.currentItem, (Object) null))
      return;
    InventoryItem currentItem1 = Hotbar.Instance.currentItem;
    InventoryItem currentItem2 = InventoryUI.Instance.arrows.currentItem;
    List<Collider> colliderList = new List<Collider>();
    for (int index = 0; index < currentItem1.bowComponent.nArrows && !Object.op_Equality((Object) InventoryUI.Instance.arrows.currentItem, (Object) null); ++index)
    {
      --currentItem2.amount;
      if (currentItem2.amount <= 0)
        InventoryUI.Instance.arrows.currentItem = (InventoryItem) null;
      Vector3 pos = Vector3.op_Addition(PlayerMovement.Instance.playerCam.get_position(), Vector3.op_Multiply(Vector3.get_down(), 0.5f));
      Vector3 forward = PlayerMovement.Instance.playerCam.get_forward();
      float angleDelta = (float) currentItem1.bowComponent.angleDelta;
      Vector3 rot = Quaternion.op_Multiply(Quaternion.AngleAxis((float) (-((double) angleDelta * (double) (currentItem1.bowComponent.nArrows - 1)) / 2.0 + (double) angleDelta * (double) index), PlayerMovement.Instance.playerCam.get_up()), forward);
      M0 m0 = Object.Instantiate<GameObject>((M0) currentItem2.prefab);
      ((Renderer) ((GameObject) m0).GetComponent<Renderer>()).set_material(currentItem2.material);
      ((GameObject) m0).get_transform().set_position(pos);
      ((GameObject) m0).get_transform().set_rotation(((Component) this).get_transform().get_rotation());
      float attackDamage1 = (float) Hotbar.Instance.currentItem.attackDamage;
      float attackDamage2 = (float) currentItem2.attackDamage;
      float projectileSpeed = currentItem1.bowComponent.projectileSpeed;
      M0 component1 = ((GameObject) m0).GetComponent<Rigidbody>();
      float force = 100f * shakeRatio * projectileSpeed * PowerupInventory.Instance.GetRobinMultiplier((int[]) null);
      Vector3 vector3 = Vector3.op_Multiply(rot, force);
      ((Rigidbody) component1).AddForce(vector3);
      Physics.IgnoreCollision((Collider) ((GameObject) m0).GetComponent<Collider>(), PlayerMovement.Instance.GetPlayerCollider(), true);
      float num = attackDamage2 * attackDamage1 * shakeRatio;
      Arrow component2 = (Arrow) ((GameObject) m0).GetComponent<Arrow>();
      component2.damage = (int) ((double) num * (double) PowerupInventory.Instance.GetRobinMultiplier((int[]) null));
      component2.fallingWhileShooting = !PlayerMovement.Instance.grounded && PlayerMovement.Instance.GetVelocity().y < 0.0;
      Arrow arrow = component2;
      Vector3 velocity = PlayerMovement.Instance.GetVelocity();
      double magnitude = (double) ((Vector3) ref velocity).get_magnitude();
      arrow.speedWhileShooting = (float) magnitude;
      component2.item = currentItem2;
      ClientSend.ShootArrow(pos, rot, force, currentItem2.id);
      colliderList.Add((Collider) ((Component) component2).GetComponent<Collider>());
    }
    using (List<Collider>.Enumerator enumerator1 = colliderList.GetEnumerator())
    {
      while (enumerator1.MoveNext())
      {
        Collider current1 = enumerator1.Current;
        using (List<Collider>.Enumerator enumerator2 = colliderList.GetEnumerator())
        {
          while (enumerator2.MoveNext())
          {
            Collider current2 = enumerator2.Current;
            Physics.IgnoreCollision(current1, current2, true);
          }
        }
      }
    }
    InventoryUI.Instance.arrows.UpdateCell();
    CameraShaker.Instance.ChargeShake(shakeRatio);
  }

  private void FinishEating()
  {
    this.eatSfx.Stop();
    ((ParticleSystem.EmissionModule) ref this.eatingEmission).set_enabled(false);
    PlayerStatus.Instance.Eat(this.currentItem);
    ClientSend.AnimationUpdate(OnlinePlayer.SharedAnimation.Eat, false);
    Hotbar.Instance.UseItem(1);
  }

  private bool IsAnimationPlaying(string animationName)
  {
    if (this.animator.GetCurrentAnimatorClipInfo(0).Length == 0)
      return false;
    string name = ((Object) ((AnimatorClipInfo) ref this.animator.GetCurrentAnimatorClipInfo(0)[0]).get_clip()).get_name();
    return name.Contains(animationName) || animationName == name;
  }

  public UseInventory() => base.\u002Ector();
}
