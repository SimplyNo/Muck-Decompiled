// Decompiled with JetBrains decompiler
// Type: GuardianSpikes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GuardianSpikes : MonoBehaviour
{
  public GameObject warningFx;
  public GameObject spikeAttack;
  public InventoryItem attack;
  public EnemyProjectile projectile;
  private EnemyAttackIndicator indicator;

  private void Awake()
  {
    this.indicator = Object.Instantiate<GameObject>(this.warningFx, this.transform.position, this.warningFx.transform.rotation).GetComponent<EnemyAttackIndicator>();
    this.indicator.SetWarning(this.attack.bowComponent.timeToImpact, this.attack.bowComponent.attackSize);
    this.Invoke("SpawnAttack", this.attack.bowComponent.timeToImpact);
  }

  private void SpawnAttack()
  {
    HitboxDamage componentInChildren = Object.Instantiate<GameObject>(this.spikeAttack, this.indicator.transform.position, this.spikeAttack.transform.rotation).GetComponentInChildren<HitboxDamage>();
    if ((bool) (Object) componentInChildren)
      componentInChildren.baseDamage = this.projectile.damage;
    Object.Destroy((Object) this.gameObject);
  }
}
