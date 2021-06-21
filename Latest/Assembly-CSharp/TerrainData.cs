// Decompiled with JetBrains decompiler
// Type: TerrainData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

[CreateAssetMenu]
public class TerrainData : UpdateableData
{
  public float uniformScale = 2.5f;
  public bool useFalloff;
  public float heightMultiplier;
  public AnimationCurve heightCurve;

  public float minHeight => this.uniformScale * this.heightMultiplier * this.heightCurve.Evaluate(0.0f);

  public float maxHeight => this.uniformScale * this.heightMultiplier * this.heightCurve.Evaluate(1f);
}
