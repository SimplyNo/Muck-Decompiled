// Decompiled with JetBrains decompiler
// Type: TopNavigate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopNavigate : MonoBehaviour
{
  public GameObject[] settingMenus;
  public TextMeshProUGUI[] texts;
  public Color selectedColor;
  public Color idleColor;

  private void OnEnable() => this.Select(0);

  public void Select(int selected)
  {
    for (int index = 0; index < this.settingMenus.Length; ++index)
    {
      if (index == selected)
      {
        this.settingMenus[index].SetActive(true);
        ((Graphic) this.texts[index]).set_color(this.selectedColor);
      }
      else
      {
        this.settingMenus[index].SetActive(false);
        ((Graphic) this.texts[index]).set_color(this.idleColor);
      }
    }
  }

  public TopNavigate() => base.\u002Ector();
}
