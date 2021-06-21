// Decompiled with JetBrains decompiler
// Type: Arrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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

  public int damage { get; set; }

  public bool otherPlayersArrow { get; set; }

  private void Awake() => this.rb = this.GetComponent<Rigidbody>();

  private void Update() => this.transform.rotation = Quaternion.LookRotation(this.rb.velocity);

  private void OnCollisionEnter(Collision other)
  {
    if (this.done)
      return;
    this.done = true;
    int layer = other.gameObject.layer;
    if (!this.otherPlayersArrow && (layer == LayerMask.NameToLayer("Player") || layer == LayerMask.NameToLayer("Enemy")))
    {
      Hitable componentInChildren = other.transform.root.GetComponentInChildren<Hitable>();
      if (!(bool) (Object) componentInChildren)
        return;
      PowerupCalculations.DamageResult damageMultiplier1 = PowerupCalculations.Instance.GetDamageMultiplier(this.fallingWhileShooting, this.speedWhileShooting);
      float damageMultiplier2 = damageMultiplier1.damageMultiplier;
      bool crit = damageMultiplier1.crit;
      float lifesteal = damageMultiplier1.lifesteal;
      int damage = (int) ((double) this.damage * (double) damageMultiplier2);
      Vector3 pos = other.collider.ClosestPoint(this.transform.position);
      HitEffect hitEffect = HitEffect.Normal;
      if (damageMultiplier1.sniped)
        hitEffect = HitEffect.Big;
      else if (crit)
        hitEffect = HitEffect.Crit;
      else if (damageMultiplier1.falling)
        hitEffect = HitEffect.Falling;
      componentInChildren.Hit(damage, 1f, (int) hitEffect, pos);
      PlayerStatus.Instance.Heal(Mathf.CeilToInt((float) damage * lifesteal));
      if (damageMultiplier1.sniped)
        PowerupCalculations.Instance.HitEffect(PowerupCalculations.Instance.sniperSfx);
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
    this.rb.isKinematic = true;
    this.transform.SetParent(other.transform);
    this.done = true;
    this.gameObject.AddComponent<DestroyObject>().time = 10f;
    Object.Destroy((Object) this);
    Object.Destroy((Object) this.audio);
    this.trail.emitting = false;
    ParticleSystem component1 = Object.Instantiate<GameObject>(this.hitFx, this.transform.position, Quaternion.LookRotation(-this.transform.forward)).GetComponent<ParticleSystem>();
    Renderer component2 = other.gameObject.GetComponent<Renderer>();
    Material material = (Material) null;
    if ((Object) component2 != (Object) null)
    {
      material = component2.material;
    }
    else
    {
      SkinnedMeshRenderer componentInChildren = other.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
      if ((bool) (Object) componentInChildren)
        material = componentInChildren.material;
    }
    if ((bool) (Object) material)
      component1.GetComponent<Renderer>().material = material;
    Object.Destroy((Object) this.gameObject);
  }
}
