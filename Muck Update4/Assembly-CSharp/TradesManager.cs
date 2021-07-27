// Decompiled with JetBrains decompiler
// Type: TradesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TradesManager : MonoBehaviour
{
  public WoodmanTrades archerTrades;
  public WoodmanTrades chefTrades;
  public WoodmanTrades smithTrades;
  public WoodmanTrades woodTrades;
  public WoodmanTrades wildcardTrades;
  public static TradesManager Instance;

  private void Awake() => TradesManager.Instance = this;
}
