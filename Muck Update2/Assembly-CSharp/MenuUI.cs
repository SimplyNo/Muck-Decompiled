// Decompiled with JetBrains decompiler
// Type: MenuUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
  public GameObject startBtn;
  public GameObject lobbyUi;
  public GameObject mainUi;
  public TextMeshProUGUI version;
  public MenuCamera menuCam;

  private void Start()
  {
    this.lobbyUi.SetActive(false);
    Cursor.set_visible(true);
    Cursor.set_lockState((CursorLockMode) 0);
    PPController.Instance.Reset();
    ((TMP_Text) this.version).set_text("Version" + Application.get_version());
  }

  public void StartLobby() => SteamManager.Instance.StartLobby();

  public void JoinLobby()
  {
    this.lobbyUi.SetActive(true);
    this.mainUi.SetActive(false);
    this.menuCam.Lobby();
  }

  public void LeaveLobby()
  {
    this.lobbyUi.SetActive(false);
    this.mainUi.SetActive(true);
    this.menuCam.Menu();
  }

  public void LeaveGame()
  {
    SteamManager.Instance.leaveLobby();
    this.startBtn.SetActive(false);
  }

  public void StartGame() => SteamLobby.Instance.StartGame();

  public MenuUI() => base.\u002Ector();
}
