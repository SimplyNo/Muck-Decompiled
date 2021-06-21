// Decompiled with JetBrains decompiler
// Type: InventoryCell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
  public InventoryCell.CellType cellType;
  public TextMeshProUGUI amount;
  public Image itemImage;
  public RawImage slot;
  [HideInInspector]
  public InventoryItem currentItem;
  public InventoryItem spawnItem;
  public int cellId;
  public Color idle;
  public Color hover;
  private bool ready;
  private float lastClickTime;
  private float doubleClickThreshold;
  public InventoryItem.ItemTag[] tags;
  public RawImage overlay;

  private void Start()
  {
    if (Object.op_Implicit((Object) this.spawnItem))
    {
      this.currentItem = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      this.currentItem.Copy(this.spawnItem, this.spawnItem.amount);
    }
    this.UpdateCell();
  }

  public void UpdateCell()
  {
    if (Object.op_Equality((Object) this.currentItem, (Object) null))
    {
      ((TMP_Text) this.amount).set_text("");
      this.itemImage.set_sprite((Sprite) null);
      ((Graphic) this.itemImage).set_color(Color.get_clear());
    }
    else
    {
      ((TMP_Text) this.amount).set_text(this.currentItem.GetAmount());
      this.itemImage.set_sprite(this.currentItem.sprite);
      ((Graphic) this.itemImage).set_color(Color.get_white());
    }
    this.SetColor(this.idle);
  }

  public void ForceAddItem(InventoryItem item, int amount)
  {
    this.currentItem = (InventoryItem) Object.Instantiate<InventoryItem>((M0) item);
    this.currentItem.amount = amount;
    this.UpdateCell();
  }

  public InventoryItem SetItem(InventoryItem pointerItem, PointerEventData eventData)
  {
    InventoryItem currentItem = this.currentItem;
    int amount1 = pointerItem.amount;
    if (eventData.get_button() == 1)
      amount1 = 1;
    InventoryItem inventoryItem1;
    InventoryItem inventoryItem2;
    if (Object.op_Equality((Object) currentItem, (Object) null))
    {
      inventoryItem1 = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      inventoryItem1.Copy(pointerItem, amount1);
      if (pointerItem.amount - amount1 < 1)
      {
        inventoryItem2 = (InventoryItem) null;
      }
      else
      {
        inventoryItem2 = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
        inventoryItem2.Copy(pointerItem, pointerItem.amount - amount1);
      }
    }
    else if (pointerItem.Compare(currentItem) && pointerItem.stackable)
    {
      if (currentItem.amount + amount1 > currentItem.max)
        amount1 = currentItem.max - currentItem.amount;
      inventoryItem1 = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      inventoryItem1.Copy(this.currentItem, this.currentItem.amount + amount1);
      if (pointerItem.amount - amount1 < 1)
      {
        inventoryItem2 = (InventoryItem) null;
      }
      else
      {
        inventoryItem2 = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
        inventoryItem2.Copy(pointerItem, pointerItem.amount - amount1);
      }
    }
    else
    {
      inventoryItem1 = pointerItem;
      inventoryItem2 = currentItem;
    }
    this.currentItem = inventoryItem1;
    this.UpdateCell();
    UiEvents.Instance.PlaceInInventory(this.currentItem);
    if (this.cellType == InventoryCell.CellType.Chest)
    {
      MonoBehaviour.print((object) ("sending chest updates, currentchest:  " + (object) OtherInput.Instance.currentChest.id));
      int itemId = -1;
      int amount2 = 0;
      if (Object.op_Implicit((Object) this.currentItem))
      {
        itemId = this.currentItem.id;
        amount2 = this.currentItem.amount;
      }
      ClientSend.ChestUpdate(OtherInput.Instance.currentChest.id, this.cellId, itemId, amount2);
      this.Invoke("GetReady", (float) (NetStatus.GetPing() * 3) * 0.01f);
    }
    return inventoryItem2;
  }

  private void GetReady() => this.ready = true;

  public InventoryItem PickupItem(PointerEventData eventData)
  {
    if (!Object.op_Implicit((Object) this.currentItem))
      return (InventoryItem) null;
    InventoryItem inventoryItem1;
    InventoryItem inventoryItem2;
    if (eventData.get_button() == 1 && this.currentItem.amount > 1)
    {
      int amount1 = this.currentItem.amount / 2;
      int amount2 = this.currentItem.amount - amount1;
      inventoryItem1 = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      inventoryItem1.Copy(this.currentItem, amount1);
      inventoryItem2 = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      inventoryItem2.Copy(this.currentItem, amount2);
    }
    else
    {
      inventoryItem1 = (InventoryItem) null;
      inventoryItem2 = this.currentItem;
    }
    this.currentItem = inventoryItem1;
    this.UpdateCell();
    if (this.cellType == InventoryCell.CellType.Chest)
    {
      int itemId = -1;
      int amount = 0;
      if (Object.op_Implicit((Object) this.currentItem))
      {
        itemId = this.currentItem.id;
        amount = this.currentItem.amount;
      }
      this.Invoke("GetReady", 1f);
      ClientSend.ChestUpdate(OtherInput.Instance.currentChest.id, this.cellId, itemId, amount);
    }
    return inventoryItem2;
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (!this.ready)
      return;
    this.ready = false;
    this.Invoke("GetReady", Time.get_deltaTime() * 2f);
    if (this.cellType == InventoryCell.CellType.Crafting)
      InventoryUI.Instance.CraftItem(this.currentItem);
    else if ((double) Time.get_time() - (double) this.lastClickTime < 0.25 && eventData.get_button() == null && InventoryUI.Instance.HoldingItem())
      this.DoubleClick();
    else if (Input.GetKey((KeyCode) 304))
    {
      this.ShiftClick();
    }
    else
    {
      if (InventoryUI.Instance.HoldingItem())
      {
        if (this.IsItemCompatibleWithCell(InventoryUI.Instance.currentMouseItem))
        {
          InventoryItem currentMouseItem = InventoryUI.Instance.currentMouseItem;
          InventoryUI.Instance.PlaceItem(this.SetItem(currentMouseItem, eventData));
        }
      }
      else
        InventoryUI.Instance.PickupItem(this.PickupItem(eventData));
      if (eventData.get_button() != null)
        return;
      this.lastClickTime = Time.get_time();
    }
  }

  private bool IsItemCompatibleWithCell(InventoryItem item)
  {
    if (this.tags.Length == 0)
      return true;
    foreach (InventoryItem.ItemTag tag in this.tags)
    {
      if (item.tag == tag)
        return true;
    }
    return false;
  }

  public void RemoveItem()
  {
    this.currentItem = (InventoryItem) null;
    this.UpdateCell();
  }

  private void DoubleClick()
  {
    InventoryItem currentMouseItem = InventoryUI.Instance.currentMouseItem;
    if (!currentMouseItem.stackable)
      return;
    foreach (InventoryCell cell in InventoryUI.Instance.cells)
    {
      if (!Object.op_Equality((Object) cell.currentItem, (Object) null) && cell.currentItem.Compare(currentMouseItem))
      {
        if (currentMouseItem.amount + cell.currentItem.amount > currentMouseItem.max)
        {
          int num = currentMouseItem.max - currentMouseItem.amount;
          currentMouseItem.amount += num;
          cell.currentItem.amount -= num;
          cell.UpdateCell();
          InventoryUI.Instance.PickupItem(currentMouseItem);
          return;
        }
        currentMouseItem.amount += cell.currentItem.amount;
        cell.RemoveItem();
      }
    }
    InventoryUI.Instance.PickupItem(currentMouseItem);
  }

  private bool ShiftClick()
  {
    if (this.cellType == InventoryCell.CellType.Chest)
    {
      if (!InventoryUI.Instance.CanPickup(this.currentItem))
        return false;
      InventoryUI.Instance.AddItemToInventory(this.currentItem);
      this.RemoveItem();
      int itemId = -1;
      int amount = 0;
      if (Object.op_Implicit((Object) this.currentItem))
      {
        itemId = this.currentItem.id;
        amount = this.currentItem.amount;
      }
      ClientSend.ChestUpdate(OtherInput.Instance.currentChest.id, this.cellId, itemId, amount);
      return true;
    }
    if (this.cellType == InventoryCell.CellType.Inventory)
    {
      int craftingState1 = (int) OtherInput.Instance.craftingState;
      int craftingState2 = (int) OtherInput.Instance.craftingState;
      int craftingState3 = (int) OtherInput.Instance.craftingState;
    }
    return false;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.SetColor(this.hover);
    if (!Object.op_Implicit((Object) this.currentItem))
      return;
    if (this.cellType == InventoryCell.CellType.Inventory)
    {
      string t = this.currentItem.name + "\n<size=50%><i>" + this.currentItem.description;
      if (this.currentItem.IsArmour())
        t = t + "\n+" + (object) this.currentItem.armor + " armor" + "\n" + this.currentItem.armorComponent.setBonus;
      ItemInfo.Instance.SetText(t);
    }
    else
    {
      if (this.cellType != InventoryCell.CellType.Crafting)
        return;
      string t = this.currentItem.name + "<size=60%>";
      foreach (InventoryItem.CraftRequirement requirement in this.currentItem.requirements)
        t = t + "\n" + requirement.item.name + " - " + (object) requirement.amount;
      ItemInfo.Instance.SetText(t);
    }
  }

  public void Eat(int amount)
  {
    this.currentItem.amount -= amount;
    if (this.currentItem.amount <= 0)
      this.RemoveItem();
    this.UpdateCell();
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.SetColor(this.idle);
    ItemInfo.Instance.Fade(0.0f);
  }

  public void SetColor(Color c)
  {
  }

  public void AddItemToChest(InventoryItem item)
  {
  }

  public void AddItemToCauldron()
  {
  }

  public void AddItemToFurnace()
  {
  }

  public void SetOverlayAlpha(float f)
  {
    MonoBehaviour.print((object) ("overlay set to: " + (object) f));
    ((Graphic) this.overlay).set_color(new Color(0.0f, 0.0f, 0.0f, f));
  }

  public InventoryCell() => base.\u002Ector();

  public enum CellType
  {
    Inventory,
    Crafting,
    Chest,
  }
}
