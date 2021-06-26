// Decompiled with JetBrains decompiler
// Type: HitableMob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HitableMob : Hitable
{
  public MobServer mobServer;
  public float maxFractionHit;

  public Mob mob { get; set; }

  private void Start()
  {
    this.mob = this.GetComponent<Mob>();
    this.mobServer = this.GetComponent<MobServer>();
    this.maxHp = (int) ((double) this.maxHp * (double) GameManager.instance.MobHpMultiplier() * (double) this.mob.multiplier * (double) this.mob.bossMultiplier);
    this.hp = this.maxHp;
  }

  public override void Hit(int damage, float sharpness, int hitEffect, Vector3 hitPos)
  {
    if ((double) this.maxFractionHit > 0.0)
    {
      int num = (int) ((double) this.maxFractionHit * (double) this.maxHp);
      if (damage > num)
      {
        Debug.LogError((object) ("reducing damage from: " + (object) damage + ", to: " + (object) num));
        damage = num;
      }
    }
    ClientSend.PlayerDamageMob(this.id, damage, sharpness, hitEffect, hitPos);
  }

  public override void OnKill(Vector3 dir)
  {
    TestRagdoll component = this.GetComponent<TestRagdoll>();
    if ((bool) (Object) component)
      component.MakeRagdoll(dir);
    MobManager.Instance.RemoveMob(this.id);
    Object.Destroy((Object) this.gameObject);
  }

  protected override void ExecuteHit()
  {
    if (!LocalClient.serverOwner)
      return;
    this.mobServer.TookDamage();
  }
}
