// Decompiled with JetBrains decompiler
// Type: WorldUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

public class WorldUtility
{
  public static TextureData.TerrainType WorldHeightToBiome(float height)
  {
    float heightMultiplier = MapGenerator.Instance.terrainData.heightMultiplier;
    height /= heightMultiplier;
    TextureData.Layer[] layers = MapGenerator.Instance.textureData.layers;
    for (int index = layers.Length - 1; index > 0; --index)
    {
      if ((double) height >= (double) layers[index].startHeight)
        return layers[index].type;
    }
    return TextureData.TerrainType.Water;
  }
}
