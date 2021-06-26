// Decompiled with JetBrains decompiler
// Type: ShrineBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ShrineBoss : MonoBehaviour, SharedObject, Interactable
{
  private bool started;
  private int id;
  public MobType boss;
  public GameObject destroyShrineFx;

  private void Start()
  {
  }

  public void Interact()
  {
    if (this.started)
      return;
    ClientSend.Interact(this.id);
  }

  public void LocalExecute()
  {
  }

  public void AllExecute()
  {
    this.started = true;
    Debug.LogError((object) "Spawning");
    Object.Instantiate<GameObject>(this.destroyShrineFx, this.transform.position, this.destroyShrineFx.transform.rotation);
    this.Invoke("RemoveFromResources", 1.33f);
  }

  public void ServerExecute(int fromClient)
  {
    this.started = true;
    this.Invoke("SpawnBoss", 1.3f);
    Object.Instantiate<GameObject>(this.destroyShrineFx, this.transform.position, this.destroyShrineFx.transform.rotation);
    ServerSend.SendChatMessage(-1, "", "<color=orange>" + GameManager.players[fromClient].username + " summoned <color=red>" + this.boss.name);
  }

  private void SpawnBoss()
  {
    int nextId = MobManager.Instance.GetNextId();
    float bossMultiplier = (float) (0.899999976158142 + 0.100000001490116 * (double) GameManager.instance.GetPlayersAlive());
    float multiplier = 1.5f;
    if ((double) Random.Range(0.0f, 1f) < 0.200000002980232)
      multiplier = 1.5f;
    Vector3 position = this.transform.position;
    MobSpawner.Instance.ServerSpawnNewMob(nextId, this.boss.id, position, multiplier, bossMultiplier, Mob.BossType.BossShrine);
  }

  private void RemoveFromResources()
  {
    ResourceManager.Instance.RemoveInteractItem(this.id);
    Object.Destroy((Object) this.gameObject.transform.root.gameObject);
  }

  public void RemoveObject() => Object.Destroy((Object) this.gameObject);

  public string GetName() => "Challenge " + this.boss.name;

  public bool IsStarted() => this.started;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;
}
