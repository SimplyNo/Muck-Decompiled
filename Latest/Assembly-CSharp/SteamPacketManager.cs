// Decompiled with JetBrains decompiler
// Type: SteamPacketManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class SteamPacketManager : MonoBehaviour
{
  private void Start()
  {
    Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
    Server.InitializeServerPackets();
    LocalClient.InitializeClientData();
  }

  private void Update()
  {
    SteamClient.RunCallbacks();
    this.CheckForPackets();
  }

  private void CheckForPackets()
  {
    for (int channel = 0; channel < 2; ++channel)
    {
      if (SteamNetworking.IsP2PPacketAvailable(channel))
      {
        while (SteamNetworking.IsP2PPacketAvailable(channel))
          SteamPacketManager.HandlePacket(SteamNetworking.ReadP2PPacket(channel), channel);
      }
    }
  }

  private static void HandlePacket(P2Packet? p2Packet, int channel)
  {
    if (!p2Packet.HasValue)
      return;
    SteamId steamId = SteamId.op_Implicit((ulong) ((SteamId) p2Packet.Value.SteamId).Value);
    byte[] data = (byte[]) p2Packet.Value.Data;
    if (!LocalClient.serverOwner && steamId.Value != LocalClient.instance.serverHost.Value)
    {
      Friend friend = new Friend(steamId);
      Debug.LogError((object) ("Received packet from someone other than server: " + ((Friend) ref friend).get_Name() + "\nDenying packet..."));
    }
    else
    {
      Packet packet = new Packet();
      packet.SetBytes(data);
      if (packet.Length() != packet.ReadInt() + 4)
        Debug.LogError((object) "didnt read entire packet");
      int key = packet.ReadInt();
      if (channel == 0)
      {
        if (steamId.Value != LocalClient.instance.serverHost.Value)
          return;
        LocalClient.packetHandlers[key](packet);
      }
      else
        Server.PacketHandlers[key](SteamLobby.steamIdToClientId[(ulong) steamId.Value], packet);
    }
  }

  public static void SendPacket(
    SteamId steamId,
    Packet p,
    P2PSend p2pSend,
    SteamPacketManager.NetworkChannel channel)
  {
    int num = p.Length();
    byte[] numArray = p.CloneBytes();
    if (steamId.Value != SteamManager.Instance.PlayerSteamId.Value)
    {
      SteamNetworking.SendP2PPacket(SteamId.op_Implicit((ulong) steamId.Value), numArray, num, (int) channel, p2pSend);
    }
    else
    {
      P2Packet p2Packet = (P2Packet) null;
      p2Packet.SteamId = (__Null) SteamId.op_Implicit((ulong) steamId.Value);
      p2Packet.Data = (__Null) numArray;
      SteamPacketManager.HandlePacket(new P2Packet?(p2Packet), (int) channel);
    }
  }

  private void OnApplicationQuit() => SteamPacketManager.CloseConnections();

  public static void CloseConnections()
  {
    foreach (ulong key in SteamLobby.steamIdToClientId.Keys)
      SteamNetworking.CloseP2PSessionWithUser(SteamId.op_Implicit(key));
    try
    {
      SteamNetworking.CloseP2PSessionWithUser(LocalClient.instance.serverHost);
    }
    catch
    {
      Debug.Log((object) "Failed to close p2p with host");
    }
    SteamClient.Shutdown();
  }

  public SteamPacketManager() => base.\u002Ector();

  public enum NetworkChannel
  {
    ToClient,
    ToServer,
  }
}
