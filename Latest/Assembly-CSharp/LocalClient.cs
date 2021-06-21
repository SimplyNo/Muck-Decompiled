// Decompiled with JetBrains decompiler
// Type: LocalClient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class LocalClient : MonoBehaviour
{
  public static LocalClient instance;
  public static int dataBufferSize = 4096;
  public SteamId serverHost;
  public string ip;
  public int port;
  public int myId;
  public LocalClient.TCP tcp;
  public LocalClient.UDP udp;
  public static bool serverOwner;
  private bool isConnected;
  public static Dictionary<int, LocalClient.PacketHandler> packetHandlers;
  public static int byteDown;
  public static int packetsReceived;

  private void Awake()
  {
    if (Object.op_Equality((Object) LocalClient.instance, (Object) null))
    {
      LocalClient.instance = this;
    }
    else
    {
      if (!Object.op_Inequality((Object) LocalClient.instance, (Object) this))
        return;
      Debug.Log((object) "Instance already exists, destroying object");
      Object.Destroy((Object) this);
    }
  }

  private void Start() => this.StartProtocols();

  private void StartProtocols()
  {
    this.tcp = new LocalClient.TCP();
    this.udp = new LocalClient.UDP();
  }

  public void ConnectToServer(string ip, string username)
  {
    this.ip = ip;
    this.StartProtocols();
    LocalClient.InitializeClientData();
    this.isConnected = true;
    this.tcp.Connect();
  }

  public static void InitializeClientData()
  {
    LocalClient.packetHandlers = new Dictionary<int, LocalClient.PacketHandler>()
    {
      {
        1,
        new LocalClient.PacketHandler(ClientHandle.Welcome)
      },
      {
        2,
        new LocalClient.PacketHandler(ClientHandle.SpawnPlayer)
      },
      {
        3,
        new LocalClient.PacketHandler(ClientHandle.PlayerPosition)
      },
      {
        4,
        new LocalClient.PacketHandler(ClientHandle.PlayerRotation)
      },
      {
        7,
        new LocalClient.PacketHandler(ClientHandle.ReceivePing)
      },
      {
        10,
        new LocalClient.PacketHandler(ClientHandle.ReceiveStatus)
      },
      {
        13,
        new LocalClient.PacketHandler(ClientHandle.Clock)
      },
      {
        50,
        new LocalClient.PacketHandler(ClientHandle.PlayerFinishedLoading)
      },
      {
        8,
        new LocalClient.PacketHandler(ClientHandle.ConnectionEstablished)
      },
      {
        11,
        new LocalClient.PacketHandler(ClientHandle.GameOver)
      },
      {
        5,
        new LocalClient.PacketHandler(ClientHandle.DisconnectPlayer)
      },
      {
        6,
        new LocalClient.PacketHandler(ClientHandle.PlayerDied)
      },
      {
        52,
        new LocalClient.PacketHandler(ClientHandle.SpawnGrave)
      },
      {
        15,
        new LocalClient.PacketHandler(ClientHandle.Ready)
      },
      {
        12,
        new LocalClient.PacketHandler(ClientHandle.StartGame)
      },
      {
        14,
        new LocalClient.PacketHandler(ClientHandle.OpenDoor)
      },
      {
        17,
        new LocalClient.PacketHandler(ClientHandle.DropItem)
      },
      {
        21,
        new LocalClient.PacketHandler(ClientHandle.DropResources)
      },
      {
        18,
        new LocalClient.PacketHandler(ClientHandle.PickupItem)
      },
      {
        49,
        new LocalClient.PacketHandler(ClientHandle.SpawnEffect)
      },
      {
        19,
        new LocalClient.PacketHandler(ClientHandle.WeaponInHand)
      },
      {
        20,
        new LocalClient.PacketHandler(ClientHandle.PlayerHitObject)
      },
      {
        45,
        new LocalClient.PacketHandler(ClientHandle.RemoveResource)
      },
      {
        42,
        new LocalClient.PacketHandler(ClientHandle.PlayerHp)
      },
      {
        43,
        new LocalClient.PacketHandler(ClientHandle.RespawnPlayer)
      },
      {
        28,
        new LocalClient.PacketHandler(ClientHandle.PlayerHit)
      },
      {
        22,
        new LocalClient.PacketHandler(ClientHandle.AnimationUpdate)
      },
      {
        44,
        new LocalClient.PacketHandler(ClientHandle.ShootArrowFromPlayer)
      },
      {
        23,
        new LocalClient.PacketHandler(ClientHandle.FinalizeBuild)
      },
      {
        24,
        new LocalClient.PacketHandler(ClientHandle.OpenChest)
      },
      {
        25,
        new LocalClient.PacketHandler(ClientHandle.UpdateChest)
      },
      {
        26,
        new LocalClient.PacketHandler(ClientHandle.PickupInteract)
      },
      {
        27,
        new LocalClient.PacketHandler(ClientHandle.DropItemAtPosition)
      },
      {
        35,
        new LocalClient.PacketHandler(ClientHandle.DropPowerupAtPosition)
      },
      {
        29,
        new LocalClient.PacketHandler(ClientHandle.MobSpawn)
      },
      {
        30,
        new LocalClient.PacketHandler(ClientHandle.MobMove)
      },
      {
        31,
        new LocalClient.PacketHandler(ClientHandle.MobSetDestination)
      },
      {
        54,
        new LocalClient.PacketHandler(ClientHandle.MobSetTarget)
      },
      {
        32,
        new LocalClient.PacketHandler(ClientHandle.MobAttack)
      },
      {
        46,
        new LocalClient.PacketHandler(ClientHandle.MobSpawnProjectile)
      },
      {
        33,
        new LocalClient.PacketHandler(ClientHandle.PlayerDamageMob)
      },
      {
        48,
        new LocalClient.PacketHandler(ClientHandle.KnockbackMob)
      },
      {
        53,
        new LocalClient.PacketHandler(ClientHandle.Interact)
      },
      {
        34,
        new LocalClient.PacketHandler(ClientHandle.ShrineCombatStart)
      },
      {
        51,
        new LocalClient.PacketHandler(ClientHandle.RevivePlayer)
      },
      {
        37,
        new LocalClient.PacketHandler(ClientHandle.MobZoneToggle)
      },
      {
        36,
        new LocalClient.PacketHandler(ClientHandle.MobZoneSpawn)
      },
      {
        38,
        new LocalClient.PacketHandler(ClientHandle.PickupSpawnZone)
      },
      {
        39,
        new LocalClient.PacketHandler(ClientHandle.ReceiveChatMessage)
      },
      {
        40,
        new LocalClient.PacketHandler(ClientHandle.ReceivePlayerPing)
      },
      {
        41,
        new LocalClient.PacketHandler(ClientHandle.ReceivePlayerArmor)
      },
      {
        47,
        new LocalClient.PacketHandler(ClientHandle.NewDay)
      }
    };
    Debug.Log((object) "Initializing packets.");
  }

  private void OnApplicationQuit() => this.Disconnect();

  public void Disconnect()
  {
    if (!this.isConnected)
      return;
    ClientSend.PlayerDisconnect();
    this.isConnected = false;
    this.tcp.socket.Close();
    this.udp.socket.Close();
    Debug.Log((object) "Disconnected from server.");
  }

  public LocalClient() => base.\u002Ector();

  public delegate void PacketHandler(Packet packet);

  public class TCP
  {
    public TcpClient socket;
    private NetworkStream stream;
    private Packet receivedData;
    private byte[] receiveBuffer;

    public void Connect()
    {
      this.socket = new TcpClient()
      {
        ReceiveBufferSize = LocalClient.dataBufferSize,
        SendBufferSize = LocalClient.dataBufferSize
      };
      this.receiveBuffer = new byte[LocalClient.dataBufferSize];
      this.socket.BeginConnect(LocalClient.instance.ip, LocalClient.instance.port, new AsyncCallback(this.ConnectCallback), (object) this.socket);
    }

    private void ConnectCallback(IAsyncResult result)
    {
      this.socket.EndConnect(result);
      if (!this.socket.Connected)
        return;
      this.stream = this.socket.GetStream();
      this.receivedData = new Packet();
      this.stream.BeginRead(this.receiveBuffer, 0, LocalClient.dataBufferSize, new AsyncCallback(this.ReceiveCallback), (object) null);
    }

    public void SendData(Packet packet)
    {
      try
      {
        if (this.socket == null)
          return;
        this.stream.BeginWrite(packet.ToArray(), 0, packet.Length(), (AsyncCallback) null, (object) null);
      }
      catch (Exception ex)
      {
        Debug.Log((object) string.Format("Error sending data to server via TCP: {0}", (object) ex));
      }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
      try
      {
        int length = this.stream.EndRead(result);
        if (length <= 0)
        {
          LocalClient.instance.Disconnect();
        }
        else
        {
          byte[] data = new byte[length];
          Array.Copy((Array) this.receiveBuffer, (Array) data, length);
          this.receivedData.Reset(this.HandleData(data));
          this.stream.BeginRead(this.receiveBuffer, 0, LocalClient.dataBufferSize, new AsyncCallback(this.ReceiveCallback), (object) null);
        }
      }
      catch
      {
        this.Disconnect();
      }
    }

    private bool HandleData(byte[] data)
    {
      ++LocalClient.packetsReceived;
      int packetLength = 0;
      this.receivedData.SetBytes(data);
      if (this.receivedData.UnreadLength() >= 4)
      {
        packetLength = this.receivedData.ReadInt();
        if (packetLength <= 0)
          return true;
      }
      while (packetLength > 0 && packetLength <= this.receivedData.UnreadLength())
      {
        byte[] packetBytes = this.receivedData.ReadBytes(packetLength);
        ThreadManagerClient.ExecuteOnMainThread((Action) (() =>
        {
          using (Packet packet = new Packet(packetBytes))
          {
            int key = packet.ReadInt();
            LocalClient.byteDown += packetLength;
            Debug.Log((object) ("received packet: " + (object) (ServerPackets) key));
            LocalClient.packetHandlers[key](packet);
          }
        }));
        packetLength = 0;
        if (this.receivedData.UnreadLength() >= 4)
        {
          packetLength = this.receivedData.ReadInt();
          if (packetLength <= 0)
            return true;
        }
      }
      return packetLength <= 1;
    }

    private void Disconnect()
    {
      LocalClient.instance.Disconnect();
      this.stream = (NetworkStream) null;
      this.receivedData = (Packet) null;
      this.receiveBuffer = (byte[]) null;
      this.socket = (TcpClient) null;
    }
  }

  public class UDP
  {
    public UdpClient socket;
    public IPEndPoint endPoint;

    public UDP() => this.endPoint = new IPEndPoint(IPAddress.Parse(LocalClient.instance.ip), LocalClient.instance.port);

    public void Connect(int localPort)
    {
      this.socket = new UdpClient(localPort);
      this.socket.Connect(this.endPoint);
      this.socket.BeginReceive(new AsyncCallback(this.ReceiveCallback), (object) null);
      using (Packet packet = new Packet())
        this.SendData(packet);
    }

    public void SendData(Packet packet)
    {
      try
      {
        packet.InsertInt(LocalClient.instance.myId);
        if (this.socket == null)
          return;
        this.socket.BeginSend(packet.ToArray(), packet.Length(), (AsyncCallback) null, (object) null);
      }
      catch (Exception ex)
      {
        Debug.Log((object) string.Format("Error sending data to server via UDP: {0}", (object) ex));
      }
    }

    private void ReceiveCallback(IAsyncResult result)
    {
      try
      {
        byte[] data = this.socket.EndReceive(result, ref this.endPoint);
        this.socket.BeginReceive(new AsyncCallback(this.ReceiveCallback), (object) null);
        if (data.Length < 4)
        {
          LocalClient.instance.Disconnect();
          Debug.Log((object) "UDP failed due to packets being split, in Client class");
        }
        else
          this.HandleData(data);
      }
      catch
      {
        this.Disconnect();
      }
    }

    private void HandleData(byte[] data)
    {
      ++LocalClient.packetsReceived;
      using (Packet packet = new Packet(data))
      {
        int _length = packet.ReadInt();
        LocalClient.byteDown += _length;
        data = packet.ReadBytes(_length);
      }
      ThreadManagerClient.ExecuteOnMainThread((Action) (() =>
      {
        using (Packet packet = new Packet(data))
        {
          int key = packet.ReadInt();
          LocalClient.packetHandlers[key](packet);
        }
      }));
    }

    private void Disconnect()
    {
      LocalClient.instance.Disconnect();
      this.endPoint = (IPEndPoint) null;
      this.socket = (UdpClient) null;
    }
  }
}
