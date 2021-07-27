// Decompiled with JetBrains decompiler
// Type: TestSpawnTrees
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TestSpawnTrees : MonoBehaviour
{
  public GameObject resourcePrefab;
  public List<GameObject> resources;

  private void Start()
  {
    this.resources.Add(this.SpawnTree(this.transform.position));
    if (!(bool) (Object) ResourceManager.Instance)
      return;
    ResourceManager.Instance.AddResources(this.resources);
  }

  private GameObject SpawnTree(Vector3 pos)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(this.resourcePrefab, pos, Quaternion.identity);
    gameObject.GetComponentInChildren<SharedObject>().SetId(ResourceManager.Instance.GetNextId());
    gameObject.SetActive(true);
    return gameObject;
  }
}
