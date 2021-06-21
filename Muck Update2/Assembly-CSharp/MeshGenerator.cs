// Decompiled with JetBrains decompiler
// Type: MeshGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public static class MeshGenerator
{
  public static MeshData GenerateTerrainMesh(
    float[,] heightMap,
    float heightMultiplier,
    AnimationCurve heightCurve,
    int levelOfDetail)
  {
    int length1 = heightMap.GetLength(0);
    int length2 = heightMap.GetLength(1);
    float num1 = (float) (length1 - 1) / -2f;
    float num2 = (float) (length2 - 1) / 2f;
    int num3 = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
    int num4 = (length1 - 1) / num3 + 1;
    MeshData meshData = new MeshData(num4, num4);
    int index1 = 0;
    for (int index2 = 0; index2 < length2; index2 += num3)
    {
      for (int index3 = 0; index3 < length1; index3 += num3)
      {
        meshData.vertices[index1] = new Vector3(num1 + (float) index3, heightCurve.Evaluate(heightMap[index3, index2]) * heightMultiplier, num2 - (float) index2);
        meshData.uvs[index1] = new Vector2((float) index3 / (float) length1, (float) index2 / (float) length2);
        if (index3 < length1 - 1 && index2 < length2 - 1)
        {
          meshData.AddTriangle(index1, index1 + num4 + 1, index1 + num4);
          meshData.AddTriangle(index1 + num4 + 1, index1, index1 + 1);
        }
        ++index1;
      }
    }
    return meshData;
  }
}
