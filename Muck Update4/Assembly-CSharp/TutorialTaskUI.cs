// Decompiled with JetBrains decompiler
// Type: TutorialTaskUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTaskUI : MonoBehaviour
{
  public RawImage overlay;
  public RawImage icon;
  public TextMeshProUGUI item;
  private HorizontalLayoutGroup layout;
  public Texture checkedBox;
  private float desiredPad;
  private float fadeStart = 1.5f;
  private float fadeTime = 1.5f;
  private float padUp;

  private void Awake()
  {
    this.layout = this.GetComponent<HorizontalLayoutGroup>();
    this.desiredPad = (float) this.layout.padding.left;
    this.layout.padding.left = 400;
    this.padUp = (float) this.layout.padding.left;
  }

  public void StartFade()
  {
    this.icon.texture = this.checkedBox;
    this.icon.CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.item.CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.overlay.CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.Invoke("DestroySelf", this.fadeTime);
  }

  private void DestroySelf() => Object.Destroy((Object) this.gameObject);

  public void SetItem(InventoryItem i, string text)
  {
    text = text.Replace("[inv]", "[" + (object) InputManager.inventory + "]");
    text = text.Replace("[m2]", "[" + (object) InputManager.rightClick + "]");
    this.item.text = text;
  }

  public void Update()
  {
    this.padUp = Mathf.Lerp(this.padUp, this.desiredPad, Time.deltaTime * 6f);
    this.layout.padding = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom)
    {
      left = (int) this.padUp
    };
  }
}
