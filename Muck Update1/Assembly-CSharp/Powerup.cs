// Decompiled with JetBrains decompiler
// Type: Powerup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[CreateAssetMenu]
public class Powerup : ScriptableObject
{
  public string name;
  public string description;
  public int id;
  public Powerup.PowerTier tier;
  public Mesh mesh;
  public Material material;
  public Sprite sprite;

  public Color GetOutlineColor()
  {
    switch (this.tier)
    {
      case Powerup.PowerTier.White:
        return Color.white;
      case Powerup.PowerTier.Blue:
        return Color.cyan;
      case Powerup.PowerTier.Orange:
        return Color.yellow;
      default:
        return Color.white;
    }
  }

  public string GetColorName()
  {
    switch (this.tier)
    {
      case Powerup.PowerTier.White:
        return "white";
      case Powerup.PowerTier.Blue:
        return "#00C0FF";
      case Powerup.PowerTier.Orange:
        return "orange";
      default:
        return "white";
    }
  }

  public enum PowerTier
  {
    White,
    Blue,
    Orange,
  }
}
