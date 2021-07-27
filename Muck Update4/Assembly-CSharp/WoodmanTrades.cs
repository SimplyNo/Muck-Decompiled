// Decompiled with JetBrains decompiler
// Type: WoodmanTrades
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WoodmanTrades : ScriptableObject
{
  public WoodmanTrades.Trade[] trades;

  public List<WoodmanTrades.Trade> GetTrades(
    int min,
    int max,
    ConsistentRandom rand,
    float priceMultiplier = 1f)
  {
    List<WoodmanTrades.Trade> tradeList1 = new List<WoodmanTrades.Trade>();
    List<WoodmanTrades.Trade> tradeList2 = new List<WoodmanTrades.Trade>();
    foreach (WoodmanTrades.Trade trade in this.trades)
      tradeList2.Add(trade);
    int num = rand.Next(min, max);
    for (int index = 0; index < num && index < this.trades.Length; ++index)
    {
      WoodmanTrades.Trade trade1 = tradeList2[rand.Next(0, tradeList2.Count)];
      WoodmanTrades.Trade trade2 = new WoodmanTrades.Trade()
      {
        amount = trade1.amount,
        item = trade1.item,
        price = trade1.price
      };
      trade2.price = (int) ((double) priceMultiplier * (double) trade1.price);
      tradeList1.Add(trade2);
      tradeList2.Remove(trade1);
    }
    tradeList1.Sort();
    return tradeList1;
  }

  [Serializable]
  public class Trade : IComparable
  {
    public InventoryItem item;
    public int price;
    public int amount;

    public int CompareTo(object obj)
    {
      WoodmanTrades.Trade trade = (WoodmanTrades.Trade) obj;
      if (this.item.id > trade.item.id)
        return 1;
      return this.item.id < trade.item.id ? -1 : 0;
    }
  }
}
