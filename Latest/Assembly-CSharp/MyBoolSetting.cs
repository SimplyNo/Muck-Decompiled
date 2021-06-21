// Decompiled with JetBrains decompiler
// Type: MyBoolSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MyBoolSetting : Setting
{
  public GameObject checkMark;

  public void SetSetting(int s)
  {
    this.currentSetting = s;
    this.UpdateSetting();
  }

  public void SetSetting(bool s)
  {
    if (s)
      this.currentSetting = 1;
    else
      this.currentSetting = 0;
    this.UpdateSetting();
  }

  public void ToggleSetting()
  {
    if (this.currentSetting == 1)
      this.currentSetting = 0;
    else
      this.currentSetting = 1;
    this.UpdateSetting();
  }

  private void UpdateSetting()
  {
    if (this.currentSetting == 1)
      this.checkMark.SetActive(true);
    else
      this.checkMark.SetActive(false);
    this.m_OnClick.Invoke();
  }
}
