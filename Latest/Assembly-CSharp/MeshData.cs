// Decompiled with JetBrains decompiler
// Type: MeshData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MeshData
{
  public Vector3[] vertices;
  public int[] triangles;
  public Vector2[] uvs;
  private int triangleIndex;

  public MeshData(int meshWidth, int meshHeight)
  {
    this.vertices = new Vector3[meshWidth * meshHeight];
    this.uvs = new Vector2[meshWidth * meshHeight];
    this.triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
  }

  public void AddTriangle(int a, int b, int c)
  {
    this.triangles[this.triangleIndex] = a;
    this.triangles[this.triangleIndex + 1] = b;
    this.triangles[this.triangleIndex + 2] = c;
    this.triangleIndex += 3;
  }

  public Mesh CreateMesh()
  {
    Mesh mesh = new Mesh();
    mesh.set_vertices(this.vertices);
    mesh.set_triangles(this.triangles);
    mesh.set_uv(this.uvs);
    mesh.RecalculateNormals();
    return mesh;
  }
}
