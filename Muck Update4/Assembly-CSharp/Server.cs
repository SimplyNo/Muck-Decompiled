// Decompiled with JetBrains decompiler
// Type: Server
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
  public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
  public static Dictionary<int, Server.PacketHandler> PacketHandlers;
  public static int idCounter;
  private static TcpListener tcpListener;
  private static UdpClient udpListener;
  public static IPAddress ipAddress = IPAddress.Any;

  public static int MaxPlayers { get; private set; }

  public static int Port { get; private set; }

  public static int GetNextId()
  {
    int idCounter = Server.idCounter;
    ++Server.idCounter;
    return idCounter;
  }

  public static void Start(int maxPlayers, int port)
  {
    Server.MaxPlayers = maxPlayers;
    Server.Port = port;
    Debug.Log((object) "Starting server.. ver.0.7");
    Server.InitializeServerData();
    Server.tcpListener = new TcpListener(Server.ipAddress, Server.Port);
    Debug.Log((object) string.Format("TclpListener on IP: {0}.", (object) Server.ipAddress));
    Server.tcpListener.Start();
    Server.tcpListener.BeginAcceptTcpClient(new AsyncCallback(Server.TCPConnectCallback), (object) null);
    Server.udpListener = new UdpClient(Server.Port);
    Server.udpListener.BeginReceive(new AsyncCallback(Server.UDPReceiveCallback), (object) null);
    Debug.Log((object) ("Server started on port:" + (object) Server.Port));
    ThreadManagerServer.Instance.ResetGame();
  }

  private static void TCPConnectCallback(IAsyncResult result)
  {
    TcpClient socket = Server.tcpListener.EndAcceptTcpClient(result);
    Server.tcpListener.BeginAcceptTcpClient(new AsyncCallback(Server.TCPConnectCallback), (object) null);
    Debug.Log((object) string.Format("Incoming connection from {0}...", (object) socket.Client.RemoteEndPoint));
    for (int key = 1; key <= Server.MaxPlayers; ++key)
    {
      if (Server.clients[key].tcp.socket == null)
      {
        Server.clients[key].tcp.Connect(socket);
        return;
      }
    }
    Debug.Log((object) string.Format("{0} failed to connect: Server full! f", (object) socket.Client.RemoteEndPoint));
  }

  private static void UDPReceiveCallback(IAsyncResult result)
  {
    try
    {
      IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
      byte[] _data = Server.udpListener.EndReceive(result, ref remoteEP);
      Server.udpListener.BeginReceive(new AsyncCallback(Server.UDPReceiveCallback), (object) null);
      if (_data.Length < 4)
        return;
      using (Packet packetData = new Packet(_data))
      {
        int key = packetData.ReadInt();
        if (key == 0)
          return;
        if (Server.clients[key].udp.endPoint == null)
        {
          Server.clients[key].udp.Connect(remoteEP);
        }
        else
        {
          if (!(Server.clients[key].udp.endPoint.ToString() == remoteEP.ToString()))
            return;
          Server.clients[key].udp.HandleData(packetData);
        }
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) string.Format("Catching error receiving UDP data: {0}", (object) ex));
      Debug.Log((object) "This error message can be ignored if just closing server. Server has been closed successfully.");
    }
  }

  public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
  {
    try
    {
      if (clientEndPoint == null)
        return;
      Server.udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, (AsyncCallback) null, (object) null);
    }
    catch (Exception ex)
    {
      Debug.Log((object) string.Format("Error sending data to {0} via UDP: {1}.", (object) clientEndPoint, (object) ex));
    }
  }

  public static void InitializeServerData()
  {
    for (int index = 1; index <= Server.MaxPlayers; ++index)
      Server.clients.Add(index, new Client(index));
    Server.InitializeServerPackets();
    Debug.Log((object) "Initialized Packets.");
  }

  public static void InitializeServerPackets() => Server.PacketHandlers = new Dictionary<int, Server.PacketHandler>()
  {
    {
      1,
      new Server.PacketHandler(ServerHandle.WelcomeReceived)
    },
    {
      2,
      new Server.PacketHandler(ServerHandle.JoinRequest)
    },
    {
      33,
      new Server.PacketHandler(ServerHandle.StartedLoading)
    },
    {
      29,
      new Server.PacketHandler(ServerHandle.PlayerFinishedLoading)
    },
    {
      5,
      new Server.PacketHandler(ServerHandle.PlayerDisconnect)
    },
    {
      3,
      new Server.PacketHandler(ServerHandle.PlayerPosition)
    },
    {
      26,
      new Server.PacketHandler(ServerHandle.PlayerHp)
    },
    {
      27,
      new Server.PacketHandler(ServerHandle.PlayerDied)
    },
    {
      31,
      new Server.PacketHandler(ServerHandle.RevivePlayer)
    },
    {
      4,
      new Server.PacketHandler(ServerHandle.PlayerRotation)
    },
    {
      6,
      new Server.PacketHandler(ServerHandle.PingReceived)
    },
    {
      9,
      new Server.PacketHandler(ServerHandle.PlayerRequestedSpawns)
    },
    {
      8,
      new Server.PacketHandler(ServerHandle.Ready)
    },
    {
      10,
      new Server.PacketHandler(ServerHandle.ItemDropped)
    },
    {
      11,
      new Server.PacketHandler(ServerHandle.ItemDroppedAtPosition)
    },
    {
      12,
      new Server.PacketHandler(ServerHandle.ItemPickedUp)
    },
    {
      13,
      new Server.PacketHandler(ServerHandle.WeaponInHand)
    },
    {
      15,
      new Server.PacketHandler(ServerHandle.AnimationUpdate)
    },
    {
      28,
      new Server.PacketHandler(ServerHandle.ShootArrow)
    },
    {
      14,
      new Server.PacketHandler(ServerHandle.PlayerHitObject)
    },
    {
      30,
      new Server.PacketHandler(ServerHandle.SpawnEffect)
    },
    {
      20,
      new Server.PacketHandler(ServerHandle.PlayerHit)
    },
    {
      16,
      new Server.PacketHandler(ServerHandle.RequestBuild)
    },
    {
      17,
      new Server.PacketHandler(ServerHandle.RequestChest)
    },
    {
      18,
      new Server.PacketHandler(ServerHandle.UpdateChest)
    },
    {
      19,
      new Server.PacketHandler(ServerHandle.ItemInteract)
    },
    {
      21,
      new Server.PacketHandler(ServerHandle.PlayerDamageMob)
    },
    {
      22,
      new Server.PacketHandler(ServerHandle.ShrineCombatStartRequest)
    },
    {
      32,
      new Server.PacketHandler(ServerHandle.Interact)
    },
    {
      23,
      new Server.PacketHandler(ServerHandle.ReceiveChatMessage)
    },
    {
      24,
      new Server.PacketHandler(ServerHandle.ReceivePing)
    },
    {
      25,
      new Server.PacketHandler(ServerHandle.ReceiveArmor)
    },
    {
      34,
      new Server.PacketHandler(ServerHandle.ReceiveShipUpdate)
    }
  };

  public static void Stop()
  {
    Server.tcpListener.Stop();
    Server.udpListener.Close();
  }

  public delegate void PacketHandler(int fromClient, Packet packet);
}
