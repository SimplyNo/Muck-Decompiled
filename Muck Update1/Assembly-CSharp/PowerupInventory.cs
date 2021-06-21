// Decompiled with JetBrains decompiler
// Type: PowerupInventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using JetBrains.Annotations;
using UnityEngine;

public class PowerupInventory : MonoBehaviour
{
  private int[] powerups;
  public GameObject powerupFx;
  public AudioClip goodPowerupSfx;
  private float juiceSpeed = 1f;
  public static PowerupInventory Instance;

  private void Awake() => PowerupInventory.Instance = this;

  private void Start() => this.powerups = new int[ItemManager.Instance.allPowerups.Count];

  public void AddPowerup(string name, int powerupId, int objectId)
  {
    ++this.powerups[powerupId];
    UiEvents.Instance.AddPowerup(ItemManager.Instance.allPowerups[powerupId]);
    PlayerStatus.Instance.UpdateStats();
    PowerupUI.Instance.AddPowerup(powerupId);
    string message = "Picked up <color=" + ItemManager.Instance.allPowerups[powerupId].GetColorName() + ">(" + name + ")<color=white>";
    ChatBox.Instance.SendMessage(message);
    ParticleSystem component = Object.Instantiate<GameObject>(this.powerupFx, ItemManager.Instance.list[objectId].transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
    component.main.startColor = (ParticleSystem.MinMaxGradient) ItemManager.Instance.allPowerups[powerupId].GetOutlineColor();
    if (ItemManager.Instance.allPowerups[powerupId].tier != Powerup.PowerTier.Orange)
      return;
    component.gameObject.GetComponent<RandomSfx>().sounds = new AudioClip[1]
    {
      this.goodPowerupSfx
    };
    component.GetComponent<RandomSfx>().Randomize(0.0f);
  }

  public float GetDefenseMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Danis Milk"]], 0.1f, 40f);
  }

  public static float CumulativeDistribution(int amount, float scaleSpeed, float maxValue) => (1f - Mathf.Pow(2.71828f, (float) -amount * scaleSpeed)) * maxValue;

