// Decompiled with JetBrains decompiler
// Type: BossUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
  public TextMeshProUGUI bossName;
  public TextMeshProUGUI hpText;
  public RawImage hpBar;
  public Mob currentBoss;
  private HitableMob hitableMob;
  private int desiredHp;
  public Transform layout;
  private Vector3 desiredScale;
  public static BossUI Instance;
  private float currentHp;

  private void Awake()
  {
    BossUI.Instance = this;
    this.layout.transform.localScale = Vector3.zero;
    this.desiredScale = Vector3.zero;
  }

  public void SetBoss(Mob b)
  {
    if ((Object) this.currentBoss != (Object) null)
      return;
    this.currentBoss = b;
    this.bossName.text = "";
    if (b.IsBuff())
      this.bossName.text += "Buff ";
    this.bossName.text += this.currentBoss.mobType.name;
    this.currentHp = 0.0f;
    this.desiredScale = Vector3.one;
    this.hitableMob = b.GetComponent<HitableMob>();
    this.layout.gameObject.SetActive(true);
    this.layout.localScale = Vector3.zero;
  }

  private void Update()
  {
    if ((Object) this.currentBoss == (Object) null)
    {
      if (!this.layout.gameObject.activeInHierarchy)
        return;
      this.layout.gameObject.SetActive(false);
      if ((double) DayCycle.time >= 0.5)
        return;
      MusicController.Instance.StopSong();
    }
    else
    {
      this.currentHp = Mathf.Lerp(this.currentHp, (float) this.hitableMob.hp, Time.deltaTime * 10f);
      this.hpText.text = Mathf.RoundToInt(this.currentHp).ToString() + " / " + (object) this.hitableMob.maxHp;
      this.hpBar.transform.localScale = new Vector3((float) this.hitableMob.hp / (float) this.hitableMob.maxHp, 1f, 1f);
      this.layout.transform.localScale = Vector3.Lerp(this.layout.transform.localScale, this.desiredScale, Time.deltaTime * 10f);
    }
  }
}
