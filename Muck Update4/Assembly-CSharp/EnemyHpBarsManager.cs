// Decompiled with JetBrains decompiler
// Type: EnemyHpBarsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarsManager : MonoBehaviour
{
  public GameObject hpBarPrefab;
  private int nHpBars = 5;
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
      this.hpBars[index] = Object.Instantiate<GameObject>(this.hpBarPrefab).GetComponent<MobHpBar>();
      this.hpBars[index].gameObject.SetActive(false);
    }
  }

  private void Update()
  {
    if (!(bool) (Object) this.camera)
    {
      if (!(bool) (Object) PlayerMovement.Instance)
        return;
      this.camera = PlayerMovement.Instance.playerCam;
    }
    RaycastHit[] raycastHitArray = Physics.SphereCastAll(this.camera.position, 4f, this.camera.forward, 100f, (int) this.whatIsEnemy);
    for (int index = 0; index < this.hpBars.Length; ++index)
    {
      if ((Object) this.hpBars[index].attachedObject != (Object) null)
      {
        bool flag = false;
        foreach (RaycastHit raycastHit in raycastHitArray)
        {
          if ((Object) raycastHit.transform.gameObject == (Object) this.hpBars[index].attachedObject)
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
      if (!hit.transform.CompareTag("NoHpBar"))
      {
        bool flag = false;
        for (int index = 0; index < this.nHpBars; ++index)
        {
          if ((Object) this.hpBars[index].attachedObject == (Object) hit.transform.gameObject)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          MobHpBar availableHpBar = this.FindAvailableHpBar(hit);
          if (!((Object) availableHpBar == (Object) null))
            availableHpBar.SetMob(hit.transform.gameObject);
        }
      }
    }
  }

  private MobHpBar FindAvailableHpBar(RaycastHit hit)
  {
    foreach (MobHpBar hpBar in this.hpBars)
    {
      if (!hpBar.gameObject.activeInHierarchy)
        return hpBar;
    }
    return (MobHpBar) null;
  }
}
