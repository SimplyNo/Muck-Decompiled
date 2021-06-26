// Decompiled with JetBrains decompiler
// Type: SliderSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine.UI;

public class SliderSetting : Setting
{
  public Slider slider;
  public TextMeshProUGUI value;

  public void SetSettings(int startVal)
  {
    this.currentSetting = startVal;
    this.slider.value = (float) startVal;
    this.UpdateSettings();
  }

  public void UpdateSettings()
  {
    this.currentSetting = (int) this.slider.value;
    this.value.text = string.Concat((object) this.currentSetting);
    this.m_OnClick.Invoke();
  }

  public static float Truncate(float value, int digits)
  {
    double num = Math.Pow(10.0, (double) digits);
    return (float) (Math.Truncate(num * (double) value) / num);
  }
}
