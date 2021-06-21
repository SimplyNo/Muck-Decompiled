// Decompiled with JetBrains decompiler
// Type: ProjectileController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class ProjectileController : MonoBehaviour
{
  public static ProjectileController Instance;

  private void Awake() => ProjectileController.Instance = this;

  public void SpawnProjectileFromPlayer(
    Vector3 spawnPos,
    Vector3 direction,
    float force,
    int arrowId,
    int fromPlayer)
  {
    InventoryItem allItem = ItemManager.Instance.allItems[arrowId];
    InventoryUI.Instance.arrows.UpdateCell();
    M0 m0 = Object.Instantiate<GameObject>((M0) allItem.prefab);
    ((Renderer) ((GameObject) m0).GetComponent<Renderer>()).set_material(allItem.material);
    ((GameObject) m0).get_transform().set_position(spawnPos);
    ((GameObject) m0).get_transform().set_rotation(Quaternion.LookRotation(direction));
    ((Rigidbody) ((GameObject) m0).GetComponent<Rigidbody>()).AddForce(Vector3.op_Multiply(direction, force));
    Physics.IgnoreCollision((Collider) ((GameObject) m0).GetComponent<Collider>(), GameManager.players[fromPlayer].GetCollider(), true);
    ((Arrow) ((GameObject) m0).GetComponent<Arrow>()).otherPlayersArrow = true;
  }

  public void SpawnMobProjectile(
    Vector3 spawnPos,
    Vector3 direction,
    float force,
    int itemId,
    int mobObjectId)
  {
    InventoryItem allItem = ItemManager.Instance.allItems[itemId];
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) allItem.prefab, spawnPos, Quaternion.LookRotation(direction));
    int attackDamage = allItem.attackDamage;
    float projectileSpeed = allItem.bowComponent.projectileSpeed;
    gameObject.get_transform().set_rotation(Quaternion.LookRotation(direction));
    M0 component1 = gameObject.GetComponent<Rigidbody>();
    ((Rigidbody) component1).AddForce(Vector3.op_Multiply(Vector3.op_Multiply(direction, force), projectileSpeed));
    ((Rigidbody) component1).set_angularVelocity(allItem.rotationOffset);
    MonoBehaviour.print((object) ("mob id: " + (object) mobObjectId + ", in mob manager: " + MobManager.Instance.mobs.ContainsKey(mobObjectId).ToString()));
    if (MobManager.Instance.mobs.ContainsKey(mobObjectId))
    {
      Collider component2 = (Collider) gameObject.GetComponent<Collider>();
      foreach (Collider componentsInChild in (Collider[]) ((Component) ((Component) MobManager.Instance.mobs[mobObjectId]).get_gameObject().get_transform().get_root()).GetComponentsInChildren<Collider>())
        Physics.IgnoreCollision(componentsInChild, component2, true);
      MonoBehaviour.print((object) ("removing collision with mob: " + (object) MobManager.Instance.mobs[mobObjectId]));
    }
    float multiplier = MobManager.Instance.mobs[mobObjectId].multiplier;
    ((EnemyProjectile) gameObject.GetComponent<EnemyProjectile>()).DisableCollider(0.1f);
    ((EnemyProjectile) gameObject.GetComponent<EnemyProjectile>()).damage = (int) ((double) attackDamage * (double) multiplier);
    MonoBehaviour.print((object) ("setting damage to: " + (object) (float) ((double) attackDamage * (double) multiplier)));
  }

  public ProjectileController() => base.\u002Ector();
}
