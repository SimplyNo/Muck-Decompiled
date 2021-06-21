// Decompiled with JetBrains decompiler
// Type: GameoverUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
        ((TMP_Text) this.daysText).set_text("Survived for " + (object) GameManager.instance.currentDay + " days.");
        break;
      case -1:
        ((TMP_Text) this.daysText).set_text("Draw...");
        break;
      default:
        ((TMP_Text) this.daysText).set_text(GameManager.players[winnerId].username + " won the game!");
        break;
    }
  }

  public GameoverUI() => base.\u002Ector();
}
