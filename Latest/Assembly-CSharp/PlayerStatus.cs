// Decompiled with JetBrains decompiler
// Type: PlayerStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
  public float hp = 100f;
  public int maxHp;
  public float shield;
  public int maxShield;
  private bool dead;
  private float staminaRegenRate = 15f;
  private float staminaDrainRate = 12f;
  private float staminaBoost = 1f;
  private bool running;
  private float jumpDrain = 10f;
  private float hungerDrainRate = 0.15f;
  private float healingDrainMultiplier = 2f;
  private float staminaDrainMultiplier = 5f;
  private bool healing;
  private float healingRate = 5f;
  private bool readyToRegenShield = true;
  private float shieldRegenRate = 20f;
  private float regenShieldDelay = 5f;
  private PlayerMovement player;
  public static PlayerStatus Instance;
  private bool invincible;
  private bool readyToAdrenalineBoost = true;
  public GameObject playerRagdoll;
  public InventoryItem[] armor;
  private float armorTotal;

  public int draculaStacks { get; set; }

  public float stamina { get; set; }

  public float maxStamina { get; set; }

  public float hunger { get; set; }

  public float maxHunger { get; set; }

  public int strength { get; set; } = 1;

  public int speed { get; set; } = 1;

  private void Awake()
  {
    PlayerStatus.Instance = this;
    this.player = this.GetComponent<PlayerMovement>();
    this.maxShield = (int) this.shield;
    this.maxHp = (int) this.hp;
    this.stamina = 100f;
    this.hunger = 100f;
    this.maxStamina = this.stamina;
    this.maxHunger = this.hunger;
    this.strength = 1;
    this.speed = 1;
    this.armor = new InventoryItem[4];
  }

  public void Respawn()
  {
    this.hp = (float) this.maxHp;
    this.shield = (float) this.maxShield;
    this.stamina = this.maxStamina;
    this.hunger = this.maxHunger;
    this.dead = false;
    GameManager.players[LocalClient.instance.myId].dead = false;
    MoveCamera.Instance.PlayerRespawn(PlayerMovement.Instance.transform.position);
    this.invincible = true;
    this.CancelInvoke("StopInvincible");
    this.Invoke("StopInvincible", 3f);
  }

  private void StopInvincible() => this.invincible = false;

  public void UpdateStats()
  {
    this.maxHp = 100 + PowerupInventory.Instance.GetHpMultiplier((int[]) null) + this.draculaStacks;
    this.maxShield = PowerupInventory.Instance.GetShield((int[]) null);
  }

  public void Damage(int newHp)
  {
    if (this.invincible || (double) this.hp + (double) this.shield <= 0.0)
      return;
    this.HandleDamage((int) ((double) this.hp + (double) this.shield) - newHp);
  }

  public void DealDamage(int damage)
  {
    if ((double) this.hp + (double) this.shield <= 0.0)
      return;
    this.HandleDamage(damage);
  }

  private void HandleDamage(int damageTaken)
  {
    if ((double) this.shield >= (double) damageTaken)
    {
      this.shield -= (float) damageTaken;
    }
    else
    {
      damageTaken -= (int) this.shield;
      this.shield = 0.0f;
      this.hp -= (float) damageTaken;
    }
    if ((double) this.hp <= 0.0)
    {
      this.hp = 0.0f;
      this.PlayerDied();
    }
    if ((double) this.hp / (double) this.maxHp < 0.300000011920929 && !this.adrenalineBoost && this.readyToAdrenalineBoost)
    {
      this.adrenalineBoost = true;
      this.readyToAdrenalineBoost = false;
      this.Invoke("StopAdrenaline", 5f);
    }
    this.readyToRegenShield = false;
    this.CancelInvoke("RegenShield");
    if (!this.dead)
      this.Invoke("RegenShield", this.regenShieldDelay);
    float shakeRatio = (float) damageTaken / (float) this.MaxHpAndShield();
    CameraShaker.Instance.DamageShake(shakeRatio);
    DamageVignette.Instance.VignetteHit();
  }

  private void StopAdrenaline()
  {
    this.adrenalineBoost = false;
    this.Invoke("ReadyAdrenaline", 10f);
  }

  private void ReadyAdrenaline() => this.readyToAdrenalineBoost = true;

  public bool adrenalineBoost { get; private set; }

  private void PlayerDied()
  {
    this.hp = 0.0f;
    this.shield = 0.0f;
    PlayerMovement.Instance.gameObject.SetActive(false);
    this.dead = true;
    GameManager.players[LocalClient.instance.myId].dead = true;
    foreach (InventoryCell allCell in InventoryUI.Instance.allCells)
    {
      if (!((Object) allCell.currentItem == (Object) null))
      {
        InventoryUI.Instance.DropItemIntoWorld(allCell.currentItem);
        allCell.currentItem = (InventoryItem) null;
        allCell.UpdateCell();
      }
    }
    Hotbar.Instance.UpdateHotbar();
    ClientSend.PlayerDied();
    PlayerRagdoll component = Object.Instantiate<GameObject>(this.playerRagdoll, PlayerMovement.Instance.transform.position, PlayerMovement.Instance.orientation.rotation).GetComponent<PlayerRagdoll>();
    MoveCamera.Instance.PlayerDied(component.transform.GetChild(0).GetChild(0).GetChild(0));
    component.SetRagdoll(LocalClient.instance.myId, -component.transform.forward);
    GameManager.players[LocalClient.instance.myId].dead = true;
    if (!InventoryUI.Instance.gameObject.activeInHierarchy)
      return;
    OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
  }

  public bool IsPlayerDead() => this.dead;

  public void DropAllItems(List<InventoryCell> cells)
  {
  }

  public bool IsFullyHealed() => (double) this.hp >= (double) this.maxHp && (double) this.shield >= (double) this.maxShield;

  public int HpAndShield() => (int) ((double) this.hp + (double) this.shield);

  public int MaxHpAndShield() => this.maxHp + this.maxShield;

  public float GetArmorRatio() => this.armorTotal / 100f;

  private void Update()
  {
    this.Stamina();
    this.Shield();
    this.Healing();
    this.Hunger();
    this.OutOfMap();
  }

  private void OutOfMap()
  {
    if (this.dead || !(bool) (Object) PlayerMovement.Instance || (double) PlayerMovement.Instance.transform.position.y >= -200.0)
      return;
    this.Damage(0);
    this.PlayerDied();
  }

  private void Shield()
  {
    if (!this.readyToRegenShield || (double) this.shield >= (double) this.maxShield || (double) this.hp + (double) this.shield <= 0.0)
      return;
    this.shield += this.shieldRegenRate * Time.deltaTime;
    if ((double) this.shield <= (double) this.maxShield)
      return;
    this.shield = (float) this.maxShield;
  }

  private void Hunger()
  {
    if ((double) this.hunger <= 0.0 || (double) this.hp <= 0.0)
      return;
    float num = 1f * PowerupInventory.Instance.GetHungerMultiplier((int[]) null);
    if (this.healing)
      num *= this.healingDrainMultiplier;
    if (this.running)
      num *= this.staminaDrainMultiplier;
    this.hunger -= this.hungerDrainRate * Time.deltaTime * num;
    if ((double) this.hunger >= 0.0)
      return;
    this.hunger = 0.0f;
  }

  private void Healing()
  {
    if ((double) this.hp <= 0.0 || (double) this.hp >= (double) this.maxHp || (double) this.hunger <= 0.0)
      return;
    this.hp += this.healingRate * Time.deltaTime * PowerupInventory.Instance.GetHealingMultiplier((int[]) null);
  }

  private void Stamina()
  {
    this.running = (double) this.player.GetVelocity().magnitude > 5.0 && this.player.sprinting;
    if (this.running)
    {
      if ((double) this.stamina <= 0.0)
        return;
      this.stamina -= this.staminaDrainRate * Time.deltaTime / PowerupInventory.Instance.GetStaminaMultiplier((int[]) null);
    }
    else
    {
      if ((double) this.stamina >= 100.0 || !this.player.grounded || (double) this.hunger <= 0.0)
        return;
      float num = 1f;
      if ((double) this.hunger <= 0.0)
        num *= 0.3f;
      this.stamina += this.staminaRegenRate * Time.deltaTime * num;
    }
  }

  public void Heal(int healAmount)
  {
    this.hp += (float) healAmount;
    if ((double) this.hp <= (double) this.maxHp)
      return;
    this.hp = (float) this.maxHp;
  }

  public void Eat(InventoryItem item)
  {
    this.hp += item.heal;
    if ((double) this.hp > (double) this.maxHp)
      this.hp = (float) this.maxHp;
    this.stamina += item.stamina;
    if ((double) this.stamina > (double) this.maxStamina)
      this.stamina = this.maxStamina;
    this.hunger += item.hunger;
    if ((double) this.hunger <= (double) this.maxHunger)
      return;
    this.hunger = this.maxHunger;
  }

  private void RegenShield() => this.readyToRegenShield = true;

  public float GetHpRatio() => this.maxHp == 0 ? 0.0f : this.hp / (float) this.maxHp * ((float) this.maxHp / (float) (this.maxShield + this.maxHp));

  public float GetShieldRatio() => this.maxShield == 0 ? 0.0f : this.shield / (float) this.maxShield * ((float) this.maxShield / (float) (this.maxShield + this.maxHp));

  public float GetStaminaRatio() => this.stamina / this.maxStamina;

  public float GetHungerRatio() => this.hunger / this.maxHunger;

  public void Jump() => this.stamina -= this.jumpDrain / PowerupInventory.Instance.GetStaminaMultiplier((int[]) null);

  public void Dracula()
  {
    int hpIncreasePerKill = PowerupInventory.Instance.GetHpIncreasePerKill((int[]) null);
    this.draculaStacks += hpIncreasePerKill;
    this.UpdateStats();
    this.hp += (float) hpIncreasePerKill;
  }

  public void UpdateArmor(int armorSlot, int itemId)
  {
    InventoryItem inventoryItem1 = (InventoryItem) null;
    if (itemId >= 0)
      inventoryItem1 = ItemManager.Instance.allItems[itemId];
    this.armor[armorSlot] = inventoryItem1;
    this.armorTotal = 0.0f;
    foreach (InventoryItem inventoryItem2 in this.armor)
    {
      if (!((Object) inventoryItem2 == (Object) null))
        this.armorTotal += (float) inventoryItem2.armor;
    }
    PreviewPlayer.Instance.SetArmor(armorSlot, itemId);
    ClientSend.SendArmor(armorSlot, itemId);
  }

  public bool CanRun() => (double) this.stamina > 0.0;

  public bool CanJump() => (double) this.stamina >= (double) this.jumpDrain;
}
