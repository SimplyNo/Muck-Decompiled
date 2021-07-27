// Decompiled with JetBrains decompiler
// Type: ButtonSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
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
