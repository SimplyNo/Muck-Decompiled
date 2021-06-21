// Decompiled with JetBrains decompiler
// Type: BuildSnappingInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class BuildSnappingInfo : MonoBehaviour
{
  public Vector3[] position;
  public bool half;

  private void OnDrawGizmos()
  {
    Gizmos.set_color(Color.get_red());
    foreach (Vector3 vector3 in this.position)
      Gizmos.DrawCube(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(vector3, 1f)), Vector3.op_Multiply(Vector3.get_one(), 0.1f));
  }

  public BuildSnappingInfo() => base.\u002Ector();
}
