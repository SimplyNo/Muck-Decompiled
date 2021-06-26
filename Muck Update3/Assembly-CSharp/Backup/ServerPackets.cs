// Decompiled with JetBrains decompiler
// Type: ServerPackets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

public enum ServerPackets
{
  welcome = 1,
  spawnPlayer = 2,
  playerPosition = 3,
  playerRotation = 4,
  playerDisconnect = 5,
  playerDied = 6,
  pingPlayer = 7,
  connectionSuccessful = 8,
  sendLevel = 9,
  sendStatus = 10, // 0x0000000A
  gameOver = 11, // 0x0000000B
  startGame = 12, // 0x0000000C
  clock = 13, // 0x0000000D
  openDoor = 14, // 0x0000000E
  ready = 15, // 0x0000000F
  taskProgress = 16, // 0x00000010
  dropItem = 17, // 0x00000011
  pickupItem = 18, // 0x00000012
  weaponInHand = 19, // 0x00000013
  playerHitObject = 20, // 0x00000014
  dropResources = 21, // 0x00000015
  animationUpdate = 22, // 0x00000016
  finalizeBuild = 23, // 0x00000017
  openChest = 24, // 0x00000018
  updateChest = 25, // 0x00000019
  pickupInteract = 26, // 0x0000001A
  dropItemAtPosition = 27, // 0x0000001B
  playerHit = 28, // 0x0000001C
  mobSpawn = 29, // 0x0000001D
  mobMove = 30, // 0x0000001E
  mobSetDestination = 31, // 0x0000001F
  mobAttack = 32, // 0x00000020
  playerDamageMob = 33, // 0x00000021
  shrineCombatStart = 34, // 0x00000022
  dropPowerupAtPosition = 35, // 0x00000023
  MobZoneSpawn = 36, // 0x00000024
  MobZoneToggle = 37, // 0x00000025
  PickupZoneSpawn = 38, // 0x00000026
  SendMessage = 39, // 0x00000027
  playerPing = 40, // 0x00000028
  sendArmor = 41, // 0x00000029
  playerHp = 42, // 0x0000002A
  respawnPlayer = 43, // 0x0000002B
  shootArrow = 44, // 0x0000002C
  removeResource = 45, // 0x0000002D
  mobProjectile = 46, // 0x0000002E
  newDay = 47, // 0x0000002F
  knockbackMob = 48, // 0x00000030
  spawnEffect = 49, // 0x00000031
  playerFinishedLoading = 50, // 0x00000032
  revivePlayer = 51, // 0x00000033
  spawnGrave = 52, // 0x00000034
  interact = 53, // 0x00000035
  setTarget = 54, // 0x00000036
  shipUpdate = 55, // 0x00000037
  dragonUpdate = 56, // 0x00000038
}
