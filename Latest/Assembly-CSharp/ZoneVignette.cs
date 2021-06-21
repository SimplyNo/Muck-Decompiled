// Decompiled with JetBrains decompiler
// Type: ZoneVignette
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ZoneVignette : MonoBehaviour
{
  private float intensity;
  private RawImage img;
  public static ZoneVignette Instance;
  private Vector3 desiredScale;

  private void Awake()
  {
    ZoneVignette.Instance = this;
    this.img = (RawImage) ((Component) this).GetComponent<RawImage>();
    ((Graphic) this.img).CrossFadeAlpha(0.0f, 0.0f, true);
    Color color = ((Graphic) this.img).get_color();
    color.a = (__Null) 0.800000011920929;
    ((Graphic) this.img).set_color(color);
  }

  public void SetVignette(bool on)
  {
    if (on)
    {
      ((Graphic) this.img).CrossFadeAlpha(0.8f, 3f, true);
      this.desiredScale = Vector3.op_Multiply(Vector3.get_one(), 1.6f);
    }
    else
    {
      ((Graphic) this.img).CrossFadeAlpha(0.0f, 2f, true);
      this.desiredScale = Vector3.op_Multiply(Vector3.get_one(), 1f);
    }
  }

  private void Update() => ((Component) this).get_transform().set_localScale(Vector3.Lerp(((Component) this).get_transform().get_localScale(), this.desiredScale, Time.get_deltaTime() * 0.2f));

  public ZoneVignette() => base.\u002Ector();
}
