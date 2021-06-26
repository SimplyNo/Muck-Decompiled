// Decompiled with JetBrains decompiler
// Type: AddToResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class AddToResources : MonoBehaviour
{
  public bool chest;

  private void Start()
  {
    int nextId = ResourceManager.Instance.GetNextId();
    this.GetComponent<Hitable>().SetId(nextId);
    ResourceManager.Instance.AddObject(nextId, this.gameObject);
    Object.Destroy((Object) this);
    if (this.chest)
    {
      Chest componentInChildren = this.GetComponentInChildren<Chest>();
      ChestManager.Instance.AddChest(componentInChildren, nextId);
    }
    this.transform.SetParent((Transform) null);
  }
}
