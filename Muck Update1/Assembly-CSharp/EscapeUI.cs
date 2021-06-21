// Decompiled with JetBrains decompiler
// Type: EscapeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class EscapeUI : MonoBehaviour
{
  public Button backBtn;

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Escape))
      return;
    this.backBtn.onClick.Invoke();
    UiSfx.Instance.PlayClick();
  }
}
