// Decompiled with JetBrains decompiler
// Type: UpdateArmor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateArmor : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
  private InventoryCell cell;

  private void Awake() => this.cell = this.GetComponent<InventoryCell>();

  public void OnPointerDown(PointerEventData eventData)
  {
    int itemId = !((Object) this.cell.currentItem == (Object) null) ? this.cell.currentItem.id : -1;
    PlayerStatus.Instance.UpdateArmor(this.transform.GetSiblingIndex(), itemId);
    UiSfx.Instance.PlayArmor();
  }
}
