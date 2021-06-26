// Decompiled with JetBrains decompiler
// Type: FadeScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
  public RawImage blackImg;
  public static FadeScreen Instance;

  private void Awake()
  {
    FadeScreen.Instance = this;
    this.blackImg.CrossFadeAlpha(0.0f, 0.0f, true);
    this.blackImg.gameObject.SetActive(true);
  }

  public void StartFade(float alpha, float duration) => this.blackImg.CrossFadeAlpha(alpha, duration, true);
}
