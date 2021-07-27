// Decompiled with JetBrains decompiler
// Type: AchievementManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Data;
using System;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
  public static AchievementManager Instance;
  public InventoryItem[] gems;

  private void Awake()
  {
    AchievementManager.Instance = this;
    SteamUserStats.OnAchievementProgress += new Action<Achievement, int, int>(this.AchievementChanged);
  }

  private void Start() => this.GameStarted();

  private void AchievementChanged(Achievement ach, int currentProgress, int progress)
  {
    if (!ach.State)
      return;
    ach.Trigger();
    Debug.Log((object) (ach.Name + " WAS UNLOCKED!"));
  }

  public void CheckGameOverAchievements(int endState)
  {
    if (!this.CanUseAchievements())
      return;
    bool onlyRock = GameManager.instance.onlyRock;
    bool damageTaken = GameManager.instance.damageTaken;
    bool powerupsPickedup = GameManager.instance.powerupsPickedup;
    int currentDay = GameManager.instance.currentDay;
    int gameMode = (int) GameManager.gameSettings.gameMode;
    GameSettings.Difficulty difficulty = GameManager.gameSettings.difficulty;
    if (gameMode == 0 && endState == -3)
    {
      switch (difficulty)
      {
        case GameSettings.Difficulty.Easy:
          SteamUserStats.AddStat("WinsEasy", 1);
          Debug.Log((object) ("Game finished on Easy: " + (object) SteamUserStats.GetStatInt("WinsEasy")));
          break;
        case GameSettings.Difficulty.Normal:
          SteamUserStats.AddStat("WinsNormal", 1);
          Debug.Log((object) ("Game finished on normal: " + (object) SteamUserStats.GetStatInt("WinsNormal")));
          break;
        case GameSettings.Difficulty.Gamer:
          SteamUserStats.AddStat("WinsGamer", 1);
          Debug.Log((object) ("Game finished on Gamer: " + (object) SteamUserStats.GetStatInt("WinsGamer")));
          if (currentDay < 10)
          {
            SteamUserStats.AddStat("GamerMove", 1);
            Debug.Log((object) ("Game finished on Gamer in less than 10 days: " + (object) SteamUserStats.GetStatInt("GamerMove")));
            break;
          }
          break;
      }
      if (currentDay < 8)
      {
        SteamUserStats.AddStat("Speedrunner", 1);
        Debug.Log((object) ("Game finished in less than 8 days: " + (object) SteamUserStats.GetStatInt("Speedrunner")));
      }
      if (!powerupsPickedup)
      {
        SteamUserStats.AddStat("NoPowerups", 1);
        Debug.Log((object) ("Game finished without powerups: " + (object) SteamUserStats.GetStatInt("NoPowerups")));
      }
      if (!damageTaken && difficulty >= GameSettings.Difficulty.Normal)
      {
        switch (NetworkController.Instance.nPlayers)
        {
          case 1:
            SteamUserStats.AddStat("Untouchable", 1);
            Debug.Log((object) ("game finished without taking damage 1: " + (object) SteamUserStats.GetStatInt("Untouchable")));
            break;
          case 2:
            SteamUserStats.AddStat("Dream Team", 1);
            Debug.Log((object) ("game finished without taking  2: " + (object) SteamUserStats.GetStatInt("Dream Team")));
            break;
          case 4:
            SteamUserStats.AddStat("The bois", 1);
            Debug.Log((object) ("game finished without taking damage 4: " + (object) SteamUserStats.GetStatInt("The bois")));
            break;
          case 8:
            SteamUserStats.AddStat("Sweat and tears", 1);
            Debug.Log((object) ("game finished without taking damage 8: " + (object) SteamUserStats.GetStatInt("Sweat and tears")));
            break;
        }
      }
      if (onlyRock)
      {
        SteamUserStats.AddStat("Caveman", 1);
        Debug.Log((object) ("game finished using only a rock: " + (object) SteamUserStats.GetStatInt("Caveman")));
      }
      if (difficulty >= GameSettings.Difficulty.Normal && onlyRock && (!damageTaken && !powerupsPickedup))
      {
        SteamUserStats.AddStat("Muck", 1);
        Debug.Log((object) ("Literally did the impossible: " + (object) SteamUserStats.GetStatInt("Muck")));
      }
      SteamUserStats.AddStat("GamesWon", 1);
      Debug.Log((object) ("GamesWon: " + (object) SteamUserStats.GetStatInt("GamesWon")));
    }
    SteamUserStats.StoreStats();
  }

  public void LeaveMuck()
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Set sail", 1);
    Debug.Log((object) ("Leaving muck. Left: " + (object) SteamUserStats.GetStatInt("Set sail")));
    SteamUserStats.StoreStats();
  }

  public void AddKill(PlayerStatus.WeaponHitType type, Mob mob)
  {
    if (!this.CanUseAchievements() || mob.countedKill)
      return;
    mob.countedKill = true;
    bool flag = mob.IsBuff();
    string name = mob.mobType.name;
    Debug.Log((object) ("Is buff: " + flag.ToString()));
    SteamUserStats.AddStat("Kills", 1);
    SteamUserStats.AddStat("TotalKills", 1);
    if (type == PlayerStatus.WeaponHitType.Ranged)
      SteamUserStats.AddStat("BowKills", 1);
    if (flag)
    {
      Debug.Log((object) "Is buff so adding kills");
      SteamUserStats.AddStat("BuffKills", 1);
    }
    if (name == "Cow")
      SteamUserStats.AddStat("Cow Kills", 1);
    if (name == "Big Chunk")
    {
      SteamUserStats.AddStat("BigChunkKills", 1);
      this.CheckAllBossesKilled();
    }
    if (name == "Gronk")
    {
      SteamUserStats.AddStat("GronkKills", 1);
      this.CheckAllBossesKilled();
    }
    if (name == "Guardian")
    {
      SteamUserStats.AddStat("GuardianKills", 1);
      this.CheckAllBossesKilled();
    }
    if (name == "Chief")
    {
      SteamUserStats.AddStat("ChiefKills", 1);
      this.CheckAllBossesKilled();
    }
    if (name == "Goblin")
      SteamUserStats.AddStat("GoblinKills", 1);
    if (name == "Woodman")
      SteamUserStats.AddStat("WoodmanKills", 1);
    Debug.Log((object) ("Killcount: " + (object) SteamUserStats.GetStatInt("Kills") + ", allkills: " + (object) SteamUserStats.GetStatInt("TotalKills") + ", bowkills: " + (object) SteamUserStats.GetStatInt("BowKills") + ", buffkills: " + (object) SteamUserStats.GetStatInt("BuffKills") + ", Cow Kills: " + (object) SteamUserStats.GetStatInt("Cow Kills") + ", chunks: " + (object) SteamUserStats.GetStatInt("BigChunkKills") + ", gronks: " + (object) SteamUserStats.GetStatInt("GronkKills") + ", guardians: " + (object) SteamUserStats.GetStatInt("GuardianKills") + ", goblins: " + (object) SteamUserStats.GetStatInt("GoblinKills") + "Woodman kills: " + (object) SteamUserStats.GetStatInt("WoodmanKills")));
    SteamUserStats.StoreStats();
  }

  private void CheckAllBossesKilled()
  {
    int statInt1 = SteamUserStats.GetStatInt("BigChunkKills");
    int statInt2 = SteamUserStats.GetStatInt("GronkKills");
    int statInt3 = SteamUserStats.GetStatInt("GuardianKills");
    int statInt4 = SteamUserStats.GetStatInt("ChiefKills");
    if (statInt3 <= 0 || statInt2 <= 0 || (statInt1 <= 0 || statInt4 <= 0))
      return;
    SteamUserStats.AddStat("Fearless", 1);
    Debug.Log((object) ("All bosses killed: " + (object) SteamUserStats.GetStatInt("Fearless")));
  }

  public void StartBattleTotem()
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Battle totems started", 1);
    Debug.Log((object) ("battle totems started: " + (object) SteamUserStats.GetStatInt("Battle totems started")));
    SteamUserStats.StoreStats();
  }

  public void ReviveTeammate()
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Revives", 1);
    Debug.Log((object) ("Revives: " + (object) SteamUserStats.GetStatInt("Revives")));
    SteamUserStats.StoreStats();
  }

  public void AddDeath(PlayerStatus.DamageType deathCause)
  {
    if (!this.CanUseAchievements())
      return;
    Debug.Log((object) ("Cause of death: " + (object) deathCause));
    if (deathCause == PlayerStatus.DamageType.Drown)
    {
      SteamUserStats.AddStat("Drown", 1);
      Debug.Log((object) ("Drowned: " + (object) SteamUserStats.GetStatInt("Drown")));
    }
    SteamUserStats.AddStat("Deaths", 1);
    SteamUserStats.AddStat("TotalDeaths", 1);
    Debug.Log((object) ("Deaths: " + (object) SteamUserStats.GetStatInt("Deaths")));
    Debug.Log((object) ("TotalDeaths: " + (object) SteamUserStats.GetStatInt("TotalDeaths")));
    SteamUserStats.StoreStats();
  }

  public bool CanUseAchievements() => (bool) (UnityEngine.Object) SteamManager.Instance && SteamClient.IsValid;

  public void OpenChest()
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Chests opened", 1);
    SteamUserStats.AddStat("TotalChestsOpened", 1);
    Debug.Log((object) ("Chests opened: " + (object) SteamUserStats.GetStatInt("Chests opened")));
    SteamUserStats.AddStat("Chests opened", 1);
    SteamUserStats.StoreStats();
  }

  public void ItemCrafted(InventoryItem item, int craftAmount)
  {
    if (!this.CanUseAchievements())
      return;
    if (item.name == "Coin")
    {
      SteamUserStats.AddStat("CoinsCrafted", craftAmount);
      Debug.Log((object) ("Coins crafted: " + (object) SteamUserStats.GetStatInt("CoinsCrafted")));
    }
    SteamUserStats.StoreStats();
  }

  public void BuildItem(int buildId)
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Builds", 1);
    Debug.Log((object) ("Builds: " + (object) SteamUserStats.GetStatInt("Builds")));
    SteamUserStats.StoreStats();
  }

  public void NewDay(int currentDay)
  {
    if (!this.CanUseAchievements())
      return;
    int statInt = SteamUserStats.GetStatInt("Longest survived");
    if (currentDay > statInt)
      SteamUserStats.SetStat("Longest survived", currentDay);
    Debug.Log((object) ("Max sruvived days: " + (object) SteamUserStats.GetStatInt("Longest survived")));
    SteamUserStats.StoreStats();
  }

  public void WieldedWeapon(InventoryItem item)
  {
    if (!this.CanUseAchievements())
      return;
    if (item.name == "Night Blade")
    {
      SteamUserStats.AddStat("The Black Swordsman", 1);
      Debug.Log((object) ("The Black Swordsman: " + (object) SteamUserStats.GetStatInt("The Black Swordsman")));
    }
    SteamUserStats.StoreStats();
  }

  public void PickupPowerup(string powerupName)
  {
    if (!this.CanUseAchievements())
      return;
    if (powerupName == "Danis Milk" && PowerupInventory.Instance.GetAmount("Danis Milk") >= 10)
    {
      SteamUserStats.AddStat("Milkman", 1);
      Debug.Log((object) ("Milkman: " + (object) SteamUserStats.GetStatInt("Milkman")));
    }
    SteamUserStats.StoreStats();
  }

  public void MoveDistance(int groundDist, int waterDist)
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Move Distance", groundDist);
    if (waterDist > 0)
      SteamUserStats.AddStat("Swim distance", waterDist);
    Debug.Log((object) ("Move dist: " + (object) SteamUserStats.GetStatInt("Move Distance") + ", added this one: " + (object) groundDist));
    Debug.Log((object) ("swim dist: " + (object) SteamUserStats.GetStatInt("Swim distance") + ", added this one: " + (object) waterDist));
    SteamUserStats.StoreStats();
  }

  public void EatFood(InventoryItem item)
  {
    if (!this.CanUseAchievements())
      return;
    if (item.name == "Gulpon Shroom")
    {
      SteamUserStats.AddStat("Red shrooms eaten", 1);
      Debug.Log((object) ("Red shrooms eaten: " + (object) SteamUserStats.GetStatInt("Red shrooms eaten")));
    }
    SteamUserStats.StoreStats();
  }

  public void AddPlayerKill()
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Friendly Kills", 1);
    Debug.Log((object) ("Friendly Kills: " + (object) SteamUserStats.GetStatInt("Friendly Kills")));
    SteamUserStats.StoreStats();
  }

  public void Jump()
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Jumps", 1);
    Debug.Log((object) ("Jumps: " + (object) SteamUserStats.GetStatInt("Jumps")));
    SteamUserStats.StoreStats();
  }

  public void PickupItem(InventoryItem item)
  {
    if (!this.CanUseAchievements())
      return;
    foreach (InventoryItem gem1 in this.gems)
    {
      if ((UnityEngine.Object) item != (UnityEngine.Object) null && gem1.id == item.id)
      {
        Debug.Log((object) "Found gem, testing");
        bool flag = true;
        foreach (InventoryItem gem2 in this.gems)
        {
          if (gem2.id != item.id && !InventoryUI.Instance.HasItem(gem2))
          {
            Debug.Log((object) ("Couldnt find item: " + gem2.name));
            flag = false;
            break;
          }
        }
        if (flag)
        {
          SteamUserStats.AddStat("AllGems", 1);
          Debug.Log((object) ("AllGems: " + (object) SteamUserStats.GetStatInt("AllGems")));
          break;
        }
        break;
      }
    }
    SteamUserStats.StoreStats();
  }

  public void Karlson()
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("Karlson monitor", 1);
    Debug.Log((object) ("Karlson monitor: " + (object) SteamUserStats.GetStatInt("Karlson monitor")));
    SteamUserStats.StoreStats();
  }

  private void GameStarted()
  {
    SteamUserStats.AddStat("Muck started", 1);
    Debug.Log((object) ("Muck started: " + (object) SteamUserStats.GetStatInt("Muck started")));
    SteamUserStats.StoreStats();
  }

  public void StartGame(GameSettings.Difficulty d)
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("GamesStarted", 1);
    Debug.Log((object) ("GamesStarted: " + (object) SteamUserStats.GetStatInt("GamesStarted")));
    switch (d)
    {
      case GameSettings.Difficulty.Easy:
        SteamUserStats.AddStat("Easy", 1);
        Debug.Log((object) ("Easy: " + (object) SteamUserStats.GetStatInt("Easy")));
        break;
      case GameSettings.Difficulty.Normal:
        SteamUserStats.AddStat("Normal", 1);
        Debug.Log((object) ("Normal: " + (object) SteamUserStats.GetStatInt("Normal")));
        break;
      case GameSettings.Difficulty.Gamer:
        SteamUserStats.AddStat("Gamer", 1);
        Debug.Log((object) ("Gamer: " + (object) SteamUserStats.GetStatInt("Gamer")));
        break;
    }
    SteamUserStats.StoreStats();
  }

  public void OpenChiefChest()
  {
    if (!this.CanUseAchievements())
      return;
    SteamUserStats.AddStat("ChiefChests", 1);
    Debug.Log((object) ("ChiefChests: " + (object) SteamUserStats.GetStatInt("ChiefChests")));
    SteamUserStats.StoreStats();
  }
}
