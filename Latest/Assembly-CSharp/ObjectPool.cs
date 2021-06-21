// Decompiled with JetBrains decompiler
// Type: ObjectPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
  public List<SharedObject>[] pools;
  private ResourceGenerator gen;

  private void Awake() => this.InitPools();

  private void InitPools()
  {
    this.gen = (ResourceGenerator) ((Component) this).GetComponent<ResourceGenerator>();
    this.pools = new List<SharedObject>[this.gen.resourcePrefabs.Length];
    for (int index = 0; index < this.gen.resourcePrefabs.Length; ++index)
      this.pools[index] = new List<SharedObject>();
  }

  public int ActivateGameObject(PooledObject activatedObject) => 0;

  public void DeactivateGameObject(PooledObject deactivatedObject)
  {
  }

  public ObjectPool() => base.\u002Ector();
}
