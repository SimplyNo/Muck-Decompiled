// Decompiled with JetBrains decompiler
// Type: GravePing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class GravePing : MonoBehaviour
{
  public TextMeshProUGUI pingText;
  private float defaultScale;
  private GraveInteract grave;
  private GameObject child;

  private void Awake()
  {
    this.child = ((Component) ((Component) this).get_transform().GetChild(0)).get_gameObject();
    this.grave = (GraveInteract) ((Component) ((Component) this).get_transform().get_root()).GetComponentInChildren<GraveInteract>();
  }

  public void SetPing(string name) => ((TMP_Text) this.pingText).set_text(string.Format("Revive {0} ({1}", (object) this.grave.username, (object) this.grave.timeLeft));

  private void Update()
  {
    if ((double) DayCycle.time <= 0.5)
    {
      this.child.SetActive(true);
      string str = "";
      if ((double) this.grave.timeLeft > 0.0)
        str = string.Format("({0})", (object) (int) this.grave.timeLeft);
      ((TMP_Text) this.pingText).set_text("Revive " + this.grave.username + " " + str);
      float num = Vector3.Distance(((Component) this).get_transform().get_position(), PlayerMovement.Instance.playerCam.get_position());
      if ((double) num < 5.0)
        num = 0.0f;
      if ((double) num > 5000.0)
        num = 5000f;
      ((Component) this).get_transform().set_localScale(Vector3.op_Multiply(this.defaultScale * num, Vector3.get_one()));
    }
    else
      this.child.SetActive(false);
  }

  public GravePing() => base.\u002Ector();
}
