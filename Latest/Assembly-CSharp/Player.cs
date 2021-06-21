// Decompiled with JetBrains decompiler
// Type: Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

public class Player
{
  public int id;
  public string username;
  public bool ready;
  public bool joined;
  public Color color;
  public Vector3 pos;
  public float yOrientation;
  public float xOrientation;
  public bool running;
  public bool dead;
  public int ping;
  public ulong damageDone;
  public ulong mobsKilled;
  public ulong damageTaken;
  public float lastPingTime;
  public int[] powerups;
  public int[] armor;
  public int totalArmor;
  public SteamId steamId;
  public int currentHp;

  public Player(int id, string username, Color color)
  {
    this.id = id;
    this.username = username;
    this.currentHp = 100;
    this.dead = false;
    this.powerups = new int[ItemManager.Instance.allPowerups.Count];
    this.armor = new int[4];
    for (int index = 0; index < this.armor.Length; ++index)
      this.armor[index] = -1;
  }

  public Player(int id, string username, Color color, SteamId steamId)
  {
    this.id = id;
    this.username = username;
    this.steamId = steamId;
    this.currentHp = 100;
    this.dead = false;
    this.powerups = new int[ItemManager.Instance.allPowerups.Count];
    this.armor = new int[4];
    for (int index = 0; index < this.armor.Length; ++index)
      this.armor[index] = -1;
  }

  public void PingPlayer() => this.lastPingTime = Time.time;

  public void UpdateArmor(int armorSlot, int itemId)
  {
    Debug.Log((object) ("slot: " + (object) armorSlot + ", itemid: " + (object) itemId));
    this.armor[armorSlot] = itemId;
    this.totalArmor = 0;
    foreach (int key in this.armor)
    {
      if (key != -1)
        this.totalArmor += ItemManager.Instance.allItems[key].armor;
    }
  }

  public void Died()
  {
    this.currentHp = 0;
    this.dead = true;
  }

  public int Damage(int damageDone)
  {
    this.currentHp -= damageDone;
    if (this.currentHp < 0)
      this.currentHp = 0;
    return this.currentHp;
  }
}
