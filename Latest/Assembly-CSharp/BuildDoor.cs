// Decompiled with JetBrains decompiler
// Type: BuildDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using UnityEngine;

public class BuildDoor : MonoBehaviour
{
  public BuildDoor.Door[] doors;

  public BuildDoor() => base.\u002Ector();

  [Serializable]
  public class Door
  {
    public Hitable hitable;
    public DoorInteractable doorInteractable;

    public void SetId(int id)
    {
      this.hitable.SetId(id);
      this.doorInteractable.SetId(id);
      ResourceManager.Instance.AddObject(id, ((Component) this.hitable).get_gameObject());
    }
  }
}
