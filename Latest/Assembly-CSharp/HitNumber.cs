// Decompiled with JetBrains decompiler
// Type: HitNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitNumber : MonoBehaviour
{
  private TextMeshProUGUI text;
  private float speed;
  private Vector3 defaultScale;
  private Vector3 dir;
  private Vector3 hitDir;

  private void Awake()
  {
    this.Invoke("StartFade", 1.5f);
    this.defaultScale = Vector3.op_Multiply(((Component) this).get_transform().get_localScale(), 0.5f);
    this.text = (TextMeshProUGUI) ((Component) this).GetComponentInChildren<TextMeshProUGUI>();
    float num = 0.5f;
    this.dir = new Vector3(Random.Range(-num, num), Random.Range(0.75f, 1.25f), Random.Range(-num, num));
  }

  private void Update()
  {
    this.speed = Mathf.Lerp(this.speed, 0.2f, Time.get_deltaTime() * 10f);
    Transform transform = ((Component) this).get_transform();
    transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Addition(this.dir, this.hitDir), Time.get_deltaTime()), this.speed)));
    ((Component) this).get_transform().set_localScale(Vector3.Lerp(((Component) this).get_transform().get_localScale(), Vector3.op_Multiply(this.defaultScale, 0.5f), Time.get_deltaTime() * 0.3f));
  }

  public void SetTextAndDir(float damage, Vector3 dir, HitEffect hitEffect)
  {
    this.hitDir = Vector3.op_UnaryNegation(dir);
    ((TMP_Text) this.text).set_text("<color=" + HitEffectExtension.GetColorName(hitEffect) + ">" + (object) damage);
  }

  private void StartFade()
  {
    ((Graphic) this.text).CrossFadeAlpha(0.0f, 1f, true);
    this.Invoke("DestroySelf", 1f);
  }

  private void DestroySelf() => Object.Destroy((Object) ((Component) this).get_gameObject());

  public HitNumber() => base.\u002Ector();
}
