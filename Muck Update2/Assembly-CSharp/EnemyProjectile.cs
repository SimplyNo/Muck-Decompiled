// Decompiled with JetBrains decompiler
// Type: EnemyProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
  public GameObject hitFx;
  private bool done;
  public bool collideWithPlayerAndBuildOnly;
  public bool ignoreGround;
  public Transform spawnPos;
  public float hideFxDistance;

  public int damage { get; set; }

  private void Awake() => this.Invoke("DestroySelf", 10f);

  public void DisableCollider(float time)
  {
    ((Collider) ((Component) this).GetComponent<Collider>()).set_enabled(false);
    this.Invoke("ActivateCollider", time);
  }

  private void ActivateCollider() => ((Collider) ((Component) this).GetComponent<Collider>()).set_enabled(true);

  private void OnCollisionEnter(Collision other)
  {
    int layer = other.get_gameObject().get_layer();
    if (this.ignoreGround && ((Component) other.get_collider()).get_gameObject().get_layer() == LayerMask.NameToLayer("Ground") || this.collideWithPlayerAndBuildOnly && layer != LayerMask.NameToLayer("Player") && layer != LayerMask.NameToLayer("Object") || this.done)
      return;
    this.done = true;
    bool flag = layer == LayerMask.NameToLayer("Player") && other.get_gameObject().CompareTag("Local");
    if (LocalClient.serverOwner && layer == LayerMask.NameToLayer("Object"))
      other.get_gameObject().CompareTag("Build");
    Object.Destroy((Object) ((Component) this).get_gameObject());
    if ((double) Vector3.Distance(((Component) this).get_transform().get_position(), PlayerMovement.Instance.playerCam.get_position()) >= (double) this.hideFxDistance)
      return;
    GameObject hitFx = this.hitFx;
    Vector3 position = ((Component) this).get_transform().get_position();
    ContactPoint contact1 = other.GetContact(0);
    Quaternion quaternion1 = Quaternion.LookRotation(((ContactPoint) ref contact1).get_normal());
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) hitFx, position, quaternion1);
    Transform transform = gameObject.get_transform();
    ContactPoint contact2 = other.GetContact(0);
    Quaternion quaternion2 = Quaternion.LookRotation(((ContactPoint) ref contact2).get_normal());
    transform.set_rotation(quaternion2);
    M0 componentInChildren = gameObject.GetComponentInChildren<ImpactDamage>();
    ((ImpactDamage) componentInChildren).SetDamage(this.damage);
    ((ImpactDamage) componentInChildren).hitPlayer = flag;
    if (!Object.op_Implicit((Object) this.spawnPos))
      return;
    gameObject.get_transform().set_position(this.spawnPos.get_position());
  }

  private void DestroySelf() => Object.Destroy((Object) ((Component) this).get_gameObject());

  public EnemyProjectile() => base.\u002Ector();
}
