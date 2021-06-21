// Decompiled with JetBrains decompiler
// Type: Arrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class Arrow : MonoBehaviour
{
  private Rigidbody rb;
  public AudioSource audio;
  public TrailRenderer trail;
  public GameObject hitFx;
  public bool fallingWhileShooting;
  public float speedWhileShooting;
  private bool done;

  public InventoryItem item { get; set; }

  public int damage { get; set; }

  public bool otherPlayersArrow { get; set; }

  private void Awake() => this.rb = (Rigidbody) ((Component) this).GetComponent<Rigidbody>();

  private void Update() => ((Component) this).get_transform().set_rotation(Quaternion.LookRotation(this.rb.get_velocity()));

  private void OnCollisionEnter(Collision other)
  {
    if (this.done)
      return;
    this.done = true;
    int layer = other.get_gameObject().get_layer();
    if (!this.otherPlayersArrow && (layer == LayerMask.NameToLayer("Player") || layer == LayerMask.NameToLayer("Enemy")))
    {
      Hitable componentInChildren = (Hitable) ((Component) other.get_transform().get_root()).GetComponentInChildren<Hitable>();
      if (!Object.op_Implicit((Object) componentInChildren))
        return;
      PowerupCalculations.DamageResult damageMultiplier1 = PowerupCalculations.Instance.GetDamageMultiplier(this.fallingWhileShooting, this.speedWhileShooting);
      float damageMultiplier2 = damageMultiplier1.damageMultiplier;
      bool flag = damageMultiplier1.crit;
      float lifesteal = damageMultiplier1.lifesteal;
      int damage = (int) ((double) this.damage * (double) damageMultiplier2);
      Mob component = (Mob) ((Component) componentInChildren).GetComponent<Mob>();
      if (Object.op_Implicit((Object) component) && this.item.attackTypes != null && component.mobType.weaknesses != null)
      {
        foreach (MobType.Weakness weakness in component.mobType.weaknesses)
        {
          foreach (MobType.Weakness attackType in this.item.attackTypes)
          {
            Debug.LogError((object) ("checking: " + (object) weakness + ", a: " + (object) attackType));
            if (attackType == weakness)
            {
              flag = true;
              damage *= 2;
            }
          }
        }
      }
      Vector3 pos = other.get_collider().ClosestPoint(((Component) this).get_transform().get_position());
      HitEffect hitEffect = HitEffect.Normal;
      if (damageMultiplier1.sniped)
        hitEffect = HitEffect.Big;
      else if (flag)
        hitEffect = HitEffect.Crit;
      else if (damageMultiplier1.falling)
        hitEffect = HitEffect.Falling;
      componentInChildren.Hit(damage, 1f, (int) hitEffect, pos);
      PlayerStatus.Instance.Heal(Mathf.CeilToInt((float) damage * lifesteal));
      if (damageMultiplier1.sniped)
        PowerupCalculations.Instance.HitEffect(PowerupCalculations.Instance.sniperSfx);
      if (flag)
        PowerupInventory.Instance.StartJuice();
      if ((double) damageMultiplier2 > 0.0 && (double) damageMultiplier1.hammerMultiplier > 0.0)
      {
        int num = 0;
        PowerupCalculations.Instance.SpawnOnHitEffect(num, true, pos, (int) ((double) damage * (double) damageMultiplier1.hammerMultiplier));
        ClientSend.SpawnEffect(num, pos);
      }
    }
    this.StopArrow(other);
  }

  private void StopArrow(Collision other)
  {
    this.rb.set_isKinematic(true);
    ((Component) this).get_transform().SetParent(other.get_transform());
    this.done = true;
    ((DestroyObject) ((Component) this).get_gameObject().AddComponent<DestroyObject>()).time = 10f;
    Object.Destroy((Object) this);
    Object.Destroy((Object) this.audio);
    this.trail.set_emitting(false);
    ParticleSystem component1 = (ParticleSystem) ((GameObject) Object.Instantiate<GameObject>((M0) this.hitFx, ((Component) this).get_transform().get_position(), Quaternion.LookRotation(Vector3.op_UnaryNegation(((Component) this).get_transform().get_forward())))).GetComponent<ParticleSystem>();
    Renderer component2 = (Renderer) other.get_gameObject().GetComponent<Renderer>();
    Material material = (Material) null;
    if (Object.op_Inequality((Object) component2, (Object) null))
    {
      material = component2.get_material();
    }
    else
    {
      SkinnedMeshRenderer componentInChildren = (SkinnedMeshRenderer) ((Component) other.get_transform().get_root()).GetComponentInChildren<SkinnedMeshRenderer>();
      if (Object.op_Implicit((Object) componentInChildren))
        material = ((Renderer) componentInChildren).get_material();
    }
    if (Object.op_Implicit((Object) material))
      ((Renderer) ((Component) component1).GetComponent<Renderer>()).set_material(material);
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }

  public Arrow() => base.\u002Ector();
}
