// Decompiled with JetBrains decompiler
// Type: TestAchievements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestAchievements : MonoBehaviour
{
  public WoodmanTrades[] ye;
  public WoodmanTrades wildcard;

  private void Awake() => Object.DontDestroyOnLoad((Object) this.gameObject);

  private void Start() => this.FillWildcard();

  private void FillWildcard()
  {
    WoodmanTrades.Trade[] tradeArray = new WoodmanTrades.Trade[0];
    foreach (WoodmanTrades woodmanTrades in this.ye)
      tradeArray = ((IEnumerable<WoodmanTrades.Trade>) tradeArray).Concat<WoodmanTrades.Trade>((IEnumerable<WoodmanTrades.Trade>) woodmanTrades.trades).ToArray<WoodmanTrades.Trade>();
    this.wildcard.trades = new WoodmanTrades.Trade[tradeArray.Length];
    for (int index = 0; index < tradeArray.Length; ++index)
    {
      WoodmanTrades.Trade trade = tradeArray[index];
      this.wildcard.trades[index] = new WoodmanTrades.Trade()
      {
        amount = trade.amount,
        item = trade.item,
        price = trade.price
      };
    }
  }

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.K))
      return;
    GameManager.instance.GameOver(-3);
  }
}
