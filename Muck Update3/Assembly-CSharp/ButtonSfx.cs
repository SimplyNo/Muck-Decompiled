// Decompiled with JetBrains decompiler
// Type: ButtonSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSfx : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerClickHandler
{
  public void OnPointerEnter(PointerEventData eventData)
  {
  }

  public void OnPointerClick(PointerEventData eventData) => UiSfx.Instance.PlayClick();
}
