// Decompiled with JetBrains decompiler
// Type: KeyListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    this.alertText.text = "Press any key for\n\"" + actionName + "\"\n\n<i><size=60%>...escape to go back";
    this.currentlyChanging = listener;
    this.overlay.SetActive(true);
  }

  private void Update()
  {
    if (!this.overlay.activeInHierarchy)
      return;
    MonoBehaviour.print((object) "listenign");
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      this.CloseListener();
    }
    else
    {
      foreach (KeyCode keyCode in Enum.GetValues(typeof (KeyCode)))
      {
        if (Input.GetKey(keyCode))
        {
          this.currentlyChanging.SetKey(keyCode);
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
}
