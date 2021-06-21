// Decompiled with JetBrains decompiler
// Type: ExtraUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtraUI : MonoBehaviour
{
  public TextMeshProUGUI money;
  public TextMeshProUGUI clockText;
  public TextMeshProUGUI dayText;
  private Dictionary<int, RawImage> IdToHpBar;
  public GameObject playerStatusPrefab;
  public RectTransform playerStatusParent;

  private void Awake()
  {
    this.IdToHpBar = new Dictionary<int, RawImage>();
    this.InvokeRepeating("SlowUpdate", 0.0f, 1f);
  }

  private void SlowUpdate()
  {
    this.UpdateClock();
    this.UpdateMoney();
    this.UpdateAllHpBars();
  }

  public void InitPlayerStatus(int id, string name)
  {
    M0 m0 = Object.Instantiate<GameObject>((M0) this.playerStatusPrefab, (Transform) this.playerStatusParent);
    RawImage component = (RawImage) ((Component) ((GameObject) m0).get_transform().GetChild(1).GetChild(1).GetChild(0)).GetComponent<RawImage>();
    this.IdToHpBar.Add(id, component);
    Vector2 sizeDelta = this.playerStatusParent.get_sizeDelta();
    ref __Null local = ref sizeDelta.y;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    ^(float&) ref local = ^(float&) ref local + 40f;
    this.playerStatusParent.set_sizeDelta(sizeDelta);
    ((TMP_Text) ((Component) ((GameObject) m0).get_transform()).GetComponentInChildren<TextMeshProUGUI>()).set_text(name);
  }

  private void UpdateClock() => ((TMP_Text) this.clockText).set_text(this.TimeToClock());

  private void UpdateMoney() => ((TMP_Text) this.money).set_text(string.Concat((object) InventoryUI.Instance.GetMoney()));

  private void UpdateAllHpBars()
  {
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (!Object.op_Equality((Object) playerManager, (Object) null))
        this.UpdatePlayerHp(playerManager.id);
    }
  }

  public void UpdateDay(int day) => ((TMP_Text) this.dayText).set_text(string.Concat((object) day));

  private void UpdatePlayerHp(int id)
  {
    if (!this.IdToHpBar.ContainsKey(id))
      return;
    RawImage rawImage = this.IdToHpBar[id];
    float num = 0.0f;
    if (id == LocalClient.instance.myId)
      num = (float) PlayerStatus.Instance.HpAndShield() / (float) PlayerStatus.Instance.MaxHpAndShield();
    else if (Object.op_Inequality((Object) GameManager.players[id], (Object) null))
    {
      num = Mathf.Clamp(GameManager.players[id].onlinePlayer.hpRatio, 0.0f, 1f);
      if (GameManager.players[id].dead || GameManager.players[id].disconnected)
        num = 0.0f;
    }
    ((Component) rawImage).get_transform().set_localScale(new Vector3(num, 1f, 1f));
  }

  private string TimeToClock() => ((12 + (int) ((double) DayCycle.time * 24.0)) % 24).ToString() + ":" + "00";

  public ExtraUI() => base.\u002Ector();
}
