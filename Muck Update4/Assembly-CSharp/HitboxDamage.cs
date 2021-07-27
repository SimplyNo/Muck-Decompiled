// Decompiled with JetBrains decompiler
// Type: HitboxDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
  public bool dontStopHitbox;
  public int baseDamage;
  private float multiplier = 1f;
  private List<GameObject> alreadyHit = new List<GameObject>();
  public Vector3 pushPlayer;
  public float hitboxTime = 0.15f;
  private bool playerHit;

  private void Awake()
  {
    if (this.dontStopHitbox)
      return;
    this.Invoke("DisableHitbox", this.hitboxTime);
  }

  private void DisableHitbox() => this.GetComponent<Collider>().enabled = false;

  public void Reset()
  {
    this.alreadyHit = new List<GameObject>();
    this.playerHit = false;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (this.alreadyHit.Contains(other.gameObject))
      return;
    this.alreadyHit.Add(other.gameObject);
    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    {
      if (this.playerHit || !other.CompareTag("Local") || (!(bool) (Object) PlayerMovement.Instance || GameManager.players[LocalClient.instance.myId].dead))
        return;
      this.playerHit = true;
      ClientSend.PlayerHit((int) ((double) this.baseDamage * (double) this.multiplier), LocalClient.instance.myId, 0.0f, 0, this.transform.position);
      PlayerMovement.Instance.grounded = false;
      PlayerMovement.Instance.GetRb().velocity += this.pushPlayer;
      PlayerMovement.Instance.PushPlayer();
    }
    else
    {
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
      this.multiplier *= 0.5f;
    }
  }

  public void SetDamage(int damage) => this.baseDamage = damage;
}
