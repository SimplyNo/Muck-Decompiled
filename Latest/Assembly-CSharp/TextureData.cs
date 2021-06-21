// Decompiled with JetBrains decompiler
// Type: TextureData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class TextureData : UpdatableData
{
  private const int textureSize = 512;
  private const TextureFormat textureFormat = TextureFormat.RGB565;
  public TextureData.Layer[] layers;
  private float savedMinHeight;
  private float savedMaxHeight;

  public void ApplyToMaterial(Material material)
  {
    material.SetInt("layerCount", this.layers.Length);
    material.SetColorArray("baseColours", ((IEnumerable<TextureData.Layer>) this.layers).Select<TextureData.Layer, Color>((Func<TextureData.Layer, Color>) (x => x.tint)).ToArray<Color>());
    material.SetFloatArray("baseStartHeights", ((IEnumerable<TextureData.Layer>) this.layers).Select<TextureData.Layer, float>((Func<TextureData.Layer, float>) (x => x.startHeight)).ToArray<float>());
    material.SetFloatArray("baseBlends", ((IEnumerable<TextureData.Layer>) this.layers).Select<TextureData.Layer, float>((Func<TextureData.Layer, float>) (x => x.blendStrength)).ToArray<float>());
    material.SetFloatArray("baseColourStrength", ((IEnumerable<TextureData.Layer>) this.layers).Select<TextureData.Layer, float>((Func<TextureData.Layer, float>) (x => x.tintStrength)).ToArray<float>());
    material.SetFloatArray("baseTextureScales", ((IEnumerable<TextureData.Layer>) this.layers).Select<TextureData.Layer, float>((Func<TextureData.Layer, float>) (x => x.textureScale)).ToArray<float>());
    Texture2DArray textureArray = this.GenerateTextureArray(((IEnumerable<TextureData.Layer>) this.layers).Select<TextureData.Layer, Texture2D>((Func<TextureData.Layer, Texture2D>) (x => x.texture)).ToArray<Texture2D>());
    material.SetTexture("baseTextures", (Texture) textureArray);
    this.UpdateMeshHeights(material, this.savedMinHeight, this.savedMaxHeight);
  }

  public void UpdateMeshHeights(Material material, float minHeight, float maxHeight)
  {
    this.savedMinHeight = minHeight;
    this.savedMaxHeight = maxHeight;
    material.SetFloat(nameof (minHeight), minHeight);
    material.SetFloat(nameof (maxHeight), maxHeight);
  }

  private Texture2DArray GenerateTextureArray(Texture2D[] textures)
  {
    Texture2DArray texture2Darray = new Texture2DArray(512, 512, textures.Length, TextureFormat.RGB565, true);
    for (int arrayElement = 0; arrayElement < textures.Length; ++arrayElement)
    {
      if ((bool) (UnityEngine.Object) textures[arrayElement])
        texture2Darray.SetPixels(textures[arrayElement].GetPixels(), arrayElement);
    }
    texture2Darray.Apply();
    return texture2Darray;
  }

  [Serializable]
  public class Layer
  {
    public Texture2D texture;
    public Color tint;
    [Range(0.0f, 1f)]
    public float tintStrength;
    [Range(0.0f, 1f)]
    public float startHeight;
    [Range(0.0f, 1f)]
    public float blendStrength;
    public float textureScale;
    public TextureData.TerrainType type;
  }

  public enum TerrainType
  {
    Water,
    Sand,
    Grass,
  }
}
