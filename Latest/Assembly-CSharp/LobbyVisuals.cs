// Decompiled with JetBrains decompiler
// Type: LobbyVisuals
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyVisuals : MonoBehaviour
{
  private Dictionary<ulong, int> steamToLobbyId = new Dictionary<ulong, int>();
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
      this.playerNames[index].text = "";
    }
  }

  private void Start() => MusicController.Instance.PlaySong(MusicController.SongType.Day, false);

  public void CopyLobbyId() => GUIUtility.systemCopyBuffer = string.Concat((object) this.currentLobby.Id.Value);

  public void CloseLobby()
  {
    for (int index = 0; index < this.lobbyPlayers.Length; ++index)
    {
      this.lobbyPlayers[index].SetActive(false);
      this.playerNames[index].text = "";
    }
    this.menuUi.LeaveLobby();
  }

  public void OpenLobby(Lobby lobby)
  {
    this.steamToLobbyId = new Dictionary<ulong, int>();
    this.currentLobby = lobby;
    NetworkController.Instance.lobby = this.currentLobby;
    LocalClient.instance.serverHost = (SteamId) lobby.Owner.Id.Value;
    this.lobbyId.text = "Lobby ID: (send to friend)<size=90%>\n" + string.Concat((object) lobby.Id.Value);
    if ((long) SteamManager.Instance.PlayerSteamId.Value != (long) (ulong) lobby.Owner.Id)
      LobbySettings.Instance.startButton.SetActive(false);
    else
      LobbySettings.Instance.startButton.SetActive(true);
    foreach (Friend member in lobby.Members)
    {
      int nextId = this.GetNextId();
      if (nextId == -1)
        return;
      SteamId steamid = (SteamId) member.Id.Value;
      this.steamToLobbyId[(ulong) steamid] = nextId;
      this.SpawnLobbyPlayer(new Friend(steamid));
    }
    this.menuUi.JoinLobby();
  }

  public void SpawnLobbyPlayer(Friend friend)
  {
    MonoBehaviour.print((object) ("spawning lobby player: " + friend.Name));
    int nextId = this.GetNextId();
    string name = friend.Name;
    this.steamToLobbyId[friend.Id.Value] = nextId;
    this.lobbyPlayers[nextId].SetActive(true);
    this.lobbyPlayers[nextId].GetComponentInChildren<TextMeshProUGUI>().text = name;
    this.playerNames[nextId].text = friend.Name;
  }

  public void DespawnLobbyPlayer(Friend friend)
  {
    int index = this.steamToLobbyId[friend.Id.Value];
    this.lobbyPlayers[index].SetActive(false);
    this.playerNames[index].text = "";
    this.steamToLobbyId.Remove(friend.Id.Value);
    this.playerNames[index].text = "";
  }

  private int GetNextId()
  {
    for (int index = 0; index < this.lobbyPlayers.Length; ++index)
    {
      if (!this.lobbyPlayers[index].activeInHierarchy)
        return index;
    }
    return -1;
  }

  public void ExitGame() => Application.Quit(0);
}
