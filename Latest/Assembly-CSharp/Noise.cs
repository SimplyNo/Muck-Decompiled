// Decompiled with JetBrains decompiler
// Type: Noise
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
      float x = (float) consistentRandom.Next(-100000, 100000) + offset.x;
      float y = (float) consistentRandom.Next(-100000, 100000) + offset.y;
      vector2Array[index] = new Vector2(x, y);
    }
    if ((double) scale <= 0.0)
      scale = 0.0001f;
    float b = float.MinValue;
    float a = float.MaxValue;
    float num1 = (float) mapWidth / 2f;
    float num2 = (float) mapHeight / 2f;
    for (int index1 = 0; index1 < mapHeight; ++index1)
    {
      for (int index2 = 0; index2 < mapWidth; ++index2)
      {
        float num3 = 1f;
        float num4 = 1f;
        float num5 = 0.0f;
        for (int index3 = 0; index3 < octaves; ++index3)
        {
          double num6 = ((double) index2 - (double) num1) / (double) scale * (double) num4 + (double) vector2Array[index3].x;
          float num7 = (float) (((double) index2 - (double) num1) / (double) scale * ((double) num4 * (double) blend)) + vector2Array[index3].x;
          float num8 = ((float) index1 - num2) / scale * num4 + vector2Array[index3].y;
          float num9 = (float) (((double) index1 - (double) num2) / (double) scale * ((double) num4 * (double) blend)) + vector2Array[index3].y;
          double num10 = (double) num7 * (double) blendStrength;
          float num11 = (float) ((double) Mathf.PerlinNoise((float) (num6 + num10), num8 + num9 * blendStrength) * 2.0 - 1.0);
          num5 += num11 * num3;
          num3 *= persistance;
          num4 *= lacunarity;
        }
        if ((double) num5 > (double) b)
          b = num5;
        else if ((double) num5 < (double) a)
          a = num5;
        numArray[index2, index1] = num5;
      }
    }
    for (int index1 = 0; index1 < mapHeight; ++index1)
    {
      for (int index2 = 0; index2 < mapWidth; ++index2)
        numArray[index2, index1] = Mathf.InverseLerp(a, b, numArray[index2, index1]);
    }
    return numArray;
  }
}
