// Decompiled with JetBrains decompiler
// Type: Powerup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
        return Color.get_white();
      case Powerup.PowerTier.Blue:
        return Color.get_cyan();
      case Powerup.PowerTier.Orange:
        return Color.get_yellow();
      default:
        return Color.get_white();
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

  public Powerup() => base.\u002Ector();

  public enum PowerTier
  {
    White,
    Blue,
    Orange,
  }
}
