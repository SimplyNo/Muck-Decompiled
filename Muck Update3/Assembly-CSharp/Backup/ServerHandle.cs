// Decompiled with JetBrains decompiler
// Type: ServerHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
  public static void WelcomeReceived(int fromClient, Packet packet)
  {
    int num = packet.ReadInt();
    string playerName = packet.ReadString();
    Color color = new Color(packet.ReadFloat(), packet.ReadFloat(), packet.ReadFloat());
    if (fromClient != num)
      Debug.Log((object) "Something went very wrong in ServerHandle");
    if (NetworkController.Instance.networkType == NetworkController.NetworkType.Classic)
    {
      Debug.Log((object) string.Format("{0} connected successfully and is now player {1}.", (object) Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint, (object) fromClient));
      Server.clients[fromClient].StartClient(playerName, color);
    }
    ServerSend.ConnectionSuccessful(fromClient);
    Server.clients[fromClient].SendIntoGame();
  }

  public static void JoinRequest(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player.joined)
    {
      Debug.LogError((object) ("Player already joined: " + (object) fromClient));
    }
    else
    {
      Debug.LogError((object) ("Player wants to join, id: " + (object) fromClient));
      Server.clients[fromClient].player.joined = true;
      Server.clients[fromClient].player.username = packet.ReadString();
      ServerSend.Welcome(fromClient, "weclome");
    }
  }

  public static void StartedLoading(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player.loading)
      return;
    Server.clients[fromClient].player.loading = true;
  }

  public static void PlayerFinishedLoading(int fromClient, Packet packet)
  {
    Debug.Log((object) ("Player finished loading: " + (object) fromClient));
    Server.clients[fromClient].player.ready = true;
    ServerSend.PlayerFinishedLoading(fromClient);
    int num = 0;
    int nPlayers = 0;
    foreach (Client client in Server.clients.Values)
    {
      if (client?.player != null)
      {
        ++nPlayers;
        if (client.player.ready)
          ++num;
      }
    }
    if (num < nPlayers)
      return;
    Debug.Log((object) ("ready players: " + (object) num + " / " + (object) nPlayers));
    List<Vector3> spawnPositions = GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus ? GameManager.instance.FindVersusSpawnPositions(nPlayers) : GameManager.instance.FindSurvivalSpawnPositions(nPlayers);
    if (num < nPlayers)
      return;
    GameManager.instance.SendPlayersIntoGame(spawnPositions);
  }

  public static void PlayerDisconnect(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    ServerHandle.DisconnectPlayer(fromClient);
  }

  public static void DisconnectPlayer(int fromClient)
  {
    ServerSend.DisconnectPlayer(fromClient);
    try
    {
      ServerSend.SendChatMessage(-1, "Server", Server.clients[fromClient].player.username + " disconnected");
    }
    catch
    {
      Debug.LogError((object) "Failed to send disconnect message to clients");
    }
    Server.clients[fromClient] = (Client) null;
  }

  public static void SpawnPlayersRequest(int fromClient, Packet packet)
  {
    Debug.Log((object) "received request to spawn players");
    if (Server.clients[fromClient].player == null)
      return;
    Server.clients[fromClient].SendIntoGame();
  }

  public static void PlayerHp(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    Server.clients[fromClient].player.currentHp = num1;
    float hpRatio = (float) num1 / (float) num2;
    ServerSend.PlayerHp(fromClient, hpRatio);
  }

  public static void PlayerDied(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null || Server.clients[fromClient].player.dead)
      return;
    Server.clients[fromClient].player.Died();
    GameManager.players[fromClient].dead = true;
    Vector3 gravePosition = GameManager.instance.GetGravePosition(fromClient);
    ServerSend.SendChatMessage(-1, "", "<color=orange>" + Server.clients[fromClient].player.username + " has died.");
    ServerSend.PlayerDied(fromClient, Server.clients[fromClient].player.pos, gravePosition);
    if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus)
      return;
    GameManager.instance.CheckIfGameOver();
  }

  public static void RevivePlayer(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num1 = packet.ReadInt();
    bool shrine = packet.ReadBool();
    if (!GameManager.players[num1].dead)
    {
      Debug.LogError((object) "not dead lol");
    }
    else
    {
      Server.clients[num1].player.dead = false;
      GameManager.players[num1].dead = false;
      GameManager.instance.RespawnPlayer(num1, Vector3.zero);
      if (fromClient == LocalClient.instance.myId && !shrine)
      {
        InventoryUI.Instance.UseMoney(RespawnTotemUI.Instance.GetRevivePrice());
        RespawnTotemUI.Instance.Refresh();
      }
      int num2 = packet.ReadInt();
      if (ResourceManager.Instance.list.ContainsKey(num2))
        ResourceManager.Instance.list[num2].GetComponentInChildren<Interactable>().AllExecute();
      ServerSend.SendChatMessage(-1, "", "<color=orange>" + Server.clients[fromClient].player.username + " has revived " + Server.clients[num1].player.username + ".");
      ServerSend.RevivePlayer(fromClient, num1, shrine, num2);
    }
  }

  public static void PlayerPosition(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    Vector3 vector3 = packet.ReadVector3();
    Server.clients[fromClient].player.pos = vector3;
    ServerSend.PlayerPosition(Server.clients[fromClient].player, 0);
  }

  public static void PlayerRotation(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    float num1 = packet.ReadFloat();
    float num2 = packet.ReadFloat();
    Server.clients[fromClient].player.yOrientation = num1;
    Server.clients[fromClient].player.xOrientation = num2;
    ServerSend.PlayerRotation(Server.clients[fromClient].player);
  }

  public static void ItemDropped(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int itemId = packet.ReadInt();
    int amount = packet.ReadInt();
    int nextId = ItemManager.Instance.GetNextId();
    ItemManager.Instance.DropItem(fromClient, itemId, amount, nextId);
    ServerSend.DropItem(fromClient, itemId, amount, nextId);
  }

  public static void ItemDroppedAtPosition(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num = packet.ReadInt();
    int amount = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    int nextId = ItemManager.Instance.GetNextId();
    ItemManager.Instance.DropItemAtPosition(num, amount, pos, num);
    ServerSend.DropItemAtPosition(num, amount, nextId, pos);
  }

  public static void ItemPickedUp(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num = packet.ReadInt();
    Debug.Log((object) ("object: " + (object) num + " picked up by player: " + (object) fromClient));
    Item component = ItemManager.Instance.list[num].GetComponent<Item>();
    if ((bool) (Object) component.powerup)
      ++Server.clients[fromClient].player.powerups[component.powerup.id];
    if (!ItemManager.Instance.list.ContainsKey(num))
      return;
    if (fromClient == LocalClient.instance.myId)
    {
      if ((bool) (Object) component.item)
        InventoryUI.Instance.AddItemToInventory(component.item);
      else if ((bool) (Object) component.powerup)
      {
        ++Server.clients[fromClient].player.powerups[component.powerup.id];
        PowerupInventory.Instance.AddPowerup(component.powerup.name, component.powerup.id, num);
      }
    }
    ItemManager.Instance.PickupItem(num);
    ServerSend.PickupItem(fromClient, num);
  }

  public static void ItemInteract(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num = packet.ReadInt();
    if (!ResourceManager.Instance.list.ContainsKey(num))
      return;
    Interactable componentInChildren = ResourceManager.Instance.list[num].GetComponentInChildren<Interactable>();
    if (fromClient == LocalClient.instance.myId)
      componentInChildren.LocalExecute();
    componentInChildren.AllExecute();
    componentInChildren.ServerExecute(fromClient);
    ServerSend.PickupInteract(fromClient, num);
  }

  public static void WeaponInHand(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int objectID = packet.ReadInt();
    ServerSend.WeaponInHand(fromClient, objectID);
  }

  public static void AnimationUpdate(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null || Server.clients[fromClient].player == null)
      return;
    int animation = packet.ReadInt();
    bool b = packet.ReadBool();
    ServerSend.AnimationUpdate(fromClient, animation, b);
  }

  public static void ShootArrow(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null || Server.clients[fromClient].player == null)
      return;
    Vector3 pos = packet.ReadVector3();
    Vector3 vector3 = packet.ReadVector3();
    float num1 = packet.ReadFloat();
    int num2 = packet.ReadInt();
    Vector3 rot = vector3;
    double num3 = (double) num1;
    int arrowId = num2;
    int playerId = fromClient;
    ServerSend.ShootArrow(pos, rot, (float) num3, arrowId, playerId);
  }

  public static void RequestChest(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num = packet.ReadInt();
    bool flag = packet.ReadBool();
    if (!ChestManager.Instance.chests.ContainsKey(num))
      return;
    if (flag)
    {
      if (ChestManager.Instance.IsChestOpen(num))
        return;
      ChestManager.Instance.UseChest(num, true);
      ServerSend.OpenChest(fromClient, num, true);
    }
    else
    {
      ChestManager.Instance.UseChest(num, false);
      ServerSend.OpenChest(fromClient, num, false);
    }
  }

  public static void UpdateChest(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int chestId = packet.ReadInt();
    int cellId = packet.ReadInt();
    int itemId = packet.ReadInt();
    int amount = packet.ReadInt();
    Debug.Log((object) "received chest update");
    Debug.Log((object) "now sending to other players");
    ChestManager.Instance.UpdateChest(chestId, cellId, itemId, amount);
    ServerSend.UpdateChest(fromClient, chestId, cellId, itemId, amount);
  }

  public static void RequestBuild(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num1 = packet.ReadInt();
    Vector3 vector3 = packet.ReadVector3();
    int num2 = packet.ReadInt();
    int num3 = ItemManager.Instance.allItems[num1].type != InventoryItem.ItemType.Storage ? BuildManager.Instance.GetNextBuildId() : ResourceManager.Instance.GetNextId();
    BuildManager.Instance.BuildItem(fromClient, num1, num3, vector3, num2);
    ServerSend.SendBuild(fromClient, num1, num3, vector3, num2);
  }

  public static void PlayerHitObject(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    int hitEffect = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    if (!ResourceManager.Instance.list.ContainsKey(num2))
      return;
    Hitable component = ResourceManager.Instance.list[num2].GetComponent<Hitable>();
    if (component.hp <= 0)
      return;
    int num3 = component.hp - num1;
    component.hp = component.Damage(num3, fromClient, hitEffect, pos);
    Debug.Log((object) ("object hit from: " + (object) fromClient));
    if (num3 <= 0)
    {
      num3 = 0;
      Debug.Log((object) "dropping item");
      LootExtra.CheckDrop(fromClient, (HitableResource) component);
    }
    ServerSend.PlayerHitObject(fromClient, num2, num3, hitEffect, pos);
  }

  public static void SpawnEffect(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    ServerSend.SpawnEffect(packet.ReadInt(), packet.ReadVector3(), fromClient);
  }

  public static void PlayerHit(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    float sharpness = packet.ReadFloat();
    int hitEffect = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    if (!(bool) (Object) GameManager.players[num2])
      return;
    if (fromClient == num2)
      num1 = (int) ((double) num1 * (double) GameManager.instance.MobDamageMultiplier());
    else if (GameManager.gameSettings.friendlyFire == GameSettings.FriendlyFire.Off && GameManager.gameSettings.gameMode != GameSettings.GameMode.Versus)
      return;
    float hardness = 0.5f;
    float armor = PowerupInventory.Instance.GetDefenseMultiplier(Server.clients[num2].player.powerups) + (float) Server.clients[num2].player.totalArmor;
    int damage = GameManager.instance.CalculateDamage((float) num1, armor, sharpness, hardness);
    Debug.Log((object) string.Format("Player{0} took {1} damage from {2} and had armor {3}", (object) num2, (object) damage, (object) fromClient, (object) armor));
    Player player = Server.clients[num2].player;
    int num3 = player.Damage(damage);
    float hpRatioEstimate = (float) num3 / (float) PowerupInventory.Instance.GetMaxHpAndShield(player.powerups);
    if ((double) hpRatioEstimate < 0.0)
      hpRatioEstimate = 0.0f;
    Debug.Log((object) ("estimated hp left: " + (object) num3 + ", ratio: " + (object) hpRatioEstimate + ", maxhp: " + (object) PowerupInventory.Instance.GetMaxHpAndShield(player.powerups)));
    if (num2 == fromClient)
      Debug.Log((object) "Player took damage from mob btw lol");
    ServerSend.HitPlayer(fromClient, damage, hpRatioEstimate, num2, hitEffect, pos);
  }

  public static void PlayerRequestedSpawns(int fromClient, Packet packet) => Debug.LogError((object) "Player requested spawns, but method is not implemented");

  public static void Ready(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    Debug.Log((object) ("Recevied ready from player: " + (object) fromClient));
    bool ready = packet.ReadBool();
    ServerSend.PlayerReady(fromClient, ready);
    Server.clients[fromClient].player.ready = ready;
  }

  public static void PingReceived(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    string ms = packet.ReadString();
    Server.clients[fromClient].player.PingPlayer();
    ServerSend.PingPlayer(fromClient, ms);
  }

  public static void ShrineCombatStartRequest(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int key = packet.ReadInt();
    if (!ResourceManager.Instance.list.ContainsKey(key))
      return;
    Debug.LogError((object) "contains shrine");
    Interactable componentInChildren = ResourceManager.Instance.list[key].GetComponentInChildren<Interactable>();
    if (componentInChildren.IsStarted())
      return;
    if (fromClient == LocalClient.instance.myId)
      componentInChildren.LocalExecute();
    componentInChildren.ServerExecute(fromClient);
  }

  public static void Interact(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num = packet.ReadInt();
    if (!ResourceManager.Instance.list.ContainsKey(num))
      return;
    Interactable componentInChildren = ResourceManager.Instance.list[num].GetComponentInChildren<Interactable>();
    if (componentInChildren.IsStarted())
      return;
    if (fromClient == LocalClient.instance.myId)
      componentInChildren.LocalExecute();
    componentInChildren.ServerExecute(fromClient);
    componentInChildren.AllExecute();
    ServerSend.Interact(num, fromClient);
  }

  public static void PlayerDamageMob(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    float sharpness = packet.ReadFloat();
    int hitEffect = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    if (!MobManager.Instance.mobs.ContainsKey(num1))
      return;
    Mob mob = MobManager.Instance.mobs[num1];
    if (!(bool) (Object) mob)
      return;
    HitableMob component1 = mob.GetComponent<HitableMob>();
    if (component1.hp <= 0)
      return;
    float sharpDefense = component1.mob.mobType.sharpDefense;
    float defense = component1.mob.mobType.defense;
    int damage = GameManager.instance.CalculateDamage((float) num2, defense, sharpness, sharpDefense);
    Debug.Log((object) string.Format("Mob took {0} damage from {1}.", (object) damage, (object) fromClient));
    int num3 = component1.hp - damage;
    if (num3 <= 0)
    {
      num3 = 0;
      LootDrop dropTable = component1.dropTable;
      float buffMultiplier = 1f;
      Mob component2 = component1.GetComponent<Mob>();
      if ((bool) (Object) component2 && component2.IsBuff())
        buffMultiplier = 1.25f;
      LootExtra.DropMobLoot(component1.transform, dropTable, fromClient, buffMultiplier);
      if (component2.bossType != Mob.BossType.None)
        LootExtra.BossLoot(component1.transform, mob.bossType);
    }
    component1.hp = component1.Damage(num3, fromClient, hitEffect, pos);
    float knockbackMultiplier = PowerupInventory.Instance.GetKnockbackMultiplier(Server.clients[fromClient].player.powerups);
    if (((double) damage / (double) mob.hitable.maxHp > (double) mob.mobType.knockbackThreshold || (double) knockbackMultiplier > 0.0) && num3 > 0)
    {
      Vector3 normalized = VectorExtensions.XZVector(component1.transform.position - GameManager.players[fromClient].transform.position).normalized;
      ServerSend.KnockbackMob(num1, normalized);
      if (hitEffect == 0)
        hitEffect = 4;
    }
    if (num3 <= 0 && LocalClient.instance.myId == fromClient)
      PlayerStatus.Instance.Dracula();
    ServerSend.PlayerHitMob(fromClient, num1, num3, hitEffect, pos);
  }

  public static void ReceiveChatMessage(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    string msg = packet.ReadString();
    string username = GameManager.players[fromClient].username;
    ServerSend.SendChatMessage(fromClient, username, msg);
  }

  public static void ReceivePing(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    Vector3 pos = packet.ReadVector3();
    string username = GameManager.players[fromClient].username;
    ServerSend.SendPing(fromClient, pos, username);
  }

  public static void ReceiveArmor(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int armorSlot = packet.ReadInt();
    int itemId = packet.ReadInt();
    Server.clients[fromClient].player.UpdateArmor(armorSlot, itemId);
    ServerSend.SendArmor(fromClient, armorSlot, itemId);
  }

  public static void ReceiveShipUpdate(int fromClient, Packet packet)
  {
    if (Server.clients[fromClient].player == null)
      return;
    int type = packet.ReadInt();
    int interactId = packet.ReadInt();
    ServerSend.SendShipUpdate(fromClient, type, interactId);
  }
}
