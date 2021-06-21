// Decompiled with JetBrains decompiler
// Type: ScrollSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
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
    ((TMP_Text) this.settingText).set_text(this.settingNames[this.currentSetting]);
    if (this.currentSetting == 0)
      ((Behaviour) this.scrollLeft).set_enabled(false);
    else if (this.currentSetting > 0)
      ((Behaviour) this.scrollLeft).set_enabled(true);
    if (this.currentSetting == this.settingNames.Length - 1)
      ((Behaviour) this.scrollRight).set_enabled(false);
    else if (this.currentSetting < this.settingNames.Length - 1)
      ((Behaviour) this.scrollRight).set_enabled(true);
    this.m_OnClick.Invoke();
  }
}
