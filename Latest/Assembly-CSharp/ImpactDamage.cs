// Decompiled with JetBrains decompiler
// Type: ImpactDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ImpactDamage : MonoBehaviour
{
  public float radius;
  public int baseDamage;
  public bool hitPlayer;
  public bool decreaseWithDistance;
  private float multiplier;
  private List<GameObject> alreadyHit;
  private bool race;

  private void Start()
  {
    if (this.race)
    {
      Object.Destroy((Object) ((Component) this).get_gameObject());
      MonoBehaviour.print((object) "destroying deu to race");
    }
    else
      this.race = true;
    if (!Object.op_Implicit((Object) PlayerMovement.Instance) || GameManager.players[LocalClient.instance.myId].dead)
      return;
    float num1 = Vector3.Distance(((Component) PlayerMovement.Instance).get_transform().get_position(), ((Component) this).get_transform().get_position());
    if (this.hitPlayer)
      num1 = 0.0f;
    if ((double) num1 > (double) this.radius)
      return;
    float num2 = Mathf.Clamp((this.radius - Mathf.Clamp(num1 - 1f, 0.0f, this.radius)) / this.radius, 0.0f, 1f);
    if (!this.decreaseWithDistance)
      num2 = 1f;
    ClientSend.PlayerHit((int) ((double) this.baseDamage * (double) num2), LocalClient.instance.myId, 0.0f, 0, ((Component) this).get_transform().get_position());
  }

  private void OnTriggerEnter(Collider other)
  {
    if (this.alreadyHit.Contains(((Component) other).get_gameObject()))
      return;
    this.alreadyHit.Add(((Component) other).get_gameObject());
    if (this.race)
      Object.Destroy((Object) ((Component) this).get_gameObject());
    else
      this.race = true;
    if (!LocalClient.serverOwner)
      return;
    Hitable componentInChildren = (Hitable) ((Component) ((Component) other).get_transform().get_root()).GetComponentInChildren<Hitable>();
    if (!Object.op_Implicit((Object) componentInChildren))
      return;
    float num = 0.5f;
    componentInChildren.Hit((int) ((double) this.baseDamage * (double) num * (double) this.multiplier), 0.0f, 0, ((Component) this).get_transform().get_position());
    this.multiplier *= 0.5f;
  }

  private void OnDrawGizmos()
  {
    Gizmos.set_color(Color.get_red());
    Gizmos.DrawWireSphere(((Component) this).get_transform().get_position(), this.radius);
  }

  public void SetDamage(int damage) => this.baseDamage = damage;

  public ImpactDamage() => base.\u002Ector();
}
