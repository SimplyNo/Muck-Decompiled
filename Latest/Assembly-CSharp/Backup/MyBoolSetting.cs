// Decompiled with JetBrains decompiler
// Type: MyBoolSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
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
