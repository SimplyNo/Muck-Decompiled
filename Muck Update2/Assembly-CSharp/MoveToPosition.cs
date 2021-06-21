// Decompiled with JetBrains decompiler
// Type: MoveToPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class MoveToPosition : MonoBehaviour
{
  public Transform target;

  public void LateUpdate() => ((Component) this).get_transform().set_position(this.target.get_position());

  public MoveToPosition() => base.\u002Ector();
}
