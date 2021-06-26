// Decompiled with JetBrains decompiler
// Type: MeshData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    mesh.vertices = this.vertices;
    mesh.triangles = this.triangles;
    mesh.uv = this.uvs;
    mesh.RecalculateNormals();
    return mesh;
  }
}
