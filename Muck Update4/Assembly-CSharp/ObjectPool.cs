// Decompiled with JetBrains decompiler
// Type: ObjectPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
  public List<SharedObject>[] pools;
  private ResourceGenerator gen;

  private void Awake() => this.InitPools();

  private void InitPools()
  {
    this.gen = this.GetComponent<ResourceGenerator>();
    this.pools = new List<SharedObject>[this.gen.resourcePrefabs.Length];
    for (int index = 0; index < this.gen.resourcePrefabs.Length; ++index)
      this.pools[index] = new List<SharedObject>();
  }

  public int ActivateGameObject(PooledObject activatedObject) => 0;

  public void DeactivateGameObject(PooledObject deactivatedObject)
  {
  }
}
