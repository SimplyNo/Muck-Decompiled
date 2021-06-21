// Decompiled with JetBrains decompiler
// Type: AudioFreqController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class AudioFreqController : MonoBehaviour
{
  public AudioLowPassFilter filter;

  private void Update()
  {
    if (!(bool) (Object) PlayerStatus.Instance)
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
    this.filter.cutoffFrequency = Mathf.Lerp(this.filter.cutoffFrequency, 22000f * num1, Time.deltaTime * 8f);
  }
}
