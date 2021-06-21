// Decompiled with JetBrains decompiler
// Type: Noise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public static class Noise
{
  public static float[,] GenerateNoiseMap(
    int mapWidth,
    int mapHeight,
    int seed,
    float scale,
    int octaves,
    float persistance,
    float lacunarity,
    float blend,
    float blendStrength,
    Vector2 offset)
  {
    float[,] numArray = new float[mapWidth, mapHeight];
    ConsistentRandom consistentRandom = new ConsistentRandom(seed);
    DebugNet.r.Add("seed: " + (object) seed + ", first random: " + (object) consistentRandom.Next(0, 10000));
    Vector2[] vector2Array = new Vector2[octaves];
    for (int index = 0; index < octaves; ++index)
    {
      float num1 = (float) consistentRandom.Next(-100000, 100000) + (float) offset.x;
      float num2 = (float) consistentRandom.Next(-100000, 100000) + (float) offset.y;
      vector2Array[index] = new Vector2(num1, num2);
    }
    if ((double) scale <= 0.0)
      scale = 0.0001f;
    float num3 = float.MinValue;
    float num4 = float.MaxValue;
    float num5 = (float) mapWidth / 2f;
    float num6 = (float) mapHeight / 2f;
    for (int index1 = 0; index1 < mapHeight; ++index1)
    {
      for (int index2 = 0; index2 < mapWidth; ++index2)
      {
        float num1 = 1f;
        float num2 = 1f;
        float num7 = 0.0f;
        for (int index3 = 0; index3 < octaves; ++index3)
        {
          double num8 = ((double) index2 - (double) num5) / (double) scale * (double) num2 + vector2Array[index3].x;
          float num9 = (float) (((double) index2 - (double) num5) / (double) scale * ((double) num2 * (double) blend) + vector2Array[index3].x);
          float num10 = (float) (((double) index1 - (double) num6) / (double) scale * (double) num2 + vector2Array[index3].y);
          float num11 = (float) (((double) index1 - (double) num6) / (double) scale * ((double) num2 * (double) blend) + vector2Array[index3].y);
          double num12 = (double) num9 * (double) blendStrength;
          float num13 = (float) ((double) Mathf.PerlinNoise((float) (num8 + num12), num10 + num11 * blendStrength) * 2.0 - 1.0);
          num7 += num13 * num1;
          num1 *= persistance;
          num2 *= lacunarity;
        }
        if ((double) num7 > (double) num3)
          num3 = num7;
        else if ((double) num7 < (double) num4)
          num4 = num7;
        numArray[index2, index1] = num7;
      }
    }
    for (int index1 = 0; index1 < mapHeight; ++index1)
    {
      for (int index2 = 0; index2 < mapWidth; ++index2)
        numArray[index2, index1] = Mathf.InverseLerp(num4, num3, numArray[index2, index1]);
    }
    return numArray;
  }
}
