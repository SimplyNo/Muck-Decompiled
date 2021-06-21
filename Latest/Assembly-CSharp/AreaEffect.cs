// Decompiled with JetBrains decompiler
// Type: AreaEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{
  private int damage;
  private List<GameObject> actorsHit;

  public void SetDamage(int d)
  {
    this.damage = d;
    ((Collider) ((Component) this).GetComponent<Collider>()).set_enabled(true);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (((Component) other).get_gameObject().CompareTag("Build"))
      return;
    Hitable component = (Hitable) ((Component) other).GetComponent<Hitable>();
    if (Object.op_Equality((Object) component, (Object) null) || ((Component) ((Component) other).get_transform().get_root()).CompareTag("Local"))
      return;
    component.Hit(this.damage, 0.0f, 3, ((Component) this).get_transform().get_position());
    Object.Destroy((Object) this);
  }

  public AreaEffect() => base.\u002Ector();
}
