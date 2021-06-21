// Decompiled with JetBrains decompiler
// Type: Hitable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class Hitable : MonoBehaviour, SharedObject
{
  protected int id;
  public string entityName;
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
    foreach (Collider component in this.GetComponents<Collider>())
    {
      if (!component.isTrigger)
        this.hitCollider = component;
    }
  }

  public abstract void Hit(int damage, float sharpness, int HitEffect, Vector3 pos);

  public virtual int Damage(int newHp, int fromClient, int hitEffect, Vector3 pos)
  {
    Vector3 normalized = (GameManager.players[fromClient].transform.position + Vector3.up * 1.5f - pos).normalized;
    this.SpawnParticles(pos, normalized, hitEffect);
    HitEffect hitEffect1 = (HitEffect) hitEffect;
    int num = this.hp - newHp;
    if ((double) Vector3.Distance(PlayerMovement.Instance.playerCam.position, this.transform.position) < 100.0)
      Object.Instantiate<GameObject>(this.numberFx, pos, Quaternion.identity).GetComponent<HitNumber>().SetTextAndDir((float) num, normalized, hitEffect1);
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
    if ((double) Vector3.Distance(PlayerMovement.Instance.playerCam.position, this.transform.position) > 100.0)
      return;
    GameObject gameObject = Object.Instantiate<GameObject>(this.hitFx);
    gameObject.transform.position = pos;
    gameObject.transform.rotation = Quaternion.LookRotation(dir);
    HitEffect effect = (HitEffect) hitEffect;
    if (effect == HitEffect.Normal)
      return;
    HitParticles componentInChildren = gameObject.GetComponentInChildren<HitParticles>();
    if (!((Object) componentInChildren != (Object) null))
      return;
    componentInChildren.SetEffect(effect);
  }

  protected virtual void SpawnDeathParticles() => Object.Instantiate<GameObject>(this.destroyFx, this.transform.position, this.destroyFx.transform.rotation);

  public void KillObject(Vector3 dir)
  {
    this.SpawnDeathParticles();
    this.OnKill(dir);
  }

  public abstract void OnKill(Vector3 dir);

  protected abstract void ExecuteHit();

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;
}
