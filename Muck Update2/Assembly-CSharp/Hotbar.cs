// Decompiled with JetBrains decompiler
// Type: Hotbar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
  private InventoryCell[] cells;
  private InventoryCell[] inventoryCells;
  public InventoryUI inventory;
  public InventoryItem currentItem;
  private int oldActive;
  private int currentActive;
  private int max;
  public static Hotbar Instance;
  private float sendDelay;

  private void Start()
  {
    Hotbar.Instance = this;
    this.inventoryCells = (InventoryCell[]) ((Component) this.inventory.hotkeysTransform).GetComponentsInChildren<InventoryCell>();
    this.cells = (InventoryCell[]) ((Component) this).GetComponentsInChildren<InventoryCell>();
    ((Graphic) this.cells[this.currentActive].slot).set_color(this.cells[this.currentActive].hover);
    this.UpdateHotbar();
    this.Invoke("UpdateHotbar", 1f);
  }

  private void Update()
  {
    for (int index = 1; index < 8; ++index)
    {
      if (Input.GetButtonDown(nameof (Hotbar) + (object) index))
      {
        this.currentActive = index - 1;
        this.UpdateHotbar();
      }
    }
    float y = (float) Input.get_mouseScrollDelta().y;
    if ((double) y > 0.5)
    {
      --this.currentActive;
      if (this.currentActive < 0)
        this.currentActive = this.max;
      this.UpdateHotbar();
    }
    else
    {
      if ((double) y >= -0.5)
        return;
      ++this.currentActive;
      if (this.currentActive > this.max)
        this.currentActive = 0;
      this.UpdateHotbar();
    }
  }

  public void UpdateHotbar()
  {
    if (Object.op_Inequality((Object) this.inventoryCells[this.currentActive].currentItem, (Object) this.currentItem))
    {
      this.currentItem = this.inventoryCells[this.currentActive].currentItem;
      if (Object.op_Implicit((Object) UseInventory.Instance))
        UseInventory.Instance.SetWeapon(this.currentItem);
      this.CancelInvoke("SendItemToServer");
      this.Invoke("SendItemToServer", this.sendDelay);
    }
    for (int index = 0; index < this.cells.Length; ++index)
    {
      if (index == this.currentActive)
        ((Graphic) this.cells[index].slot).set_color(this.cells[index].hover);
      else
        ((Graphic) this.cells[index].slot).set_color(this.cells[index].idle);
    }
    for (int index = 0; index < this.cells.Length; ++index)
    {
      this.cells[index].itemImage.set_sprite(this.inventoryCells[index].itemImage.get_sprite());
      ((Graphic) this.cells[index].itemImage).set_color(((Graphic) this.inventoryCells[index].itemImage).get_color());
      ((TMP_Text) this.cells[index].amount).set_text(((TMP_Text) this.inventoryCells[index].amount).get_text());
    }
  }

  private void SendItemToServer()
  {
    if (Object.op_Equality((Object) this.currentItem, (Object) null))
    {
      ClientSend.WeaponInHand(-1);
    }
    else
    {
      ClientSend.WeaponInHand(this.currentItem.id);
      if (!Object.op_Implicit((Object) PreviewPlayer.Instance))
        return;
      PreviewPlayer.Instance.WeaponInHand(this.currentItem.id);
    }
  }

  public void UseItem(int n)
  {
    this.currentItem.amount -= n;
    if (this.currentItem.amount <= 0)
      this.inventoryCells[this.currentActive].RemoveItem();
    this.inventoryCells[this.currentActive].UpdateCell();
    this.UpdateHotbar();
  }

  public Hotbar() => base.\u002Ector();
}
