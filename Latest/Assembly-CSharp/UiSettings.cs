// Decompiled with JetBrains decompiler
// Type: UiSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiSettings : MonoBehaviour
{
  public GameObject settingButton;
  private TextMeshProUGUI[] texts;
  private Color selected;
  private Color deselected;

  public int setting { get; private set; }

  public void AddSettings(int defaultValue, string[] enumNames)
  {
    this.setting = defaultValue;
    this.texts = new TextMeshProUGUI[enumNames.Length];
    for (int index = 0; index < enumNames.Length; ++index)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UiSettings.\u003C\u003Ec__DisplayClass8_0 cDisplayClass80 = new UiSettings.\u003C\u003Ec__DisplayClass8_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass80.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass80.index = index;
      Button component = (Button) ((GameObject) Object.Instantiate<GameObject>((M0) this.settingButton, ((Component) this).get_transform())).GetComponent<Button>();
      // ISSUE: method pointer
      ((UnityEvent) component.get_onClick()).AddListener(new UnityAction((object) cDisplayClass80, __methodptr(\u003CAddSettings\u003Eb__0)));
      this.texts[index] = (TextMeshProUGUI) ((Component) component).GetComponentInChildren<TextMeshProUGUI>();
      ((TMP_Text) this.texts[index]).set_text(enumNames[index]);
    }
    this.UpdateSelection();
  }

  private void UpdateSelection()
  {
    for (int index = 0; index < this.texts.Length; ++index)
    {
      if (index == this.setting)
        ((Graphic) this.texts[index]).set_color(this.selected);
      else
        ((Graphic) this.texts[index]).set_color(this.deselected);
    }
  }

  private void UpdateSetting(int i)
  {
    this.setting = i;
    this.UpdateSelection();
  }

  public UiSettings() => base.\u002Ector();
}
