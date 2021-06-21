// Decompiled with JetBrains decompiler
// Type: ItemPickedupUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickedupUI : MonoBehaviour
{
  public Image icon;
  public TextMeshProUGUI item;
  private HorizontalLayoutGroup layout;
  private float desiredPad;
  private float fadeStart;
  private float fadeTime;
  private float padLeft;

  private void Awake()
  {
    this.layout = (HorizontalLayoutGroup) ((Component) this).GetComponent<HorizontalLayoutGroup>();
    this.desiredPad = (float) ((LayoutGroup) this.layout).get_padding().get_left();
    ((LayoutGroup) this.layout).get_padding().set_left(-300);
    this.padLeft = (float) ((LayoutGroup) this.layout).get_padding().get_left();
    this.Invoke("StartFade", this.fadeStart);
  }

  private void StartFade()
  {
    ((Graphic) this.icon).CrossFadeAlpha(0.0f, this.fadeTime, true);
    ((Graphic) this.item).CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.Invoke("DestroySelf", this.fadeTime);
  }

  private void DestroySelf() => Object.Destroy((Object) ((Component) this).get_gameObject());

  public void SetItem(InventoryItem i)
  {
    if (i.amount < 1)
    {
      this.icon.set_sprite((Sprite) null);
      ((TMP_Text) this.item).set_text("Inventory full");
    }
    else
    {
      this.icon.set_sprite(i.sprite);
      ((TMP_Text) this.item).set_text(string.Format("{0}x {1}", (object) i.amount, (object) i.name));
    }
  }

  public void SetPowerup(Powerup i)
  {
    this.icon.set_sprite(i.sprite);
    ((TMP_Text) this.item).set_text(i.name + "\n<size=75%>" + i.description);
  }

  public void Update()
  {
    this.padLeft = Mathf.Lerp(this.padLeft, this.desiredPad, Time.get_deltaTime() * 7f);
    RectOffset rectOffset = new RectOffset(((LayoutGroup) this.layout).get_padding().get_left(), ((LayoutGroup) this.layout).get_padding().get_right(), ((LayoutGroup) this.layout).get_padding().get_top(), ((LayoutGroup) this.layout).get_padding().get_bottom());
    rectOffset.set_left((int) this.padLeft);
    ((LayoutGroup) this.layout).set_padding(rectOffset);
    ((LayoutGroup) this.layout).get_padding().set_left((int) this.padLeft);
  }

  public ItemPickedupUI() => base.\u002Ector();
}
