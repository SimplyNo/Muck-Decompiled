// Decompiled with JetBrains decompiler
// Type: HitEffectExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal static class HitEffectExtension
{
  public static Color GetColor(HitEffect effect)
  {
    switch (effect)
    {
      case HitEffect.Normal:
        return Color.white;
      case HitEffect.Crit:
        return Color.yellow;
      case HitEffect.Big:
        return Color.red;
      case HitEffect.Electro:
        return Color.yellow;
      case HitEffect.Falling:
        return Color.cyan;
      default:
        return Color.white;
    }
  }

  public static string GetColorName(HitEffect effect)
  {
    switch (effect)
    {
      case HitEffect.Normal:
        return "white";
      case HitEffect.Crit:
        return "yellow";
      case HitEffect.Big:
        return "red";
      case HitEffect.Electro:
        return "#" + ColorUtility.ToHtmlStringRGB(Color.yellow);
      case HitEffect.Falling:
        return "#" + ColorUtility.ToHtmlStringRGB(Color.cyan);
      default:
        return "white";
    }
  }
}
