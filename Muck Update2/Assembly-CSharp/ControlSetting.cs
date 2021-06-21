// Decompiled with JetBrains decompiler
// Type: ControlSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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

  private void UpdateSetting() => ((TMP_Text) this.keyText).set_text(this.currentKey.ToString() ?? "");

  public void SetKey(KeyCode k)
  {
    this.currentKey = k;
    this.onClick.Invoke();
    this.UpdateSetting();
  }

  public void StartListening() => KeyListener.Instance.ListenForKey(this, this.actionName);
}
