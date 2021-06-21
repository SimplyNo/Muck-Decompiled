// Decompiled with JetBrains decompiler
// Type: KeyListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class KeyListener : MonoBehaviour
{
  public ControlSetting currentlyChanging;
  public TextMeshProUGUI alertText;
  public GameObject overlay;
  public static KeyListener Instance;

  private void Awake()
  {
    KeyListener.Instance = this;
    this.overlay.SetActive(false);
  }

  public void ListenForKey(ControlSetting listener, string actionName)
  {
    ((TMP_Text) this.alertText).set_text("Press any key for\n\"" + actionName + "\"\n\n<i><size=60%>...escape to go back");
    this.currentlyChanging = listener;
    this.overlay.SetActive(true);
  }

  private void Update()
  {
    if (!this.overlay.get_activeInHierarchy())
      return;
    MonoBehaviour.print((object) "listenign");
    if (Input.GetKeyDown((KeyCode) 27))
    {
      this.CloseListener();
    }
    else
    {
      foreach (KeyCode k in Enum.GetValues(typeof (KeyCode)))
      {
        if (Input.GetKey(k))
        {
          this.currentlyChanging.SetKey(k);
          this.CloseListener();
          break;
        }
      }
    }
  }

  private void CloseListener()
  {
    this.overlay.SetActive(false);
    this.currentlyChanging = (ControlSetting) null;
    UiSfx.Instance.PlayClick();
  }

  public KeyListener() => base.\u002Ector();
}
