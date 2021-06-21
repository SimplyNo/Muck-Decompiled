// Decompiled with JetBrains decompiler
// Type: NoiseData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
