// Decompiled with JetBrains decompiler
// Type: MobServerNeutral
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MobServerNeutral : MobServer
{
  public int mobZoneId { get; set; }

  private void Start() => this.FindPositionInterval = 12f;

  protected override void Behaviour()
  {
  }

  public override void TookDamage()
  {
    this.mob.SetSpeed(2f);
    this.SyncFindNextPosition();
  }

  protected override Vector3 FindNextPosition()
  {
    this.Invoke("SyncFindNextPosition", 12f);
    return MobZoneManager.Instance.zones[this.mobZoneId].FindRandomPos();
  }

  private void OnDisable() => this.CancelInvoke();

  private void OnEnable()
  {
    this.FindPositionInterval = 10f;
    this.StartRoutines();
  }
}
