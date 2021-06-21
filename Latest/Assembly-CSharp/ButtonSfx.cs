// Decompiled with JetBrains decompiler
// Type: ButtonSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSfx : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler
{
  public void OnPointerEnter(PointerEventData eventData)
  {
  }

  public void OnPointerClick(PointerEventData eventData) => UiSfx.Instance.PlayClick();

  public ButtonSfx() => base.\u002Ector();
}
