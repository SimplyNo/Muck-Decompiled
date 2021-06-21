// Decompiled with JetBrains decompiler
// Type: Hitable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public abstract class Hitable : MonoBehaviour, SharedObject
{
  protected int id;
  public string entityName;
  public bool canHitMoreThanOnce;
  public LootDrop dropTable;
  public int hp;
  public int maxHp;
  public GameObject destroyFx;
  public GameObject hitFx;
  public GameObject numberFx;
  protected Collider hitCollider;

  protected void Awake()
  {
    this.hp = this.maxHp;
    foreach (Collider component in (Collider[]) ((Component) this).GetComponents<Collider>())
    {
      if (!component.get_isTrigger())
        this.hitCollider = component;
    }
  }

  public abstract void Hit(int damage, float sharpness, int HitEffect, Vector3 pos);

  public virtual int Damage(int newHp, int fromClient, int hitEffect, Vector3 pos)
  {
    Vector3 vector3 = Vector3.op_Subtraction(Vector3.op_Addition(((Component) GameManager.players[fromClient]).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 1.5f)), pos);
    Vector3 normalized = ((Vector3) ref vector3).get_normalized();
    this.SpawnParticles(pos, normalized, hitEffect);
    HitEffect hitEffect1 = (HitEffect) hitEffect;
    int num = this.hp - newHp;
    if ((double) Vector3.Distance(PlayerMovement.Instance.playerCam.get_position(), ((Component) this).get_transform().get_position()) < 100.0)
      ((HitNumber) ((GameObject) Object.Instantiate<GameObject>((M0) this.numberFx, pos, Quaternion.get_identity())).GetComponent<HitNumber>()).SetTextAndDir((float) num, normalized, hitEffect1);
    this.hp = newHp;
    if (this.hp <= 0)
    {
      this.hp = 0;
      this.KillObject(normalized);
    }
    this.ExecuteHit();
    return this.hp;
  }

  protected virtual void SpawnParticles(Vector3 pos, Vector3 dir, int hitEffect)
  {
    if ((double) Vector3.Distance(PlayerMovement.Instance.playerCam.get_position(), ((Component) this).get_transform().get_position()) > 100.0)
      return;
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.hitFx);
    gameObject.get_transform().set_position(pos);
    gameObject.get_transform().set_rotation(Quaternion.LookRotation(dir));
    HitEffect effect = (HitEffect) hitEffect;
    if (effect == HitEffect.Normal)
      return;
    HitParticles componentInChildren = (HitParticles) gameObject.GetComponentInChildren<HitParticles>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    componentInChildren.SetEffect(effect);
  }

  protected virtual void SpawnDeathParticles() => Object.Instantiate<GameObject>((M0) this.destroyFx, ((Component) this).get_transform().get_position(), this.destroyFx.get_transform().get_rotation());

  public void KillObject(Vector3 dir)
  {
    this.SpawnDeathParticles();
    this.OnKill(dir);
  }

  public abstract void OnKill(Vector3 dir);

  protected abstract void ExecuteHit();

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  protected Hitable() => base.\u002Ector();
}
