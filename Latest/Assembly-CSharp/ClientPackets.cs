// Decompiled with JetBrains decompiler
// Type: ClientPackets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

public enum ClientPackets
{
  welcomeReceived = 1,
  joinLobby = 2,
  playerPosition = 3,
  playerRotation = 4,
  sendDisconnect = 5,
  sendPing = 6,
  playerKilled = 7,
  ready = 8,
  requestSpawns = 9,
  dropItem = 10, // 0x0000000A
  dropItemAtPosition = 11, // 0x0000000B
  pickupItem = 12, // 0x0000000C
  weaponInHand = 13, // 0x0000000D
  playerHitObject = 14, // 0x0000000E
  animationUpdate = 15, // 0x0000000F
  requestBuild = 16, // 0x00000010
  requestChest = 17, // 0x00000011
  updateChest = 18, // 0x00000012
  pickupInteract = 19, // 0x00000013
  playerHit = 20, // 0x00000014
  playerDamageMob = 21, // 0x00000015
  shrineCombatStart = 22, // 0x00000016
  sendChatMessage = 23, // 0x00000017
  playerPing = 24, // 0x00000018
  sendArmor = 25, // 0x00000019
  playerHp = 26, // 0x0000001A
  playerDied = 27, // 0x0000001B
  shootArrow = 28, // 0x0000001C
  finishedLoading = 29, // 0x0000001D
  spawnEffect = 30, // 0x0000001E
  reviveRequest = 31, // 0x0000001F
}
