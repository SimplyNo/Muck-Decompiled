// Decompiled with JetBrains decompiler
// Type: RespawnTotemUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class RespawnTotemUI : MonoBehaviour
{
  public GameObject namePrefab;
  public Transform nameContainer;
  public GameObject root;
  public TextMeshProUGUI respawnPrice;
  public int basePrice = 25;
  public static RespawnTotemUI Instance;

  public bool active { get; set; }

  private void Awake() => RespawnTotemUI.Instance = this;

  public void Show()
  {
    this.root.SetActive(true);
    this.respawnPrice.text = string.Concat((object) this.GetRevivePrice());
    this.Refresh();
    this.active = true;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
  }

  public void Hide()
  {
    this.root.SetActive(false);
    this.active = false;
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  public void RequestRevive(int playerId)
  {
    Debug.LogError((object) "trying");
    if (InventoryUI.Instance.GetMoney() < this.GetRevivePrice())
      return;
    PlayerManager player = GameManager.players[playerId];
    if ((Object) player == (Object) null || player.disconnected || !player.dead)
      return;
    Debug.LogError((object) "sendinging revie");
    ClientSend.RevivePlayer(playerId);
  }

  public int GetRevivePrice()
  {
    GameSettings gameSettings = GameManager.gameSettings;
    float num1 = 1f;
    if (gameSettings.difficulty == GameSettings.Difficulty.Gamer)
      num1 = 1.2f;
    else if (gameSettings.difficulty == GameSettings.Difficulty.Easy)
      num1 = 0.8f;
    float num2 = 5f;
    float min = num1;
    return (int) ((double) this.basePrice * (double) Mathf.Clamp(num1 * (float) (1.0 + (double) (GameManager.instance.currentDay - 2) / (double) num2), min, 100f));
  }

  public void Refresh()
  {
    for (int index = this.nameContainer.childCount - 1; index >= 0; --index)
      Object.Destroy((Object) this.nameContainer.GetChild(index).gameObject);
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (!((Object) playerManager == (Object) null) && !((Object) playerManager == (Object) null) && (!playerManager.disconnected && playerManager.dead))
        Object.Instantiate<GameObject>(this.namePrefab, this.nameContainer).GetComponent<RespawnPrefab>().Set(playerManager.id, InventoryUI.Instance.GetMoney() >= this.GetRevivePrice(), playerManager.username);
    }
  }
}
