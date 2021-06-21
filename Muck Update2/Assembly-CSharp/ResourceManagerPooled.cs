// Decompiled with JetBrains decompiler
// Type: ResourceManagerPooled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerPooled : MonoBehaviour
{
  public Dictionary<int, GameObject> list;
  public GameObject debug;
  public bool attatchDebug;
  public static ResourceManagerPooled Instance;

  private void Awake()
  {
    ResourceManagerPooled.Instance = this;
    this.list = new Dictionary<int, GameObject>();
  }

  public void PopulateTrees(List<GameObject>[] trees)
  {
    for (int index1 = 0; index1 < trees.Length; ++index1)
    {
      for (int index2 = 0; index2 < trees[index1].Count; ++index2)
      {
        GameObject o = trees[index1][index2] = trees[index1][index2];
        this.AddObject(((SharedObject) o.GetComponent<SharedObject>()).GetId(), o);
      }
    }
  }

  public void AddObject(int key, GameObject o)
  {
    if (this.list.ContainsKey(key))
    {
      Debug.Log((object) "Tried to add same key twice to resource manager, returning...");
    }
    else
    {
      this.list.Add(key, o);
      if (!this.attatchDebug)
        return;
      ((DebugObject) ((GameObject) Object.Instantiate<GameObject>((M0) this.debug, o.get_transform())).GetComponentInChildren<DebugObject>()).text = "id" + (object) key;
    }
  }

  public void RemoveItem(int id)
  {
    GameObject gameObject = this.list[id];
    if (gameObject.get_activeInHierarchy())
    {
      gameObject.SetActive(false);
    }
    else
    {
      this.list.Remove(id);
      Object.Destroy((Object) gameObject);
    }
  }

  public bool RemoveInteractItem(int id)
  {
    if (!this.list.ContainsKey(id))
      return false;
    GameObject gameObject = this.list[id];
    this.list.Remove(id);
    Object.Destroy((Object) gameObject);
    return true;
  }

  public ResourceManagerPooled() => base.\u002Ector();
}
