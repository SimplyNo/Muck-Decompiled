// Decompiled with JetBrains decompiler
// Type: TraderUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TraderUI : MonoBehaviour
{
  public TextMeshProUGUI title;
  private List<WoodmanTrades.Trade> buy;
  private List<WoodmanTrades.Trade> sell;
  public Transform listParent;
  public GameObject tradePrefab;
  public GameObject root;
  private bool buying = true;
  public static TraderUI Instance;

  private void Awake() => TraderUI.Instance = this;

  public void SetTrades(
    List<WoodmanTrades.Trade> buy,
    List<WoodmanTrades.Trade> sell,
    WoodmanBehaviour.WoodmanType type)
  {
    if (this.root.activeInHierarchy)
      return;
    this.buy = buy;
    this.sell = sell;
    this.title.text = type.ToString() + " Trader";
    this.OpenBuy();
    this.Show();
  }

  public void FillTrades()
  {
    for (int index = this.listParent.childCount - 1; index >= 0; --index)
      Object.Destroy((Object) this.listParent.GetChild(index).gameObject);
    if (this.buying)
    {
      foreach (WoodmanTrades.Trade t in this.buy)
        Object.Instantiate<GameObject>(this.tradePrefab, this.listParent).GetComponent<TradeUi>().SetTrade(t, true);
    }
    else
    {
      foreach (WoodmanTrades.Trade t in this.sell)
        Object.Instantiate<GameObject>(this.tradePrefab, this.listParent).GetComponent<TradeUi>().SetTrade(t, false);
    }
  }

  public void Show()
  {
    OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
    this.root.SetActive(true);
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  public void Hide()
  {
    this.root.SetActive(false);
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
  }

  public void OpenBuy()
  {
    this.buying = true;
    this.FillTrades();
  }

  public void OpenSell()
  {
    this.buying = false;
    this.FillTrades();
  }
}
