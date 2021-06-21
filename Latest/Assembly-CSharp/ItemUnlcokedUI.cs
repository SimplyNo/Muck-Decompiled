// Decompiled with JetBrains decompiler
// Type: ItemUnlcokedUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUnlcokedUI : MonoBehaviour
{
  public Image overlay;
  public Image icon;
  public TextMeshProUGUI item;
  private HorizontalLayoutGroup layout;
  private float desiredPad;
  private float fadeStart;
  private float fadeTime;
  private float padUp;

  private void Awake()
  {
    this.layout = (HorizontalLayoutGroup) ((Component) this).GetComponent<HorizontalLayoutGroup>();
    this.desiredPad = (float) ((LayoutGroup) this.layout).get_padding().get_top();
    ((LayoutGroup) this.layout).get_padding().set_top(400);
    this.padUp = (float) ((LayoutGroup) this.layout).get_padding().get_top();
    this.Invoke("StartFade", this.fadeStart);
  }

  private void StartFade()
  {
    ((Graphic) this.icon).CrossFadeAlpha(0.0f, this.fadeTime, true);
    ((Graphic) this.item).CrossFadeAlpha(0.0f, this.fadeTime, true);
    ((Graphic) this.overlay).CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.Invoke("DestroySelf", this.fadeTime);
  }

  private void DestroySelf() => Object.Destroy((Object) ((Component) this).get_gameObject());

  public void SetItem(InventoryItem i)
  {
    this.icon.set_sprite(i.sprite);
    ((TMP_Text) this.item).set_text("Unlocked " + i.name);
  }

  public void Update()
  {
    this.padUp = Mathf.Lerp(this.padUp, this.desiredPad, Time.get_deltaTime() * 10f);
    RectOffset rectOffset = new RectOffset(((LayoutGroup) this.layout).get_padding().get_left(), ((LayoutGroup) this.layout).get_padding().get_right(), ((LayoutGroup) this.layout).get_padding().get_top(), ((LayoutGroup) this.layout).get_padding().get_bottom());
    rectOffset.set_top((int) this.padUp);
    ((LayoutGroup) this.layout).set_padding(rectOffset);
  }

  public ItemUnlcokedUI() => base.\u002Ector();
}
