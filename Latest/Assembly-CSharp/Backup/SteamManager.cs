// Decompiled with JetBrains decompiler
// Type: SteamManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SteamManager : MonoBehaviour
{
  public static SteamManager Instance;
  private static uint gameAppId = 1625450;
  private string playerSteamIdString;
  private bool connectedToSteam;
  private Friend lobbyPartner;
  public List<Lobby> activeUnrankedLobbies;
  public List<Lobby> activeRankedLobbies;
  public Lobby currentLobby;
  private Lobby hostedMultiplayerLobby;
  private SteamId originalLobbyOwnerId;
  private bool applicationHasQuit;
  private bool daRealOne;

  public string PlayerName { get; set; }

  public SteamId PlayerSteamId { get; set; }

  public string PlayerSteamIdString => this.playerSteamIdString;

  public Friend LobbyPartner
  {
    get => this.lobbyPartner;
    set => this.lobbyPartner = value;
  }

  public SteamId lobbyOwnerSteamId { get; set; }

  public bool LobbyPartnerDisconnected { get; set; }

  public void Awake()
  {
    if ((UnityEngine.Object) SteamManager.Instance == (UnityEngine.Object) null)
    {
      this.daRealOne = true;
      UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.gameObject);
      SteamManager.Instance = this;
      this.PlayerName = "";
      try
      {
        SteamClient.Init(SteamManager.gameAppId);
        if (!SteamClient.IsValid)
        {
          Debug.Log((object) "Steam client not valid");
          throw new Exception();
        }
        this.PlayerName = SteamClient.Name;
        this.PlayerSteamId = SteamClient.SteamId;
        this.playerSteamIdString = this.PlayerSteamId.ToString();
        this.activeUnrankedLobbies = new List<Lobby>();
        this.activeRankedLobbies = new List<Lobby>();
        this.connectedToSteam = true;
        Debug.Log((object) ("Steam initialized: " + this.PlayerName));
      }
      catch (Exception ex)
      {
        this.connectedToSteam = false;
        this.playerSteamIdString = "NoSteamId";
        Debug.Log((object) "Error connecting to Steam");
        Debug.Log((object) ex);
      }
    }
    else
    {
      if (!((UnityEngine.Object) SteamManager.Instance != (UnityEngine.Object) this))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
  }

  public bool TryToReconnectToSteam()
  {
    Debug.Log((object) "Attempting to reconnect to Steam");
    try
    {
      SteamClient.Init(SteamManager.gameAppId);
      if (!SteamClient.IsValid)
      {
        Debug.Log((object) "Steam client not valid");
        throw new Exception();
      }
      this.PlayerName = SteamClient.Name;
      this.PlayerSteamId = SteamClient.SteamId;
      this.activeUnrankedLobbies = new List<Lobby>();
      this.activeRankedLobbies = new List<Lobby>();
      Debug.Log((object) ("Steam initialized: " + this.PlayerName));
      this.connectedToSteam = true;
      return true;
    }
    catch (Exception ex)
    {
      this.connectedToSteam = false;
      Debug.Log((object) "Error connecting to Steam");
      Debug.Log((object) ex);
      return false;
    }
  }

  public bool ConnectedToSteam() => this.connectedToSteam;

  public void StartLobby() => this.CreateLobby(0);

  public void StopLobby() => this.leaveLobby();

  private void Start()
  {
    SteamMatchmaking.OnLobbyGameCreated += new Action<Lobby, uint, ushort, SteamId>(this.OnLobbyGameCreatedCallback);
    SteamMatchmaking.OnLobbyCreated += new Action<Result, Lobby>(this.OnLobbyCreatedCallback);
    SteamMatchmaking.OnLobbyEntered += new Action<Lobby>(this.OnLobbyEnteredCallback);
    SteamMatchmaking.OnLobbyMemberJoined += new Action<Lobby, Friend>(this.OnLobbyMemberJoinedCallback);
    SteamMatchmaking.OnChatMessage += new Action<Lobby, Friend, string>(this.OnChatMessageCallback);
    SteamMatchmaking.OnLobbyMemberDisconnected += new Action<Lobby, Friend>(this.OnLobbyMemberDisconnectedCallback);
    SteamMatchmaking.OnLobbyMemberLeave += new Action<Lobby, Friend>(this.OnLobbyMemberLeaveCallback);
    SteamFriends.OnGameLobbyJoinRequested += new Action<Lobby, SteamId>(this.OnGameLobbyJoinRequestedCallback);
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
    this.UpdateRichPresenceStatus(SceneManager.GetActiveScene().name);
  }

  private void Update() => SteamClient.RunCallbacks();

  private void OnDisable()
  {
    if (!this.daRealOne)
      return;
    this.gameCleanup();
  }

  private void OnDestroy()
  {
    if (!this.daRealOne)
      return;
    this.gameCleanup();
  }

  private void OnApplicationQuit()
  {
    if (!this.daRealOne)
      return;
    this.gameCleanup();
  }

  private void gameCleanup()
  {
    if (this.applicationHasQuit)
      return;
    this.applicationHasQuit = true;
    this.leaveLobby();
    SteamClient.Shutdown();
  }

  private void OnLobbyMemberDisconnectedCallback(Lobby lobby, Friend friend) => this.OtherLobbyMemberLeft(friend);

  private void OnLobbyMemberLeaveCallback(Lobby lobby, Friend friend) => this.OtherLobbyMemberLeft(friend);

  private void OtherLobbyMemberLeft(Friend friend)
  {
    if ((long) friend.Id.Value == (long) this.PlayerSteamId.Value)
      return;
    Debug.LogError((object) "someone is leaving lobby");
    if ((long) (ulong) friend.Id != (long) (ulong) this.PlayerSteamId)
    {
      this.LobbyPartnerDisconnected = true;
      LobbyVisuals.Instance.DespawnLobbyPlayer(friend);
      try
      {
        SteamNetworking.CloseP2PSessionWithUser(friend.Id);
      }
      catch
      {
        Debug.Log((object) "Unable to update disconnected player nameplate / process disconnect cleanly");
      }
    }
    if ((long) this.originalLobbyOwnerId.Value == (long) this.PlayerSteamId.Value)
    {
      Debug.LogError((object) "player left sucess");
      SteamLobby.Instance.RemovePlayerFromLobby(friend);
    }
    if ((long) this.originalLobbyOwnerId.Value != (long) friend.Id.Value)
      return;
    this.leaveLobby();
    StatusMessage.Instance.DisplayMessage("Server host left the lobby...");
    if (!(bool) (UnityEngine.Object) GameManager.instance)
      return;
    GameManager.instance.LeaveGame();
  }

  private void OnLobbyGameCreatedCallback(Lobby lobby, uint ip, ushort port, SteamId steamId)
  {
  }

  public async void JoinLobby(Lobby lobby)
  {
    if ((long) lobby.Id.Value == (long) this.currentLobby.Id.Value)
    {
      Debug.LogError((object) "Attempted to join the same lobby twice...");
    }
    else
    {
      LocalClient.serverOwner = false;
      this.leaveLobby();
      if (await lobby.Join() != RoomEnter.Success)
      {
        Debug.Log((object) "failed to join lobby");
        StatusMessage.Instance.DisplayMessage("Couldn't find lobby. Make sure it's a valid lobbyID from someone");
      }
      else
      {
        this.currentLobby = lobby;
        this.lobbyOwnerSteamId = (SteamId) lobby.Owner.Id.Value;
        this.LobbyPartnerDisconnected = false;
        this.AcceptP2P(this.lobbyOwnerSteamId);
      }
    }
  }

  private void AcceptP2P(SteamId opponentId)
  {
    try
    {
      SteamNetworking.AcceptP2PSessionWithUser(opponentId);
    }
    catch
    {
      Debug.Log((object) "Unable to accept P2P Session with user");
    }
  }

  private void OnChatMessageCallback(Lobby lobby, Friend friend, string message)
  {
    if ((long) (ulong) friend.Id == (long) (ulong) this.PlayerSteamId)
      return;
    Debug.Log((object) "incoming chat message");
    Debug.Log((object) message);
    lobby.SetGameServer(this.PlayerSteamId);
  }

  private void OnLobbyEnteredCallback(Lobby lobby)
  {
    if (lobby.MemberCount < 1)
    {
      lobby.Leave();
    }
    else
    {
      string version = Application.version;
      string data = lobby.GetData("Version");
      if (version != data)
      {
        StatusMessage.Instance.DisplayMessage("You're on version " + version + ", but server is on " + data + ". Update your game on Steam to play.\n<size=60%>If there is no update button, right click on the game > manage > uninstall, then install again");
        this.leaveLobby();
      }
      else
      {
        LobbyVisuals.Instance.OpenLobby(lobby);
        LocalClient.serverOwner = false;
        this.originalLobbyOwnerId = (SteamId) lobby.Owner.Id.Value;
        if (lobby.MemberCount == 1)
          return;
        this.AcceptP2P(this.originalLobbyOwnerId);
        lobby.SendChatString("incoming player info");
      }
    }
  }

  private async void OnGameLobbyJoinRequestedCallback(Lobby joinedLobby, SteamId id)
  {
    Debug.LogError((object) "trying to join lobby");
    if ((long) joinedLobby.Id.Value == (long) this.currentLobby.Id.Value)
    {
      Debug.LogError((object) "Attempted to join the same lobby twice...");
    }
    else
    {
      LocalClient.serverOwner = false;
      this.leaveLobby();
      if (await joinedLobby.Join() != RoomEnter.Success)
      {
        Debug.Log((object) "failed to join lobby");
      }
      else
      {
        this.currentLobby = joinedLobby;
        this.lobbyOwnerSteamId = (SteamId) joinedLobby.Owner.Id.Value;
        this.LobbyPartnerDisconnected = false;
        this.AcceptP2P(this.lobbyOwnerSteamId);
        Debug.LogError((object) "Join success");
      }
    }
  }

  private void OnLobbyCreatedCallback(Result result, Lobby lobby)
  {
    Debug.LogError((object) "lobbyu created opkay");
    this.LobbyPartnerDisconnected = false;
    if (result != Result.OK)
    {
      Debug.Log((object) "lobby creation result not ok");
      Debug.Log((object) result.ToString());
    }
    this.lobbyOwnerSteamId = this.PlayerSteamId;
    lobby.SetData("Version", Application.version);
    Debug.Log((object) ("on version: " + lobby.GetData("Version")));
    SteamLobby.Instance.StartLobby(this.PlayerSteamId, lobby);
  }

  private void OnLobbyMemberJoinedCallback(Lobby lobby, Friend friend)
  {
    Debug.Log((object) "someone else joined lobby");
    if ((long) (ulong) friend.Id != (long) (ulong) this.PlayerSteamId)
    {
      this.LobbyPartner = friend;
      this.lobbyOwnerSteamId = (SteamId) lobby.Owner.Id.Value;
      this.AcceptP2P(this.lobbyOwnerSteamId);
      this.LobbyPartnerDisconnected = false;
      LobbyVisuals.Instance.SpawnLobbyPlayer(friend);
    }
    if ((long) this.currentLobby.Owner.Id.Value != (long) (ulong) this.PlayerSteamId)
      return;
    SteamLobby.Instance.AddPlayerToLobby(friend);
  }

  public void leaveLobby()
  {
    try
    {
      if ((long) this.currentLobby.Owner.Id.Value == (long) this.PlayerSteamId.Value)
        SteamLobby.Instance.CloseLobby();
    }
    catch
    {
      Debug.Log((object) "Steam lobby doesn't exist...");
    }
    if (!(bool) (UnityEngine.Object) GameManager.instance)
      LobbyVisuals.Instance.CloseLobby();
    try
    {
      this.currentLobby.Leave();
      Debug.Log((object) "Lobby left successfully");
    }
    catch
    {
      Debug.Log((object) "Error leaving current lobby");
    }
    try
    {
      SteamNetworking.CloseP2PSessionWithUser(this.lobbyOwnerSteamId);
    }
    catch
    {
      Debug.Log((object) "Error closing P2P session with opponent");
    }
    this.currentLobby = new Lobby();
  }

  public async Task<bool> CreateFriendLobby()
  {
    try
    {
      Lobby? lobbyAsync = await SteamMatchmaking.CreateLobbyAsync(8);
      if (!lobbyAsync.HasValue)
      {
        Debug.Log((object) "Lobby created but not correctly instantiated");
        throw new Exception();
      }
      this.LobbyPartnerDisconnected = false;
      this.hostedMultiplayerLobby = lobbyAsync.Value;
      this.hostedMultiplayerLobby.SetFriendsOnly();
      this.currentLobby = this.hostedMultiplayerLobby;
      this.hostedMultiplayerLobby.SetData("Version", Application.version ?? "");
      return true;
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Failed to create multiplayer lobby");
      Debug.Log((object) ex.ToString());
      return false;
    }
  }

  public async Task<bool> CreateLobby(int lobbyParameters)
  {
    try
    {
      Lobby? lobbyAsync = await SteamMatchmaking.CreateLobbyAsync(8);
      if (!lobbyAsync.HasValue)
      {
        Debug.Log((object) "Lobby created but not correctly instantiated");
        throw new Exception();
      }
      this.LobbyPartnerDisconnected = false;
      this.hostedMultiplayerLobby = lobbyAsync.Value;
      this.hostedMultiplayerLobby.SetPublic();
      this.hostedMultiplayerLobby.SetJoinable(true);
      this.hostedMultiplayerLobby.SetData("Version", Application.version ?? "");
      this.currentLobby = this.hostedMultiplayerLobby;
      return true;
    }
    catch (Exception ex)
    {
      Debug.Log((object) "Failed to create multiplayer lobby");
      StatusMessage.Instance.DisplayMessage("Failed to connect to Steam, can't start a lobby. \n<size=72%><i>Make sure you have an internet connection on a valid steam account");
      Debug.Log((object) ex.ToString());
      this.TryToReconnectToSteam();
      return false;
    }
  }

  public void OpenFriendOverlayForGameInvite() => SteamFriends.OpenGameInviteOverlay(this.currentLobby.Id);

  private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) => this.UpdateRichPresenceStatus(scene.name);

  public void UpdateRichPresenceStatus(string SceneName)
  {
    if (!this.connectedToSteam)
      return;
    string key = "steam_display";
    if (SceneName.Equals("SillyScene"))
    {
      SteamFriends.SetRichPresence(key, "#SillyScene");
    }
    else
    {
      if (!SceneName.Contains("SillyScene2"))
        return;
      SteamFriends.SetRichPresence(key, "#SillyScene2");
    }
  }
}
