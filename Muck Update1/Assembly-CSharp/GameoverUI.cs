// Decompiled with JetBrains decompiler
// Type: GameoverUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class GameoverUI : MonoBehaviour
{
  public TextMeshProUGUI daysText;

  private void Awake()
  {
    int winnerId = GameManager.instance.winnerId;
    switch (winnerId)
    {
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
