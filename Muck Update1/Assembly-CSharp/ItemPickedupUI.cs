// Decompiled with JetBrains decompiler
// Type: ItemPickedupUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickedupUI : MonoBehaviour
{
  public Image icon;
  public TextMeshProUGUI item;
  private HorizontalLayoutGroup layout;
  private float desiredPad;
  private float fadeStart = 6f;
  private float fadeTime = 1f;
  private float padLeft;

  private void Awake()
  {
    this.layout = this.GetComponent<HorizontalLayoutGroup>();
    this.desiredPad = (float) this.layout.padding.left;
    this.layout.padding.left = -300;
    this.padLeft = (float) this.layout.padding.left;
    this.Invoke("StartFade", this.fadeStart);
  }

  private void StartFade()
  {
    this.icon.CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.item.CrossFadeAlpha(0.0f, this.fadeTime, true);
    this.Invoke("DestroySelf", this.fadeTime);
  }

  private void DestroySelf() => Object.Destroy((Object) this.gameObject);

  public void SetItem(InventoryItem i)
  {
    if (i.amount < 1)
    {
      this.icon.sprite = (Sprite) null;
      this.item.text = "Inventory full";
    }
    else
    {
      this.icon.sprite = i.sprite;
      this.item.text = string.Format("{0}x {1}", (object) i.amount, (object) i.name);
    }
  }

  public void SetPowerup(Powerup i)
  {
    this.icon.sprite = i.sprite;
    this.item.text = i.name + "\n<size=75%>" + i.description;
  }

  public void Update()
  {
    this.padLeft = Mathf.Lerp(this.padLeft, this.desiredPad, Time.deltaTime * 7f);
    this.layout.padding = new RectOffset(this.layout.padding.left, this.layout.padding.right, this.layout.padding.top, this.layout.padding.bottom)
    {
      left = (int) this.padLeft
    };
    this.layout.padding.left = (int) this.padLeft;
  }
}
