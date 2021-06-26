// Decompiled with JetBrains decompiler
// Type: HitNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class HitNumber : MonoBehaviour
{
  private TextMeshProUGUI text;
  private float speed = 10f;
  private Vector3 defaultScale;
  private Vector3 dir;
  private Vector3 hitDir;

  private void Awake()
  {
    this.Invoke("StartFade", 1.5f);
    this.defaultScale = this.transform.localScale * 0.5f;
    this.text = this.GetComponentInChildren<TextMeshProUGUI>();
    float max = 0.5f;
    this.dir = new Vector3(Random.Range(-max, max), Random.Range(0.75f, 1.25f), Random.Range(-max, max));
  }

  private void Update()
  {
    this.speed = Mathf.Lerp(this.speed, 0.2f, Time.deltaTime * 10f);
    this.transform.position += (this.dir + this.hitDir) * Time.deltaTime * this.speed;
    this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.defaultScale * 0.5f, Time.deltaTime * 0.3f);
  }

  public void SetTextAndDir(float damage, Vector3 dir, HitEffect hitEffect)
  {
    this.hitDir = -dir;
    this.text.text = "<color=" + HitEffectExtension.GetColorName(hitEffect) + ">" + (object) damage;
  }

  private void StartFade()
  {
    this.text.CrossFadeAlpha(0.0f, 1f, true);
    this.Invoke("DestroySelf", 1f);
  }

  private void DestroySelf() => Object.Destroy((Object) this.gameObject);
}
