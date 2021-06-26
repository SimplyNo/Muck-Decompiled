// Decompiled with JetBrains decompiler
// Type: InventoryUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    if ((Object) i == (Object) null)
      return false;
    int amount = i.amount;
    if (!this.IsInventoryFull())
      return true;
    foreach (InventoryCell cell in this.cells)
    {
      if ((Object) cell != (Object) null && cell.currentItem.id == i.id)
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
      if ((Object) cell.currentItem == (Object) null)
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
      if ((Object) cell.currentItem == (Object) null)
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
    if (!(bool) (Object) Boat.Instance)
      return;
    Boat.Instance.CheckForMap();
  }

  private void UpdateMouseSprite()
  {
    if ((Object) this.currentMouseItem != (Object) null)
    {
      this.mouseItemSprite.sprite = this.currentMouseItem.sprite;
      this.mouseItemSprite.color = Color.white;
      this.mouseItemText.text = this.currentMouseItem.GetAmount();
    }
    else
    {
      this.mouseItemSprite.sprite = (Sprite) null;
      this.mouseItemSprite.color = Color.clear;
      this.mouseItemText.text = "";
    }
    if (!(bool) (Object) this.CraftingUi)
      return;
    this.CraftingUi.UpdateCraftables();
  }

  private void Update() => this.mouseItemSprite.transform.position = Input.mousePosition;

  public void DropItem([CanBeNull] PointerEventData eventData)
  {
    if ((Object) this.currentMouseItem == (Object) null)
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
      if (eventData.button == PointerEventData.InputButton.Right)
        amount = 1;
      InventoryItem instance = ScriptableObject.CreateInstance<InventoryItem>();
      instance.Copy(this.currentMouseItem, amount);
      InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
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
    if ((Object) item == (Object) null)
      return;
    ClientSend.DropItem(item.id, item.amount);
  }

  private void FillCellList()
  {
    this.cells = new List<InventoryCell>();
    foreach (InventoryCell componentsInChild in this.inventoryParent.GetComponentsInChildren<InventoryCell>())
      this.cells.Add(componentsInChild);
    foreach (InventoryCell componentsInChild in this.hotkeysTransform.GetComponentsInChildren<InventoryCell>())
      this.cells.Add(componentsInChild);
  }

  public void UpdateAllCells()
  {
    foreach (InventoryCell cell in this.cells)
      cell.UpdateCell();
  }

  public void ToggleInventory()
  {
    this.backDrop.SetActive(!this.backDrop.activeInHierarchy);
    if (this.transform.parent.gameObject.activeInHierarchy || !((Object) this.currentMouseItem != (Object) null))
      return;
    this.DropItem((PointerEventData) null);
  }

  public int AddItemToInventory(InventoryItem item)
  {
    InventoryItem instance = ScriptableObject.CreateInstance<InventoryItem>();
    instance.Copy(item, item.amount);
    InventoryCell inventoryCell = (InventoryCell) null;
    UiSfx.Instance.PlayPickup();
    foreach (InventoryCell cell in this.cells)
    {
      if ((Object) cell.currentItem == (Object) null)
      {
        if (!((Object) inventoryCell != (Object) null))
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
    if ((bool) (Object) inventoryCell)
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
      if (!((Object) cell.currentItem == (Object) null) && cell.currentItem.name == "Coin")
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
      if (!((Object) cell.currentItem == (Object) null) && cell.currentItem.Compare(itemByName))
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
        if (!((Object) cell.currentItem == (Object) null) && cell.currentItem.Compare(requirement.item))
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
    if (!this.IsCraftable(item) || (Object) this.currentMouseItem != (Object) null && (!item.Compare(this.currentMouseItem) || this.currentMouseItem.amount + item.craftAmount > this.currentMouseItem.max))
      return;
    foreach (InventoryItem.CraftRequirement requirement in item.requirements)
    {
      int num1 = 0;
      foreach (InventoryCell cell in this.cells)
      {
        if (!((Object) cell.currentItem == (Object) null) && cell.currentItem.Compare(requirement.item))
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
    if ((Object) this.currentMouseItem != (Object) null)
    {
      this.currentMouseItem.amount += item.craftAmount;
    }
    else
    {
      this.currentMouseItem = ScriptableObject.CreateInstance<InventoryItem>();
      this.currentMouseItem.Copy(item, item.craftAmount);
    }
    UiEvents.Instance.CheckNewUnlocks(item.id);
    this.UpdateMouseSprite();
  }

  public bool CanRepair(InventoryItem[] requirements)
  {
    foreach (InventoryItem requirement in requirements)
    {
      if (!this.HasItem(requirement))
        return false;
    }
    return true;
  }

  public bool Repair(InventoryItem[] requirements)
  {
    foreach (InventoryItem requirement in requirements)
    {
      if (!this.HasItem(requirement))
        return false;
    }
    foreach (InventoryItem requirement in requirements)
    {
      int num1 = 0;
      foreach (InventoryCell cell in this.cells)
      {
        if (!((Object) cell.currentItem == (Object) null) && cell.currentItem.Compare(requirement))
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
    return true;
  }

  public bool HasItem(InventoryItem requirement)
  {
    int num = 0;
    foreach (InventoryCell cell in this.cells)
    {
      if (!((Object) cell.currentItem == (Object) null) && cell.currentItem.Compare(requirement))
      {
        num += cell.currentItem.amount;
        if (num >= requirement.amount)
          break;
      }
    }
    return num >= requirement.amount;
  }

  public bool AddArmor(InventoryItem item)
  {
    for (int index = 0; index < this.armorCells.Length; ++index)
    {
      if ((Object) this.armorCells[index].currentItem == (Object) null && item.tag == this.armorCells[index].tags[0])
      {
        this.armorCells[index].currentItem = item;
        this.armorCells[index].UpdateCell();
        return true;
      }
    }
    return false;
  }

  public bool HoldingItem() => (Object) this.currentMouseItem != (Object) null;
}
