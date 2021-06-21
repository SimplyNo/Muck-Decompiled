// Decompiled with JetBrains decompiler
// Type: AddToResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class AddToResources : MonoBehaviour
{
  public bool chest;

  private void Start()
  {
    int nextId = ResourceManager.Instance.GetNextId();
    ((Hitable) ((Component) this).GetComponent<Hitable>()).SetId(nextId);
    ResourceManager.Instance.AddObject(nextId, ((Component) this).get_gameObject());
    Object.Destroy((Object) this);
    if (this.chest)
    {
      Chest componentInChildren = (Chest) ((Component) this).GetComponentInChildren<Chest>();
      ChestManager.Instance.AddChest(componentInChildren, nextId);
    }
    ((Component) this).get_transform().SetParent((Transform) null);
  }

  public AddToResources() => base.\u002Ector();
}
