// Decompiled with JetBrains decompiler
// Type: SpawnAttackFx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SpawnAttackFx : MonoBehaviour
{
  public GameObject[] attackFx;
  public Transform spawnPos;
  private Mob m;

  private void Awake() => this.m = (Mob) ((Component) this).GetComponent<Mob>();

  public void SpawnFx(int n)
  {
    M0 componentInChildren = ((GameObject) Object.Instantiate<GameObject>((M0) this.attackFx[n], this.spawnPos.get_position(), this.attackFx[n].get_transform().get_rotation())).GetComponentInChildren<ImpactDamage>();
    ((ImpactDamage) componentInChildren).baseDamage = (int) ((double) ((ImpactDamage) componentInChildren).baseDamage * (double) this.m.multiplier);
  }

  public SpawnAttackFx() => base.\u002Ector();
}
