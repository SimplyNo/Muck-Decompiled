// Decompiled with JetBrains decompiler
// Type: MobServer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public abstract class MobServer : MonoBehaviour
{
  protected Mob mob;
  private float syncPositionInterval;
  protected float FindPositionInterval;
  protected float behaviourInterval;
  protected float[] findPositionInterval;
  protected int previousTargetId;

  private void Awake() => this.mob = (Mob) ((Component) this).GetComponent<Mob>();

  protected void StartRoutines()
  {
    this.InvokeRepeating("SyncPosition", Random.Range(0.0f, this.syncPositionInterval), this.syncPositionInterval);
    this.Invoke("SyncFindNextPosition", Random.Range(0.0f, this.FindPositionInterval) + this.mob.mobType.spawnTime);
    this.InvokeRepeating("Behaviour", Random.Range(0.0f, this.behaviourInterval) + this.mob.mobType.spawnTime, this.behaviourInterval);
  }

  private void Update()
  {
    if (!this.mob.ready)
      return;
    this.Behaviour();
  }

  protected abstract void Behaviour();

  public abstract void TookDamage();

  private void SyncPosition()
  {
    using (Dictionary<int, PlayerManager>.ValueCollection.Enumerator enumerator = GameManager.players.Values.GetEnumerator())
    {
      while (enumerator.MoveNext() && Object.op_Implicit((Object) enumerator.Current))
        ServerSend.MobMove(this.mob.GetId(), ((Component) this).get_transform().get_position());
    }
  }

  protected void SyncFindNextPosition()
  {
    if (GameManager.players == null)
      return;
    Vector3 nextPosition = this.FindNextPosition();
    if (this.mob.targetPlayerId != this.previousTargetId)
      ServerSend.SendMobTarget(this.mob.id, this.mob.targetPlayerId);
    this.previousTargetId = this.mob.targetPlayerId;
    if (Vector3.op_Equality(nextPosition, Vector3.get_zero()))
      return;
    this.mob.SetDestination(nextPosition);
    ServerSend.MobSetDestination(this.mob.GetId(), nextPosition);
  }

  protected abstract Vector3 FindNextPosition();

  protected MobServer() => base.\u002Ector();
}
