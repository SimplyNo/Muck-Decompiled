// Decompiled with JetBrains decompiler
// Type: AudioFreqController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class AudioFreqController : MonoBehaviour
{
  public AudioLowPassFilter filter;

  private void Update()
  {
    if (!Object.op_Implicit((Object) PlayerStatus.Instance))
      return;
    float num1;
    if ((double) PlayerStatus.Instance.hp <= 0.0)
    {
      num1 = 1f;
    }
    else
    {
      float num2 = 0.75f;
      int num3 = PlayerStatus.Instance.HpAndShield();
      int num4 = PlayerStatus.Instance.MaxHpAndShield();
      num1 = (double) ((float) num3 / (float) num4) <= (double) num2 ? (float) num3 / ((float) num4 * num2) : 1f;
    }
    this.filter.set_cutoffFrequency(Mathf.Lerp(this.filter.get_cutoffFrequency(), 22000f * num1, Time.get_deltaTime() * 8f));
  }

  public AudioFreqController() => base.\u002Ector();
}
