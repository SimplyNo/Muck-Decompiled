// Decompiled with JetBrains decompiler
// Type: InventoryUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
  public Transform inventoryParent;
  public Transform hotkeysTransform;
  public Transform armorTransform;
  public Transform leftTransform;
  public InventoryCell[] armorCells;
  public InventoryCell[] hotkeyCells;
  public InventoryCell[] allCells;
  public InventoryCell leftHand;
  public InventoryCell arrows;
  public Hotbar hotbar;
  public List<InventoryCell> cells;
  public InventoryItem currentMouseItem;
  public Image mouseItemSprite;
  public TextMeshProUGUI mouseItemText;
  public static InventoryUI Instance;
  public static readonly float throwForce = 700f;
  public GameObject backDrop;
  public InventoryExtensions CraftingUi;

  private void Awake() => InventoryUI.Instance = this;

  private void Start()
  {
    this.FillCellList();
    this.UpdateMouseSprite();
    this.backDrop.SetActive(false);
  }

  public bool CanPickup(InventoryItem i)
  {
    if (Object.op_Equality((Object) i, (Object) null))
      return false;
    int amount = i.amount;
    if (!this.IsInventoryFull())
      return true;
    foreach (InventoryCell cell in this.cells)
    {
      if (Object.op_Inequality((Object) cell, (Object) null) && cell.currentItem.id == i.id)
      {
        amount -= cell.currentItem.max - cell.currentItem.amount;
        if (amount <= 0)
          return true;
      }
    }
    return false;
  }

  public bool IsInventoryFull()
  {
    foreach (InventoryCell cell in this.cells)
    {
      if (Object.op_Equality((Object) cell.currentItem, (Object) null))
        return false;
    }
    return true;
  }

  public bool pickupCooldown { get; set; }

  public void CooldownPickup()
  {
    this.pickupCooldown = true;
    this.Invoke("ResetCooldown", (float) (NetStatus.GetPing() * 2) / 1000f);
  }

  private void ResetCooldown() => this.pickupCooldown = false;

  public void CheckInventoryAlmostFull()
  {
    int num = 0;
    foreach (InventoryCell cell in this.cells)
    {
      if (Object.op_Equality((Object) cell.currentItem, (Object) null))
      {
        ++num;
        if (num > 2)
          return;
      }
    }
    if (num != 1)
      return;
    this.CooldownPickup();
  }

  public void PickupItem(InventoryItem item)
  {
    this.hotbar.UpdateHotbar();
    this.currentMouseItem = item;
    this.UpdateMouseSprite();
  }

  public void PlaceItem(InventoryItem item)
  {
    this.hotbar.UpdateHotbar();
    this.currentMouseItem = item;
    this.UpdateMouseSprite();
  }

  private void UpdateMouseSprite()
  {
    if (Object.op_Inequality((Object) this.currentMouseItem, (Object) null))
    {
      this.mouseItemSprite.set_sprite(this.currentMouseItem.sprite);
      ((Graphic) this.mouseItemSprite).set_color(Color.get_white());
      ((TMP_Text) this.mouseItemText).set_text(this.currentMouseItem.GetAmount());
    }
    else
    {
      this.mouseItemSprite.set_sprite((Sprite) null);
      ((Graphic) this.mouseItemSprite).set_color(Color.get_clear());
      ((TMP_Text) this.mouseItemText).set_text("");
    }
    if (!Object.op_Implicit((Object) this.CraftingUi))
      return;
    this.CraftingUi.UpdateCraftables();
  }

  private void Update() => ((Component) this.mouseItemSprite).get_transform().set_position(Input.get_mousePosition());

  public void DropItem([CanBeNull] PointerEventData eventData)
  {
    if (Object.op_Equality((Object) this.currentMouseItem, (Object) null))
      return;
    this.hotbar.UpdateHotbar();
    if (eventData == null)
    {
      this.DropItemIntoWorld(this.currentMouseItem);
      this.currentMouseItem = (InventoryItem) null;
    }
    else
    {
      int amount = this.currentMouseItem.amount;
      if (eventData.get_button() == 1)
        amount = 1;
      InventoryItem instance = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      instance.Copy(this.currentMouseItem, amount);
      InventoryItem inventoryItem = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      inventoryItem.Copy(this.currentMouseItem, this.currentMouseItem.amount - amount);
      if (inventoryItem.amount < 1)
        inventoryItem = (InventoryItem) null;
      this.currentMouseItem = inventoryItem;
      this.DropItemIntoWorld(instance);
    }
    this.UpdateMouseSprite();
  }

  public void DropItemIntoWorld(InventoryItem item)
  {
    if (Object.op_Equality((Object) item, (Object) null))
      return;
    ClientSend.DropItem(item.id, item.amount);
  }

  private void FillCellList()
  {
    this.cells = new List<InventoryCell>();
    foreach (InventoryCell componentsInChild in (InventoryCell[]) ((Component) this.inventoryParent).GetComponentsInChildren<InventoryCell>())
      this.cells.Add(componentsInChild);
    foreach (InventoryCell componentsInChild in (InventoryCell[]) ((Component) this.hotkeysTransform).GetComponentsInChildren<InventoryCell>())
      this.cells.Add(componentsInChild);
  }

  public void UpdateAllCells()
  {
    foreach (InventoryCell cell in this.cells)
      cell.UpdateCell();
  }

  public void ToggleInventory()
  {
    this.backDrop.SetActive(!this.backDrop.get_activeInHierarchy());
    if (((Component) ((Component) this).get_transform().get_parent()).get_gameObject().get_activeInHierarchy() || !Object.op_Inequality((Object) this.currentMouseItem, (Object) null))
      return;
    this.DropItem((PointerEventData) null);
  }

  public int AddItemToInventory(InventoryItem item)
  {
    InventoryItem instance = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
    instance.Copy(item, item.amount);
    InventoryCell inventoryCell = (InventoryCell) null;
    UiSfx.Instance.PlayPickup();
    foreach (InventoryCell cell in this.cells)
    {
      if (Object.op_Equality((Object) cell.currentItem, (Object) null))
      {
        if (!Object.op_Inequality((Object) inventoryCell, (Object) null))
          inventoryCell = cell;
      }
      else if (cell.currentItem.Compare(instance) && cell.currentItem.stackable)
      {
        if (cell.currentItem.amount + instance.amount > cell.currentItem.max)
        {
          int num = cell.currentItem.max - cell.currentItem.amount;
          cell.currentItem.amount += num;
          instance.amount -= num;
          cell.UpdateCell();
        }
        else
        {
          cell.currentItem.amount += instance.amount;
          cell.UpdateCell();
          UiEvents.Instance.AddPickup(instance);
          return 0;
        }
      }
    }
    if (Object.op_Implicit((Object) inventoryCell))
    {
      inventoryCell.currentItem = instance;
      inventoryCell.UpdateCell();
      MonoBehaviour.print((object) "added to available cell");
      UiEvents.Instance.AddPickup(instance);
      return 0;
    }
    UiEvents.Instance.AddPickup(instance);
    return instance.amount;
  }

  public int GetMoney()
  {
    int num = 0;
    foreach (InventoryCell cell in this.cells)
    {
      if (!Object.op_Equality((Object) cell.currentItem, (Object) null) && cell.currentItem.name == "Coin")
        num += cell.currentItem.amount;
    }
    return num;
  }

  public void UseMoney(int amount)
  {
    int num1 = 0;
    InventoryItem itemByName = ItemManager.Instance.GetItemByName("Coin");
    foreach (InventoryCell cell in this.cells)
    {
      if (!Object.op_Equality((Object) cell.currentItem, (Object) null) && cell.currentItem.Compare(itemByName))
      {
        if (cell.currentItem.amount <= amount)
        {
          num1 += cell.currentItem.amount;
          MonoBehaviour.print((object) "removing money");
          cell.RemoveItem();
        }
        else
        {
          int num2 = amount - num1;
          cell.currentItem.amount -= num2;
          cell.UpdateCell();
          MonoBehaviour.print((object) "taking money");
          break;
        }
      }
    }
  }

  public bool IsCraftable(InventoryItem item)
  {
    foreach (InventoryItem.CraftRequirement requirement in item.requirements)
    {
      int num = 0;
      foreach (InventoryCell cell in this.cells)
      {
        if (!Object.op_Equality((Object) cell.currentItem, (Object) null) && cell.currentItem.Compare(requirement.item))
        {
          num += cell.currentItem.amount;
          if (num >= requirement.amount)
            break;
        }
      }
      if (num < requirement.amount)
        return false;
    }
    return true;
  }

  public void CraftItem(InventoryItem item)
  {
    if (!this.IsCraftable(item) || Object.op_Inequality((Object) this.currentMouseItem, (Object) null) && (!item.Compare(this.currentMouseItem) || this.currentMouseItem.amount + item.craftAmount > this.currentMouseItem.max))
      return;
    foreach (InventoryItem.CraftRequirement requirement in item.requirements)
    {
      int num1 = 0;
      foreach (InventoryCell cell in this.cells)
      {
        if (!Object.op_Equality((Object) cell.currentItem, (Object) null) && cell.currentItem.Compare(requirement.item))
        {
          if (cell.currentItem.amount <= requirement.amount)
          {
            num1 += cell.currentItem.amount;
            cell.RemoveItem();
          }
          else
          {
            int num2 = requirement.amount - num1;
            cell.currentItem.amount -= num2;
            cell.UpdateCell();
            break;
          }
        }
      }
    }
    this.CraftingUi.UpdateCraftables();
    if (Object.op_Inequality((Object) this.currentMouseItem, (Object) null))
    {
      this.currentMouseItem.amount += item.craftAmount;
    }
    else
    {
      this.currentMouseItem = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      this.currentMouseItem.Copy(item, item.craftAmount);
    }
    UiEvents.Instance.CheckNewUnlocks(item.id);
    this.UpdateMouseSprite();
  }

  public bool AddArmor(InventoryItem item)
  {
    for (int index = 0; index < this.armorCells.Length; ++index)
    {
      if (Object.op_Equality((Object) this.armorCells[index].currentItem, (Object) null) && item.tag == this.armorCells[index].tags[0])
      {
        this.armorCells[index].currentItem = item;
        this.armorCells[index].UpdateCell();
        return true;
      }
    }
    return false;
  }

  public bool HoldingItem() => Object.op_Inequality((Object) this.currentMouseItem, (Object) null);

  public InventoryUI() => base.\u002Ector();
}
