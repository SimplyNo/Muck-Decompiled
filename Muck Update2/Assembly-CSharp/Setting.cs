// Decompiled with JetBrains decompiler
// Type: Setting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Setting : MonoBehaviour
{
  public int currentSetting;
  [FormerlySerializedAs("onClick")]
  [SerializeField]
  public Setting.ButtonClickedEvent m_OnClick;

  public Setting.ButtonClickedEvent onClick
  {
    get => this.m_OnClick;
    set => this.m_OnClick = value;
  }

  public Setting() => base.\u002Ector();

  public class ButtonClickedEvent : UnityEvent
  {
    public ButtonClickedEvent() => base.\u002Ector();
  }
}
