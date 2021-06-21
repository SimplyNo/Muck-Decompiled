// Decompiled with JetBrains decompiler
// Type: TextureGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class TextureGenerator
{
  public static Texture2D textureFromColorMap(Color[] colorMap, int width, int height)
  {
    Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
    texture2D.filterMode = FilterMode.Point;
    texture2D.wrapMode = TextureWrapMode.Clamp;
    texture2D.SetPixels(colorMap);
    texture2D.Apply();
    return texture2D;
  }

  public static Texture2D TextureFromHeightMap(float[,] heightMap)
  {
    int length1 = heightMap.GetLength(0);
    int length2 = heightMap.GetLength(1);
    Color[] colorMap = new Color[length1 * length2];
    for (int index1 = 0; index1 < length2; ++index1)
    {
      for (int index2 = 0; index2 < length1; ++index2)
        colorMap[index1 * length1 + index2] = Color.Lerp(Color.black, Color.white, heightMap[index2, index1]);
    }
    return TextureGenerator.textureFromColorMap(colorMap, length1, length2);
  }

  public static Texture2D ColorTextureFromHeightMap(
    float[,] heightMap,
    TextureData textureData)
  {
    int length1 = heightMap.GetLength(0);
    int length2 = heightMap.GetLength(1);
    Color[] colorMap = new Color[length1 * length2];
    for (int index1 = 0; index1 < length2; ++index1)
    {
      for (int index2 = 0; index2 < length1; ++index2)
      {
        int index3 = index1 * length1 + index2;
        colorMap[index3] = TextureGenerator.GetColor(heightMap[index2, 240 - index1], textureData);
      }
    }
    return TextureGenerator.textureFromColorMap(colorMap, length1, length2);
  }

  public static Color GetColor(float height, TextureData textureData)
  {
    for (int index = textureData.layers.Length - 1; index >= 0; --index)
    {
      if ((double) height - 0.100000001490116 > (double) textureData.layers[index].startHeight)
        return textureData.layers[index].tint;
    }
    return textureData.layers[0].tint;
  }
}
