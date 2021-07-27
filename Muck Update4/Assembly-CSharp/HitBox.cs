// Decompiled with JetBrains decompiler
// Type: HitBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
  public Transform playerCam;
  public LayerMask whatIsHittable;
  private List<Vector3> hitPoints = new List<Vector3>();
  private List<Hitable> alreadyHit = new List<Hitable>();
  public GameObject dirt;
  public GameObject sand;

  public void UseHitbox()
  {
    this.alreadyHit.Clear();
    this.alreadyHit = new List<Hitable>();
    if ((UnityEngine.Object) Hotbar.Instance.currentItem == (UnityEngine.Object) null)
      return;
    InventoryItem currentItem = Hotbar.Instance.currentItem;
    RaycastHit[] array = Physics.SphereCastAll(this.playerCam.position + this.playerCam.forward * 0.1f, 3f, this.playerCam.forward, 1.2f + currentItem.attackRange + PlayerStatus.Instance.currentChunkArmorMultiplier, (int) this.whatIsHittable);
    Array.Sort<RaycastHit>(array, (Comparison<RaycastHit>) ((x, y) => x.distance.CompareTo(y.distance)));
    if (array.Length < 1)
      return;
    bool falling = !PlayerMovement.Instance.grounded && (double) PlayerMovement.Instance.GetVelocity().y < 0.0;
    PowerupCalculations.DamageResult damageMultiplier1 = PowerupCalculations.Instance.GetDamageMultiplier(falling);
    float damageMultiplier2 = damageMultiplier1.damageMultiplier;
    bool flag1 = damageMultiplier1.crit;
    float lifesteal = damageMultiplier1.lifesteal;
    float sharpness = currentItem.sharpness;
    bool flag2 = false;
    int num1 = 0;
    float num2 = 1f;
    float num3 = 1f;
    if (flag1)
      num3 = 2f;
    Vector3 pos = Vector3.zero;
    bool flag3 = array[0].transform.CompareTag("Build");
    int HitWeaponType = 0;
    if (currentItem.name == "Rock")
      HitWeaponType = 2;
    foreach (RaycastHit raycastHit in array)
    {
      Collider collider = raycastHit.collider;
      Hitable component1 = collider.transform.root.GetComponent<Hitable>();
      if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null) && (collider.gameObject.layer != LayerMask.NameToLayer("Player") || component1.GetId() != LocalClient.instance.myId))
      {
        if (!flag3 && raycastHit.transform.CompareTag("Build"))
          return;
        if (!this.alreadyHit.Contains(component1))
        {
          if (!component1.canHitMoreThanOnce)
            this.alreadyHit.Add(component1);
          int damage = 0;
          if (collider.gameObject.layer == LayerMask.NameToLayer("Object"))
          {
            HitableResource hitableResource = (HitableResource) component1;
            if (currentItem.type == hitableResource.compatibleItem && currentItem.tier >= hitableResource.minTier || hitableResource.compatibleItem == InventoryItem.ItemType.Item)
            {
              float resourceMultiplier = PowerupInventory.Instance.GetResourceMultiplier((int[]) null);
              damage = (int) ((double) currentItem.resourceDamage * (double) damageMultiplier2 * (double) resourceMultiplier * (double) num2);
              CameraShaker.Instance.DamageShake(0.1f * num3);
            }
          }
          else
          {
            CameraShaker.Instance.DamageShake(0.4f);
            int num4 = currentItem.attackDamage;
            if (currentItem.tag == InventoryItem.ItemTag.Arrow)
              num4 = 1;
            damage = (int) ((double) num4 * (double) damageMultiplier2 * (double) num2);
            Mob component2 = component1.GetComponent<Mob>();
            if ((bool) (UnityEngine.Object) component2 && currentItem.attackTypes != null && component2.mobType.weaknesses != null)
            {
              foreach (MobType.Weakness weakness in component2.mobType.weaknesses)
              {
                foreach (MobType.Weakness attackType in currentItem.attackTypes)
                {
                  if (attackType == weakness)
                  {
                    flag1 = true;
                    damage *= 2;
                  }
                }
              }
            }
          }
          HitEffect hitEffect = HitEffect.Normal;
          if (damageMultiplier1.sniped)
            hitEffect = HitEffect.Big;
          else if (flag1)
            hitEffect = HitEffect.Crit;
          else if (damageMultiplier1.falling)
            hitEffect = HitEffect.Falling;
          component1.Hit(damage, sharpness, (int) hitEffect, raycastHit.collider.ClosestPoint(PlayerMovement.Instance.playerCam.position), HitWeaponType);
          num2 *= 0.5f;
          PlayerStatus.Instance.Heal(Mathf.CeilToInt((float) damage * lifesteal));
          if (flag1)
            PowerupInventory.Instance.StartJuice();
          if (!flag2)
          {
            pos = raycastHit.collider.ClosestPoint(PlayerMovement.Instance.playerCam.position);
            num1 = damage;
          }
          flag2 = true;
        }
      }
    }
    if (!flag2)
      return;
    if (damageMultiplier1.sniped)
      PowerupCalculations.Instance.HitEffect(PowerupCalculations.Instance.sniperSfx);
    if ((double) damageMultiplier2 <= 0.0 || (double) damageMultiplier1.hammerMultiplier <= 0.0)
      return;
    int num5 = 0;
    PowerupCalculations.Instance.SpawnOnHitEffect(num5, true, pos, (int) ((double) num1 * (double) damageMultiplier1.hammerMultiplier));
    ClientSend.SpawnEffect(num5, pos);
  }

  private void ShovelHitGround(Collider other)
  {
    Vector3 vector3 = other.ClosestPoint(this.transform.position);
    TextureData.TerrainType biome = WorldUtility.WorldHeightToBiome(vector3.y);
    GameObject original = this.dirt;
    InventoryItem inventoryItem = (InventoryItem) null;
    float num1 = 0.5f;
    float num2 = 0.15f;
    switch (biome)
    {
      case TextureData.TerrainType.Sand:
        original = this.sand;
        if ((double) UnityEngine.Random.Range(0.0f, 1f) < (double) num2)
        {
          inventoryItem = ItemManager.Instance.GetItemByName("Flint");
          break;
        }
        break;
      case TextureData.TerrainType.Grass:
        original = this.dirt;
        if ((double) UnityEngine.Random.Range(0.0f, 1f) < (double) num1)
        {
          inventoryItem = ItemManager.Instance.GetItemByName("Rock");
          break;
        }
        break;
    }
    UnityEngine.Object.Instantiate<GameObject>(original, vector3, Quaternion.LookRotation(Vector3.up));
    if (!((UnityEngine.Object) inventoryItem != (UnityEngine.Object) null))
      return;
    ClientSend.DropItemAtPosition(inventoryItem.id, 1, vector3);
  }

  private void OnDrawGizmos()
  {
    foreach (Vector3 hitPoint in this.hitPoints)
      Gizmos.DrawWireSphere(hitPoint, 1.5f);
  }
}
