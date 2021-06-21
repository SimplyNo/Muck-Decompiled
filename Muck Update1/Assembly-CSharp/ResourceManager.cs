// Decompiled with JetBrains decompiler
// Type: ResourceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
  public static int globalId;
  public static int generatorSeedOffset;
  public Dictionary<int, GameObject> list;
  public Dictionary<int, GameObject> builds;
  public GameObject debug;
  public bool attatchDebug;
  public static ResourceManager Instance;

  private void Awake()
  {
    ResourceManager.Instance = this;
    this.list = new Dictionary<int, GameObject>();
    this.builds = new Dictionary<int, GameObject>();
    ResourceManager.generatorSeedOffset = 0;
    ResourceManager.globalId = 0;
  }

  public static int GetNextGenOffset()
  {
    int generatorSeedOffset = ResourceManager.generatorSeedOffset;
    ++ResourceManager.generatorSeedOffset;
    return generatorSeedOffset;
  }

  public void AddResources(List<GameObject>[] trees)
  {
    for (int index1 = 0; index1 < trees.Length; ++index1)
    {
      for (int index2 = 0; index2 < trees[index1].Count; ++index2)
      {
        GameObject o = trees[index1][index2] = trees[index1][index2];
        this.AddObject(o.GetComponentInChildren<SharedObject>().GetId(), o);
      }
    }
  }

  public void AddResources(List<GameObject> trees)
  {
    for (int index = 0; index < trees.Count; ++index)
    {
      GameObject o = trees[index] = trees[index];
      this.AddObject(o.GetComponentInChildren<SharedObject>().GetId(), o);
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
      Object.Instantiate<GameObject>(this.debug, o.transform).GetComponentInChildren<DebugObject>().text = "id" + (object) key;
    }
  }

  public void AddBuild(int key, GameObject o)
  {
    if (this.builds.ContainsKey(key))
      return;
    this.builds.Add(key, o);
    if (!this.attatchDebug)
      return;
    Object.Instantiate<GameObject>(this.debug, o.transform).GetComponentInChildren<DebugObject>().text = "id" + (object) key;
  }

  public void RemoveItem(int id)
  {
    GameObject gameObject = this.list[id];
    if (this.builds.ContainsKey(id))
      this.builds.Remove(id);
    this.list.Remove(id);
    Object.Destroy((Object) gameObject);
  }

  public bool RemoveInteractItem(int id)
  {
    if (!this.list.ContainsKey(id))
      return false;
    Interactable componentInChildren = this.list[id].GetComponentInChildren<Interactable>();
    this.list.Remove(id);
    componentInChildren.RemoveObject();
    return true;
  }

  public int GetNextId() => ResourceManager.globalId++;
}
