// Decompiled with JetBrains decompiler
// Type: ItemUnlcokedUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
  private float fadeStart = 1.5f;
  private float fadeTime = 0.5f;
  private float padUp;

  private void Awake()
  {
    this.layout = this.GetComponent<HorizontalLayoutGroup>();
    this.desiredPad = (float) this.layout.padding.top;
    this.layout.padding.top = 400;
    this.padUp = (float) this.layout.padding.top;
    this.Invoke("StartFade", this.fadeStart);
  }

  private void StartFade()
  {
    this.icon.CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.item.CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.overlay.CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.Invoke("DestroySelf", this.fadeTime);
  }

  private void DestroySelf() => Object.Destroy((Object) this.gameObject);

  public void SetItem(InventoryItem i)
  {
    this.icon.sprite = i.sprite;
    this.item.text = "Unlocked " + i.name;
  }

  public void Update()
  {
    this.padUp = Mathf.Lerp(this.padUp, this.desiredPad, Time.deltaTime * 10f);
    this.layout.padding = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom)
    {
      top = (int) this.padUp
    };
  }
}
