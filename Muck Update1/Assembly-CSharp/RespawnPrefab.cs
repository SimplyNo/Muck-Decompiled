// Decompiled with JetBrains decompiler
// Type: RespawnPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RespawnPrefab : MonoBehaviour
{
  public RawImage overlay;
  public Button button;
  public TextMeshProUGUI nameText;

  public int playerId { get; set; }

  public void Set(int id, bool active, string username)
  {
    this.playerId = id;
    this.overlay.gameObject.SetActive(!active);
    this.button.enabled = active;
    this.nameText.text = username;
  }

  public void RespawnPlayer()
  {
    Debug.LogError((object) "requesting revive");
    RespawnTotemUI.Instance.RequestRevive(this.playerId);
  }
}
