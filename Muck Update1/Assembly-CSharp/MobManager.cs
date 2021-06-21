// Decompiled with JetBrains decompiler
// Type: MobManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
  public Dictionary<int, Mob> mobs;
  private static int mobId;
  public static MobManager Instance;
  public LayerMask whatIsRaycastable;
  public bool attatchDebug;
  public GameObject debug;

  private void Awake()
  {
    MobManager.Instance = this;
    MobManager.mobId = 0;
    this.mobs = new Dictionary<int, Mob>();
  }

  public void AddMob(Mob c, int id)
  {
    c.SetId(id);
    this.mobs.Add(id, c);
    if (!this.attatchDebug)
      return;
    Object.Instantiate<GameObject>(this.debug, c.transform).GetComponentInChildren<DebugObject>().text = nameof (id) + (object) id;
  }

  public int GetActiveEnemies()
  {
    int num = 0;
    foreach (Mob mob in this.mobs.Values)
    {
      if (mob.mobType.behaviour != MobType.MobBehaviour.Neutral)
        ++num;
    }
    return num;
  }

  public int GetNextId() => MobManager.mobId++;

  public void RemoveMob(int mobId) => this.mobs.Remove(mobId);
}
