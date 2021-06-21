// Decompiled with JetBrains decompiler
// Type: SpawnZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class SpawnZone : MonoBehaviour, SharedObject
{
  public int id;
  protected List<GameObject> entities;
  protected int entityBuffer;
  protected int entityQueue;
  public float roamDistance;
  public float renderDistance;
  public bool despawn;
  public int entityCap;
  public float respawnTime;
  public float updateRate;
  private bool rendered;
  public LayerMask whatIsGround;

  private void Start()
  {
    this.entityQueue = this.entityCap;
    this.entityBuffer = this.entityCap;
    if (!Application.get_isPlaying())
      return;
    this.entities = new List<GameObject>();
    this.InvokeRepeating("SlowUpdate", Random.Range(0.0f, this.updateRate), this.updateRate);
  }

  private void SlowUpdate()
  {
    if (!LocalClient.serverOwner || GameManager.state != GameManager.GameState.Playing)
      return;
    this.entities.RemoveAll((Predicate<GameObject>) (item => Object.op_Equality((Object) item, (Object) null)));
    if (this.entities.Count + this.entityBuffer < this.entityCap)
    {
      this.Invoke("QueueEntity", this.respawnTime);
      ++this.entityBuffer;
    }
    bool flag = false;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (!Object.op_Equality((Object) playerManager, (Object) null) && (double) Vector3.Distance(((Component) this).get_transform().get_position(), ((Component) playerManager).get_transform().get_position()) < (double) this.renderDistance)
      {
        flag = true;
        break;
      }
    }
    if (flag)
    {
      int entityQueue = this.entityQueue;
      for (int index = 0; index < entityQueue; ++index)
      {
        MonoBehaviour.print((object) "dequeing");
        this.ServerSpawnEntity();
        --this.entityQueue;
      }
      if (this.rendered)
        return;
      ServerSend.MobZoneToggle(true, this.id);
      this.rendered = true;
    }
    else
    {
      if (!this.rendered)
        return;
      this.rendered = false;
      if (!this.despawn)
        return;
      ServerSend.MobZoneToggle(false, this.id);
    }
  }

  private void QueueEntity() => ++this.entityQueue;

  public abstract void ServerSpawnEntity();

  public Vector3 FindRandomPos()
  {
    Vector2 insideUnitCircle = Random.get_insideUnitCircle();
    Vector3 vector3 = Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(new Vector3((float) insideUnitCircle.x, 0.0f, (float) insideUnitCircle.y), this.roamDistance));
    vector3.y = (__Null) 200.0;
    RaycastHit raycastHit;
    return Physics.Raycast(vector3, Vector3.get_down(), ref raycastHit, 500f, LayerMask.op_Implicit(this.whatIsGround)) ? ((RaycastHit) ref raycastHit).get_point() : Vector3.get_zero();
  }

  public abstract GameObject LocalSpawnEntity(
    Vector3 pos,
    int entityType,
    int objectId,
    int zoneId);

  public void ToggleEntities(bool show)
  {
    using (List<GameObject>.Enumerator enumerator = this.entities.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        GameObject current = enumerator.Current;
        if (Object.op_Inequality((Object) current, (Object) null))
          current.get_gameObject().SetActive(show);
      }
    }
  }

  private void OnDrawGizmos()
  {
  }

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  protected SpawnZone() => base.\u002Ector();
}
