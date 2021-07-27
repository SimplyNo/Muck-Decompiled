// Decompiled with JetBrains decompiler
// Type: BuildDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
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
