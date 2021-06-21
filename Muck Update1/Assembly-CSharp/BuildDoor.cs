// Decompiled with JetBrains decompiler
// Type: BuildDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class BuildDoor : MonoBehaviour
{
  public BuildDoor.Door[] doors;

  [Serializable]
  public class Door
  {
    public Hitable hitable;
    public DoorInteractable doorInteractable;

    public void SetId(int id)
    {
      this.hitable.SetId(id);
      this.doorInteractable.SetId(id);
      ResourceManager.Instance.AddObject(id, this.hitable.gameObject);
    }
  }
}
