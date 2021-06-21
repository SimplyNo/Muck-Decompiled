// Decompiled with JetBrains decompiler
// Type: DayCycle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class DayCycle : MonoBehaviour
{
  public bool alwaysDay;
  public static float dayDuration = 1f;
  public float nightDuration;
  public float timeSpeed;
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
  private float desiredSunlight;
  private float desiredMoonlight;
  public AnimationCurve waterGraph;

  private void Awake()
  {
    this.sky.set_mainTextureOffset(new Vector2(0.5f, 0.0f));
    this.waterColor = ((Renderer) this.water).get_material().GetColor("_ShallowColor");
    this.waterShallowColor = ((Renderer) this.water).get_material().GetColor("_DeepColor");
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
    float num2 = num1 * Time.get_deltaTime();
    DayCycle.time += num2;
    DayCycle.time %= 1f;
    DayCycle.totalTime += num2;
    if (this.alwaysDay)
      DayCycle.time = 0.25f;
    this.sun.set_rotation(Quaternion.Euler(new Vector3(DayCycle.time * 360f, 0.0f, 0.0f)));
    this.sky.set_mainTextureOffset(new Vector2(DayCycle.time - 0.25f, 0.0f));
    this.SunLight();
    float waterColor = this.EvaluateWaterColor((float) (((double) DayCycle.time + 0.75) % 1.0));
    RenderSettings.set_fogColor(Color.Lerp(this.dayFog, this.nightFog, waterColor));
    this.skybox.SetFloat("_Exposure", 1f - waterColor);
    float num3 = (float) (0.75 - (double) waterColor * 0.5);
    Color color1;
    ((Color) ref color1).\u002Ector(num3, num3, num3);
    RenderSettings.set_ambientSkyColor(Color.Lerp(RenderSettings.get_ambientSkyColor(), color1, Time.get_deltaTime() * 0.5f));
    Color color2 = Color.op_Addition(Color.op_Multiply(this.waterColor, 1f - waterColor), Color.op_Multiply(Color.get_black(), waterColor));
    Color color3 = Color.op_Addition(Color.op_Multiply(this.waterShallowColor, 1f - waterColor), Color.op_Multiply(Color.get_black(), waterColor));
    ((Renderer) this.water).get_material().SetColor("_ShallowColor", color2);
    ((Renderer) this.water).get_material().SetColor("_DeepColor", color3);
  }

  private void SunLight()
  {
    if ((double) DayCycle.time > 0.5)
      this.desiredSunlight = 0.0f;
    else if ((double) DayCycle.time < 0.5 && (double) this.moonLight.get_intensity() < 0.0500000007450581)
      this.desiredSunlight = 0.6f;
    if ((double) this.sunLight.get_intensity() < 0.0500000007450581 && (double) DayCycle.time > 0.5 && (double) DayCycle.time < 0.75)
      this.desiredMoonlight = 0.6f;
    else if ((double) DayCycle.time > 0.970000028610229 || (double) DayCycle.time < 0.5)
      this.desiredMoonlight = 0.0f;
    this.sunLight.set_intensity(Mathf.Lerp(this.sunLight.get_intensity(), this.desiredSunlight, Time.get_deltaTime() * 1f));
    this.moonLight.set_intensity(Mathf.Lerp(this.moonLight.get_intensity(), this.desiredMoonlight, Time.get_deltaTime() * 1f));
  }

  private float EvaluateWaterColor(float x) => this.waterGraph.Evaluate(x);

  public DayCycle() => base.\u002Ector();
}
