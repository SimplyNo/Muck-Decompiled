// Decompiled with JetBrains decompiler
// Type: Setting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Setting : MonoBehaviour
{
  public int currentSetting;
  [FormerlySerializedAs("onClick")]
  [SerializeField]
  public Setting.ButtonClickedEvent m_OnClick = new Setting.ButtonClickedEvent();

  public Setting.ButtonClickedEvent onClick
  {
    get => this.m_OnClick;
    set => this.m_OnClick = value;
  }

  public class ButtonClickedEvent : UnityEvent
  {
  }
}
