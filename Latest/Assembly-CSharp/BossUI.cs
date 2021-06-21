// Decompiled with JetBrains decompiler
// Type: BossUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    ((Component) this.layout).get_transform().set_localScale(Vector3.get_zero());
    this.desiredScale = Vector3.get_zero();
  }

  public void SetBoss(Mob b)
  {
    if (Object.op_Inequality((Object) this.currentBoss, (Object) null))
      return;
    this.currentBoss = b;
    ((TMP_Text) this.bossName).set_text("");
    if (b.IsBuff())
    {
      TextMeshProUGUI bossName = this.bossName;
      ((TMP_Text) bossName).set_text(((TMP_Text) bossName).get_text() + "Buff ");
    }
    TextMeshProUGUI bossName1 = this.bossName;
    ((TMP_Text) bossName1).set_text(((TMP_Text) bossName1).get_text() + this.currentBoss.mobType.name);
    this.currentHp = 0.0f;
    this.desiredScale = Vector3.get_one();
    this.hitableMob = (HitableMob) ((Component) b).GetComponent<HitableMob>();
    ((Component) this.layout).get_gameObject().SetActive(true);
    this.layout.set_localScale(Vector3.get_zero());
  }

  private void Update()
  {
    if (Object.op_Equality((Object) this.currentBoss, (Object) null))
    {
      if (!((Component) this.layout).get_gameObject().get_activeInHierarchy())
        return;
      ((Component) this.layout).get_gameObject().SetActive(false);
      if ((double) DayCycle.time >= 0.5)
        return;
      MusicController.Instance.StopSong();
    }
    else
    {
      this.currentHp = Mathf.Lerp(this.currentHp, (float) this.hitableMob.hp, Time.get_deltaTime() * 10f);
      ((TMP_Text) this.hpText).set_text(Mathf.RoundToInt(this.currentHp).ToString() + " / " + (object) this.hitableMob.maxHp);
      float num = (float) this.hitableMob.hp / (float) this.hitableMob.maxHp;
      ((Component) this.hpBar).get_transform().set_localScale(new Vector3(num, 1f, 1f));
      ((Component) this.layout).get_transform().set_localScale(Vector3.Lerp(((Component) this.layout).get_transform().get_localScale(), this.desiredScale, Time.get_deltaTime() * 10f));
    }
  }

  public BossUI() => base.\u002Ector();
}
