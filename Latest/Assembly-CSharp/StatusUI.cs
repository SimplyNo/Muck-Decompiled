// Decompiled with JetBrains decompiler
// Type: StatusUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
  public RectTransform hpBar;
  public RectTransform shieldBar;
  public RectTransform armorBar;
  private float hpRatio;
  private float shieldRatio;
  public Transform hungerBar;
  public Transform staminaBar;
  public TextMeshProUGUI hpText;
  private float currentHp;
  private PlayerStatus playerStatus;
  private float speed;

  private void Start()
  {
    if (!Object.op_Implicit((Object) PlayerMovement.Instance))
      return;
    this.playerStatus = (PlayerStatus) ((Component) PlayerMovement.Instance).get_gameObject().GetComponent<PlayerStatus>();
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.playerStatus, (Object) null))
    {
      if (Object.op_Equality((Object) PlayerMovement.Instance, (Object) null))
        return;
      this.playerStatus = (PlayerStatus) ((Component) PlayerMovement.Instance).get_gameObject().GetComponent<PlayerStatus>();
    }
    else
    {
      this.currentHp = Mathf.Lerp(this.currentHp, PlayerStatus.Instance.hp + PlayerStatus.Instance.shield, Time.get_deltaTime() * 3f);
      ((TMP_Text) this.hpText).set_text(Mathf.Round(this.currentHp).ToString() + " / " + (object) (PlayerStatus.Instance.maxHp + PlayerStatus.Instance.maxShield));
      ((Transform) this.hpBar).set_localScale(new Vector3(Mathf.Lerp((float) ((Transform) this.hpBar).get_localScale().x, this.playerStatus.GetHpRatio(), Time.get_deltaTime() * this.speed), 1f, 1f));
      ((Transform) this.shieldBar).set_localScale(new Vector3(Mathf.Lerp((float) ((Transform) this.shieldBar).get_localScale().x, this.playerStatus.GetShieldRatio(), Time.get_deltaTime() * this.speed), 1f, 1f));
      this.hungerBar.set_localScale(new Vector3(Mathf.Lerp((float) this.hungerBar.get_localScale().x, this.playerStatus.GetHungerRatio(), Time.get_deltaTime() * this.speed), 1f, 1f));
      this.staminaBar.set_localScale(new Vector3(Mathf.Lerp((float) this.staminaBar.get_localScale().x, this.playerStatus.GetStaminaRatio(), Time.get_deltaTime() * this.speed), 1f, 1f));
      ((Transform) this.armorBar).set_localScale(new Vector3(Mathf.Lerp((float) ((Transform) this.armorBar).get_localScale().x, this.playerStatus.GetArmorRatio(), Time.get_deltaTime() * this.speed), 1f, 1f));
    }
  }

  public StatusUI() => base.\u002Ector();
}
