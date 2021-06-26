// Decompiled with JetBrains decompiler
// Type: GameoverUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class GameoverUI : MonoBehaviour
{
  public TextMeshProUGUI daysText;
  public TextMeshProUGUI header;

  private void Awake()
  {
    int winnerId = GameManager.instance.winnerId;
    switch (winnerId)
    {
      case -3:
        this.header.text = "Victory!";
        this.daysText.text = "<size=80%>Muck escaped after " + (object) GameManager.instance.currentDay + " days!";
        break;
      case -2:
        this.daysText.text = "Survived for " + (object) GameManager.instance.currentDay + " days.";
        break;
      case -1:
        this.daysText.text = "Draw...";
        break;
      default:
        this.daysText.text = GameManager.players[winnerId].username + " won the game!";
        break;
    }
  }
}
