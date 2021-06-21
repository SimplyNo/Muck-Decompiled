﻿// Decompiled with JetBrains decompiler
// Type: ClientSend
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using Steamworks;
using System;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
  public static int packetsSent;
  public static int bytesSent;

  private static void SendTCPData(Packet packet)
  {
    ClientSend.bytesSent += packet.Length();
    ++ClientSend.packetsSent;
    packet.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
      LocalClient.instance.tcp.SendData(packet);
    else
      SteamPacketManager.SendPacket(SteamId.op_Implicit((ulong) LocalClient.instance.serverHost.Value), packet, (P2PSend) 2, SteamPacketManager.NetworkChannel.ToServer);
  }

  private static void SendUDPData(Packet packet)
  {
    ClientSend.bytesSent += packet.Length();
    ++ClientSend.packetsSent;
    packet.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
      LocalClient.instance.udp.SendData(packet);
    else
      SteamPacketManager.SendPacket(SteamId.op_Implicit((ulong) LocalClient.instance.serverHost.Value), packet, (P2PSend) 0, SteamPacketManager.NetworkChannel.ToServer);
  }

  public static void JoinLobby()
  {
    using (Packet packet = new Packet(2))
    {
      packet.Write(SteamClient.get_Name());
      ClientSend.SendTCPData(packet);
    }
  }

  public static void StartedLoading()
  {
    using (Packet packet = new Packet(33))
      ClientSend.SendTCPData(packet);
  }

  public static void PlayerFinishedLoading()
  {
    using (Packet packet = new Packet(29))
      ClientSend.SendTCPData(packet);
  }

  public static void WelcomeReceived(int id, string username)
  {
    using (Packet packet = new Packet(1))
    {
      packet.Write(id);
      packet.Write(username);
      Color blue = Color.get_blue();
      packet.Write((float) blue.r);
      packet.Write((float) blue.g);
      packet.Write((float) blue.b);
      ClientSend.SendTCPData(packet);
    }
  }

  public static void PlayerPosition(Vector3 pos)
  {
    try
    {
      using (Packet packet = new Packet(3))
      {
        packet.Write(pos);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerHp(int hp, int maxHp)
  {
    try
    {
      using (Packet packet = new Packet(26))
      {
        packet.Write(hp);
        packet.Write(maxHp);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerDied()
  {
    try
    {
      using (Packet packet = new Packet(27))
        ClientSend.SendTCPData(packet);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void RevivePlayer(int revivePlayerId, int objectId = -1, bool grave = false)
  {
    try
    {
      using (Packet packet = new Packet(31))
      {
        packet.Write(revivePlayerId);
        packet.Write(grave);
        packet.Write(objectId);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerDied(int hp)
  {
    try
    {
      using (Packet packet = new Packet(26))
      {
        packet.Write(hp);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerRotation(float yOrientation, float xOrientation)
  {
    try
    {
      using (Packet packet = new Packet(4))
      {
        packet.Write(yOrientation);
        packet.Write(xOrientation);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerKilled(Vector3 position, int killedID)
  {
    try
    {
      using (Packet packet = new Packet(7))
      {
        MonoBehaviour.print((object) "sending killed info");
        packet.Write(position);
        packet.Write(killedID);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerHit(
    int damage,
    int hurtPlayer,
    float sharpness,
    int hitEffect,
    Vector3 pos)
  {
    try
    {
      using (Packet packet = new Packet(20))
      {
        packet.Write(damage);
        packet.Write(hurtPlayer);
        packet.Write(sharpness);
        packet.Write(hitEffect);
        packet.Write(pos);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void ShootArrow(Vector3 pos, Vector3 rot, float force, int projectileId)
  {
    try
    {
      using (Packet packet = new Packet(28))
      {
        packet.Write(pos);
        packet.Write(rot);
        packet.Write(force);
        packet.Write(projectileId);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void DropItem(int itemID, int amount)
  {
    try
    {
      using (Packet packet = new Packet(10))
      {
        MonoBehaviour.print((object) "sending drop item requesty");
        packet.Write(itemID);
        packet.Write(amount);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void DropItemAtPosition(int itemID, int amount, Vector3 pos)
  {
    try
    {
      using (Packet packet = new Packet(11))
      {
        packet.Write(itemID);
        packet.Write(amount);
        packet.Write(pos);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PickupItem(int itemID)
  {
    try
    {
      using (Packet packet = new Packet(12))
      {
        packet.Write(itemID);
        ClientSend.SendTCPData(packet);
        MonoBehaviour.print((object) ("sending pickup now from: " + (object) LocalClient.instance.myId));
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PickupInteract(int objectId)
  {
    try
    {
      using (Packet packet = new Packet(19))
      {
        packet.Write(objectId);
        ClientSend.SendTCPData(packet);
        MonoBehaviour.print((object) ("sending pickup interact now from: " + (object) LocalClient.instance.myId));
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerHitObject(int damage, int objectID, int hitEffect, Vector3 pos)
  {
    try
    {
      using (Packet packet = new Packet(14))
      {
        packet.Write(damage);
        packet.Write(objectID);
        packet.Write(hitEffect);
        packet.Write(pos);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void SpawnEffect(int effectId, Vector3 pos)
  {
    try
    {
      using (Packet packet = new Packet(30))
      {
        packet.Write(effectId);
        packet.Write(pos);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void WeaponInHand(int itemID)
  {
    try
    {
      using (Packet packet = new Packet(13))
      {
        packet.Write(itemID);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void SendArmor(int armorSlot, int itemId)
  {
    try
    {
      using (Packet packet = new Packet(25))
      {
        packet.Write(armorSlot);
        packet.Write(itemId);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void AnimationUpdate(OnlinePlayer.SharedAnimation animation, bool b)
  {
    try
    {
      using (Packet packet = new Packet(15))
      {
        int num = (int) animation;
        packet.Write(num);
        packet.Write(b);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void RequestBuild(int itemId, Vector3 pos, int yRot)
  {
    try
    {
      using (Packet packet = new Packet(16))
      {
        packet.Write(itemId);
        packet.Write(pos);
        packet.Write(yRot);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void RequestChest(int chestId, bool use)
  {
    try
    {
      using (Packet packet = new Packet(17))
      {
        packet.Write(chestId);
        packet.Write(use);
        MonoBehaviour.print((object) string.Format("sending new request to chest{0}, use: {1}", (object) chestId, (object) use));
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void ChestUpdate(int chestId, int cellId, int itemId, int amount)
  {
    ChestManager.Instance.SendChestUpdate(chestId, cellId);
    ChestManager.Instance.chests[chestId].UpdateCraftables();
    try
    {
      using (Packet packet = new Packet(18))
      {
        packet.Write(chestId);
        packet.Write(cellId);
        packet.Write(itemId);
        packet.Write(amount);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PingServer()
  {
    try
    {
      using (Packet packet = new Packet(6))
      {
        packet.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerDisconnect()
  {
    try
    {
      using (Packet packet = new Packet(5))
        ClientSend.SendTCPData(packet);
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void StartCombatShrine(int shrineId)
  {
    try
    {
      using (Packet packet = new Packet(22))
      {
        packet.Write(shrineId);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void Interact(int objectId)
  {
    try
    {
      using (Packet packet = new Packet(32))
      {
        packet.Write(objectId);
        ClientSend.SendTCPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerDamageMob(
    int mobId,
    int damage,
    float sharpness,
    int hitEffect,
    Vector3 pos)
  {
    try
    {
      using (Packet packet = new Packet(21))
      {
        packet.Write(mobId);
        packet.Write(damage);
        packet.Write(sharpness);
        packet.Write(hitEffect);
        packet.Write(pos);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void SendChatMessage(string msg)
  {
    try
    {
      using (Packet packet = new Packet(23))
      {
        packet.Write(msg);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public static void PlayerPing(Vector3 pos)
  {
    try
    {
      using (Packet packet = new Packet(24))
      {
        packet.Write(pos);
        ClientSend.SendUDPData(packet);
      }
    }
    catch (Exception ex)
    {
      Debug.Log((object) ex);
    }
  }

  public ClientSend() => base.\u002Ector();
}
