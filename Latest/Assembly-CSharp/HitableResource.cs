// Decompiled with JetBrains decompiler
// Type: HitableResource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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

  protected void Start() => this.materialText = ((Renderer) ((Component) this).GetComponentInChildren<Renderer>()).get_materials()[0].get_mainTexture();

  public override void Hit(int damage, float sharpness, int hitEffect, Vector3 pos)
  {
    if (damage > 0)
    {
      ClientSend.PlayerHitObject(damage, this.id, hitEffect, pos);
    }
    else
    {
      Vector3 vector3_1 = Vector3.op_Addition(((Component) GameManager.players[LocalClient.instance.myId]).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 1.5f));
      Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, pos);
      Vector3 normalized = ((Vector3) ref vector3_2).get_normalized();
      pos = this.hitCollider.ClosestPoint(vector3_1);
      this.SpawnParticles(pos, normalized, hitEffect);
      float num = Vector3.Distance(pos, vector3_1);
      pos = Vector3.op_Addition(pos, Vector3.op_Multiply(Vector3.op_Multiply(normalized, num), 0.5f));
      HitEffect hitEffect1 = (HitEffect) hitEffect;
      ((HitNumber) ((GameObject) Object.Instantiate<GameObject>((M0) this.numberFx, pos, Quaternion.get_identity())).GetComponent<HitNumber>()).SetTextAndDir(0.0f, normalized, hitEffect1);
    }
  }

  protected override void SpawnDeathParticles() => ((Renderer) ((GameObject) Object.Instantiate<GameObject>((M0) this.destroyFx, ((Component) this).get_transform().get_position(), this.destroyFx.get_transform().get_rotation())).GetComponent<ParticleSystemRenderer>()).get_material().set_mainTexture(this.materialText);

  protected override void SpawnParticles(Vector3 pos, Vector3 dir, int hitEffect)
  {
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.hitFx);
    gameObject.get_transform().set_position(pos);
    gameObject.get_transform().set_rotation(Quaternion.LookRotation(dir));
    ((Renderer) gameObject.GetComponent<ParticleSystemRenderer>()).get_material().set_mainTexture(this.materialText);
    HitEffect effect = (HitEffect) hitEffect;
    if (effect == HitEffect.Normal)
      return;
    HitParticles componentInChildren = (HitParticles) gameObject.GetComponentInChildren<HitParticles>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.SetEffect(effect);
  }

  public override void OnKill(Vector3 dir) => ResourceManager.Instance.RemoveItem(this.id);

  private void OnEnable()
  {
    if (this.dontScale)
      return;
    ((Component) this).get_transform().set_localScale(Vector3.get_zero());
  }

  private new void Awake()
  {
    base.Awake();
    if (this.dontScale)
      return;
    if (Vector3.op_Equality(this.defaultScale, Vector3.get_zero()))
      this.defaultScale = ((Component) this).get_transform().get_localScale();
    this.desiredScale = this.defaultScale;
    ((Component) this).get_transform().set_localScale(Vector3.get_zero());
  }

  public void SetDefaultScale(Vector3 scale) => this.defaultScale = scale;

  protected override void ExecuteHit()
  {
    MonoBehaviour.print((object) "changing scale lol");
    this.currentScale = Vector3.op_Multiply(this.defaultScale, 0.7f);
  }

  public void PopIn()
  {
    ((Component) this).get_transform().set_localScale(Vector3.get_zero());
    this.desiredScale = this.defaultScale;
  }

  private void Update()
  {
    if (this.dontScale || (double) Mathf.Abs((float) (((Component) this).get_transform().get_localScale().x - this.desiredScale.x)) < 1.0 / 500.0 && (double) Mathf.Abs((float) (this.desiredScale.x - this.currentScale.x)) < 1.0 / 500.0)
      return;
    this.currentScale = Vector3.Lerp(this.currentScale, this.desiredScale, Time.get_deltaTime() * 10f);
    ((Component) this).get_transform().set_localScale(Vector3.Lerp(((Component) this).get_transform().get_localScale(), this.currentScale, Time.get_deltaTime() * 15f));
  }
}
