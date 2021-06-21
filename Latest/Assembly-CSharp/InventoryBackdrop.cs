// Decompiled with JetBrains decompiler
// Type: InventoryBackdrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBackdrop : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
  public void OnPointerDown(PointerEventData eventData)
  {
    if (!eventData.get_eligibleForClick())
      return;
    RaycastResult pointerCurrentRaycast = eventData.get_pointerCurrentRaycast();
    if (Object.op_Inequality((Object) ((RaycastResult) ref pointerCurrentRaycast).get_gameObject(), (Object) ((Component) this).get_gameObject()))
      return;
    InventoryUI.Instance.DropItem(eventData);
  }

  public InventoryBackdrop() => base.\u002Ector();
}
