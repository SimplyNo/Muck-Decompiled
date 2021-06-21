// Decompiled with JetBrains decompiler
// Type: DayUi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayUi : MonoBehaviour
{
  public TextMeshProUGUI dayText;
  private Vector3 desiredScale;
  private Vector3 defaultScale;
  private bool done;
  private float fadeTime;
  private float scaleSpeed;
  public AudioSource sfx;

  private void Awake() => this.defaultScale = ((TMP_Text) this.dayText).get_transform().get_localScale();

  public void SetDay(int day)
  {
    this.Invoke("StartFade", 2f);
    ((TMP_Text) this.dayText).set_text(string.Format("-DAY {0}-", (object) day));
  }

  private void StartFade()
  {
    if (GameManager.state != GameManager.GameState.Playing)
      return;
    ((Component) this).get_gameObject().SetActive(true);
    if (Vector3.op_Equality(this.defaultScale, Vector3.get_zero()))
      this.defaultScale = ((TMP_Text) this.dayText).get_transform().get_localScale();
    ((CanvasRenderer) ((Component) this.dayText).GetComponent<CanvasRenderer>()).SetAlpha(0.0f);
    ((TMP_Text) this.dayText).get_transform().set_localScale(Vector3.op_Multiply(this.defaultScale, 3f));
    this.desiredScale = Vector3.op_Multiply(this.defaultScale, 1.2f);
    ((Graphic) this.dayText).CrossFadeAlpha(1f, this.fadeTime, true);
    this.Invoke("FadeAway", 4f);
    this.Invoke("Hide", 4f + this.fadeTime);
    this.done = false;
    this.sfx.Play();
  }

  private void Update()
  {
    this.desiredScale = Vector3.op_Addition(this.desiredScale, Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), this.scaleSpeed), Time.get_deltaTime()));
    ((TMP_Text) this.dayText).get_transform().set_localScale(Vector3.Lerp(((TMP_Text) this.dayText).get_transform().get_localScale(), this.desiredScale, Time.get_deltaTime() * 3f));
  }

  private void Hide() => ((Component) this).get_gameObject().SetActive(false);

  private void FadeAway() => ((Graphic) this.dayText).CrossFadeAlpha(0.0f, this.fadeTime, true);

  public DayUi() => base.\u002Ector();
}
