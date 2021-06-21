// Decompiled with JetBrains decompiler
// Type: InventoryBackdrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryBackdrop : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
  public void OnPointerDown(PointerEventData eventData)
  {
    if (!eventData.eligibleForClick || (Object) eventData.pointerCurrentRaycast.gameObject != (Object) this.gameObject)
      return;
    InventoryUI.Instance.DropItem(eventData);
  }
}
