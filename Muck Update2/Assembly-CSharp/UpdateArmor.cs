// Decompiled with JetBrains decompiler
// Type: UpdateArmor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateArmor : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
  private InventoryCell cell;

  private void Awake() => this.cell = (InventoryCell) ((Component) this).GetComponent<InventoryCell>();

  public void OnPointerDown(PointerEventData eventData)
  {
    int itemId = !Object.op_Equality((Object) this.cell.currentItem, (Object) null) ? this.cell.currentItem.id : -1;
    PlayerStatus.Instance.UpdateArmor(((Component) this).get_transform().GetSiblingIndex(), itemId);
    UiSfx.Instance.PlayArmor();
  }

  public UpdateArmor() => base.\u002Ector();
}