  public float GetStrengthMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    int playerPowerup1 = playerPowerups[ItemManager.Instance.stringToPowerupId["Dumbbell"]];
    float num1 = 0.1f;
    int playerPowerup2 = playerPowerups[ItemManager.Instance.stringToPowerupId["Berserk"]];
    float num2 = 0.0f;
    if (playerPowerup2 > 0)
      num2 = ((float) PlayerStatus.Instance.maxHp - PlayerStatus.Instance.hp) / (float) PlayerStatus.Instance.maxHp;
    MonoBehaviour.print((object) ("berserk multiplier: " + (object) num2));
    return (float) (1.0 + (double) playerPowerup1 * (double) num1 + (double) playerPowerup2 * (double) num2);
  }

  public int GetExtraDamage([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return 0;
  }

  public float GetAttackSpeedMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    float num1 = PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Orange Juice"]], 0.12f, 1f);
    float num2 = 1f;
    if (PlayerStatus.Instance.adrenalineBoost)
      num2 = this.GetAdrenalineBoost((int[]) null);
    return (1f + num1) * num2 * this.juiceSpeed;
  }

  public float GetStaminaMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    int playerPowerup = playerPowerups[ItemManager.Instance.stringToPowerupId["Peanut Butter"]];
    float num1 = 0.15f;
    float num2 = 1f;
    if (PlayerStatus.Instance.adrenalineBoost)
      num2 = this.GetAdrenalineBoost((int[]) null);
    return (float) (1.0 + (double) playerPowerup * (double) num1) * num2;
  }

  public float GetHealingMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return (float) playerPowerups[ItemManager.Instance.stringToPowerupId["Broccoli"]] * 0.1f;
  }

  public float GetResourceMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    float num = 0.0f;
    if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
      num = 1.75f;
    return 1f + PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Checkered Shirt"]], 0.3f, 4f) + num;
  }

  public float GetLootMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return 1f + PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Piggybank"]], 0.16f, 2f);
  }

  public float GetSniperScopeMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    int playerPowerup = playerPowerups[ItemManager.Instance.stringToPowerupId["Sniper Scope"]];
    float num1 = PowerupInventory.CumulativeDistribution(playerPowerup, 0.13f, 0.3f);
    float num2 = PowerupInventory.CumulativeDistribution(playerPowerup, 0.3f, 70f);
    return (double) num1 > (double) Random.Range(0.0f, 1f) ? num2 : 1f;
  }

  public float GetSniperScopeDamageMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return 1f + PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Sniper Scope"]], 0.3f, 70f);
  }

  public float GetLightningMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    int playerPowerup = playerPowerups[ItemManager.Instance.stringToPowerupId["Knuts Hammer"]];
    if (playerPowerup <= 0)
      return -1f;
    float num1 = PowerupInventory.CumulativeDistribution(playerPowerup, 0.12f, 0.4f);
    float num2 = PowerupInventory.CumulativeDistribution(playerPowerup, 0.12f, 1f);
    return (double) num1 > (double) Random.Range(0.0f, 1f) ? 2f + num2 : -1f;
  }

  public int GetHpMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return playerPowerups[ItemManager.Instance.stringToPowerupId["Red Pill"]] * 10;
  }

  public int GetHpIncreasePerKill([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return playerPowerups[ItemManager.Instance.stringToPowerupId["Dracula"]];
  }

  public int GetShield([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return playerPowerups[ItemManager.Instance.stringToPowerupId["Blue Pill"]] * 10;
  }

  public float GetHungerMultiplier([CanBeNull] int[] playerPowerups) => 1f;

  public float GetJuiceMultiplier([CanBeNull] int[] playerPowerups) => 1f;

  public float GetRobinMultiplier([CanBeNull] int[] playerPowerups) => 1f;

  public float GetEnforcerMultiplier([CanBeNull] int[] playerPowerups, float speed = -1f) => 1f;

  public float GetSpeedMultiplier([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    float num1 = PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Sneaker"]], 0.08f, 2f);
    float num2 = 1f;
    if (PlayerStatus.Instance.adrenalineBoost)
      num2 = this.GetAdrenalineBoost((int[]) null);
    return (1f + num1) * num2;
  }

  public float GetAdrenalineBoost([CanBeNull] int[] playerPowerups)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return 1f + PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Adrenaline"]], 1f, 2f);
  }

  public int GetMaxHpAndShield(int[] playerPowerups = null)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return 100 + this.GetHpMultiplier(playerPowerups) + this.GetShield(playerPowerups);
  }

  public float GetCritChance(int[] playerPowerups = null)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return 0.1f + PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Horseshoe"]], 0.08f, 0.9f);
  }

  public float GetJumpMultiplier(int[] playerPowerups = null)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return 1f + PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Jetpack"]], 0.075f, 2.5f);
  }

  public int GetExtraJumps(int[] playerPowerups = null)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return playerPowerups[ItemManager.Instance.stringToPowerupId["Janniks Frog"]];
  }

  public float GetFallWingsMultiplier(int[] playerPowerups = null)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    int playerPowerup = playerPowerups[ItemManager.Instance.stringToPowerupId["Wings of Glory"]];
    return playerPowerup == 0 ? 1f : 1f + PowerupInventory.CumulativeDistribution(playerPowerup, 0.45f, 2.5f);
  }

  public float GetKnockbackMultiplier(int[] playerPowerups = null)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return (double) PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Bulldozer"]], 0.15f, 1f) > (double) Random.Range(0.0f, 1f) ? 1f : 0.0f;
  }

  public float GetLifestealMultiplier(int[] playerPowerups = null)
  {
    if (playerPowerups == null)
      playerPowerups = this.powerups;
    return PowerupInventory.CumulativeDistribution(playerPowerups[ItemManager.Instance.stringToPowerupId["Crimson Dagger"]], 0.1f, 0.5f);
  }

  public float GetDamageMultiplier() => 1f;

  public void StartJuice()
  {
  }

  private void StopJuice() => this.juiceSpeed = 1f;
}
