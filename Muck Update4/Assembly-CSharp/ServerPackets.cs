// Decompiled with JetBrains decompiler
// Type: ServerPackets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

public enum ServerPackets
{
  welcome = 1,
  spawnPlayer = 2,
  playerPosition = 3,
  playerRotation = 4,
  playerDisconnect = 5,
  playerKick = 6,
  playerDied = 7,
  pingPlayer = 8,
  connectionSuccessful = 9,
  sendLevel = 10, // 0x0000000A
  sendStatus = 11, // 0x0000000B
  gameOver = 12, // 0x0000000C
  startGame = 13, // 0x0000000D
  clock = 14, // 0x0000000E
  openDoor = 15, // 0x0000000F
  ready = 16, // 0x00000010
  taskProgress = 17, // 0x00000011
  dropItem = 18, // 0x00000012
  pickupItem = 19, // 0x00000013
  weaponInHand = 20, // 0x00000014
  playerHitObject = 21, // 0x00000015
  dropResources = 22, // 0x00000016
  animationUpdate = 23, // 0x00000017
  finalizeBuild = 24, // 0x00000018
  openChest = 25, // 0x00000019
  updateChest = 26, // 0x0000001A
  pickupInteract = 27, // 0x0000001B
  dropItemAtPosition = 28, // 0x0000001C
  playerHit = 29, // 0x0000001D
  mobSpawn = 30, // 0x0000001E
  mobMove = 31, // 0x0000001F
  mobSetDestination = 32, // 0x00000020
  mobAttack = 33, // 0x00000021
  playerDamageMob = 34, // 0x00000022
  shrineCombatStart = 35, // 0x00000023
  dropPowerupAtPosition = 36, // 0x00000024
  MobZoneSpawn = 37, // 0x00000025
  MobZoneToggle = 38, // 0x00000026
  PickupZoneSpawn = 39, // 0x00000027
  SendMessage = 40, // 0x00000028
  playerPing = 41, // 0x00000029
  sendArmor = 42, // 0x0000002A
  playerHp = 43, // 0x0000002B
  respawnPlayer = 44, // 0x0000002C
  shootArrow = 45, // 0x0000002D
  removeResource = 46, // 0x0000002E
  mobProjectile = 47, // 0x0000002F
  newDay = 48, // 0x00000030
  knockbackMob = 49, // 0x00000031
  spawnEffect = 50, // 0x00000032
  playerFinishedLoading = 51, // 0x00000033
  revivePlayer = 52, // 0x00000034
  spawnGrave = 53, // 0x00000035
  interact = 54, // 0x00000036
  setTarget = 55, // 0x00000037
  shipUpdate = 56, // 0x00000038
  dragonUpdate = 57, // 0x00000039
  sendStats = 58, // 0x0000003A
}
