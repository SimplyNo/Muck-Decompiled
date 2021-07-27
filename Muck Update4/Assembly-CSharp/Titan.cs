// Decompiled with JetBrains decompiler
// Type: Titan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Titan : MonoBehaviour
{
  public GameObject stompAttack;
  public GameObject jumpFx;
  public Transform stompPosition;
  private Mob m;

  private void Awake() => this.m = this.GetComponent<Mob>();

  public void StompFx()
  {
    ImpactDamage componentInChildren = Object.Instantiate<GameObject>(this.stompAttack, this.stompPosition.transform.position, this.stompAttack.transform.rotation).GetComponentInChildren<ImpactDamage>();
    componentInChildren.baseDamage = (int) ((double) componentInChildren.baseDamage * (double) this.m.multiplier);
  }

  public void JumpFx()
  {
    ImpactDamage componentInChildren = Object.Instantiate<GameObject>(this.jumpFx, this.stompPosition.transform.position, this.stompAttack.transform.rotation).GetComponentInChildren<ImpactDamage>();
    componentInChildren.baseDamage = (int) ((double) componentInChildren.baseDamage * (double) this.m.multiplier);
  }
}
