// Decompiled with JetBrains decompiler
// Type: ClientHandle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
  public static void Welcome(Packet packet)
  {
    string str = packet.ReadString();
    double num = (double) packet.ReadFloat();
    int id = packet.ReadInt();
    Debug.Log((object) ("Message from server: " + str));
    UiManager.instance.ConnectionSuccessful();
    LocalClient.instance.myId = id;
    ClientSend.WelcomeReceived(id, LocalClient.instance.name);
    if (NetworkController.Instance.networkType != NetworkController.NetworkType.Classic)
      return;
    LocalClient.instance.udp.Connect(((IPEndPoint) LocalClient.instance.tcp.socket.Client.LocalEndPoint).Port);
  }

  public static void Clock(Packet packet)
  {
    int index = packet.ReadInt();
    LoadingScreen.Instance.players[index] = true;
  }

  public static void PlayerFinishedLoading(Packet packet) => LoadingScreen.Instance.UpdateStatuses(packet.ReadInt());

  public static void DropItem(Packet packet)
  {
    int fromClient = packet.ReadInt();
    int itemId = packet.ReadInt();
    int amount = packet.ReadInt();
    int objectID = packet.ReadInt();
    ItemManager.Instance.DropItem(fromClient, itemId, amount, objectID);
  }

  public static void DropItemAtPosition(Packet packet)
  {
    int itemId = packet.ReadInt();
    int amount = packet.ReadInt();
    int objectID = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    ItemManager.Instance.DropItemAtPosition(itemId, amount, pos, objectID);
  }

  public static void DropPowerupAtPosition(Packet packet)
  {
    int powerupId = packet.ReadInt();
    int objectID = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    ItemManager.Instance.DropPowerupAtPosition(powerupId, pos, objectID);
  }

  public static void DropResources(Packet packet)
  {
    int fromClient = packet.ReadInt();
    int dropTableId = packet.ReadInt();
    int droppedObjectID = packet.ReadInt();
    MonoBehaviour.print((object) ("CLIENT: Dropping resources with id: " + (object) droppedObjectID));
    ItemManager.Instance.DropResource(fromClient, dropTableId, droppedObjectID);
  }

  public static void PickupItem(Packet packet)
  {
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    if (LocalClient.instance.myId == num1 && !LocalClient.serverOwner)
    {
      Item component = ItemManager.Instance.list[num2].GetComponent<Item>();
      if ((bool) (UnityEngine.Object) component.item)
        InventoryUI.Instance.AddItemToInventory(component.item);
      else if ((bool) (UnityEngine.Object) component.powerup)
        PowerupInventory.Instance.AddPowerup(component.powerup.name, component.powerup.id, num2);
    }
    if (LocalClient.serverOwner)
      return;
    ItemManager.Instance.PickupItem(num2);
  }

  public static void SpawnEffect(Packet packet)
  {
    int id = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    PowerupCalculations.Instance.SpawnOnHitEffect(id, false, pos, 0);
  }

  public static void WeaponInHand(Packet packet)
  {
    int key = packet.ReadInt();
    int objectID = packet.ReadInt();
    GameManager.players[key].onlinePlayer.UpdateWeapon(objectID);
  }

  public static void AnimationUpdate(Packet packet)
  {
    int key = packet.ReadInt();
    int animation = packet.ReadInt();
    bool b = packet.ReadBool();
    GameManager.players[key].onlinePlayer.NewAnimation(animation, b);
  }

  public static void ShootArrowFromPlayer(Packet packet)
  {
    Vector3 spawnPos = packet.ReadVector3();
    Vector3 direction = packet.ReadVector3();
    float force = packet.ReadFloat();
    int arrowId = packet.ReadInt();
    int fromPlayer = packet.ReadInt();
    ProjectileController.Instance.SpawnProjectileFromPlayer(spawnPos, direction, force, arrowId, fromPlayer);
  }

  public static void PlayerHitObject(Packet packet)
  {
    int fromClient = packet.ReadInt();
    int key = packet.ReadInt();
    int newHp = packet.ReadInt();
    int hitEffect = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    ResourceManager.Instance.list[key].GetComponent<Hitable>().Damage(newHp, fromClient, hitEffect, pos);
  }

  public static void RemoveResource(Packet packet)
  {
    int id = packet.ReadInt();
    ResourceManager.Instance.RemoveItem(id);
  }

  public static void PlayerHp(Packet packet)
  {
    int key = packet.ReadInt();
    float hpRatio = packet.ReadFloat();
    GameManager.players[key].SetHpRatio(hpRatio);
  }

  public static void RespawnPlayer(Packet packet)
  {
    int id = packet.ReadInt();
    Vector3 zero = packet.ReadVector3();
    GameManager.instance.RespawnPlayer(id, zero);
  }

  public static void PlayerHit(Packet packet)
  {
    int fromClient = packet.ReadInt();
    int damage = packet.ReadInt();
    float hpRatio = packet.ReadFloat();
    int key = packet.ReadInt();
    int hitEffect = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    MonoBehaviour.print((object) ("recevied player hit. Damage: " + (object) damage + ", ratio: " + (object) hpRatio + "from: " + (object) fromClient + ", to: " + (object) key));
    PlayerManager player = GameManager.players[key];
    if (key == LocalClient.instance.myId)
      PlayerStatus.Instance.DealDamage(damage);
    else
      player.SetHpRatio(hpRatio);
    player.hitable.Damage(damage, fromClient, hitEffect, pos);
  }

  public static void FinalizeBuild(Packet packet)
  {
    int buildOwner = packet.ReadInt();
    int itemID = packet.ReadInt();
    int objectId = packet.ReadInt();
    Vector3 position = packet.ReadVector3();
    int yRotation = packet.ReadInt();
    MonoBehaviour.print((object) "Received build, now building");
    BuildManager.Instance.BuildItem(buildOwner, itemID, objectId, position, yRotation);
  }

  public static void OpenChest(Packet packet)
  {
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    bool inUse = packet.ReadBool();
    MonoBehaviour.print((object) string.Format("player{0} now {1} chest{2}", (object) num1, (object) inUse, (object) num2));
    ChestManager.Instance.UseChest(num2, inUse);
    if (!inUse || num1 != LocalClient.instance.myId)
      return;
    if ((UnityEngine.Object) OtherInput.Instance.currentChest != (UnityEngine.Object) null)
    {
      ClientSend.RequestChest(OtherInput.Instance.currentChest.id, false);
      OtherInput.Instance.currentChest = (Chest) null;
    }
    OtherInput.Instance.currentChest = ChestManager.Instance.chests[num2];
    OtherInput.CraftingState state = ChestManager.Instance.chests[num2].GetComponentInChildren<ChestInteract>().state;
    OtherInput.Instance.ToggleInventory(state);
  }

  public static void UpdateChest(Packet packet)
  {
    packet.ReadInt();
    int chestId = packet.ReadInt();
    int cellId = packet.ReadInt();
    int itemId = packet.ReadInt();
    int amount = packet.ReadInt();
    ChestManager.Instance.UpdateChest(chestId, cellId, itemId, amount);
  }

  public static void PickupInteract(Packet packet)
  {
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    Debug.Log((object) ("Received pickup with id: " + (object) num2));
    Interactable componentInChildren = ResourceManager.Instance.list[num2].GetComponentInChildren<Interactable>();
    componentInChildren.AllExecute();
    if (LocalClient.instance.myId == num1 && !LocalClient.serverOwner)
      componentInChildren.LocalExecute();
    if (LocalClient.serverOwner)
      return;
    ResourceManager.Instance.RemoveInteractItem(num2);
  }

  public static void SpawnPlayer(Packet packet)
  {
    int id = packet.ReadInt();
    string username = packet.ReadString();
    Vector3 vector3 = packet.ReadVector3();
    Vector3 position = packet.ReadVector3();
    float orientationY = packet.ReadFloat();
    GameManager.instance.SpawnPlayer(id, username, new Color(vector3.x, vector3.y, vector3.z), position, orientationY);
    GameManager.instance.StartGame();
  }

  public static void StartGame(Packet packet)
  {
    if (NetworkController.Instance.loading)
      return;
    LocalClient.instance.myId = packet.ReadInt();
    int seed = packet.ReadInt();
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    int num3 = packet.ReadInt();
    int num4 = packet.ReadInt();
    int num5 = packet.ReadInt();
    int gameMode = num1;
    int friendlyFire = num2;
    int difficulty = num3;
    int gameLength = num4;
    int multiplayer = num5;
    GameManager.gameSettings = new GameSettings(seed, gameMode, friendlyFire, difficulty, gameLength, multiplayer);
    MonoBehaviour.print((object) "Game settings successfully loaded");
    MonoBehaviour.print((object) ("loading game scene, assigned id: " + (object) LocalClient.instance.myId));
    int length = packet.ReadInt();
    string[] names = new string[length];
    for (int index = 0; index < length; ++index)
    {
      packet.ReadInt();
      string str = packet.ReadString();
      names[index] = str;
    }
    NetworkController.Instance.LoadGame(names);
    ClientSend.StartedLoading();
  }

  public static void PlayerPosition(Packet packet)
  {
    int key = packet.ReadInt();
    Vector3 position = packet.ReadVector3();
    if (!GameManager.players.ContainsKey(key))
      return;
    GameManager.players[key].SetDesiredPosition(position);
  }

  public static void PlayerRotation(Packet packet)
  {
    int key = packet.ReadInt();
    if (!GameManager.players.ContainsKey(key))
      return;
    float orientationY = packet.ReadFloat();
    float orientationX = packet.ReadFloat();
    GameManager.players[key].SetDesiredRotation(orientationY, orientationX);
  }

  public static void ReceivePing(Packet packet)
  {
    packet.ReadInt();
    NetStatus.AddPing((int) (DateTime.Now - DateTime.Parse(packet.ReadString())).TotalMilliseconds);
  }

  public static void ReceiveStatus(Packet packet) => MonoBehaviour.print((object) "received status");

  public static void ConnectionEstablished(Packet packet)
  {
    MonoBehaviour.print((object) "connection has successfully been established. ready to enter game");
    GameManager.connected = true;
  }

  public static void OpenDoor(Packet packet) => packet.ReadInt();

  public static void PlayerDied(Packet packet)
  {
    int id = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    GameManager.instance.KillPlayer(id, pos);
  }

  public static void SpawnGrave(Packet packet)
  {
    int playerId = packet.ReadInt();
    int graveObjectId = packet.ReadInt();
    Vector3 gravePos = packet.ReadVector3();
    GameManager.instance.SpawnGrave(gravePos, playerId, graveObjectId);
  }

  public static void Ready(Packet packet)
  {
    packet.ReadInt();
    packet.ReadBool();
  }

  public static void DisconnectPlayer(Packet packet)
  {
    int id = packet.ReadInt();
    Debug.Log((object) string.Format("Player {0} has disconnected", (object) id));
    if (id == LocalClient.instance.myId)
      GameManager.instance.LeaveGame();
    else
      GameManager.instance.DisconnectPlayer(id);
  }

  public static void ShrineCombatStart(Packet packet)
  {
    int key = packet.ReadInt();
    int length = packet.ReadInt();
    ShrineInteractable componentInChildren = ResourceManager.Instance.list[key].GetComponentInChildren<ShrineInteractable>();
    if (!(bool) (UnityEngine.Object) componentInChildren)
      return;
    MonoBehaviour.print((object) ("starting new shrine with mobs: " + (object) length));
    int[] mobIds = new int[length];
    for (int index = 0; index < length; ++index)
      mobIds[index] = packet.ReadInt();
    componentInChildren.StartShrine(mobIds);
  }

  public static void RevivePlayer(Packet packet)
  {
    int num1 = packet.ReadInt();
    int num2 = packet.ReadInt();
    bool flag = packet.ReadBool();
    GameManager.instance.RespawnPlayer(num2, Vector3.zero);
    int id = LocalClient.instance.myId;
    if (num1 == id && !flag)
      InventoryUI.Instance.UseMoney(RespawnTotemUI.Instance.GetRevivePrice());
    if (!flag && (UnityEngine.Object) GameManager.players[num2] != (UnityEngine.Object) null)
      GameManager.players[num2].RemoveGrave();
    RespawnTotemUI.Instance.Refresh();
    int key = packet.ReadInt();
    if (!ResourceManager.Instance.list.ContainsKey(key))
      return;
    ResourceManager.Instance.list[key].GetComponentInChildren<Interactable>().AllExecute();
  }

  public static void MobSpawn(Packet packet)
  {
    Vector3 pos = packet.ReadVector3();
    int mobType = packet.ReadInt();
    int mobId = packet.ReadInt();
    float multiplier = packet.ReadFloat();
    float bossMultiplier = packet.ReadFloat();
    int guardianType = packet.ReadInt();
    MobSpawner.Instance.SpawnMob(pos, mobType, mobId, multiplier, bossMultiplier, guardianType: guardianType);
  }

  public static void MobMove(Packet packet)
  {
    int key = packet.ReadInt();
    Vector3 nextPosition = packet.ReadVector3();
    if (!(bool) (UnityEngine.Object) MobManager.Instance.mobs[key])
      return;
    MobManager.Instance.mobs[key].SetPosition(nextPosition);
  }

  public static void MobSetDestination(Packet packet)
  {
    int key = packet.ReadInt();
    Vector3 dest = packet.ReadVector3();
    MobManager.Instance.mobs[key].SetDestination(dest);
  }

  public static void MobSetTarget(Packet packet)
  {
    int key = packet.ReadInt();
    int targetId = packet.ReadInt();
    MobManager.Instance.mobs[key].SetTarget(targetId);
  }

  public static void MobAttack(Packet packet)
  {
    int key = packet.ReadInt();
    int targetPlayerId = packet.ReadInt();
    int attackAnimationIndex = packet.ReadInt();
    MobManager.Instance.mobs[key].Attack(targetPlayerId, attackAnimationIndex);
  }

  public static void MobSpawnProjectile(Packet packet)
  {
    Vector3 spawnPos = packet.ReadVector3();
    Vector3 direction = packet.ReadVector3();
    float force = packet.ReadFloat();
    int itemId = packet.ReadInt();
    int mobObjectId = packet.ReadInt();
    ProjectileController.Instance.SpawnMobProjectile(spawnPos, direction, force, itemId, mobObjectId);
  }

  public static void PlayerDamageMob(Packet packet)
  {
    int fromClient = packet.ReadInt();
    int key = packet.ReadInt();
    int newHp = packet.ReadInt();
    int hitEffect = packet.ReadInt();
    Vector3 pos = packet.ReadVector3();
    if (newHp <= 0 && LocalClient.instance.myId == fromClient)
      PlayerStatus.Instance.Dracula();
    MobManager.Instance.mobs[key].hitable.Damage(newHp, fromClient, hitEffect, pos);
  }

  public static void KnockbackMob(Packet packet)
  {
    int key = packet.ReadInt();
    Vector3 dir = packet.ReadVector3();
    if (!MobManager.Instance.mobs.ContainsKey(key))
      return;
    MobManager.Instance.mobs[key].Knockback(dir);
  }

  public static void Interact(Packet packet)
  {
    int key = packet.ReadInt();
    int num = packet.ReadInt();
    if (!ResourceManager.Instance.list.ContainsKey(key))
      return;
    Interactable componentInChildren = ResourceManager.Instance.list[key].GetComponentInChildren<Interactable>();
    if (componentInChildren.IsStarted())
      return;
    if (num == LocalClient.instance.myId)
      componentInChildren.LocalExecute();
    componentInChildren.AllExecute();
  }

  public static void MobZoneToggle(Packet packet)
  {
    bool show = packet.ReadBool();
    int key = packet.ReadInt();
    MobZoneManager.Instance.zones[key].ToggleEntities(show);
  }

  public static void MobZoneSpawn(Packet packet)
  {
    Vector3 pos = packet.ReadVector3();
    int entityType = packet.ReadInt();
    int objectId = packet.ReadInt();
    int num = packet.ReadInt();
    MobZoneManager.Instance.zones[num].LocalSpawnEntity(pos, entityType, objectId, num);
  }

  public static void PickupSpawnZone(Packet packet)
  {
    Vector3 pos = packet.ReadVector3();
    int entityType = packet.ReadInt();
    int objectId = packet.ReadInt();
    int num = packet.ReadInt();
    MobZoneManager.Instance.zones[num].LocalSpawnEntity(pos, entityType, objectId, num);
  }

  public static void ReceiveChatMessage(Packet packet)
  {
    int fromUser = packet.ReadInt();
    string fromUsername = packet.ReadString();
    string message = packet.ReadString();
    ChatBox.Instance.AppendMessage(fromUser, message, fromUsername);
  }

  public static void ReceivePlayerPing(Packet packet)
  {
    Vector3 pos = packet.ReadVector3();
    string name = packet.ReadString();
    PingController.Instance.MakePing(pos, name, "");
  }

  public static void ReceivePlayerArmor(Packet packet)
  {
    int key = packet.ReadInt();
    int armorSlot = packet.ReadInt();
    int itemId = packet.ReadInt();
    MonoBehaviour.print((object) ("received armor slot: " + (object) armorSlot + ", armor: " + (object) itemId));
    GameManager.players[key].SetArmor(armorSlot, itemId);
  }

  public static void NewDay(Packet packet)
  {
    int day = packet.ReadInt();
    GameManager.instance.UpdateDay(day);
    DayCycle.time = 0.0f;
  }

  public static void GameOver(Packet packet)
  {
    int winnerId = packet.ReadInt();
    GameManager.instance.GameOver(winnerId);
  }

  public static void ShipUpdate(Packet packet)
  {
    Boat.BoatPackets p = (Boat.BoatPackets) packet.ReadInt();
    int interactId = packet.ReadInt();
    Boat.Instance.UpdateShipStatus(p, interactId);
  }

  public static void DragonUpdate(Packet packet)
  {
    BobMob.DragonState state = (BobMob.DragonState) packet.ReadInt();
    if (!(bool) (UnityEngine.Object) Dragon.Instance)
      return;
    Dragon.Instance.transform.root.GetComponent<BobMob>().DragonUpdate(state);
  }
}
