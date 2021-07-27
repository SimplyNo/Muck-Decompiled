// Decompiled with JetBrains decompiler
// Type: TraderInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class TraderInteract : MonoBehaviour, SharedObject, Interactable
{
  private int id;
  private WoodmanBehaviour.WoodmanType type;
  private List<WoodmanTrades.Trade> buy;
  private List<WoodmanTrades.Trade> sell;

  public void SetType(WoodmanBehaviour.WoodmanType type, ConsistentRandom rand)
  {
    this.type = type;
    switch (type)
    {
      case WoodmanBehaviour.WoodmanType.Archer:
        this.GenerateTrades(TradesManager.Instance.archerTrades, rand);
        break;
      case WoodmanBehaviour.WoodmanType.Smith:
        this.GenerateTrades(TradesManager.Instance.smithTrades, rand);
        break;
      case WoodmanBehaviour.WoodmanType.Woodcutter:
        this.GenerateTrades(TradesManager.Instance.woodTrades, rand);
        break;
      case WoodmanBehaviour.WoodmanType.Chef:
        this.GenerateTrades(TradesManager.Instance.chefTrades, rand);
        break;
      case WoodmanBehaviour.WoodmanType.Wildcard:
        this.GenerateTrades(TradesManager.Instance.wildcardTrades, rand);
        break;
      default:
        this.GenerateTrades(TradesManager.Instance.archerTrades, rand);
        break;
    }
  }

  private void GenerateTrades(WoodmanTrades trades, ConsistentRandom rand)
  {
    trades = UnityEngine.Object.Instantiate<WoodmanTrades>(trades);
    this.buy = trades.GetTrades(5, 10, rand);
    this.sell = trades.GetTrades(5, 10, rand, 0.5f);
  }

  private void GenerateWildcardTrades()
  {
  }

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  public void Interact() => TraderUI.Instance.SetTrades(this.buy, this.sell, this.type);

  public void LocalExecute() => throw new NotImplementedException();

  public void AllExecute() => throw new NotImplementedException();

  public void ServerExecute(int fromClient = -1) => throw new NotImplementedException();

  public void RemoveObject() => throw new NotImplementedException();

  public string GetName() => string.Format("Press {0} to trade with {1}", (object) InputManager.interact, (object) this.type);

  public bool IsStarted() => throw new NotImplementedException();
}
