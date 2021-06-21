// Decompiled with JetBrains decompiler
// Type: SteamPacketManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class SteamPacketManager : MonoBehaviour
{
  private void Start()
  {
    Object.DontDestroyOnLoad((Object) this.gameObject);
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
    SteamId steamid = (SteamId) p2Packet.Value.SteamId.Value;
    byte[] data = p2Packet.Value.Data;
    if (!LocalClient.serverOwner && (long) steamid.Value != (long) LocalClient.instance.serverHost.Value)
    {
      Debug.LogError((object) ("Received packet from someone other than server: " + new Friend(steamid).Name + "\nDenying packet..."));
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
        if ((long) steamid.Value != (long) LocalClient.instance.serverHost.Value)
          return;
        LocalClient.packetHandlers[key](packet);
      }
      else
        Server.PacketHandlers[key](SteamLobby.steamIdToClientId[steamid.Value], packet);
    }
  }

  public static void SendPacket(
    SteamId steamId,
    Packet p,
    P2PSend p2pSend,
    SteamPacketManager.NetworkChannel channel)
  {
    int length = p.Length();
    byte[] data = p.CloneBytes();
    if ((long) steamId.Value != (long) SteamManager.Instance.PlayerSteamId.Value)
      SteamNetworking.SendP2PPacket((SteamId) steamId.Value, data, length, (int) channel, p2pSend);
    else
      SteamPacketManager.HandlePacket(new P2Packet?(new P2Packet()
      {
        SteamId = (SteamId) steamId.Value,
        Data = data
      }), (int) channel);
  }

  private void OnApplicationQuit() => SteamPacketManager.CloseConnections();

  public static void CloseConnections()
  {
    foreach (ulong key in SteamLobby.steamIdToClientId.Keys)
      SteamNetworking.CloseP2PSessionWithUser((SteamId) key);
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

  public enum NetworkChannel
  {
    ToClient,
    ToServer,
  }
}
