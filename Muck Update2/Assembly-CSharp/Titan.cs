// Decompiled with JetBrains decompiler
// Type: Titan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class Titan : MonoBehaviour
{
  public GameObject stompAttack;
  public GameObject jumpFx;
  public Transform stompPosition;
  private Mob m;

  private void Awake() => this.m = (Mob) ((Component) this).GetComponent<Mob>();

  public void StompFx()
  {
    M0 componentInChildren = ((GameObject) Object.Instantiate<GameObject>((M0) this.stompAttack, ((Component) this.stompPosition).get_transform().get_position(), this.stompAttack.get_transform().get_rotation())).GetComponentInChildren<ImpactDamage>();
    ((ImpactDamage) componentInChildren).baseDamage = (int) ((double) ((ImpactDamage) componentInChildren).baseDamage * (double) this.m.multiplier);
  }

  public void JumpFx()
  {
    M0 componentInChildren = ((GameObject) Object.Instantiate<GameObject>((M0) this.jumpFx, ((Component) this.stompPosition).get_transform().get_position(), this.stompAttack.get_transform().get_rotation())).GetComponentInChildren<ImpactDamage>();
    ((ImpactDamage) componentInChildren).baseDamage = (int) ((double) ((ImpactDamage) componentInChildren).baseDamage * (double) this.m.multiplier);
  }

  public Titan() => base.\u002Ector();
}
