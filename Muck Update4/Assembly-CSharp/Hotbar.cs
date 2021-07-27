// Decompiled with JetBrains decompiler
// Type: Hotbar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Hotbar : MonoBehaviour
{
  private InventoryCell[] cells;
  private InventoryCell[] inventoryCells;
  public InventoryUI inventory;
  public InventoryItem currentItem;
  private int oldActive = -1;
  private int currentActive;
  private int max = 6;
  public static Hotbar Instance;
  private float sendDelay = 0.25f;

  private void Start()
  {
    Hotbar.Instance = this;
    this.inventoryCells = this.inventory.hotkeysTransform.GetComponentsInChildren<InventoryCell>();
    this.cells = this.GetComponentsInChildren<InventoryCell>();
    this.cells[this.currentActive].slot.color = this.cells[this.currentActive].hover;
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
    float y = Input.mouseScrollDelta.y;
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
    if ((Object) this.inventoryCells[this.currentActive].currentItem != (Object) this.currentItem)
    {
      this.currentItem = this.inventoryCells[this.currentActive].currentItem;
      if ((bool) (Object) UseInventory.Instance)
        UseInventory.Instance.SetWeapon(this.currentItem);
      this.CancelInvoke("SendItemToServer");
      this.Invoke("SendItemToServer", this.sendDelay);
    }
    for (int index = 0; index < this.cells.Length; ++index)
    {
      if (index == this.currentActive)
        this.cells[index].slot.color = this.cells[index].hover;
      else
        this.cells[index].slot.color = this.cells[index].idle;
    }
    for (int index = 0; index < this.cells.Length; ++index)
    {
      this.cells[index].itemImage.sprite = this.inventoryCells[index].itemImage.sprite;
      this.cells[index].itemImage.color = this.inventoryCells[index].itemImage.color;
      this.cells[index].amount.text = this.inventoryCells[index].amount.text;
    }
  }

  private void SendItemToServer()
  {
    if ((Object) this.currentItem == (Object) null)
    {
      ClientSend.WeaponInHand(-1);
      if (!(bool) (Object) PreviewPlayer.Instance)
        return;
      PreviewPlayer.Instance.WeaponInHand(-1);
    }
    else
    {
      ClientSend.WeaponInHand(this.currentItem.id);
      if (!(bool) (Object) PreviewPlayer.Instance)
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
}
