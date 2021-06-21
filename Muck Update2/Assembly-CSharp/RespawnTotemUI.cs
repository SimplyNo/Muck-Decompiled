// Decompiled with JetBrains decompiler
// Type: RespawnTotemUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class RespawnTotemUI : MonoBehaviour
{
  public GameObject namePrefab;
  public Transform nameContainer;
  public GameObject root;
  public TextMeshProUGUI respawnPrice;
  public int basePrice;
  public static RespawnTotemUI Instance;

  public bool active { get; set; }

  private void Awake() => RespawnTotemUI.Instance = this;

  public void Show()
  {
    this.root.SetActive(true);
    ((TMP_Text) this.respawnPrice).set_text(string.Concat((object) this.GetRevivePrice()));
    this.Refresh();
    this.active = true;
    Cursor.set_lockState((CursorLockMode) 0);
    Cursor.set_visible(true);
  }

  public void Hide()
  {
    this.root.SetActive(false);
    this.active = false;
    Cursor.set_lockState((CursorLockMode) 1);
    Cursor.set_visible(false);
  }

  public void RequestRevive(int playerId)
  {
    Debug.LogError((object) "trying");
    if (InventoryUI.Instance.GetMoney() < this.GetRevivePrice())
      return;
    PlayerManager player = GameManager.players[playerId];
    if (Object.op_Equality((Object) player, (Object) null) || player.disconnected || !player.dead)
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
    float num3 = num1;
    return (int) ((double) this.basePrice * (double) Mathf.Clamp(num1 * (float) (1.0 + (double) (GameManager.instance.currentDay - 2) / (double) num2), num3, 100f));
  }

  public void Refresh()
  {
    for (int index = this.nameContainer.get_childCount() - 1; index >= 0; --index)
      Object.Destroy((Object) ((Component) this.nameContainer.GetChild(index)).get_gameObject());
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (!Object.op_Equality((Object) playerManager, (Object) null) && !Object.op_Equality((Object) playerManager, (Object) null) && (!playerManager.disconnected && playerManager.dead))
        ((RespawnPrefab) ((GameObject) Object.Instantiate<GameObject>((M0) this.namePrefab, this.nameContainer)).GetComponent<RespawnPrefab>()).Set(playerManager.id, InventoryUI.Instance.GetMoney() >= this.GetRevivePrice(), playerManager.username);
    }
  }

  public RespawnTotemUI() => base.\u002Ector();
}
