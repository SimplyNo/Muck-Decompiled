// Decompiled with JetBrains decompiler
// Type: ProjectileController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    GameObject gameObject = Object.Instantiate<GameObject>(allItem.prefab);
    gameObject.GetComponent<Renderer>().material = allItem.material;
    gameObject.transform.position = spawnPos;
    gameObject.transform.rotation = Quaternion.LookRotation(direction);
    gameObject.GetComponent<Rigidbody>().AddForce(direction * force);
    Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameManager.players[fromPlayer].GetCollider(), true);
    gameObject.GetComponent<Arrow>().otherPlayersArrow = true;
  }

  public void SpawnMobProjectile(
    Vector3 spawnPos,
    Vector3 direction,
    float force,
    int itemId,
    int mobObjectId)
  {
    InventoryItem allItem = ItemManager.Instance.allItems[itemId];
    GameObject gameObject = Object.Instantiate<GameObject>(allItem.prefab, spawnPos, Quaternion.LookRotation(direction));
    int attackDamage = allItem.attackDamage;
    float projectileSpeed = allItem.bowComponent.projectileSpeed;
    gameObject.transform.rotation = Quaternion.LookRotation(direction);
    Rigidbody component1 = gameObject.GetComponent<Rigidbody>();
    if ((bool) (Object) component1)
    {
      component1.AddForce(direction * force * projectileSpeed);
      component1.angularVelocity = allItem.rotationOffset;
    }
    MonoBehaviour.print((object) ("mob id: " + (object) mobObjectId + ", in mob manager: " + MobManager.Instance.mobs.ContainsKey(mobObjectId).ToString()));
    if (MobManager.Instance.mobs.ContainsKey(mobObjectId))
    {
      Collider component2 = gameObject.GetComponent<Collider>();
      if ((Object) component2 != (Object) null)
      {
        foreach (Collider componentsInChild in MobManager.Instance.mobs[mobObjectId].gameObject.transform.root.GetComponentsInChildren<Collider>())
          Physics.IgnoreCollision(componentsInChild, component2, true);
      }
    }
    float multiplier = MobManager.Instance.mobs[mobObjectId].multiplier;
    gameObject.GetComponent<EnemyProjectile>().DisableCollider(0.1f);
    gameObject.GetComponent<EnemyProjectile>().damage = (int) ((double) attackDamage * (double) multiplier);
    MonoBehaviour.print((object) ("setting damage to: " + (object) (float) ((double) attackDamage * (double) multiplier)));
  }
}
