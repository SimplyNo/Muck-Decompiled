// Decompiled with JetBrains decompiler
// Type: MobHpBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class MobHpBar : MonoBehaviour
{
  public GameObject attachedObject;
  private HitableMob mob;
  private Vector3 offsetPos;
  public GameObject buffIcon;
  public Image hpBar;

  public void SetMob(GameObject mob)
  {
    this.buffIcon.SetActive(false);
    this.gameObject.SetActive(true);
    this.attachedObject = mob;
    Bounds bounds = mob.GetComponent<Collider>().bounds;
    this.mob = mob.transform.root.GetComponent<HitableMob>();
    Vector3 vector3_1 = new Vector3((float) this.mob.hp / (float) this.mob.maxHp, 1f, 1f);
    Vector3 vector3_2 = bounds.center - mob.transform.position;
    this.offsetPos = new Vector3(0.0f, bounds.extents.y + 0.5f, 0.0f) + vector3_2;
    SendToBossUi component1 = mob.transform.root.GetComponent<SendToBossUi>();
    if ((bool) (Object) component1)
      BossUI.Instance.SetBoss(component1.GetComponent<Mob>());
    Mob component2 = mob.transform.root.GetComponent<Mob>();
    if (!(bool) (Object) component2 || !component2.IsBuff())
      return;
    this.buffIcon.SetActive(true);
  }

  public void RemoveMob()
  {
    this.gameObject.SetActive(false);
    this.buffIcon.SetActive(false);
    this.attachedObject = (GameObject) null;
    this.mob = (HitableMob) null;
  }

  private void Update()
  {
    if (!(bool) (Object) this.mob)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      this.transform.position = this.mob.transform.position + this.offsetPos;
      this.hpBar.transform.localScale = Vector3.Lerp(this.hpBar.transform.localScale, new Vector3((float) this.mob.hp / (float) this.mob.maxHp, 1f, 1f), Time.deltaTime * 10f);
    }
  }
}
