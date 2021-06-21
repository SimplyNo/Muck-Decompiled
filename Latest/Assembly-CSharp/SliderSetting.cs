// Decompiled with JetBrains decompiler
// Type: SliderSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    this.slider.set_value((float) startVal);
    this.UpdateSettings();
  }

  public void UpdateSettings()
  {
    this.currentSetting = (int) this.slider.get_value();
    ((TMP_Text) this.value).set_text(string.Concat((object) this.currentSetting));
    this.m_OnClick.Invoke();
  }

  public static float Truncate(float value, int digits)
  {
    double num = Math.Pow(10.0, (double) digits);
    return (float) (Math.Truncate(num * (double) value) / num);
  }
}
