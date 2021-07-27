// Decompiled with JetBrains decompiler
// Type: PlayerStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
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
  private float oneShotThreshold = 0.9f;
  private float oneShotProtectionCooldown = 20f;
  private bool protectionActive = true;
  private bool readyToAdrenalineBoost = true;
  public GameObject playerRagdoll;
  public GameObject drownParticles;
  public AudioSource underwaterAudio;
  public GameObject leafParticles;
  public GameObject windParticles;
  private bool underwater;
  public InventoryItem[] armor;
  private float armorTotal;
  public float currentSpeedArmorMultiplier = 1f;
  public float currentChunkArmorMultiplier = 1f;

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
    this.InvokeRepeating("SlowUpdate", 1f, 1f);
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

  public void Damage(int newHp, int damageType = 0, bool ignoreProtection = false)
  {
    if (this.invincible || (double) this.hp + (double) this.shield <= 0.0)
      return;
    this.HandleDamage((int) ((double) this.hp + (double) this.shield) - newHp, damageType, ignoreProtection);
  }

  public void DealDamage(int damage, int damageType = 0, bool ignoreProtection = false, int damageFromPlayer = -1)
  {
    if ((double) this.hp + (double) this.shield <= 0.0)
      return;
    this.HandleDamage(damage, damageType, ignoreProtection, damageFromPlayer);
  }

  private void HandleDamage(
    int damageTaken,
    int damageType = 0,
    bool ignoreProtection = false,
    int damageFromPlayer = -1)
  {
    if (!ignoreProtection)
      damageTaken = this.OneShotProtection(damageTaken);
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
      this.PlayerDied(damageType, damageFromPlayer);
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

  private int OneShotProtection(int damageDone)
  {
    if (GameManager.gameSettings.difficulty == GameSettings.Difficulty.Gamer || !this.protectionActive)
      return damageDone;
    if ((double) damageDone / (double) this.MaxHpAndShield() > 0.899999976158142)
      damageDone = (int) ((double) this.MaxHpAndShield() * (double) this.oneShotThreshold);
    this.protectionActive = false;
    this.Invoke("ActivateProtection", this.oneShotProtectionCooldown);
    return damageDone;
  }

  private void ActivateProtection() => this.protectionActive = true;

  private void StopAdrenaline()
  {
    this.adrenalineBoost = false;
    this.Invoke("ReadyAdrenaline", 10f);
  }

  private void ReadyAdrenaline() => this.readyToAdrenalineBoost = true;

  public bool adrenalineBoost { get; private set; }

  private void PlayerDied(int damageType, int damageFromPlayer = -1)
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
    ClientSend.PlayerDied(damageFromPlayer);
    PlayerRagdoll component = Object.Instantiate<GameObject>(this.playerRagdoll, PlayerMovement.Instance.transform.position, PlayerMovement.Instance.orientation.rotation).GetComponent<PlayerRagdoll>();
    MoveCamera.Instance.PlayerDied(component.transform.GetChild(0).GetChild(0).GetChild(0));
    component.SetRagdoll(LocalClient.instance.myId, -component.transform.forward);
    GameManager.players[LocalClient.instance.myId].dead = true;
    if (InventoryUI.Instance.gameObject.activeInHierarchy)
      OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
    for (int armorSlot = 0; armorSlot < this.armor.Length; ++armorSlot)
      this.UpdateArmor(armorSlot, -1);
    AchievementManager.Instance.AddDeath((PlayerStatus.DamageType) damageType);
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

  public void EnterOcean()
  {
    this.windParticles.SetActive(true);
    this.leafParticles.GetComponent<ParticleSystem>().emission.enabled = false;
  }

  private void SlowUpdate()
  {
    if ((double) this.player.playerCam.position.y < (double) World.Instance.water.position.y)
    {
      if (!this.underwaterAudio.enabled)
      {
        this.underwaterAudio.enabled = true;
        this.underwaterAudio.Play();
      }
    }
    else if (this.underwaterAudio.enabled)
      this.underwaterAudio.enabled = false;
    if ((double) this.stamina > 0.0 || !this.underwater || (double) this.hp <= 0.0)
      return;
    this.DealDamage(5, 2);
    Object.Instantiate<GameObject>(this.drownParticles, this.transform.position, Quaternion.LookRotation(this.player.playerCam.transform.forward));
  }

  private void OutOfMap()
  {
    if (this.dead || !(bool) (Object) PlayerMovement.Instance || (double) PlayerMovement.Instance.transform.position.y >= -200.0)
      return;
    this.Damage(1);
    RaycastHit hitInfo;
    if (!Physics.Raycast(Vector3.up * 500f, Vector3.down, out hitInfo, 1000f, (int) GameManager.instance.whatIsGround))
      return;
    PlayerMovement.Instance.transform.position = hitInfo.point + Vector3.up * 2f;
    PlayerMovement.Instance.GetRb().velocity = Vector3.zero;
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
    this.underwater = this.player.IsUnderWater();
    if (this.running || this.underwater)
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
    if ((double) this.hunger > (double) this.maxHunger)
      this.hunger = this.maxHunger;
    AchievementManager.Instance.EatFood(item);
  }

  private void RegenShield() => this.readyToRegenShield = true;

  public float GetHpRatio() => this.maxHp == 0 ? 0.0f : this.hp / (float) this.maxHp * ((float) this.maxHp / (float) (this.maxShield + this.maxHp));

  public float GetShieldRatio() => this.maxShield == 0 ? 0.0f : this.shield / (float) this.maxShield * ((float) this.maxShield / (float) (this.maxShield + this.maxHp));

  public float GetStaminaRatio() => this.stamina / this.maxStamina;

  public float GetHungerRatio() => this.hunger / this.maxHunger;

  public void Jump() => this.stamina -= this.jumpDrain / PowerupInventory.Instance.GetStaminaMultiplier((int[]) null);

  public void AddKill(int killType, Mob mob)
  {
    this.Dracula();
    AchievementManager.Instance.AddKill((PlayerStatus.WeaponHitType) killType, mob);
  }

  public void Dracula()
  {
    int hpIncreasePerKill = PowerupInventory.Instance.GetHpIncreasePerKill((int[]) null);
    this.draculaStacks += hpIncreasePerKill;
    int maxDraculaStacks = PowerupInventory.Instance.GetMaxDraculaStacks();
    if (this.draculaStacks >= maxDraculaStacks)
      this.draculaStacks = maxDraculaStacks;
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
    ClientSend.SendArmor(armorSlot, itemId);
    this.CheckArmorSetBonus();
    if (!(bool) (Object) PreviewPlayer.Instance)
      return;
    PreviewPlayer.Instance.SetArmor(armorSlot, itemId);
  }

  private void CheckArmorSetBonus()
  {
    this.currentSpeedArmorMultiplier = 1f;
    this.currentChunkArmorMultiplier = 1f;
    if ((Object) this.armor[0] == (Object) null)
      return;
    int id = this.armor[0].requirements[0].item.id;
    foreach (InventoryItem inventoryItem in this.armor)
    {
      if ((Object) inventoryItem == (Object) null || inventoryItem.requirements[0].item.id != id)
        return;
    }
    string name = this.armor[0].requirements[0].item.name;
    if (!(name == "Wolfskin"))
    {
      if (!(name == "Chunkium bar"))
        return;
      this.currentChunkArmorMultiplier = 1.6f;
    }
    else
      this.currentSpeedArmorMultiplier = 1.5f;
  }

  public bool CanRun() => (double) this.stamina > 0.0;

  public bool CanJump() => (double) this.stamina >= (double) this.jumpDrain;

  public enum DamageType
  {
    Mob,
    Player,
    Drown,
  }

  public enum WeaponHitType
  {
    Undefined = -1, // 0xFFFFFFFF
    Melee = 0,
    Ranged = 1,
    Rock = 2,
  }
}
