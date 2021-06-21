// Decompiled with JetBrains decompiler
// Type: GameSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

public class GameSettings
{
  public int Seed;

  public GameSettings.GameMode gameMode { get; set; }

  public GameSettings.FriendlyFire friendlyFire { get; set; }

  public GameSettings.Difficulty difficulty { get; set; }

  public GameSettings.Respawn respawn { get; set; }

  public GameSettings.GameLength gameLength { get; set; }

  public GameSettings(
    int seed,
    GameSettings.GameMode gameMode = GameSettings.GameMode.Survival,
    GameSettings.FriendlyFire friendlyFire = GameSettings.FriendlyFire.Off,
    GameSettings.Difficulty difficulty = GameSettings.Difficulty.Normal,
    GameSettings.GameLength gameLength = GameSettings.GameLength.Short)
  {
    this.Seed = seed;
    this.gameMode = gameMode;
    this.friendlyFire = friendlyFire;
    this.difficulty = difficulty;
    this.gameLength = gameLength;
  }

  public GameSettings(int seed, int gameMode, int friendlyFire, int difficulty, int gameLength)
  {
    this.Seed = seed;
    this.gameMode = (GameSettings.GameMode) gameMode;
    this.friendlyFire = (GameSettings.FriendlyFire) friendlyFire;
    this.difficulty = (GameSettings.Difficulty) difficulty;
    this.gameLength = (GameSettings.GameLength) gameLength;
  }

  public int BossDay()
  {
    switch (this.difficulty)
    {
      case GameSettings.Difficulty.Easy:
        return 6;
      case GameSettings.Difficulty.Normal:
        return 5;
      case GameSettings.Difficulty.Gamer:
        return 3;
      default:
        return 5;
    }
  }

  public float GetChestPriceMultiplier()
  {
    switch (this.difficulty)
    {
      case GameSettings.Difficulty.Easy:
        return 9f;
      case GameSettings.Difficulty.Normal:
        return 7f;
      case GameSettings.Difficulty.Gamer:
        return 6f;
      default:
        return 5f;
    }
  }

  public int DayLength()
  {
    switch (this.difficulty)
    {
      case GameSettings.Difficulty.Easy:
        return 56;
      case GameSettings.Difficulty.Normal:
        return 54;
      case GameSettings.Difficulty.Gamer:
        return 52;
      default:
        return 5;
    }
  }

  public enum GameMode
  {
    Survival,
    Versus,
    Creative,
  }

  public enum FriendlyFire
  {
    Off,
    On,
  }

  public enum Difficulty
  {
    Easy,
    Normal,
    Gamer,
  }

  public enum Respawn
  {
    OnNewDay,
    Never,
  }

  public enum GameLength
  {
    Short = 3,
    Medium = 8,
    Long = 14, // 0x0000000E
  }
}
