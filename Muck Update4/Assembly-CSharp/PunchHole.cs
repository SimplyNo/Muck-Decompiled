// Decompiled with JetBrains decompiler
// Type: PunchHole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PunchHole : MonoBehaviour
{
  public LayerMask whatIsGround;
  public Transform ground;

  private void Update()
  {
    RaycastHit hitInfo;
    if (!Input.GetMouseButtonDown(0) || !Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, 1000f, (int) this.whatIsGround))
      return;
    int triangleIndex = hitInfo.triangleIndex;
    this.ground = hitInfo.transform;
    int[] triangles = this.transform.GetComponent<MeshFilter>().mesh.triangles;
    Vector3[] vertices = this.transform.GetComponent<MeshFilter>().mesh.vertices;
    Vector3 a = vertices[triangles[triangleIndex * 3]];
    Vector3 vector3 = vertices[triangles[triangleIndex * 3 + 1]];
    Vector3 b = vertices[triangles[triangleIndex * 3 + 2]];
    float num1 = Vector3.Distance(a, vector3);
    float num2 = Vector3.Distance(a, b);
    float num3 = Vector3.Distance(vector3, b);
    Vector3 v1;
    Vector3 v2;
    if ((double) num1 > (double) num2 && (double) num1 > (double) num3)
    {
      v1 = a;
      v2 = vector3;
    }
    else if ((double) num2 > (double) num1 && (double) num2 > (double) num3)
    {
      v1 = a;
      v2 = b;
    }
    else
    {
      v1 = vector3;
      v2 = b;
    }
    this.findVertex(v1);
    this.findVertex(v2);
  }

  private int findVertex(Vector3 v)
  {
    Vector3[] vertices = this.ground.GetComponent<MeshFilter>().mesh.vertices;
    for (int index = 0; index < vertices.Length; ++index)
    {
      if (vertices[index] == v)
        return index;
    }
    return -1;
  }

  private int findTriangle(Vector3 v1, Vector3 v2, int notTriIndex)
  {
    int[] triangles = this.ground.GetComponent<MeshFilter>().mesh.triangles;
    Vector3[] vertices = this.transform.GetComponent<MeshFilter>().mesh.vertices;
    int num = 0;
    do
      ;
    while (num < triangles.Length && num / 3 == notTriIndex);
    return -1;
  }
}
