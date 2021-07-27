// Decompiled with JetBrains decompiler
// Type: FadeScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
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
