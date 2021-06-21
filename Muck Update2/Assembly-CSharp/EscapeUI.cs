// Decompiled with JetBrains decompiler
// Type: EscapeUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EscapeUI : MonoBehaviour
{
  public Button backBtn;

  private void Update()
  {
    if (!Input.GetKeyDown((KeyCode) 27))
      return;
    ((UnityEvent) this.backBtn.get_onClick()).Invoke();
    UiSfx.Instance.PlayClick();
  }

  public EscapeUI() => base.\u002Ector();
}
