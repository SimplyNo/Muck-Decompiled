// Decompiled with JetBrains decompiler
// Type: NoiseData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[CreateAssetMenu]
public class NoiseData : UpdateableData
{
  public float noiseScale;
  [Range(1f, 20f)]
  public int octaves;
  [Range(0.0f, 1f)]
  public float persistance;
  public float lacunarity;
  public float blend;
  public float blendStrength;
  public int seed;
  public Vector2 offset;
}
