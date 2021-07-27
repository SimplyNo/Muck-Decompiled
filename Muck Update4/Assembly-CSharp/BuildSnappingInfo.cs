// Decompiled with JetBrains decompiler
// Type: BuildSnappingInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class BuildSnappingInfo : MonoBehaviour
{
  public Vector3[] position;
  public bool half;

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    foreach (Vector3 vector3 in this.position)
      Gizmos.DrawCube(this.transform.position + vector3 * 1f, Vector3.one * 0.1f);
  }
}
