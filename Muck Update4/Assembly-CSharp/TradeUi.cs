// Decompiled with JetBrains decompiler
// Type: TradeUi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeUi : MonoBehaviour
{
  public TextMeshProUGUI name;
  public TextMeshProUGUI price;
  public TextMeshProUGUI buyText;
  public RawImage itemIcon;
  public GameObject overlay;
  public GameObject button;
  private WoodmanTrades.Trade trade;
  private bool buy;

  public void SetTrade(WoodmanTrades.Trade t, bool buy)
  {
    this.name.text = string.Format("{0} (x{1})", (object) t.item.name, (object) t.amount);
    this.price.text = string.Concat((object) t.price);
    this.itemIcon.texture = (Texture) t.item.sprite.texture;
    if (buy)
    {
      if (InventoryUI.Instance.GetMoney() < t.price)
        this.overlay.SetActive(true);
      else
        this.overlay.SetActive(false);
    }
    else
    {
      InventoryItem requirement = Object.Instantiate<InventoryItem>(t.item);
      requirement.amount = t.amount;
      if (InventoryUI.Instance.HasItem(requirement))
        this.overlay.SetActive(false);
      else
        this.overlay.SetActive(true);
    }
    this.trade = t;
    this.buy = buy;
    if (buy)
      this.buyText.text = "Buy";
    else
      this.buyText.text = "Sell";
  }

  public void BuySell()
  {
    if (this.buy)
    {
      if (InventoryUI.Instance.GetMoney() < this.trade.price)
        return;
      InventoryItem i = Object.Instantiate<InventoryItem>(this.trade.item);
      i.amount = this.trade.amount;
      if (!InventoryUI.Instance.CanPickup(i))
        return;
      InventoryUI.Instance.UseMoney(this.trade.price);
      InventoryUI.Instance.AddItemToInventory(i);
      if (InventoryUI.Instance.GetMoney() < this.trade.price)
        this.overlay.SetActive(true);
      else
        this.overlay.SetActive(false);
    }
    else
    {
      InventoryItem requirement = Object.Instantiate<InventoryItem>(this.trade.item);
      requirement.amount = this.trade.amount;
      if (!InventoryUI.Instance.HasItem(requirement))
        return;
      InventoryUI.Instance.RemoveItem(requirement);
      InventoryItem inventoryItem = Object.Instantiate<InventoryItem>(ItemManager.Instance.GetItemByName("Coin"));
      inventoryItem.amount = this.trade.price;
      InventoryUI.Instance.AddItemToInventory(inventoryItem);
      if (InventoryUI.Instance.HasItem(requirement))
        this.overlay.SetActive(false);
      else
        this.overlay.SetActive(true);
    }
  }
}
