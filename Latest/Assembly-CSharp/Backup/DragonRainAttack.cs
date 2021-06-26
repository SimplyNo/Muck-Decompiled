// Decompiled with JetBrains decompiler
// Type: DragonRainAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DragonRainAttack : MonoBehaviour
{
  public InventoryItem fireball;
  private float height = 120f;
  private int balls;
  private float delay = 0.5f;

  private void Awake()
  {
    if (!LocalClient.serverOwner)
      return;
    this.Invoke("SpawnFireBall", this.delay);
  }

  private void SpawnFireBall()
  {
    PlayerManager randomAlivePlayer = this.GetRandomAlivePlayer();
    if ((Object) randomAlivePlayer == (Object) null)
      return;
    Vector3 vector3 = randomAlivePlayer.transform.position + (Random.insideUnitSphere * 15f + Vector3.up * this.height);
    Vector3 down = Vector3.down;
    int id = Dragon.Instance.transform.root.GetComponent<Hitable>().GetId();
    ServerSend.MobSpawnProjectile(vector3, down, 0.0f, this.fireball.id, id);
    ProjectileController.Instance.SpawnMobProjectile(vector3, down, 0.0f, this.fireball.id, id);
    ++this.balls;
    if (this.balls >= 6)
      Object.Destroy((Object) this.gameObject);
    else
      this.Invoke(nameof (SpawnFireBall), this.delay);
  }

  private PlayerManager GetRandomAlivePlayer()
  {
    List<PlayerManager> playerManagerList = new List<PlayerManager>();
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((bool) (Object) playerManager && !playerManager.dead && !playerManager.disconnected)
        playerManagerList.Add(playerManager);
    }
    return playerManagerList.Count == 0 ? (PlayerManager) null : playerManagerList[Random.Range(0, playerManagerList.Count)];
  }
}
