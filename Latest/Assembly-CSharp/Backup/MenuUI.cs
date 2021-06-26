// Decompiled with JetBrains decompiler
// Type: MenuUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    PPController.Instance.Reset();
    this.version.text = "Version" + Application.version;
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
}
