// Decompiled with JetBrains decompiler
// Type: ImpactDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ImpactDamage : MonoBehaviour
{
  public float radius = 1f;
  public int baseDamage;
  public bool hitPlayer;
  public bool decreaseWithDistance;
  private float multiplier = 1f;
  private List<GameObject> alreadyHit = new List<GameObject>();
  private bool race;

  private void Start()
  {
    if (this.race)
      Object.Destroy((Object) this.gameObject);
    else
      this.race = true;
    if (!(bool) (Object) PlayerMovement.Instance || GameManager.players[LocalClient.instance.myId].dead)
      return;
    float num1 = Vector3.Distance(PlayerMovement.Instance.transform.position, this.transform.position);
    if (this.hitPlayer)
      num1 = 0.0f;
    if ((double) num1 > (double) this.radius)
      return;
    float num2 = Mathf.Clamp((this.radius - Mathf.Clamp(num1 - 1f, 0.0f, this.radius)) / this.radius, 0.0f, 1f);
    if (!this.decreaseWithDistance)
      num2 = 1f;
    ClientSend.PlayerHit((int) ((double) this.baseDamage * (double) num2), LocalClient.instance.myId, 0.0f, 0, this.transform.position);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (this.alreadyHit.Contains(other.gameObject))
      return;
    this.alreadyHit.Add(other.gameObject);
    if (this.race)
      Object.Destroy((Object) this.gameObject);
    else
      this.race = true;
    if (!LocalClient.serverOwner)
      return;
    Hitable componentInChildren = other.transform.root.GetComponentInChildren<Hitable>();
    if (!(bool) (Object) componentInChildren)
      return;
    float num = 1f;
    if (other.CompareTag("Build"))
    {
      num = 0.5f;
      this.multiplier *= 0.5f;
    }
    componentInChildren.Hit((int) ((double) this.baseDamage * (double) num * (double) this.multiplier), 0.0f, 0, this.transform.position, -1);
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(this.transform.position, this.radius);
  }

  public void SetDamage(int damage) => this.baseDamage = damage;
}
