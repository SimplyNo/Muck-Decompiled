// Decompiled with JetBrains decompiler
// Type: HitableActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class HitableActor : Hitable
{
  public HitableActor.ActorType actorType;

  public override void Hit(int damage, float sharpness, int hitEffect, Vector3 pos)
  {
    if (GameManager.gameSettings.friendlyFire == GameSettings.FriendlyFire.Off || this.actorType != HitableActor.ActorType.Player)
      return;
    ClientSend.PlayerHit(damage, this.id, sharpness, hitEffect, pos);
  }

  private void Update()
  {
  }

  public new virtual int Damage(int damage, int fromClient, int hitEffect, Vector3 pos)
  {
    Vector3 dir = Vector3.op_Subtraction(((Component) GameManager.players[fromClient]).get_transform().get_position(), pos);
    this.SpawnParticles(pos, dir, hitEffect);
    HitEffect hitEffect1 = (HitEffect) hitEffect;
    ((HitNumber) ((GameObject) Object.Instantiate<GameObject>((M0) this.numberFx, pos, Quaternion.get_identity())).GetComponent<HitNumber>()).SetTextAndDir((float) damage, dir, hitEffect1);
    return this.hp;
  }

  public override void OnKill(Vector3 dir)
  {
  }

  protected override void ExecuteHit()
  {
  }

  public enum ActorType
  {
    Player,
    Enemy,
  }
}
