// Decompiled with JetBrains decompiler
// Type: ScrollSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine.UI;

public class ScrollSettings : Setting
{
  public TextMeshProUGUI settingText;
  private string[] settingNames;
  public RawImage scrollLeft;
  public RawImage scrollRight;

  public void SetSettings(string[] settings, int startVal)
  {
    this.settingNames = settings;
    this.currentSetting = startVal;
    this.UpdateSetting();
  }

  public void Scroll(int i)
  {
    this.currentSetting += i;
    this.UpdateSetting();
  }

  private void UpdateSetting()
  {
    this.settingText.text = this.settingNames[this.currentSetting];
    if (this.currentSetting == 0)
      this.scrollLeft.enabled = false;
    else if (this.currentSetting > 0)
      this.scrollLeft.enabled = true;
    if (this.currentSetting == this.settingNames.Length - 1)
      this.scrollRight.enabled = false;
    else if (this.currentSetting < this.settingNames.Length - 1)
      this.scrollRight.enabled = true;
    this.m_OnClick.Invoke();
  }
}
