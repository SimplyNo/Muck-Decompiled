// Decompiled with JetBrains decompiler
// Type: ItemInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    this.defaultTextPos = ((TMP_Text) this.text).get_transform().get_localPosition();
  }

  private void Update()
  {
    ((Component) this).get_transform().set_position(Input.get_mousePosition());
    this.FitToText();
  }

  private void OnEnable() => this.SetText("");

  public void FitToText()
  {
    Vector2 vector2;
    ref Vector2 local1 = ref vector2;
    Bounds bounds = ((TMP_Text) this.text).get_mesh().get_bounds();
    // ISSUE: variable of the null type
    __Null x = ((Bounds) ref bounds).get_size().x;
    bounds = ((TMP_Text) this.text).get_mesh().get_bounds();
    // ISSUE: variable of the null type
    __Null y = ((Bounds) ref bounds).get_size().y;
    ((Vector2) ref local1).\u002Ector((float) x, (float) y);
    ref __Null local2 = ref vector2.x;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    ^(float&) ref local2 = ^(float&) ref local2 + this.padding;
    ref __Null local3 = ref vector2.y;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    ^(float&) ref local3 = ^(float&) ref local3 + this.padding;
    if (this.leftCorner)
      ((TMP_Text) this.text).get_transform().set_localPosition(Vector3.op_Subtraction(Vector3.op_UnaryNegation(this.defaultTextPos), new Vector3((float) vector2.x, (float) vector2.y, 0.0f)));
    else
      ((TMP_Text) this.text).get_transform().set_localPosition(this.defaultTextPos);
    ((Graphic) this.image).get_rectTransform().set_sizeDelta(vector2);
    ((Transform) ((Graphic) this.image).get_rectTransform()).set_position(((Transform) ((TMP_Text) this.text).get_rectTransform()).get_position());
    Vector3 vector3;
    ((Vector3) ref vector3).\u002Ector(this.padding / 2f, 0.0f, 0.0f);
    ((Transform) ((Graphic) this.image).get_rectTransform()).set_localPosition(Vector3.op_Subtraction(((Transform) ((TMP_Text) this.text).get_rectTransform()).get_localPosition(), vector3));
  }

  public void SetText(string t, bool leftCorner = false)
  {
    ((TMP_Text) this.text).set_text(t);
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
    ((Graphic) this.text).CrossFadeAlpha(opacity, time, true);
    ((Graphic) this.image).CrossFadeAlpha(opacity, time, true);
  }

  public ItemInfo() => base.\u002Ector();
}
