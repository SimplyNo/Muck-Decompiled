// Decompiled with JetBrains decompiler
// Type: StatusUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
  private float speed = 10f;

  private void Start()
  {
    if (!(bool) (Object) PlayerMovement.Instance)
      return;
    this.playerStatus = PlayerMovement.Instance.gameObject.GetComponent<PlayerStatus>();
  }

  private void Update()
  {
    if ((Object) this.playerStatus == (Object) null)
    {
      if ((Object) PlayerMovement.Instance == (Object) null)
        return;
      this.playerStatus = PlayerMovement.Instance.gameObject.GetComponent<PlayerStatus>();
    }
    else
    {
      this.currentHp = Mathf.Lerp(this.currentHp, PlayerStatus.Instance.hp + PlayerStatus.Instance.shield, Time.deltaTime * 3f);
      this.hpText.text = Mathf.Round(this.currentHp).ToString() + " / " + (object) (PlayerStatus.Instance.maxHp + PlayerStatus.Instance.maxShield);
      this.hpBar.localScale = new Vector3(Mathf.Lerp(this.hpBar.localScale.x, this.playerStatus.GetHpRatio(), Time.deltaTime * this.speed), 1f, 1f);
      this.shieldBar.localScale = new Vector3(Mathf.Lerp(this.shieldBar.localScale.x, this.playerStatus.GetShieldRatio(), Time.deltaTime * this.speed), 1f, 1f);
      this.hungerBar.localScale = new Vector3(Mathf.Lerp(this.hungerBar.localScale.x, this.playerStatus.GetHungerRatio(), Time.deltaTime * this.speed), 1f, 1f);
      this.staminaBar.localScale = new Vector3(Mathf.Lerp(this.staminaBar.localScale.x, this.playerStatus.GetStaminaRatio(), Time.deltaTime * this.speed), 1f, 1f);
      this.armorBar.localScale = new Vector3(Mathf.Lerp(this.armorBar.localScale.x, this.playerStatus.GetArmorRatio(), Time.deltaTime * this.speed), 1f, 1f);
    }
  }
}
