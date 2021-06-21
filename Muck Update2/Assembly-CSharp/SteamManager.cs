// Decompiled with JetBrains decompiler
// Type: SteamManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    if (Object.op_Equality((Object) SteamManager.Instance, (Object) null))
    {
      this.daRealOne = true;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      SteamManager.Instance = this;
      this.PlayerName = "";
      try
      {
        SteamClient.Init(SteamManager.gameAppId, true);
        if (!SteamClient.get_IsValid())
        {
          Debug.Log((object) "Steam client not valid");
          throw new Exception();
        }
        this.PlayerName = SteamClient.get_Name();
        this.PlayerSteamId = SteamClient.get_SteamId();
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
      if (!Object.op_Inequality((Object) SteamManager.Instance, (Object) this))
        return;
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
  }

  public bool TryToReconnectToSteam()
  {
    Debug.Log((object) "Attempting to reconnect to Steam");
    try
    {
      SteamClient.Init(SteamManager.gameAppId, true);
      if (!SteamClient.get_IsValid())
      {
        Debug.Log((object) "Steam client not valid");
        throw new Exception();
      }
      this.PlayerName = SteamClient.get_Name();
      this.PlayerSteamId = SteamClient.get_SteamId();
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
    SteamMatchmaking.add_OnLobbyGameCreated(new Action<Lobby, uint, ushort, SteamId>(this.OnLobbyGameCreatedCallback));
    SteamMatchmaking.add_OnLobbyCreated(new Action<Result, Lobby>(this.OnLobbyCreatedCallback));
    SteamMatchmaking.add_OnLobbyEntered(new Action<Lobby>(this.OnLobbyEnteredCallback));
    SteamMatchmaking.add_OnLobbyMemberJoined(new Action<Lobby, Friend>(this.OnLobbyMemberJoinedCallback));
    SteamMatchmaking.add_OnChatMessage(new Action<Lobby, Friend, string>(this.OnChatMessageCallback));
    SteamMatchmaking.add_OnLobbyMemberDisconnected(new Action<Lobby, Friend>(this.OnLobbyMemberDisconnectedCallback));
    SteamMatchmaking.add_OnLobbyMemberLeave(new Action<Lobby, Friend>(this.OnLobbyMemberLeaveCallback));
    SteamFriends.add_OnGameLobbyJoinRequested(new Action<Lobby, SteamId>(this.OnGameLobbyJoinRequestedCallback));
    // ISSUE: method pointer
    SceneManager.add_sceneLoaded(new UnityAction<Scene, LoadSceneMode>((object) this, __methodptr(OnSceneLoaded)));
    Scene activeScene = SceneManager.GetActiveScene();
    this.UpdateRichPresenceStatus(((Scene) ref activeScene).get_name());
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
    if (((SteamId) friend.Id).Value == this.PlayerSteamId.Value)
      return;
    Debug.LogError((object) "someone is leaving lobby");
    if ((long) SteamId.op_Implicit((SteamId) friend.Id) != (long) SteamId.op_Implicit(this.PlayerSteamId))
    {
      this.LobbyPartnerDisconnected = true;
      LobbyVisuals.Instance.DespawnLobbyPlayer(friend);
      try
      {
        SteamNetworking.CloseP2PSessionWithUser((SteamId) friend.Id);
      }
      catch
      {
        Debug.Log((object) "Unable to update disconnected player nameplate / process disconnect cleanly");
      }
    }
    if (this.originalLobbyOwnerId.Value == this.PlayerSteamId.Value)
    {
      Debug.LogError((object) "player left sucess");
      SteamLobby.Instance.RemovePlayerFromLobby(friend);
    }
    if (this.originalLobbyOwnerId.Value != ((SteamId) friend.Id).Value)
      return;
    this.leaveLobby();
    StatusMessage.Instance.DisplayMessage("Server host left the lobby...");
    if (!Object.op_Implicit((Object) GameManager.instance))
      return;
    GameManager.instance.LeaveGame();
  }

  private void OnLobbyGameCreatedCallback(Lobby lobby, uint ip, ushort port, SteamId steamId)
  {
  }

  public async void JoinLobby(Lobby lobby)
  {
    if (((Lobby) ref lobby).get_Id().Value == ((Lobby) ref this.currentLobby).get_Id().Value)
    {
      Debug.LogError((object) "Attempted to join the same lobby twice...");
    }
    else
    {
      LocalClient.serverOwner = false;
      this.leaveLobby();
      if (await ((Lobby) ref lobby).Join() != 1)
      {
        Debug.Log((object) "failed to join lobby");
        StatusMessage.Instance.DisplayMessage("Couldn't find lobby. Make sure it's a valid lobbyID from someone");
      }
      else
      {
        this.currentLobby = lobby;
        this.lobbyOwnerSteamId = SteamId.op_Implicit((ulong) ((SteamId) ((Lobby) ref lobby).get_Owner().Id).Value);
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
    if ((long) SteamId.op_Implicit((SteamId) friend.Id) == (long) SteamId.op_Implicit(this.PlayerSteamId))
      return;
    Debug.Log((object) "incoming chat message");
    Debug.Log((object) message);
    ((Lobby) ref lobby).SetGameServer(this.PlayerSteamId);
  }

  private void OnLobbyEnteredCallback(Lobby lobby)
  {
    if (((Lobby) ref lobby).get_MemberCount() < 1)
    {
      ((Lobby) ref lobby).Leave();
    }
    else
    {
      string version = Application.get_version();
      string data = ((Lobby) ref lobby).GetData("Version");
      if (version != data)
      {
        StatusMessage.Instance.DisplayMessage("You're on version " + version + ", but server is on " + data + ". Update your game on Steam to play.\n<size=60%>If there is no update button, right click on the game > manage > uninstall, then install again");
        this.leaveLobby();
      }
      else
      {
        LobbyVisuals.Instance.OpenLobby(lobby);
        LocalClient.serverOwner = false;
        this.originalLobbyOwnerId = SteamId.op_Implicit((ulong) ((SteamId) ((Lobby) ref lobby).get_Owner().Id).Value);
        if (((Lobby) ref lobby).get_MemberCount() == 1)
          return;
        this.AcceptP2P(this.originalLobbyOwnerId);
        ((Lobby) ref lobby).SendChatString("incoming player info");
      }
    }
  }

  private async void OnGameLobbyJoinRequestedCallback(Lobby joinedLobby, SteamId id)
  {
    Debug.LogError((object) "trying to join lobby");
    if (((Lobby) ref joinedLobby).get_Id().Value == ((Lobby) ref this.currentLobby).get_Id().Value)
    {
      Debug.LogError((object) "Attempted to join the same lobby twice...");
    }
    else
    {
      LocalClient.serverOwner = false;
      this.leaveLobby();
      if (await ((Lobby) ref joinedLobby).Join() != 1)
      {
        Debug.Log((object) "failed to join lobby");
      }
      else
      {
        this.currentLobby = joinedLobby;
        this.lobbyOwnerSteamId = SteamId.op_Implicit((ulong) ((SteamId) ((Lobby) ref joinedLobby).get_Owner().Id).Value);
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
    if (result != 1)
    {
      Debug.Log((object) "lobby creation result not ok");
      Debug.Log((object) result.ToString());
    }
    this.lobbyOwnerSteamId = this.PlayerSteamId;
    ((Lobby) ref lobby).SetData("Version", Application.get_version());
    Debug.Log((object) ("on version: " + ((Lobby) ref lobby).GetData("Version")));
    SteamLobby.Instance.StartLobby(this.PlayerSteamId, lobby);
  }

  private void OnLobbyMemberJoinedCallback(Lobby lobby, Friend friend)
  {
    Debug.Log((object) "someone else joined lobby");
    if ((long) SteamId.op_Implicit((SteamId) friend.Id) != (long) SteamId.op_Implicit(this.PlayerSteamId))
    {
      this.LobbyPartner = friend;
      this.lobbyOwnerSteamId = SteamId.op_Implicit((ulong) ((SteamId) ((Lobby) ref lobby).get_Owner().Id).Value);
      this.AcceptP2P(this.lobbyOwnerSteamId);
      this.LobbyPartnerDisconnected = false;
      LobbyVisuals.Instance.SpawnLobbyPlayer(friend);
    }
    if (((SteamId) ((Lobby) ref this.currentLobby).get_Owner().Id).Value != (long) SteamId.op_Implicit(this.PlayerSteamId))
      return;
    SteamLobby.Instance.AddPlayerToLobby(friend);
  }

  public void leaveLobby()
  {
    try
    {
      if (((SteamId) ((Lobby) ref this.currentLobby).get_Owner().Id).Value == this.PlayerSteamId.Value)
        SteamLobby.Instance.CloseLobby();
    }
    catch
    {
      Debug.Log((object) "Steam lobby doesn't exist...");
    }
    if (!Object.op_Implicit((Object) GameManager.instance))
      LobbyVisuals.Instance.CloseLobby();
    try
    {
      ((Lobby) ref this.currentLobby).Leave();
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
    this.currentLobby = (Lobby) null;
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
      ((Lobby) ref this.hostedMultiplayerLobby).SetFriendsOnly();
      this.currentLobby = this.hostedMultiplayerLobby;
      ((Lobby) ref this.hostedMultiplayerLobby).SetData("Version", Application.get_version() ?? "");
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
      ((Lobby) ref this.hostedMultiplayerLobby).SetPublic();
      ((Lobby) ref this.hostedMultiplayerLobby).SetJoinable(true);
      ((Lobby) ref this.hostedMultiplayerLobby).SetData("Version", Application.get_version() ?? "");
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

  public void OpenFriendOverlayForGameInvite() => SteamFriends.OpenGameInviteOverlay(((Lobby) ref this.currentLobby).get_Id());

  private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) => this.UpdateRichPresenceStatus(((Scene) ref scene).get_name());

  public void UpdateRichPresenceStatus(string SceneName)
  {
    if (!this.connectedToSteam)
      return;
    string str = "steam_display";
    if (SceneName.Equals("SillyScene"))
    {
      SteamFriends.SetRichPresence(str, "#SillyScene");
    }
    else
    {
      if (!SceneName.Contains("SillyScene2"))
        return;
      SteamFriends.SetRichPresence(str, "#SillyScene2");
    }
  }

  public SteamManager() => base.\u002Ector();
}
