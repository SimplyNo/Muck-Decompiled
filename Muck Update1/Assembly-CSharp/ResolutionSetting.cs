// Decompiled with JetBrains decompiler
// Type: ResolutionSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
      if (current.width == resolutions[index].width && current.height == resolutions[index].height)
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
    this.settingText.text = this.ResolutionToText(this.resolutions[this.currentSetting]);
    if (this.currentSetting == 0)
      this.scrollLeft.enabled = false;
    else if (this.currentSetting > 0)
      this.scrollLeft.enabled = true;
    if (this.currentSetting == this.resolutions.Length - 1)
    {
      this.scrollRight.enabled = false;
    }
    else
    {
      if (this.currentSetting >= this.resolutions.Length - 1)
        return;
      this.scrollRight.enabled = true;
    }
  }

  private string ResolutionToText(Resolution r) => r.ToString();

  public void ApplySetting()
  {
    Resolution resolution = this.resolutions[this.currentSetting];
    CurrentSettings.Instance.UpdateResolution(resolution.width, resolution.height, resolution.refreshRate);
  }
}
