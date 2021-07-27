// Decompiled with JetBrains decompiler
// Type: HitableActor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HitableActor : Hitable
{
  public HitableActor.ActorType actorType;

  public override void Hit(
    int damage,
    float sharpness,
    int hitEffect,
    Vector3 pos,
    int hitWeaponType)
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
    Vector3 dir = GameManager.players[fromClient].transform.position - pos;
    this.SpawnParticles(pos, dir, hitEffect);
    HitEffect hitEffect1 = (HitEffect) hitEffect;
    Object.Instantiate<GameObject>(this.numberFx, pos, Quaternion.identity).GetComponent<HitNumber>().SetTextAndDir((float) damage, dir, hitEffect1);
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
