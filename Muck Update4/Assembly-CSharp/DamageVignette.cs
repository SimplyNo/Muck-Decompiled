// Decompiled with JetBrains decompiler
// Type: DamageVignette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class DamageVignette : MonoBehaviour
{
  public RawImage vignette;
  public static DamageVignette Instance;

  private void Awake() => DamageVignette.Instance = this;

  private void Update()
  {
    if (!(bool) (Object) PlayerStatus.Instance)
      return;
    float num1;
    if ((bool) (Object) MoveCamera.Instance && MoveCamera.Instance.state == MoveCamera.CameraState.Spectate || MoveCamera.Instance.state == MoveCamera.CameraState.Freecam)
      num1 = 0.0f;
    else if ((double) PlayerStatus.Instance.hp <= 0.0)
    {
      num1 = 1f;
    }
    else
    {
      float num2 = 0.75f;
      int num3 = PlayerStatus.Instance.HpAndShield();
      int num4 = PlayerStatus.Instance.MaxHpAndShield();
      num1 = (double) ((float) num3 / (float) num4) <= (double) num2 ? (float) (1.0 - (double) num3 / ((double) num4 * (double) num2)) : 0.0f;
    }
    Color color = this.vignette.color;
    color.a = num1;
    this.vignette.color = Color.Lerp(this.vignette.color, color, Time.deltaTime * 12f);
  }

  public void VignetteHit()
  {
    Color color = this.vignette.color;
    color.a += 0.8f;
    this.vignette.color = color;
  }
}
