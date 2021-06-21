// Decompiled with JetBrains decompiler
// Type: PlayerStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
  public float hp;
  public int maxHp;
  public float shield;
  public int maxShield;
  private bool dead;
  private float staminaRegenRate;
  private float staminaDrainRate;
  private float staminaBoost;
  private bool running;
  private float jumpDrain;
  private float hungerDrainRate;
  private float healingDrainMultiplier;
  private float staminaDrainMultiplier;
  private bool healing;
  private float healingRate;
  private bool readyToRegenShield;
  private float shieldRegenRate;
  private float regenShieldDelay;
  private PlayerMovement player;
  public static PlayerStatus Instance;
  private bool invincible;
  private float oneShotThreshold;
  private float oneShotProtectionCooldown;
  private bool protectionActive;
  private bool readyToAdrenalineBoost;
  public GameObject playerRagdoll;
  public InventoryItem[] armor;
  private float armorTotal;
  public float currentSpeedArmorMultiplier;
  public float currentChunkArmorMultiplier;

  public int draculaStacks { get; set; }

  public float stamina { get; set; }

  public float maxStamina { get; set; }

  public float hunger { get; set; }

  public float maxHunger { get; set; }

  public int strength { get; set; }

  public int speed { get; set; }

  private void Awake()
  {
    PlayerStatus.Instance = this;
    this.player = (PlayerMovement) ((Component) this).GetComponent<PlayerMovement>();
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
    MoveCamera.Instance.PlayerRespawn(((Component) PlayerMovement.Instance).get_transform().get_position());
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

  public void Damage(int newHp, bool ignoreProtection = false)
  {
    if (this.invincible || (double) this.hp + (double) this.shield <= 0.0)
      return;
    this.HandleDamage((int) ((double) this.hp + (double) this.shield) - newHp, ignoreProtection);
  }

  public void DealDamage(int damage, bool ignoreProtection = false)
  {
    if ((double) this.hp + (double) this.shield <= 0.0)
      return;
    this.HandleDamage(damage, ignoreProtection);
  }

  private void HandleDamage(int damageTaken, bool ignoreProtection = false)
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

  private void PlayerDied()
  {
    this.hp = 0.0f;
    this.shield = 0.0f;
    ((Component) PlayerMovement.Instance).get_gameObject().SetActive(false);
    this.dead = true;
    GameManager.players[LocalClient.instance.myId].dead = true;
    foreach (InventoryCell allCell in InventoryUI.Instance.allCells)
    {
      if (!Object.op_Equality((Object) allCell.currentItem, (Object) null))
      {
        InventoryUI.Instance.DropItemIntoWorld(allCell.currentItem);
        allCell.currentItem = (InventoryItem) null;
        allCell.UpdateCell();
      }
    }
    Hotbar.Instance.UpdateHotbar();
    ClientSend.PlayerDied();
    PlayerRagdoll component = (PlayerRagdoll) ((GameObject) Object.Instantiate<GameObject>((M0) this.playerRagdoll, ((Component) PlayerMovement.Instance).get_transform().get_position(), PlayerMovement.Instance.orientation.get_rotation())).GetComponent<PlayerRagdoll>();
    MoveCamera.Instance.PlayerDied(((Component) component).get_transform().GetChild(0).GetChild(0).GetChild(0));
    component.SetRagdoll(LocalClient.instance.myId, Vector3.op_UnaryNegation(((Component) component).get_transform().get_forward()));
    GameManager.players[LocalClient.instance.myId].dead = true;
    if (((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy())
      OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
    for (int armorSlot = 0; armorSlot < this.armor.Length; ++armorSlot)
      this.UpdateArmor(armorSlot, -1);
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
    if (this.dead || !Object.op_Implicit((Object) PlayerMovement.Instance) || ((Component) PlayerMovement.Instance).get_transform().get_position().y >= -200.0)
      return;
    this.Damage(1);
    RaycastHit raycastHit;
    if (!Physics.Raycast(Vector3.op_Multiply(Vector3.get_up(), 500f), Vector3.get_down(), ref raycastHit, 1000f, LayerMask.op_Implicit(GameManager.instance.whatIsGround)))
      return;
    ((Component) PlayerMovement.Instance).get_transform().set_position(Vector3.op_Addition(((RaycastHit) ref raycastHit).get_point(), Vector3.op_Multiply(Vector3.get_up(), 2f)));
    PlayerMovement.Instance.GetRb().set_velocity(Vector3.get_zero());
  }

  private void Shield()
  {
    if (!this.readyToRegenShield || (double) this.shield >= (double) this.maxShield || (double) this.hp + (double) this.shield <= 0.0)
      return;
    this.shield += this.shieldRegenRate * Time.get_deltaTime();
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
    this.hunger -= this.hungerDrainRate * Time.get_deltaTime() * num;
    if ((double) this.hunger >= 0.0)
      return;
    this.hunger = 0.0f;
  }

  private void Healing()
  {
    if ((double) this.hp <= 0.0 || (double) this.hp >= (double) this.maxHp || (double) this.hunger <= 0.0)
      return;
    this.hp += this.healingRate * Time.get_deltaTime() * PowerupInventory.Instance.GetHealingMultiplier((int[]) null);
  }

  private void Stamina()
  {
    Vector3 velocity = this.player.GetVelocity();
    this.running = (double) ((Vector3) ref velocity).get_magnitude() > 5.0 && this.player.sprinting;
    if (this.running)
    {
      if ((double) this.stamina <= 0.0)
        return;
      this.stamina -= this.staminaDrainRate * Time.get_deltaTime() / PowerupInventory.Instance.GetStaminaMultiplier((int[]) null);
    }
    else
    {
      if ((double) this.stamina >= 100.0 || !this.player.grounded || (double) this.hunger <= 0.0)
        return;
      float num = 1f;
      if ((double) this.hunger <= 0.0)
        num *= 0.3f;
      this.stamina += this.staminaRegenRate * Time.get_deltaTime() * num;
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
    int num = PowerupInventory.Instance.GetAmount(nameof (Dracula)) * PowerupInventory.Instance.GetMaxDraculaStacks();
    if (this.draculaStacks >= num)
      this.draculaStacks = num;
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
      if (!Object.op_Equality((Object) inventoryItem2, (Object) null))
        this.armorTotal += (float) inventoryItem2.armor;
    }
    ClientSend.SendArmor(armorSlot, itemId);
    this.CheckArmorSetBonus();
    PreviewPlayer.Instance.SetArmor(armorSlot, itemId);
  }

  private void CheckArmorSetBonus()
  {
    this.currentSpeedArmorMultiplier = 1f;
    this.currentChunkArmorMultiplier = 1f;
    if (Object.op_Equality((Object) this.armor[0], (Object) null))
      return;
    int id = this.armor[0].requirements[0].item.id;
    foreach (InventoryItem inventoryItem in this.armor)
    {
      if (Object.op_Equality((Object) inventoryItem, (Object) null) || inventoryItem.requirements[0].item.id != id)
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

  public PlayerStatus() => base.\u002Ector();
}
