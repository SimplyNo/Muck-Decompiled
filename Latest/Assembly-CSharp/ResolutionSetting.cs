// Decompiled with JetBrains decompiler
// Type: ResolutionSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionSetting : Setting
{
  public RawImage scrollLeft;
  public RawImage scrollRight;
  public TextMeshProUGUI settingText;
  private Resolution[] resolutions;

  public void SetSettings(Resolution[] resolutions, Resolution current)
  {
    this.resolutions = resolutions;
    for (int index = 0; index < resolutions.Length; ++index)
    {
      if (((Resolution) ref current).get_width() == ((Resolution) ref resolutions[index]).get_width() && ((Resolution) ref current).get_height() == ((Resolution) ref resolutions[index]).get_height())
      {
        this.currentSetting = index;
        MonoBehaviour.print((object) "found current res");
      }
    }
    this.UpdateSetting();
  }

  public void Scroll(int i)
  {
    this.currentSetting += i;
    this.UpdateSetting();
  }

  private void UpdateSetting()
  {
    ((TMP_Text) this.settingText).set_text(this.ResolutionToText(this.resolutions[this.currentSetting]));
    if (this.currentSetting == 0)
      ((Behaviour) this.scrollLeft).set_enabled(false);
    else if (this.currentSetting > 0)
      ((Behaviour) this.scrollLeft).set_enabled(true);
    if (this.currentSetting == this.resolutions.Length - 1)
    {
      ((Behaviour) this.scrollRight).set_enabled(false);
    }
    else
    {
      if (this.currentSetting >= this.resolutions.Length - 1)
        return;
      ((Behaviour) this.scrollRight).set_enabled(true);
    }
  }

  private string ResolutionToText(Resolution r) => r.ToString();

  public void ApplySetting()
  {
    Resolution resolution = this.resolutions[this.currentSetting];
    CurrentSettings.Instance.UpdateResolution(((Resolution) ref resolution).get_width(), ((Resolution) ref resolution).get_height(), ((Resolution) ref resolution).get_refreshRate());
  }
}
