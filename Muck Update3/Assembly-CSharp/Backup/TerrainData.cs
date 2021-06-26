// Decompiled with JetBrains decompiler
// Type: TerrainData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
