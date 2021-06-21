// Decompiled with JetBrains decompiler
// Type: HitBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
  public Transform playerCam;
  public LayerMask whatIsHittable;
  private List<Vector3> hitPoints;
  private List<Hitable> alreadyHit;
  public GameObject dirt;
  public GameObject sand;

  public void UseHitbox()
  {
    this.alreadyHit.Clear();
    this.alreadyHit = new List<Hitable>();
    if (Object.op_Equality((Object) Hotbar.Instance.currentItem, (Object) null))
      return;
    float num1 = 1.2f + PlayerStatus.Instance.currentChunkArmorMultiplier;
    RaycastHit[] array = Physics.SphereCastAll(Vector3.op_Addition(this.playerCam.get_position(), Vector3.op_Multiply(this.playerCam.get_forward(), 0.1f)), 3f, this.playerCam.get_forward(), num1, LayerMask.op_Implicit(this.whatIsHittable));
    Array.Sort<RaycastHit>(array, (Comparison<RaycastHit>) ((x, y) => ((RaycastHit) ref x).get_distance().CompareTo(((RaycastHit) ref y).get_distance())));
    if (array.Length < 1)
      return;
    InventoryItem currentItem = Hotbar.Instance.currentItem;
    bool falling = !PlayerMovement.Instance.grounded && PlayerMovement.Instance.GetVelocity().y < 0.0;
    PowerupCalculations.DamageResult damageMultiplier1 = PowerupCalculations.Instance.GetDamageMultiplier(falling);
    float damageMultiplier2 = damageMultiplier1.damageMultiplier;
    bool flag1 = damageMultiplier1.crit;
    float lifesteal = damageMultiplier1.lifesteal;
    float sharpness = currentItem.sharpness;
    bool flag2 = false;
    int num2 = 0;
    float num3 = 1f;
    float num4 = 1f;
    if (flag1)
      num4 = 2f;
    Vector3 pos = Vector3.get_zero();
    bool flag3 = ((Component) ((RaycastHit) ref array[0]).get_transform()).CompareTag("Build");
    foreach (RaycastHit raycastHit in array)
    {
      Collider collider = ((RaycastHit) ref raycastHit).get_collider();
      Hitable component1 = (Hitable) ((Component) ((Component) collider).get_transform().get_root()).GetComponent<Hitable>();
      if (!Object.op_Equality((Object) component1, (Object) null) && (((Component) collider).get_gameObject().get_layer() != LayerMask.NameToLayer("Player") || component1.GetId() != LocalClient.instance.myId))
      {
        if (!flag3 && ((Component) ((RaycastHit) ref raycastHit).get_transform()).CompareTag("Build"))
          return;
        if (!this.alreadyHit.Contains(component1))
        {
          if (!component1.canHitMoreThanOnce)
            this.alreadyHit.Add(component1);
          int damage = 0;
          if (((Component) collider).get_gameObject().get_layer() == LayerMask.NameToLayer("Object"))
          {
            HitableResource hitableResource = (HitableResource) component1;
            if (currentItem.type == hitableResource.compatibleItem && currentItem.tier >= hitableResource.minTier || hitableResource.compatibleItem == InventoryItem.ItemType.Item)
            {
              float resourceMultiplier = PowerupInventory.Instance.GetResourceMultiplier((int[]) null);
              damage = (int) ((double) currentItem.resourceDamage * (double) damageMultiplier2 * (double) resourceMultiplier * (double) num3);
              CameraShaker.Instance.DamageShake(0.1f * num4);
            }
          }
          else
          {
            CameraShaker.Instance.DamageShake(0.4f);
            int num5 = currentItem.attackDamage;
            if (currentItem.tag == InventoryItem.ItemTag.Arrow)
              num5 = 1;
            damage = (int) ((double) num5 * (double) damageMultiplier2 * (double) num3);
            Mob component2 = (Mob) ((Component) component1).GetComponent<Mob>();
            if (Object.op_Implicit((Object) component2) && currentItem.attackTypes != null && component2.mobType.weaknesses != null)
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
          component1.Hit(damage, sharpness, (int) hitEffect, ((RaycastHit) ref raycastHit).get_collider().ClosestPoint(PlayerMovement.Instance.playerCam.get_position()));
          num3 *= 0.5f;
          PlayerStatus.Instance.Heal(Mathf.CeilToInt((float) damage * lifesteal));
          if (flag1)
            PowerupInventory.Instance.StartJuice();
          if (!flag2)
          {
            pos = ((RaycastHit) ref raycastHit).get_collider().ClosestPoint(PlayerMovement.Instance.playerCam.get_position());
            num2 = damage;
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
    int num6 = 0;
    PowerupCalculations.Instance.SpawnOnHitEffect(num6, true, pos, (int) ((double) num2 * (double) damageMultiplier1.hammerMultiplier));
    ClientSend.SpawnEffect(num6, pos);
  }

  private void ShovelHitGround(Collider other)
  {
    Vector3 pos = other.ClosestPoint(((Component) this).get_transform().get_position());
    TextureData.TerrainType biome = WorldUtility.WorldHeightToBiome((float) pos.y);
    GameObject gameObject = this.dirt;
    InventoryItem inventoryItem = (InventoryItem) null;
    float num1 = 0.5f;
    float num2 = 0.15f;
    switch (biome)
    {
      case TextureData.TerrainType.Sand:
        gameObject = this.sand;
        if ((double) Random.Range(0.0f, 1f) < (double) num2)
        {
          inventoryItem = ItemManager.Instance.GetItemByName("Flint");
          break;
        }
        break;
      case TextureData.TerrainType.Grass:
        gameObject = this.dirt;
        if ((double) Random.Range(0.0f, 1f) < (double) num1)
        {
          inventoryItem = ItemManager.Instance.GetItemByName("Rock");
          break;
        }
        break;
    }
    Object.Instantiate<GameObject>((M0) gameObject, pos, Quaternion.LookRotation(Vector3.get_up()));
    if (!Object.op_Inequality((Object) inventoryItem, (Object) null))
      return;
    ClientSend.DropItemAtPosition(inventoryItem.id, 1, pos);
  }

  private void OnDrawGizmos()
  {
    using (List<Vector3>.Enumerator enumerator = this.hitPoints.GetEnumerator())
    {
      while (enumerator.MoveNext())
        Gizmos.DrawWireSphere(enumerator.Current, 1.5f);
    }
  }

  public HitBox() => base.\u002Ector();
}
