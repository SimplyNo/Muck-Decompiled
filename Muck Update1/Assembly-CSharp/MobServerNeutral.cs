// Decompiled with JetBrains decompiler
// Type: MobServerNeutral
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MobServerNeutral : MobServer
{
  public int mobZoneId { get; set; }

  private void Start() => this.FindPositionInterval = 10f;

  protected override void Behaviour()
  {
  }

  public override void TookDamage()
  {
    this.mob.SetSpeed(2f);
    this.SyncFindNextPosition();
  }

  protected override Vector3 FindNextPosition() => MobZoneManager.Instance.zones[this.mobZoneId].FindRandomPos();

  private void OnDisable() => this.CancelInvoke();

  private void OnEnable()
  {
    this.FindPositionInterval = 10f;
    this.StartRoutines();
  }
}
