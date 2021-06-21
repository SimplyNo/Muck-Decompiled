// Decompiled with JetBrains decompiler
// Type: ItemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
  public TextMeshProUGUI text;
  public RawImage image;
  public float padding;
  private Vector3 defaultTextPos;
  public static ItemInfo Instance;
  private bool leftCorner;

  private void Awake()
  {
    ItemInfo.Instance = this;
    this.defaultTextPos = this.text.transform.localPosition;
  }

  private void Update()
  {
    this.transform.position = Input.mousePosition;
    this.FitToText();
  }

  private void OnEnable() => this.SetText("");

  public void FitToText()
  {
    Vector2 vector2;
    ref Vector2 local = ref vector2;
    Bounds bounds = this.text.mesh.bounds;
    double x = (double) bounds.size.x;
    bounds = this.text.mesh.bounds;
    double y = (double) bounds.size.y;
    local = new Vector2((float) x, (float) y);
    vector2.x += this.padding;
    vector2.y += this.padding;
    if (this.leftCorner)
      this.text.transform.localPosition = -this.defaultTextPos - new Vector3(vector2.x, vector2.y, 0.0f);
    else
      this.text.transform.localPosition = this.defaultTextPos;
    this.image.rectTransform.sizeDelta = vector2;
    this.image.rectTransform.position = this.text.rectTransform.position;
    this.image.rectTransform.localPosition = this.text.rectTransform.localPosition - new Vector3(this.padding / 2f, 0.0f, 0.0f);
  }

  public void SetText(string t, bool leftCorner = false)
  {
    this.text.text = t;
    if (t == "")
      this.Fade(0.0f);
    else
      this.Fade(1f);
    this.FitToText();
    if (leftCorner)
      this.leftCorner = true;
    else
      this.leftCorner = false;
  }

  public void Fade(float opacity, float time = 0.2f)
  {
    this.text.CrossFadeAlpha(opacity, time, true);
    this.image.CrossFadeAlpha(opacity, time, true);
  }
}
