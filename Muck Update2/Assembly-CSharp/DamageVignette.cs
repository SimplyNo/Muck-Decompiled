// Decompiled with JetBrains decompiler
// Type: DamageVignette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class DamageVignette : MonoBehaviour
{
  public RawImage vignette;
  public static DamageVignette Instance;

  private void Awake() => DamageVignette.Instance = this;

  private void Update()
  {
    if (!Object.op_Implicit((Object) PlayerStatus.Instance))
      return;
    float num1;
    if (Object.op_Implicit((Object) MoveCamera.Instance) && MoveCamera.Instance.state == MoveCamera.CameraState.Spectate)
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
    Color color = ((Graphic) this.vignette).get_color();
    color.a = (__Null) (double) num1;
    ((Graphic) this.vignette).set_color(Color.Lerp(((Graphic) this.vignette).get_color(), color, Time.get_deltaTime() * 12f));
  }

  public void VignetteHit()
  {
    Color color = ((Graphic) this.vignette).get_color();
    ref __Null local = ref color.a;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    ^(float&) ref local = ^(float&) ref local + 0.8f;
    ((Graphic) this.vignette).set_color(color);
  }

  public DamageVignette() => base.\u002Ector();
}
