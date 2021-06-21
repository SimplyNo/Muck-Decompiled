// Decompiled with JetBrains decompiler
// Type: Billboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class Billboard : MonoBehaviour
{
  private Vector3 defaultScale;
  public bool xz;
  public bool affectScale;
  private Transform t;

  private void Awake() => this.defaultScale = ((Component) this).get_transform().get_localScale();

  private void Update()
  {
    if (!Object.op_Implicit((Object) this.t))
    {
      if (!Object.op_Equality((Object) this.t, (Object) null) && ((Component) this.t).get_gameObject().get_activeInHierarchy())
        return;
      if (Object.op_Implicit((Object) PlayerMovement.Instance))
      {
        this.t = PlayerMovement.Instance.playerCam;
      }
      else
      {
        if (!Object.op_Implicit((Object) Camera.get_main()))
          return;
        this.t = ((Component) Camera.get_main()).get_transform();
      }
    }
    else
    {
      ((Component) this).get_transform().LookAt(this.t);
      if (!this.xz)
      {
        Transform transform = ((Component) this).get_transform();
        Quaternion rotation = ((Component) this).get_transform().get_rotation();
        Quaternion quaternion = Quaternion.Euler(0.0f, (float) (((Quaternion) ref rotation).get_eulerAngles().y + 180.0), 0.0f);
        transform.set_rotation(quaternion);
      }
      if (!this.affectScale)
        return;
      ((Component) this).get_transform().set_localScale(this.defaultScale);
    }
  }

  public Billboard() => base.\u002Ector();
}
