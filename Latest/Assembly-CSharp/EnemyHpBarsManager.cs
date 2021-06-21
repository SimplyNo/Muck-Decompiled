// Decompiled with JetBrains decompiler
// Type: EnemyHpBarsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarsManager : MonoBehaviour
{
  public GameObject hpBarPrefab;
  private int nHpBars;
  private MobHpBar[] hpBars;
  private float[] distances;
  private List<GameObject> onMobs;
  private Transform camera;
  public LayerMask whatIsEnemy;

  private void Awake()
  {
    this.onMobs = new List<GameObject>();
    this.hpBars = new MobHpBar[this.nHpBars];
    for (int index = 0; index < this.nHpBars; ++index)
    {
      this.hpBars[index] = (MobHpBar) ((GameObject) Object.Instantiate<GameObject>((M0) this.hpBarPrefab)).GetComponent<MobHpBar>();
      ((Component) this.hpBars[index]).get_gameObject().SetActive(false);
    }
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) this.camera))
    {
      if (!Object.op_Implicit((Object) PlayerMovement.Instance))
        return;
      this.camera = PlayerMovement.Instance.playerCam;
    }
    float num = 4f;
    RaycastHit[] raycastHitArray = Physics.SphereCastAll(this.camera.get_position(), num, this.camera.get_forward(), 100f, LayerMask.op_Implicit(this.whatIsEnemy));
    for (int index = 0; index < this.hpBars.Length; ++index)
    {
      if (Object.op_Inequality((Object) this.hpBars[index].attachedObject, (Object) null))
      {
        bool flag = false;
        foreach (RaycastHit raycastHit in raycastHitArray)
        {
          if (Object.op_Equality((Object) ((Component) ((RaycastHit) ref raycastHit).get_transform()).get_gameObject(), (Object) this.hpBars[index].attachedObject))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          this.hpBars[index].RemoveMob();
      }
    }
    foreach (RaycastHit hit in raycastHitArray)
    {
      if (!((Component) ((RaycastHit) ref hit).get_transform()).CompareTag("NoHpBar"))
      {
        bool flag = false;
        for (int index = 0; index < this.nHpBars; ++index)
        {
          if (Object.op_Equality((Object) this.hpBars[index].attachedObject, (Object) ((Component) ((RaycastHit) ref hit).get_transform()).get_gameObject()))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          MobHpBar availableHpBar = this.FindAvailableHpBar(hit);
          if (!Object.op_Equality((Object) availableHpBar, (Object) null))
            availableHpBar.SetMob(((Component) ((RaycastHit) ref hit).get_transform()).get_gameObject());
        }
      }
    }
  }

  private MobHpBar FindAvailableHpBar(RaycastHit hit)
  {
    foreach (MobHpBar hpBar in this.hpBars)
    {
      if (!((Component) hpBar).get_gameObject().get_activeInHierarchy())
        return hpBar;
    }
    return (MobHpBar) null;
  }

  public EnemyHpBarsManager() => base.\u002Ector();
}
