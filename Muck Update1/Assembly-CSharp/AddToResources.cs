// Decompiled with JetBrains decompiler
// Type: AddToResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
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
