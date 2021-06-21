// Decompiled with JetBrains decompiler
// Type: TestSpawnTrees
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TestSpawnTrees : MonoBehaviour
{
  public GameObject resourcePrefab;
  public List<GameObject> resources;

  private void Start()
  {
    this.resources.Add(this.SpawnTree(((Component) this).get_transform().get_position()));
    if (!Object.op_Implicit((Object) ResourceManager.Instance))
      return;
    ResourceManager.Instance.AddResources(this.resources);
  }

  private GameObject SpawnTree(Vector3 pos)
  {
    M0 m0 = Object.Instantiate<GameObject>((M0) this.resourcePrefab, pos, Quaternion.get_identity());
    ((SharedObject) ((GameObject) m0).GetComponentInChildren<SharedObject>()).SetId(ResourceManager.Instance.GetNextId());
    ((GameObject) m0).SetActive(true);
    return (GameObject) m0;
  }

  public TestSpawnTrees() => base.\u002Ector();
}
