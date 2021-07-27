// Decompiled with JetBrains decompiler
// Type: ColorAtmosphere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ColorAtmosphere : MonoBehaviour
{
  [Header("Ground / Grass")]
  public TextureData textureData;
  public Color defaultGrassColor;
  public Color[] colorRange;
  public Color[] grassColorRange;
  public Color groundColor;
  public Color grassColor;
  public Material grass;
  [Header("Fog")]
  public DayCycle dayCycle;
  public Color[] fogColors;

  private void Awake()
  {
    ConsistentRandom consistentRandom = new ConsistentRandom(GameManager.GetSeed());
    float rand = (float) consistentRandom.NextDouble();
    Color randomBlendColor1 = this.FindRandomBlendColor(this.colorRange, rand);
    Color randomBlendColor2 = this.FindRandomBlendColor(this.grassColorRange, rand);
    this.textureData.layers[2].tint = randomBlendColor1;
    Color color = randomBlendColor2 * 1.5f;
    this.grass.SetColor("_BottomColor", randomBlendColor2);
    this.grass.SetColor("_TopColor", color);
    Debug.Log((object) ("grassindex: " + (object) (float) ((double) rand * (double) this.colorRange.Length)));
    Color randomBlendColor3 = this.FindRandomBlendColor(this.fogColors, (float) consistentRandom.NextDouble());
    RenderSettings.fogColor = randomBlendColor3;
    this.dayCycle.dayFog = randomBlendColor3;
  }

  public Color FindRandomBlendColor(Color[] colors, float rand)
  {
    rand *= (float) (colors.Length - 1);
    int index1 = Mathf.FloorToInt(rand);
    int index2 = Mathf.CeilToInt(rand);
    Color color1 = colors[index1];
    Color color2 = colors[index2];
    float num1 = rand - (float) index1;
    float num2 = 1f - num1;
    double num3 = (double) num1;
    return color1 * (float) num3 + color2 * num2;
  }
}
