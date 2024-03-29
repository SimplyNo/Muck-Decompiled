﻿// Decompiled with JetBrains decompiler
// Type: ServerSend
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
  private static P2PSend TCPvariant = P2PSend.Reliable;
  private static P2PSend UDPVariant = P2PSend.Unreliable;

  private static void SendTCPData(int toClient, Packet packet)
  {
    Packet packet1 = new Packet();
    packet1.SetBytes(packet.CloneBytes());
    packet1.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
      Server.clients[toClient].tcp.SendData(packet1);
    else
      SteamPacketManager.SendPacket((SteamId) Server.clients[toClient].player.steamId.Value, packet1, ServerSend.TCPvariant, SteamPacketManager.NetworkChannel.ToClient);
  }

  private static void SendUDPData(int toClient, Packet packet)
  {
    Packet packet1 = new Packet();
    packet1.SetBytes(packet.CloneBytes());
    packet1.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
      Server.clients[toClient].udp.SendData(packet1);
    else
      SteamPacketManager.SendPacket((SteamId) Server.clients[toClient].player.steamId.Value, packet1, ServerSend.UDPVariant, SteamPacketManager.NetworkChannel.ToClient);
  }

  private static void SendTCPDataToAll(Packet packet)
  {
    packet.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
    {
      for (int key = 1; key < Server.MaxPlayers; ++key)
        Server.clients[key].tcp.SendData(packet);
    }
    else
    {
      foreach (Client client in Server.clients.Values)
      {
        if (client?.player != null)
          SteamPacketManager.SendPacket((SteamId) client.player.steamId.Value, packet, ServerSend.TCPvariant, SteamPacketManager.NetworkChannel.ToClient);
      }
    }
  }

  private static void SendTCPDataToAll(int exceptClient, Packet packet)
  {
    packet.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
    {
      for (int key = 1; key < Server.MaxPlayers; ++key)
      {
        if (key != exceptClient)
          Server.clients[key].tcp.SendData(packet);
      }
    }
    else
    {
      foreach (Client client in Server.clients.Values)
      {
        if (client?.player != null && SteamLobby.steamIdToClientId[client.player.steamId.Value] != exceptClient)
          SteamPacketManager.SendPacket((SteamId) client.player.steamId.Value, packet, ServerSend.TCPvariant, SteamPacketManager.NetworkChannel.ToClient);
      }
    }
  }

  public static void SendTCPDataToSteamId(SteamId steamId, Packet packet)
  {
    Packet p = new Packet();
    p.SetBytes(packet.CloneBytes());
    p.WriteLength();
    SteamPacketManager.SendPacket(steamId, p, ServerSend.TCPvariant, SteamPacketManager.NetworkChannel.ToClient);
  }

  private static void SendTCPDataToAll(int[] exceptClients, Packet packet)
  {
    packet.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
    {
      for (int key = 1; key < Server.MaxPlayers; ++key)
      {
        bool flag = false;
        foreach (int exceptClient in exceptClients)
        {
          if (key == exceptClient)
            flag = true;
        }
        if (!flag)
          Server.clients[key].tcp.SendData(packet);
      }
    }
    else
    {
      foreach (Client client in Server.clients.Values)
      {
        if (client?.player != null)
        {
          bool flag = false;
          foreach (int exceptClient in exceptClients)
          {
            if (SteamLobby.steamIdToClientId[client.player.steamId.Value] == exceptClient)
              flag = true;
          }
          if (!flag)
            SteamPacketManager.SendPacket((SteamId) client.player.steamId.Value, packet, ServerSend.TCPvariant, SteamPacketManager.NetworkChannel.ToClient);
        }
      }
    }
  }

  private static void SendUDPDataToAll(Packet packet)
  {
    packet.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
    {
      for (int key = 1; key < Server.MaxPlayers; ++key)
        Server.clients[key].udp.SendData(packet);
    }
    else
    {
      foreach (Client client in Server.clients.Values)
      {
        if (client?.player != null)
          SteamPacketManager.SendPacket((SteamId) client.player.steamId.Value, packet, ServerSend.UDPVariant, SteamPacketManager.NetworkChannel.ToClient);
      }
    }
  }

  private static void SendUDPDataToAll(int exceptClient, Packet packet)
  {
    packet.WriteLength();
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
    {
      for (int key = 1; key < Server.MaxPlayers; ++key)
      {
        if (key != exceptClient)
          Server.clients[key].udp.SendData(packet);
      }
    }
    else
    {
      foreach (Client client in Server.clients.Values)
      {
        if (client?.player != null && SteamLobby.steamIdToClientId[client.player.steamId.Value] != exceptClient)
          SteamPacketManager.SendPacket((SteamId) client.player.steamId.Value, packet, ServerSend.UDPVariant, SteamPacketManager.NetworkChannel.ToClient);
      }
    }
  }

  public static void Welcome(int toClient, string msg)
  {
    using (Packet packet = new Packet(1))
    {
      packet.Write(msg);
      packet.Write(NetworkManager.Clock);
      packet.Write(toClient);
      ServerSend.SendTCPData(toClient, packet);
    }
  }

  public static void StartGame(int playerLobbyId, GameSettings settings)
  {
    using (Packet packet = new Packet(13))
    {
      packet.Write(playerLobbyId);
      packet.Write(settings.Seed);
      packet.Write((int) settings.gameMode);
      packet.Write((int) settings.friendlyFire);
      packet.Write((int) settings.difficulty);
      packet.Write((int) settings.gameLength);
      packet.Write((int) settings.multiplayer);
      List<Player> playerList = new List<Player>();
      for (int key = 0; key < Server.clients.Values.Count; ++key)
      {
        if (Server.clients[key] != null && Server.clients[key].player != null)
          playerList.Add(Server.clients[key].player);
      }
      packet.Write(playerList.Count);
      foreach (Player player in playerList)
      {
        packet.Write(player.id);
        packet.Write(player.username);
      }
      Debug.Log((object) "Sending start game packet");
      ServerSend.SendTCPData(playerLobbyId, packet);
    }
  }

  public static void ConnectionSuccessful(int toClient)
  {
    using (Packet packet = new Packet(9))
      ServerSend.SendTCPData(toClient, packet);
  }

  public static void PlayerDied(
    int deadPlayerId,
    Vector3 deathPos,
    Vector3 gravePos,
    int damageFromPlayer)
  {
    using (Packet packet = new Packet(7))
    {
      Debug.Log((object) ("Player" + (object) deadPlayerId + " has been killed, sending to players"));
      packet.Write(deadPlayerId);
      packet.Write(gravePos);
      packet.Write(damageFromPlayer);
      ServerSend.SendTCPDataToAll(deadPlayerId, packet);
    }
    if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus || GameManager.instance.boatLeft)
      return;
    using (Packet packet = new Packet(53))
    {
      int nextId = ResourceManager.Instance.GetNextId();
      packet.Write(deadPlayerId);
      packet.Write(nextId);
      packet.Write(gravePos);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void RespawnPlayer(int respawnId)
  {
    using (Packet packet = new Packet(44))
    {
      packet.Write(respawnId);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void RevivePlayer(int fromClient, int revivedId, bool shrine, int objectID)
  {
    using (Packet packet = new Packet(52))
    {
      packet.Write(fromClient);
      packet.Write(revivedId);
      packet.Write(shrine);
      packet.Write(objectID);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void PlayerReady(int fromClient, bool ready)
  {
    using (Packet packet = new Packet(16))
    {
      packet.Write(fromClient);
      packet.Write(ready);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void PlayerReady(int fromClient, bool ready, int toClient)
  {
    using (Packet packet = new Packet(16))
    {
      packet.Write(fromClient);
      packet.Write(ready);
      ServerSend.SendTCPData(toClient, packet);
    }
  }

  public static void DropItem(int fromClient, int itemId, int amount, int objectID)
  {
    using (Packet packet = new Packet(18))
    {
      packet.Write(fromClient);
      packet.Write(itemId);
      packet.Write(amount);
      packet.Write(objectID);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void DropItemAtPosition(int itemId, int amount, int objectID, Vector3 pos)
  {
    using (Packet packet = new Packet(28))
    {
      packet.Write(itemId);
      packet.Write(amount);
      packet.Write(objectID);
      packet.Write(pos);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void DropPowerupAtPosition(int itemId, int objectID, Vector3 pos)
  {
    using (Packet packet = new Packet(36))
    {
      packet.Write(itemId);
      packet.Write(objectID);
      packet.Write(pos);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void DropResources(int fromClient, int dropTableId, int droppedItemID)
  {
    using (Packet packet = new Packet(22))
    {
      packet.Write(fromClient);
      packet.Write(dropTableId);
      packet.Write(droppedItemID);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void PickupItem(int fromClient, int objectID)
  {
    using (Packet packet = new Packet(19))
    {
      packet.Write(fromClient);
      packet.Write(objectID);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void PickupInteract(int fromClient, int objectID)
  {
    using (Packet packet = new Packet(27))
    {
      packet.Write(fromClient);
      packet.Write(objectID);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void WeaponInHand(int fromClient, int objectID)
  {
    using (Packet packet = new Packet(20))
    {
      packet.Write(fromClient);
      packet.Write(objectID);
      ServerSend.SendTCPDataToAll(fromClient, packet);
    }
  }

  public static void SendBuild(
    int fromClient,
    int itemId,
    int newObjectId,
    Vector3 pos,
    int yRot)
  {
    using (Packet packet = new Packet(24))
    {
      packet.Write(fromClient);
      packet.Write(itemId);
      packet.Write(newObjectId);
      packet.Write(pos);
      packet.Write(yRot);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void AnimationUpdate(int fromClient, int animation, bool b)
  {
    using (Packet packet = new Packet(23))
    {
      packet.Write(fromClient);
      packet.Write(animation);
      packet.Write(b);
      ServerSend.SendUDPDataToAll(fromClient, packet);
    }
  }

  public static void ShootArrow(Vector3 pos, Vector3 rot, float force, int arrowId, int playerId)
  {
    using (Packet packet = new Packet(45))
    {
      packet.Write(pos);
      packet.Write(rot);
      packet.Write(force);
      packet.Write(arrowId);
      packet.Write(playerId);
      ServerSend.SendTCPDataToAll(playerId, packet);
    }
  }

  public static void OpenChest(int fromClient, int chestId, bool use)
  {
    using (Packet packet = new Packet(25))
    {
      packet.Write(fromClient);
      packet.Write(chestId);
      packet.Write(use);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void UpdateChest(int fromClient, int chestId, int cellId, int itemId, int amount)
  {
    using (Packet packet = new Packet(26))
    {
      packet.Write(fromClient);
      packet.Write(chestId);
      packet.Write(cellId);
      packet.Write(itemId);
      packet.Write(amount);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void PlayerHitObject(
    int fromClient,
    int objectID,
    int hp,
    int hitEffect,
    Vector3 pos,
    int weaponHitType)
  {
    using (Packet packet = new Packet(21))
    {
      packet.Write(fromClient);
      packet.Write(objectID);
      packet.Write(hp);
      packet.Write(hitEffect);
      packet.Write(pos);
      packet.Write(weaponHitType);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void SpawnEffect(int effectId, Vector3 pos, int fromClient)
  {
    using (Packet packet = new Packet(50))
    {
      packet.Write(effectId);
      packet.Write(pos);
      ServerSend.SendUDPDataToAll(fromClient, packet);
    }
  }

  public static void HitPlayer(
    int fromClient,
    int damage,
    float hpRatioEstimate,
    int hurtPlayerId,
    int hitEffect,
    Vector3 pos)
  {
    using (Packet packet = new Packet(29))
    {
      packet.Write(fromClient);
      packet.Write(damage);
      packet.Write(hpRatioEstimate);
      packet.Write(hurtPlayerId);
      packet.Write(hitEffect);
      packet.Write(pos);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void SpawnPlayer(int toClient, Player player, Vector3 pos)
  {
    using (Packet packet = new Packet(2))
    {
      Debug.Log((object) ("spawning player, id: " + (object) player.id + ", sending to " + (object) toClient));
      packet.Write(player.id);
      packet.Write(player.username);
      Vector3 vector3 = new Vector3(player.color.r, player.color.g, player.color.b);
      packet.Write(vector3);
      player.pos = pos;
      packet.Write(pos);
      packet.Write(player.yOrientation);
      ServerSend.SendTCPData(toClient, packet);
    }
  }

  public static void PlayerHp(int fromId, float hpRatio)
  {
    using (Packet packet = new Packet(43))
    {
      packet.Write(fromId);
      packet.Write(hpRatio);
      ServerSend.SendUDPDataToAll(fromId, packet);
    }
  }

  public static void PlayerPosition(Player player, int t)
  {
    using (Packet packet = new Packet(3))
    {
      packet.Write(player.id);
      packet.Write(player.pos);
      ServerSend.SendUDPDataToAll(player.id, packet);
    }
  }

  public static void PlayerRotation(Player player)
  {
    using (Packet packet = new Packet(4))
    {
      packet.Write(player.id);
      packet.Write(player.yOrientation);
      packet.Write(player.xOrientation);
      ServerSend.SendUDPDataToAll(player.id, packet);
    }
  }

  public static void PingPlayer(int player, string ms)
  {
    using (Packet packet = new Packet(8))
    {
      packet.Write(player);
      packet.Write(ms);
      ServerSend.SendUDPData(player, packet);
    }
  }

  public static void DisconnectPlayer(int player)
  {
    using (Packet packet = new Packet(5))
    {
      packet.Write(player);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void ShrineStart(int[] mobIds, int shrineId)
  {
    using (Packet packet = new Packet(35))
    {
      packet.Write(shrineId);
      int length = mobIds.Length;
      packet.Write(length);
      for (int index = 0; index < length; ++index)
        packet.Write(mobIds[index]);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void MobMove(int mobId, Vector3 pos)
  {
    using (Packet packet = new Packet(31))
    {
      packet.Write(mobId);
      packet.Write(pos);
      ServerSend.SendUDPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void MobSetDestination(int mobId, Vector3 dest)
  {
    using (Packet packet = new Packet(32))
    {
      packet.Write(mobId);
      packet.Write(dest);
      ServerSend.SendUDPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void SendMobTarget(int mobId, int targetId)
  {
    using (Packet packet = new Packet(55))
    {
      packet.Write(mobId);
      packet.Write(targetId);
      ServerSend.SendUDPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void MobSpawn(
    Vector3 pos,
    int mobType,
    int mobId,
    float multiplier,
    float bossMultiplier,
    int guardianType)
  {
    using (Packet packet = new Packet(30))
    {
      packet.Write(pos);
      packet.Write(mobType);
      packet.Write(mobId);
      packet.Write(multiplier);
      packet.Write(bossMultiplier);
      packet.Write(guardianType);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void MobAttack(int mobId, int targetPlayerId, int attackAnimationIndex)
  {
    using (Packet packet = new Packet(33))
    {
      packet.Write(mobId);
      packet.Write(targetPlayerId);
      packet.Write(attackAnimationIndex);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void MobSpawnProjectile(
    Vector3 pos,
    Vector3 dir,
    float force,
    int itemId,
    int mobObjectId)
  {
    using (Packet packet = new Packet(47))
    {
      packet.Write(pos);
      packet.Write(dir);
      packet.Write(force);
      packet.Write(itemId);
      packet.Write(mobObjectId);
      ServerSend.SendUDPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void PlayerHitMob(
    int fromClient,
    int mobId,
    int hpLeft,
    int hitEffect,
    Vector3 pos,
    int hitWeaponType)
  {
    using (Packet packet = new Packet(34))
    {
      packet.Write(fromClient);
      packet.Write(mobId);
      packet.Write(hpLeft);
      packet.Write(hitEffect);
      packet.Write(pos);
      packet.Write(hitWeaponType);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void KnockbackMob(int mobId, Vector3 dir)
  {
    using (Packet packet = new Packet(49))
    {
      packet.Write(mobId);
      packet.Write(dir);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void Interact(int interactId, int fromId)
  {
    using (Packet packet = new Packet(54))
    {
      packet.Write(interactId);
      packet.Write(fromId);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void MobZoneSpawn(Vector3 pos, int mobType, int mobId, int mobZoneId)
  {
    using (Packet packet = new Packet(37))
    {
      packet.Write(pos);
      packet.Write(mobType);
      packet.Write(mobId);
      packet.Write(mobZoneId);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void PickupZoneSpawn(Vector3 pos, int entityId, int mobId, int mobZoneId)
  {
    using (Packet packet = new Packet(39))
    {
      packet.Write(pos);
      packet.Write(entityId);
      packet.Write(mobId);
      packet.Write(mobZoneId);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void MobZoneToggle(bool show, int objectID)
  {
    using (Packet packet = new Packet(38))
    {
      packet.Write(show);
      packet.Write(objectID);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void SendChatMessage(int fromClient, string username, string msg)
  {
    using (Packet packet = new Packet(40))
    {
      packet.Write(fromClient);
      packet.Write(username);
      packet.Write(msg);
      ServerSend.SendUDPDataToAll(fromClient, packet);
    }
  }

  public static void SendPing(int fromClient, Vector3 pos, string username)
  {
    using (Packet packet = new Packet(41))
    {
      packet.Write(pos);
      packet.Write(username);
      ServerSend.SendUDPDataToAll(fromClient, packet);
    }
  }

  public static void SendArmor(int fromClient, int armorSlot, int itemId)
  {
    using (Packet packet = new Packet(42))
    {
      packet.Write(fromClient);
      packet.Write(armorSlot);
      packet.Write(itemId);
      ServerSend.SendTCPDataToAll(fromClient, packet);
    }
  }

  public static void NewDay(int day)
  {
    using (Packet packet = new Packet(48))
    {
      packet.Write(day);
      foreach (Client client in Server.clients.Values)
      {
        if (client != null && client.player != null)
          client.player.stats["Day"] = day;
      }
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void GameOver(int winnerId = -2)
  {
    using (Packet packet = new Packet(12))
    {
      packet.Write(winnerId);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
    ServerSend.SendStats();
  }

  public static void PlayerFinishedLoading(int playerId)
  {
    using (Packet packet = new Packet(51))
    {
      packet.Write(playerId);
      ServerSend.SendTCPDataToAll(packet);
    }
  }

  public static void SendShipUpdate(int fromClient, int type, int interactId)
  {
    using (Packet packet = new Packet(56))
    {
      Debug.LogError((object) "server sending ship update");
      packet.Write(type);
      packet.Write(interactId);
      ServerSend.SendTCPDataToAll(fromClient, packet);
    }
  }

  public static void DragonUpdate(int dragonUpdateType)
  {
    using (Packet packet = new Packet(57))
    {
      packet.Write(dragonUpdateType);
      ServerSend.SendTCPDataToAll(LocalClient.instance.myId, packet);
    }
  }

  public static void SendStats()
  {
    using (Packet packet = new Packet(58))
    {
      int num = 0;
      foreach (Client client in Server.clients.Values)
      {
        if (client != null && client.player != null)
          ++num;
      }
      packet.Write(num);
      foreach (Client client in Server.clients.Values)
      {
        if (client != null && client.player != null)
        {
          packet.Write(client.player.id);
          foreach (string allStat in Player.allStats)
            packet.Write(client.player.stats[allStat]);
        }
      }
      ServerSend.SendTCPDataToAll(packet);
    }
  }
}
