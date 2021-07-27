// Decompiled with JetBrains decompiler
// Type: EnemyProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
  public GameObject hitFx;
  private bool done;
  public bool collideWithPlayerAndBuildOnly;
  public bool ignoreGround;
  public Transform spawnPos;
  public float hideFxDistance = 40f;

  public int damage { get; set; }

  private void Awake() => this.Invoke("DestroySelf", 10f);

  public void DisableCollider(float time)
  {
    if (!(bool) (Object) this.GetComponent<Collider>())
      return;
    this.GetComponent<Collider>().enabled = false;
    this.Invoke("ActivateCollider", time);
  }

  private void ActivateCollider() => this.GetComponent<Collider>().enabled = true;

  private void OnCollisionEnter(Collision other)
  {
    int layer = other.gameObject.layer;
    if (this.ignoreGround && other.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || this.collideWithPlayerAndBuildOnly && layer != LayerMask.NameToLayer("Player") && layer != LayerMask.NameToLayer("Object") || this.done)
      return;
    this.done = true;
    bool flag = layer == LayerMask.NameToLayer("Player") && other.gameObject.CompareTag("Local");
    if (LocalClient.serverOwner && layer == LayerMask.NameToLayer("Object"))
      other.gameObject.CompareTag("Build");
    Object.Destroy((Object) this.gameObject);
    if ((double) Vector3.Distance(this.transform.position, PlayerMovement.Instance.playerCam.position) >= (double) this.hideFxDistance)
      return;
    GameObject gameObject = Object.Instantiate<GameObject>(this.hitFx, this.transform.position, Quaternion.LookRotation(other.GetContact(0).normal));
    gameObject.transform.rotation = Quaternion.LookRotation(other.GetContact(0).normal);
    ImpactDamage componentInChildren = gameObject.GetComponentInChildren<ImpactDamage>();
    componentInChildren.SetDamage(this.damage);
    componentInChildren.hitPlayer = flag;
    if (!(bool) (Object) this.spawnPos)
      return;
    gameObject.transform.position = this.spawnPos.position;
  }

  private void DestroySelf() => Object.Destroy((Object) this.gameObject);
}
