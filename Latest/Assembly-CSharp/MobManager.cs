// Decompiled with JetBrains decompiler
// Type: MobManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    ((DebugObject) ((GameObject) Object.Instantiate<GameObject>((M0) this.debug, ((Component) c).get_transform())).GetComponentInChildren<DebugObject>()).text = nameof (id) + (object) id;
  }

  public int GetActiveEnemies()
  {
    int num = 0;
    foreach (Mob mob in this.mobs.Values)
    {
      if (!((Component) mob).get_gameObject().CompareTag("DontCount") && mob.mobType.behaviour != MobType.MobBehaviour.Neutral)
        ++num;
    }
    return num;
  }

  public int GetNextId() => MobManager.mobId++;

  public void RemoveMob(int mobId) => this.mobs.Remove(mobId);

  public MobManager() => base.\u002Ector();
}
