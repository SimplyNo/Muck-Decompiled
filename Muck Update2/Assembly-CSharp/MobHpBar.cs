// Decompiled with JetBrains decompiler
// Type: MobHpBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    ((Component) this).get_gameObject().SetActive(true);
    this.attachedObject = mob;
    Bounds bounds = ((Collider) mob.GetComponent<Collider>()).get_bounds();
    this.mob = (HitableMob) ((Component) mob.get_transform().get_root()).GetComponent<HitableMob>();
    Vector3 vector3_1 = new Vector3((float) this.mob.hp / (float) this.mob.maxHp, 1f, 1f);
    Vector3 vector3_2 = Vector3.op_Subtraction(((Bounds) ref bounds).get_center(), mob.get_transform().get_position());
    this.offsetPos = Vector3.op_Addition(new Vector3(0.0f, (float) (((Bounds) ref bounds).get_extents().y + 0.5), 0.0f), vector3_2);
    SendToBossUi component1 = (SendToBossUi) ((Component) mob.get_transform().get_root()).GetComponent<SendToBossUi>();
    if (Object.op_Implicit((Object) component1))
      BossUI.Instance.SetBoss((Mob) ((Component) component1).GetComponent<Mob>());
    Mob component2 = (Mob) ((Component) mob.get_transform().get_root()).GetComponent<Mob>();
    if (!Object.op_Implicit((Object) component2) || !component2.IsBuff())
      return;
    this.buffIcon.SetActive(true);
  }

  public void RemoveMob()
  {
    ((Component) this).get_gameObject().SetActive(false);
    this.buffIcon.SetActive(false);
    this.attachedObject = (GameObject) null;
    this.mob = (HitableMob) null;
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) this.mob))
    {
      ((Component) this).get_gameObject().SetActive(false);
    }
    else
    {
      ((Component) this).get_transform().set_position(Vector3.op_Addition(this.attachedObject.get_transform().get_position(), this.offsetPos));
      float num = (float) this.mob.hp / (float) this.mob.maxHp;
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(num, 1f, 1f);
      ((Component) this.hpBar).get_transform().set_localScale(Vector3.Lerp(((Component) this.hpBar).get_transform().get_localScale(), vector3, Time.get_deltaTime() * 10f));
    }
  }

  public MobHpBar() => base.\u002Ector();
}
