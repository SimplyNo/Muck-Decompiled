// Decompiled with JetBrains decompiler
// Type: HitableResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HitableResource : Hitable
{
  public InventoryItem.ItemType compatibleItem;
  public int minTier;
  [Header("Loot")]
  public InventoryItem dropItem;
  public InventoryItem[] dropExtra;
  public float[] dropChance;
  public int amount;
  public bool dontScale;
  private Texture materialText;
  public int poolId;
  private Vector3 defaultScale;
  private float scaleMultiplier;
  private Vector3 desiredScale;
  private Vector3 currentScale;

  protected void Start() => this.materialText = this.GetComponentInChildren<Renderer>().materials[0].mainTexture;

  public override void Hit(int damage, float sharpness, int hitEffect, Vector3 pos)
  {
    if (damage > 0)
    {
      ClientSend.PlayerHitObject(damage, this.id, hitEffect, pos);
    }
    else
    {
      Vector3 vector3 = GameManager.players[LocalClient.instance.myId].transform.position + Vector3.up * 1.5f;
      Vector3 normalized = (vector3 - pos).normalized;
      pos = this.hitCollider.ClosestPoint(vector3);
      this.SpawnParticles(pos, normalized, hitEffect);
      float num = Vector3.Distance(pos, vector3);
      pos += normalized * num * 0.5f;
      HitEffect hitEffect1 = (HitEffect) hitEffect;
      Object.Instantiate<GameObject>(this.numberFx, pos, Quaternion.identity).GetComponent<HitNumber>().SetTextAndDir(0.0f, normalized, hitEffect1);
    }
  }

  protected override void SpawnDeathParticles() => Object.Instantiate<GameObject>(this.destroyFx, this.transform.position, this.destroyFx.transform.rotation).GetComponent<ParticleSystemRenderer>().material.mainTexture = this.materialText;

  protected override void SpawnParticles(Vector3 pos, Vector3 dir, int hitEffect)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(this.hitFx);
    gameObject.transform.position = pos;
    gameObject.transform.rotation = Quaternion.LookRotation(dir);
    gameObject.GetComponent<ParticleSystemRenderer>().material.mainTexture = this.materialText;
    HitEffect effect = (HitEffect) hitEffect;
    if (effect == HitEffect.Normal)
      return;
    HitParticles componentInChildren = gameObject.GetComponentInChildren<HitParticles>();
    if (!((Object) componentInChildren != (Object) null))
      return;
    componentInChildren.SetEffect(effect);
  }

  public override void OnKill(Vector3 dir) => ResourceManager.Instance.RemoveItem(this.id);

  private void OnEnable()
  {
    if (this.dontScale)
      return;
    this.transform.localScale = Vector3.zero;
  }

  private new void Awake()
  {
    base.Awake();
    if (this.dontScale)
      return;
    if (this.defaultScale == Vector3.zero)
      this.defaultScale = this.transform.localScale;
    this.desiredScale = this.defaultScale;
    this.transform.localScale = Vector3.zero;
  }

  public void SetDefaultScale(Vector3 scale) => this.defaultScale = scale;

  protected override void ExecuteHit()
  {
    MonoBehaviour.print((object) "changing scale lol");
    this.currentScale = this.defaultScale * 0.7f;
  }

  public void PopIn()
  {
    this.transform.localScale = Vector3.zero;
    this.desiredScale = this.defaultScale;
  }

  private void Update()
  {
    if (this.dontScale || (double) Mathf.Abs(this.transform.localScale.x - this.desiredScale.x) < 1.0 / 500.0 && (double) Mathf.Abs(this.desiredScale.x - this.currentScale.x) < 1.0 / 500.0)
      return;
    this.currentScale = Vector3.Lerp(this.currentScale, this.desiredScale, Time.deltaTime * 10f);
    this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.currentScale, Time.deltaTime * 15f);
  }
}
