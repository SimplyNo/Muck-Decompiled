// Decompiled with JetBrains decompiler
// Type: InventoryBackdrop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
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
