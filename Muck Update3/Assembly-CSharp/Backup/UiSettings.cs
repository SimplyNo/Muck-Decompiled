// Decompiled with JetBrains decompiler
// Type: UiSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiSettings : MonoBehaviour
{
  public GameObject settingButton;
  private TextMeshProUGUI[] texts;
  private Color selected = Color.white;
  private Color deselected = Color.gray;

  public int setting { get; private set; }

  public void AddSettings(int defaultValue, string[] enumNames)
  {
    this.setting = defaultValue;
    this.texts = new TextMeshProUGUI[enumNames.Length];
    for (int index1 = 0; index1 < enumNames.Length; ++index1)
    {
      int index = index1;
      Button component = Object.Instantiate<GameObject>(this.settingButton, this.transform).GetComponent<Button>();
      component.onClick.AddListener((UnityAction) (() => this.UpdateSetting(index)));
      this.texts[index1] = component.GetComponentInChildren<TextMeshProUGUI>();
      this.texts[index1].text = enumNames[index1];
    }
    this.UpdateSelection();
  }

  private void UpdateSelection()
  {
    for (int index = 0; index < this.texts.Length; ++index)
    {
      if (index == this.setting)
        this.texts[index].color = this.selected;
      else
        this.texts[index].color = this.deselected;
    }
  }

  private void UpdateSetting(int i)
  {
    this.setting = i;
    this.UpdateSelection();
  }
}
