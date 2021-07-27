// Decompiled with JetBrains decompiler
// Type: DayCycle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DayCycle : MonoBehaviour
{
  public bool alwaysDay;
  public static float dayDuration = 1f;
  public float nightDuration = 0.5f;
  public float timeSpeed = 0.01f;
  public static float time;
  public Transform sun;
  public Material sky;
  public float skyOffset;
  public MeshRenderer water;
  public Color waterColor;
  public Color waterShallowColor;
  public Material skybox;
  public Light sunLight;
  public Light moonLight;
  public Color dayFog;
  public Color nightFog;
  public Color waterFog;
  private float desiredSunlight;
  private float desiredMoonlight;
  public AnimationCurve waterGraph;

  private void Awake()
  {
    this.sky.mainTextureOffset = new Vector2(0.5f, 0.0f);
    this.waterColor = this.water.material.GetColor("_ShallowColor");
    this.waterShallowColor = this.water.material.GetColor("_DeepColor");
    DayCycle.time = 0.0f;
    DayCycle.totalTime = 0.0f;
  }

  public static float totalTime { get; private set; }

  private void Update()
  {
    if (GameManager.state != GameManager.GameState.Playing)
      return;
    float num1 = 1f * this.timeSpeed / DayCycle.dayDuration;
    if ((double) DayCycle.time > 0.5)
      num1 /= this.nightDuration;
    float num2 = num1 * Time.deltaTime;
    DayCycle.time += num2;
    DayCycle.time %= 1f;
    DayCycle.totalTime += num2;
    if (this.alwaysDay)
      DayCycle.time = 0.25f;
    this.sun.rotation = Quaternion.Euler(new Vector3(DayCycle.time * 360f, 0.0f, 0.0f));
    this.sky.mainTextureOffset = new Vector2(DayCycle.time - 0.25f, 0.0f);
    this.SunLight();
    float waterColor = this.EvaluateWaterColor((float) (((double) DayCycle.time + 0.75) % 1.0));
    Color color1 = Color.Lerp(this.dayFog, this.nightFog, waterColor);
    if ((Object) MoveCamera.Instance != (Object) null && (double) MoveCamera.Instance.transform.position.y < (double) World.Instance.water.position.y)
    {
      RenderSettings.fogDensity = 0.015f;
      color1 = this.waterFog;
    }
    else
      RenderSettings.fogDensity = 0.0035f;
    RenderSettings.fogColor = color1;
    this.skybox.SetFloat("_Exposure", 1f - waterColor);
    float num3 = (float) (0.75 - (double) waterColor * 0.5);
    RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, new Color(num3, num3, num3), Time.deltaTime * 0.5f);
    Color color2 = this.waterColor * (1f - waterColor) + Color.black * waterColor;
    Color color3 = this.waterShallowColor * (1f - waterColor) + Color.black * waterColor;
    this.water.material.SetColor("_ShallowColor", color2);
    this.water.material.SetColor("_DeepColor", color3);
  }

  private void SunLight()
  {
    if ((double) DayCycle.time > 0.5)
      this.desiredSunlight = 0.0f;
    else if ((double) DayCycle.time < 0.5 && (double) this.moonLight.intensity < 0.0500000007450581)
      this.desiredSunlight = 0.6f;
    if ((double) this.sunLight.intensity < 0.0500000007450581 && (double) DayCycle.time > 0.5 && (double) DayCycle.time < 0.75)
      this.desiredMoonlight = 0.6f;
    else if ((double) DayCycle.time > 0.970000028610229 || (double) DayCycle.time < 0.5)
      this.desiredMoonlight = 0.0f;
    this.sunLight.intensity = Mathf.Lerp(this.sunLight.intensity, this.desiredSunlight, Time.deltaTime * 1f);
    this.moonLight.intensity = Mathf.Lerp(this.moonLight.intensity, this.desiredMoonlight, Time.deltaTime * 1f);
  }

  private float EvaluateWaterColor(float x) => this.waterGraph.Evaluate(x);
}
