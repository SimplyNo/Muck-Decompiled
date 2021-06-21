// Decompiled with JetBrains decompiler
// Type: MobServer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class MobServer : MonoBehaviour
{
  protected Mob mob;
  private float syncPositionInterval = 2f;
  protected float FindPositionInterval = 0.5f;
  protected float behaviourInterval = 0.1f;
  protected float[] findPositionInterval = new float[3]
  {
    0.5f,
    2f,
    5f
  };

  private void Awake() => this.mob = this.GetComponent<Mob>();

  protected void StartRoutines()
  {
    this.InvokeRepeating("SyncPosition", Random.Range(0.0f, this.syncPositionInterval), this.syncPositionInterval);
    this.Invoke("SyncFindNextPosition", Random.Range(0.0f, this.FindPositionInterval) + this.mob.mobType.spawnTime);
    this.InvokeRepeating("Behaviour", Random.Range(0.0f, this.behaviourInterval) + this.mob.mobType.spawnTime, this.behaviourInterval);
  }

  private void Update() => this.Behaviour();

  protected abstract void Behaviour();

  public abstract void TookDamage();

  private void SyncPosition()
  {
    using (Dictionary<int, PlayerManager>.ValueCollection.Enumerator enumerator = GameManager.players.Values.GetEnumerator())
    {
      while (enumerator.MoveNext() && (bool) (Object) enumerator.Current)
        ServerSend.MobMove(this.mob.GetId(), this.transform.position);
    }
  }

  protected void SyncFindNextPosition()
  {
    if (GameManager.players == null)
      return;
    Vector3 nextPosition = this.FindNextPosition();
    if (nextPosition == Vector3.zero)
      return;
    this.mob.SetDestination(nextPosition);
    ServerSend.MobSetDestination(this.mob.GetId(), nextPosition);
  }

  protected abstract Vector3 FindNextPosition();
}
