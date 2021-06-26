// Decompiled with JetBrains decompiler
// Type: SpawnZone
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
  public bool despawn = true;
  public int entityCap = 3;
  public float respawnTime = 60f;
  public float updateRate = 2f;
  private bool rendered;
  public LayerMask whatIsGround;

  private void Start()
  {
    this.entityQueue = this.entityCap;
    this.entityBuffer = this.entityCap;
    if (!Application.isPlaying)
      return;
    this.entities = new List<GameObject>();
    this.InvokeRepeating("SlowUpdate", UnityEngine.Random.Range(0.0f, this.updateRate), this.updateRate);
  }

  private void SlowUpdate()
  {
    if (!LocalClient.serverOwner || GameManager.state != GameManager.GameState.Playing)
      return;
    this.entities.RemoveAll((Predicate<GameObject>) (item => (UnityEngine.Object) item == (UnityEngine.Object) null));
    if (this.entities.Count + this.entityBuffer < this.entityCap)
    {
      this.Invoke("QueueEntity", this.respawnTime);
      ++this.entityBuffer;
    }
    bool flag = false;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (!((UnityEngine.Object) playerManager == (UnityEngine.Object) null) && (double) Vector3.Distance(this.transform.position, playerManager.transform.position) < (double) this.renderDistance)
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
    Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
    Vector3 origin = this.transform.position + new Vector3(insideUnitCircle.x, 0.0f, insideUnitCircle.y) * this.roamDistance;
    origin.y = 200f;
    RaycastHit hitInfo;
    return Physics.Raycast(origin, Vector3.down, out hitInfo, 500f, (int) this.whatIsGround) ? hitInfo.point : Vector3.zero;
  }

  public abstract GameObject LocalSpawnEntity(
    Vector3 pos,
    int entityType,
    int objectId,
    int zoneId);

  public void ToggleEntities(bool show)
  {
    foreach (GameObject entity in this.entities)
    {
      if ((UnityEngine.Object) entity != (UnityEngine.Object) null)
        entity.gameObject.SetActive(show);
    }
  }

  private void OnDrawGizmos()
  {
  }

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;
}
