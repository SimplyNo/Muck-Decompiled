// Decompiled with JetBrains decompiler
// Type: HitableMob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class HitableMob : Hitable
{
  public MobServer mobServer;

  public Mob mob { get; set; }

  private void Start()
  {
    this.mob = (Mob) ((Component) this).GetComponent<Mob>();
    this.mobServer = (MobServer) ((Component) this).GetComponent<MobServer>();
    this.maxHp = (int) ((double) this.maxHp * (double) GameManager.instance.MobHpMultiplier() * (double) this.mob.multiplier * (double) this.mob.bossMultiplier);
    this.hp = this.maxHp;
  }

  public override void Hit(int damage, float sharpness, int hitEffect, Vector3 hitPos) => ClientSend.PlayerDamageMob(this.id, damage, sharpness, hitEffect, hitPos);

  public override void OnKill(Vector3 dir)
  {
    TestRagdoll component = (TestRagdoll) ((Component) this).GetComponent<TestRagdoll>();
    if (Object.op_Implicit((Object) component))
      component.MakeRagdoll(dir);
    MobManager.Instance.RemoveMob(this.id);
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }

  protected override void ExecuteHit()
  {
    if (!LocalClient.serverOwner)
      return;
    this.mobServer.TookDamage();
  }
}
