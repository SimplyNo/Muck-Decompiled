// Decompiled with JetBrains decompiler
// Type: PlayerPing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class PlayerPing : MonoBehaviour
{
  private float desiredScale;
  private float localScale;
  public TextMeshProUGUI pingText;

  private void Awake()
  {
    this.desiredScale = 1f;
    ((Component) this).get_transform().set_localScale(Vector3.get_zero());
    this.Invoke("HidePing", 5f);
  }

  public void SetPing(string username, string item) => ((TMP_Text) this.pingText).set_text(username + "\n<size=75>" + item);

  private void Update()
  {
    this.localScale = Mathf.Lerp(this.localScale, this.desiredScale, Time.get_deltaTime() * 10f);
    float num = Vector3.Distance(((Component) this).get_transform().get_position(), PlayerMovement.Instance.playerCam.get_position());
    if ((double) num < 7.0)
      num = 7f;
    if ((double) num > 100.0)
      num = 100f;
    ((Component) this).get_transform().set_localScale(Vector3.op_Multiply(this.localScale * num, Vector3.get_one()));
  }

  private void HidePing() => this.desiredScale = 0.0f;

  public PlayerPing() => base.\u002Ector();
}
