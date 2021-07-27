// Decompiled with JetBrains decompiler
// Type: ControlSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class ControlSetting : Setting
{
  public TextMeshProUGUI keyText;
  public KeyCode currentKey;
  private string actionName;

  public void SetSetting(KeyCode k, string actionName)
  {
    this.currentKey = k;
    MonoBehaviour.print((object) ("key: " + (object) k));
    this.actionName = actionName;
    this.UpdateSetting();
  }

  private void UpdateSetting() => this.keyText.text = this.currentKey.ToString() ?? "";

  public void SetKey(KeyCode k)
  {
    this.currentKey = k;
    this.onClick.Invoke();
    this.UpdateSetting();
  }

  public void StartListening() => KeyListener.Instance.ListenForKey(this, this.actionName);
}
