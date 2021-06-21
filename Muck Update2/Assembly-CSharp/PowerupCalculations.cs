// Decompiled with JetBrains decompiler
// Type: PowerupCalculations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class PowerupCalculations : MonoBehaviour
{
  public GameObject[] onHitEffects;
  private static Vector2 randomDamageRange = new Vector2(0.4f, 1.2f);
  public GameObject hitFx;
  public AudioClip sniperSfx;
  public static PowerupCalculations Instance;

  private void Awake() => PowerupCalculations.Instance = this;

  public PowerupCalculations.DamageResult GetDamageMultiplier(
    bool falling,
    float speedWhileShooting = -1f)
  {
    bool crit = (double) Random.Range(0.0f, 1f) < (double) PowerupInventory.Instance.GetCritChance();
    float num1 = Random.Range((float) PowerupCalculations.randomDamageRange.x, (float) PowerupCalculations.randomDamageRange.y) * PowerupInventory.Instance.GetStrengthMultiplier((int[]) null);
    if (crit)
      num1 *= 2f;
    float lifestealMultiplier = PowerupInventory.Instance.GetLifestealMultiplier();
    float sniperScopeMultiplier = PowerupInventory.Instance.GetSniperScopeMultiplier((int[]) null);
    bool sniped = false;
    float num2 = num1 * sniperScopeMultiplier;
    if ((double) sniperScopeMultiplier > 1.0)
      sniped = true;
    float lightningMultiplier = PowerupInventory.Instance.GetLightningMultiplier((int[]) null);
    float num3 = 1f;
    if (falling)
      num3 = PowerupInventory.Instance.GetFallWingsMultiplier();
    float enforcerMultiplier = PowerupInventory.Instance.GetEnforcerMultiplier((int[]) null, speedWhileShooting);
    return new PowerupCalculations.DamageResult(num2 * (num3 * enforcerMultiplier), crit, lifestealMultiplier, sniped, lightningMultiplier, (double) num3 > 1.0);
  }

  public void HitEffect(AudioClip clip)
  {
    M0 m0 = Object.Instantiate<GameObject>((M0) this.hitFx);
    ((DestroyObject) ((GameObject) m0).AddComponent<DestroyObject>()).time = 2f;
    M0 component = ((GameObject) m0).GetComponent<AudioSource>();
    ((AudioSource) component).set_clip(clip);
    ((AudioSource) component).Play();
  }

  public void SpawnOnHitEffect(int id, bool owner, Vector3 pos, int damage)
  {
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.onHitEffects[id], pos, this.onHitEffects[id].get_transform().get_rotation());
    if (!owner)
      return;
    ((AreaEffect) gameObject.GetComponent<AreaEffect>()).SetDamage(damage);
  }

  public PowerupCalculations.DamageResult GetMaxMultiplier()
  {
    bool crit = true;
    float num1 = (float) PowerupCalculations.randomDamageRange.y * PowerupInventory.Instance.GetStrengthMultiplier((int[]) null);
    if (crit)
      num1 *= 2f;
    float lifestealMultiplier = PowerupInventory.Instance.GetLifestealMultiplier();
    float damageMultiplier = PowerupInventory.Instance.GetSniperScopeDamageMultiplier((int[]) null);
    bool sniped = false;
    float num2 = num1 * damageMultiplier;
    if ((double) damageMultiplier > 1.0)
      sniped = true;
    float lightningMultiplier = PowerupInventory.Instance.GetLightningMultiplier((int[]) null);
    float num3 = 1f;
    if (true)
      num3 = PowerupInventory.Instance.GetFallWingsMultiplier();
    return new PowerupCalculations.DamageResult(num2 * num3, crit, lifestealMultiplier, sniped, lightningMultiplier, (double) num3 > 1.0);
  }

  public PowerupCalculations() => base.\u002Ector();

  public class DamageResult
  {
    public float damageMultiplier;
    public bool crit;
    public float lifesteal;
    public bool sniped;
    public float hammerMultiplier;
    public bool falling;

    public DamageResult(
      float damage,
      bool crit,
      float life,
      bool sniped,
      float hammerMultiplier,
      bool falling)
    {
      this.damageMultiplier = damage;
      this.crit = crit;
      this.lifesteal = life;
      this.sniped = sniped;
      this.hammerMultiplier = hammerMultiplier;
      this.falling = falling;
    }
  }
}
