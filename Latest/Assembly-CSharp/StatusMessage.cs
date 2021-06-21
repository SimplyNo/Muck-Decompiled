// Decompiled with JetBrains decompiler
// Type: StatusMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class StatusMessage : MonoBehaviour
{
  public TextMeshProUGUI statusText;
  public GameObject status;
  private Vector3 defaultScale;
  public static StatusMessage Instance;

  private void Awake()
  {
    StatusMessage.Instance = this;
    this.defaultScale = this.status.get_transform().get_localScale();
  }

  private void Update() => this.status.get_transform().set_localScale(Vector3.Lerp(this.status.get_transform().get_localScale(), this.defaultScale, Time.get_deltaTime() * 25f));

  public void DisplayMessage(string message)
  {
    ((Component) this.status.get_transform().get_parent()).get_gameObject().SetActive(true);
    this.status.get_transform().set_localScale(Vector3.get_zero());
    ((TMP_Text) this.statusText).set_text(message);
  }

  public void OkayDokay() => ((Component) this.status.get_transform().get_parent()).get_gameObject().SetActive(false);

  public StatusMessage() => base.\u002Ector();
}
