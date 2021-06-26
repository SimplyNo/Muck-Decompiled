// Decompiled with JetBrains decompiler
// Type: DayUi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class DayUi : MonoBehaviour
{
  public TextMeshProUGUI dayText;
  private Vector3 desiredScale;
  private Vector3 defaultScale;
  private bool done;
  private float fadeTime = 2f;
  private float scaleSpeed = -0.2f;
  public AudioSource sfx;

  private void Awake() => this.defaultScale = this.dayText.transform.localScale;

  public void SetDay(int day)
  {
    this.Invoke("StartFade", 2f);
    this.dayText.text = string.Format("-DAY {0}-", (object) day);
  }

  private void StartFade()
  {
    if (GameManager.state != GameManager.GameState.Playing)
      return;
    this.gameObject.SetActive(true);
    if (this.defaultScale == Vector3.zero)
      this.defaultScale = this.dayText.transform.localScale;
    this.dayText.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
    this.dayText.transform.localScale = this.defaultScale * 3f;
    this.desiredScale = this.defaultScale * 1.2f;
    this.dayText.CrossFadeAlpha(1f, this.fadeTime, true);
    this.Invoke("FadeAway", 4f);
    this.Invoke("Hide", 4f + this.fadeTime);
    this.done = false;
    this.sfx.Play();
  }

  private void Update()
  {
    this.desiredScale += Vector3.one * this.scaleSpeed * Time.deltaTime;
    this.dayText.transform.localScale = Vector3.Lerp(this.dayText.transform.localScale, this.desiredScale, Time.deltaTime * 3f);
  }

  private void Hide() => this.gameObject.SetActive(false);

  private void FadeAway() => this.dayText.CrossFadeAlpha(0.0f, this.fadeTime, true);
}
