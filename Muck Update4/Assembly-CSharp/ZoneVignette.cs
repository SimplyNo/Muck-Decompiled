// Decompiled with JetBrains decompiler
// Type: ZoneVignette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ZoneVignette : MonoBehaviour
{
  private float intensity;
  private RawImage img;
  public static ZoneVignette Instance;
  private Vector3 desiredScale = Vector3.one;

  private void Awake()
  {
    ZoneVignette.Instance = this;
    this.img = this.GetComponent<RawImage>();
    this.img.CrossFadeAlpha(0.0f, 0.0f, true);
    Color color = this.img.color;
    color.a = 0.8f;
    this.img.color = color;
  }

  public void SetVignette(bool on)
  {
    if (on)
    {
      this.img.CrossFadeAlpha(0.8f, 3f, true);
      this.desiredScale = Vector3.one * 1.6f;
    }
    else
    {
      this.img.CrossFadeAlpha(0.0f, 2f, true);
      this.desiredScale = Vector3.one * 1f;
    }
  }

  private void Update() => this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.desiredScale, Time.deltaTime * 0.2f);
}
