// Decompiled with JetBrains decompiler
// Type: HitEffectExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

internal static class HitEffectExtension
{
  public static Color GetColor(HitEffect effect)
  {
    switch (effect)
    {
      case HitEffect.Normal:
        return Color.get_white();
      case HitEffect.Crit:
        return Color.get_yellow();
      case HitEffect.Big:
        return Color.get_red();
      case HitEffect.Electro:
        return Color.get_yellow();
      case HitEffect.Falling:
        return Color.get_cyan();
      default:
        return Color.get_white();
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
        return "#" + ColorUtility.ToHtmlStringRGB(Color.get_yellow());
      case HitEffect.Falling:
        return "#" + ColorUtility.ToHtmlStringRGB(Color.get_cyan());
      default:
        return "white";
    }
  }
}
