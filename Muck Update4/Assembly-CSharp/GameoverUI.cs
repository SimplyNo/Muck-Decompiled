// Decompiled with JetBrains decompiler
// Type: GameoverUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameoverUI : MonoBehaviour
{
  public TextMeshProUGUI header;
  public TextMeshProUGUI nameText;
  public GameObject statPrefab;
  public List<StatPrefab> statPrefabs;
  public Transform statsParent;
  private int page;

  private void Awake()
  {
    this.HeaderText();
    this.InitStats();
    this.FillStats();
  }

  private void InitStats()
  {
    for (int index = 0; index < Player.allStats.Length; ++index)
      this.statPrefabs.Add(Object.Instantiate<GameObject>(this.statPrefab, this.statsParent).GetComponent<StatPrefab>());
  }

  private void FillStats()
  {
    int page = this.page;
    Dictionary<string, int> stat = GameManager.instance.stats[page];
    int num = 0;
    foreach (KeyValuePair<string, int> s in stat)
    {
      if (num == 0)
        this.nameText.text = GameManager.players[s.Value].username ?? "";
      else
        this.statPrefabs[num - 1].SetStat(s);
      ++num;
    }
  }

  public void FlipPage(int dir)
  {
    if (dir < 0 && this.page <= 0 || dir > 0 && this.page >= GameManager.instance.nStatsPlayers - 1)
      return;
    this.page += dir;
    this.FillStats();
  }

  private void HeaderText()
  {
    int winnerId = GameManager.instance.winnerId;
    switch (winnerId)
    {
      case -3:
        this.header.text = "Victory!";
        break;
      case -2:
        this.header.text = "Defeat..";
        break;
      case -1:
        this.header.text = "Draw...";
        break;
      default:
        this.header.text = GameoverUI.Truncate(GameManager.players[winnerId].username, 10) + " won!";
        break;
    }
  }

  public static string Truncate(string value, int maxLength) => string.IsNullOrEmpty(value) || value.Length <= maxLength ? value : value.Substring(0, maxLength);
}
