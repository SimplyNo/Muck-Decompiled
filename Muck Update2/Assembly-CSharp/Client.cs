// Decompiled with JetBrains decompiler
// Type: Client
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client
{
  public static int dataBufferSize = 4096;
  public int id;
  public Player player;
  public Client.TCP tcp;
  public Client.UDP udp;
  public bool inLobby;

  public Client(int clientId)
  {
    this.id = clientId;
    if (NetworkController.Instance.networkType != NetworkController.NetworkType.Classic)
      return;
    this.tcp = new Client.TCP(this.id);
    this.udp = new Client.UDP(this.id);
  }

  public void StartClient(string playerName, Color color) => this.player = new Player(this.id, playerName, color);

  public void StartClientSteam(string playerName, Color color, SteamId steamId) => this.player = new Player(this.id, playerName, color, steamId);

  public void SendIntoGame()
  {
  }

  public void Disconnect()
  {
    ServerSend.DisconnectPlayer(this.player.id);
    this.player = (Player) null;
    try
    {
      this.player = (Player) null;
      Debug.Log((object) string.Format("player{0} has disconnected", (object) this.id));
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("Handled an error in Client's disconnect method on server...'\n" + (object) ex));
    }
    try
    {
      this.tcp.Disconnect();
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("Handled an error in Client's disconnect method on tcp disconnect server...'\n" + (object) ex));
    }
    try
    {
      this.udp.Disconnect();
    }
    catch (Exception ex)
    {
      Debug.Log((object) ("Handled an error in Client's disconnect method on tcp disconnect server...'\n" + (object) ex));
    }
  }

  public class TCP
  {
    public TcpClient socket;
    public readonly int id;
    private NetworkStream stream;
    private Packet receivedData;
    private byte[] receiveBuffer;

    public TCP(int i) => this.id = i;

    public void Connect(TcpClient socket)
    {
      this.socket = socket;
      this.socket.ReceiveBufferSize = Client.dataBufferSize;
      this.socket.SendBufferSize = Client.dataBufferSize;
      this.stream = socket.GetStream();
      this.receivedData = new Packet();
      this.receiveBuffer = new byte[Client.dataBufferSize];
      this.stream.BeginRead(this.receiveBuffer, 0, Client.dataBufferSize, new AsyncCallback(this.ReceiveCallback), (object) null);
      ServerSend.Welcome(this.id, "Weclome to the server");
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
        Debug.Log((object) string.Format("Error sending data to player {0} via TCP: {1}", (object) this.id, (object) ex));
        throw;
      }
    }

    private void ReceiveCallback(IAsyncResult _result)
    {
      try
      {
        int length = this.stream.EndRead(_result);
        if (length <= 0)
        {
          Server.clients[this.id].Disconnect();
        }
        else
        {
          byte[] data = new byte[length];
          Array.Copy((Array) this.receiveBuffer, (Array) data, length);
          this.receivedData.Reset(this.HandleData(data));
          this.stream.BeginRead(this.receiveBuffer, 0, Client.dataBufferSize, new AsyncCallback(this.ReceiveCallback), (object) null);
        }
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Error receiving TCP data: " + (object) ex));
        Server.clients[this.id].Disconnect();
      }
    }

    private bool HandleData(byte[] data)
    {
      int _length = 0;
      this.receivedData.SetBytes(data);
      if (this.receivedData.UnreadLength() >= 4)
      {
        _length = this.receivedData.ReadInt();
        if (_length <= 0)
          return true;
      }
      while (_length > 0 && _length <= this.receivedData.UnreadLength())
      {
        byte[] packetBytes = this.receivedData.ReadBytes(_length);
        ThreadManagerServer.ExecuteOnMainThread((Action) (() =>
        {
          using (Packet packet = new Packet(packetBytes))
          {
            int key = packet.ReadInt();
            Server.PacketHandlers[key](this.id, packet);
          }
        }));
        _length = 0;
        if (this.receivedData.UnreadLength() >= 4)
        {
          _length = this.receivedData.ReadInt();
          if (_length <= 0)
            return true;
        }
      }
      return _length <= 1;
    }

    public void Disconnect()
    {
      this.socket.Close();
      this.stream = (NetworkStream) null;
      this.receivedData = (Packet) null;
      this.receiveBuffer = (byte[]) null;
      this.socket = (TcpClient) null;
    }
  }

  public class UDP
  {
    public IPEndPoint endPoint;
    private int id;

    public UDP(int id) => this.id = id;

    public void Connect(IPEndPoint endPoint) => this.endPoint = endPoint;

    public void SendData(Packet packet) => Server.SendUDPData(this.endPoint, packet);

    public void HandleData(Packet packetData)
    {
      int _length = packetData.ReadInt();
      byte[] packetBytes = packetData.ReadBytes(_length);
      ThreadManagerServer.ExecuteOnMainThread((Action) (() =>
      {
        using (Packet packet = new Packet(packetBytes))
        {
          int key = packet.ReadInt();
          Server.PacketHandlers[key](this.id, packet);
        }
      }));
    }

    public void Disconnect() => this.endPoint = (IPEndPoint) null;
  }
}
