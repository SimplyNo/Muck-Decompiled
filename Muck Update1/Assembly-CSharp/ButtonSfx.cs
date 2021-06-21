// Decompiled with JetBrains decompiler
// Type: ButtonSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSfx : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler
{
  public void OnPointerEnter(PointerEventData eventData)
  {
  }

  public void OnPointerClick(PointerEventData eventData) => UiSfx.Instance.PlayClick();
}
