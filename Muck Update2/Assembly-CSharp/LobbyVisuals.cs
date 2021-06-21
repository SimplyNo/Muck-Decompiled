// Decompiled with JetBrains decompiler
// Type: LobbyVisuals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyVisuals : MonoBehaviour
{
  private Dictionary<ulong, int> steamToLobbyId;
  public GameObject[] lobbyPlayers;
  public TextMeshProUGUI[] playerNames;
  public TextMeshProUGUI lobbyId;
  private Lobby currentLobby;
  public MenuUI menuUi;
  public static LobbyVisuals Instance;

  private void Awake()
  {
    LobbyVisuals.Instance = this;
    for (int index = 0; index < this.lobbyPlayers.Length; ++index)
    {
      this.lobbyPlayers[index].SetActive(false);
      ((TMP_Text) this.playerNames[index]).set_text("");
    }
  }

  private void Start() => MusicController.Instance.PlaySong(MusicController.SongType.Day, false);

  public void CopyLobbyId() => GUIUtility.set_systemCopyBuffer(string.Concat((object) (ulong) ((Lobby) ref this.currentLobby).get_Id().Value));

  public void CloseLobby()
  {
    for (int index = 0; index < this.lobbyPlayers.Length; ++index)
    {
      this.lobbyPlayers[index].SetActive(false);
      ((TMP_Text) this.playerNames[index]).set_text("");
    }
    this.menuUi.LeaveLobby();
  }

  public void OpenLobby(Lobby lobby)
  {
    this.steamToLobbyId = new Dictionary<ulong, int>();
    this.currentLobby = lobby;
    NetworkController.Instance.lobby = this.currentLobby;
    LocalClient.instance.serverHost = SteamId.op_Implicit((ulong) ((SteamId) ((Lobby) ref lobby).get_Owner().Id).Value);
    ((TMP_Text) this.lobbyId).set_text("Lobby ID: (send to friend)<size=90%>\n" + string.Concat((object) (ulong) ((Lobby) ref lobby).get_Id().Value));
    if (SteamManager.Instance.PlayerSteamId.Value != (long) SteamId.op_Implicit((SteamId) ((Lobby) ref lobby).get_Owner().Id))
      LobbySettings.Instance.startButton.SetActive(false);
    else
      LobbySettings.Instance.startButton.SetActive(true);
    using (IEnumerator<Friend> enumerator = ((Lobby) ref lobby).get_Members().GetEnumerator())
    {
      while (((IEnumerator) enumerator).MoveNext())
      {
        Friend current = enumerator.Current;
        int nextId = this.GetNextId();
        if (nextId == -1)
          return;
        SteamId steamId = SteamId.op_Implicit((ulong) ((SteamId) current.Id).Value);
        this.steamToLobbyId[SteamId.op_Implicit(steamId)] = nextId;
        this.SpawnLobbyPlayer(new Friend(steamId));
      }
    }
    this.menuUi.JoinLobby();
  }

  public void SpawnLobbyPlayer(Friend friend)
  {
    MonoBehaviour.print((object) ("spawning lobby player: " + ((Friend) ref friend).get_Name()));
    int nextId = this.GetNextId();
    string name = ((Friend) ref friend).get_Name();
    this.steamToLobbyId[(ulong) ((SteamId) friend.Id).Value] = nextId;
    this.lobbyPlayers[nextId].SetActive(true);
    ((TMP_Text) this.lobbyPlayers[nextId].GetComponentInChildren<TextMeshProUGUI>()).set_text(name);
    ((TMP_Text) this.playerNames[nextId]).set_text(((Friend) ref friend).get_Name());
  }

  public void DespawnLobbyPlayer(Friend friend)
  {
    int index = this.steamToLobbyId[(ulong) ((SteamId) friend.Id).Value];
    this.lobbyPlayers[index].SetActive(false);
    ((TMP_Text) this.playerNames[index]).set_text("");
    this.steamToLobbyId.Remove((ulong) ((SteamId) friend.Id).Value);
    ((TMP_Text) this.playerNames[index]).set_text("");
  }

  private int GetNextId()
  {
    for (int index = 0; index < this.lobbyPlayers.Length; ++index)
    {
      if (!this.lobbyPlayers[index].get_activeInHierarchy())
        return index;
    }
    return -1;
  }

  public void ExitGame() => Application.Quit(0);

  public LobbyVisuals() => base.\u002Ector();
}
