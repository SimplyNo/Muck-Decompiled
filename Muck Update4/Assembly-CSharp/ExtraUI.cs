// Decompiled with JetBrains decompiler
// Type: ExtraUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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

  public void InitPlayerStatus(int id, string name, PlayerManager pm)
  {
    GameObject gameObject = Object.Instantiate<GameObject>(this.playerStatusPrefab, (Transform) this.playerStatusParent);
    RawImage component = gameObject.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<RawImage>();
    this.IdToHpBar.Add(id, component);
    Vector2 sizeDelta = this.playerStatusParent.sizeDelta;
    sizeDelta.y += 40f;
    this.playerStatusParent.sizeDelta = sizeDelta;
    gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = name;
    float scale = 0.85f;
    if (pm.id == LocalClient.instance.myId)
      scale = 1f;
    Map.Instance.AddMarker(pm.transform, Map.MarkerType.Player, (Texture) null, Color.white, name, scale);
  }

  private void UpdateClock() => this.clockText.text = this.TimeToClock();

  private void UpdateMoney() => this.money.text = string.Concat((object) InventoryUI.Instance.GetMoney());

  private void UpdateAllHpBars()
  {
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (!((Object) playerManager == (Object) null))
        this.UpdatePlayerHp(playerManager.id);
    }
    List<int> intList = new List<int>();
    foreach (int key in this.IdToHpBar.Keys)
    {
      if (!GameManager.players.ContainsKey(key))
        intList.Add(key);
    }
    foreach (int key in intList)
    {
      GameObject gameObject = this.IdToHpBar[key].transform.parent.parent.parent.gameObject;
      this.IdToHpBar.Remove(key);
      Object.Destroy((Object) gameObject.gameObject);
    }
  }

  public void UpdateDay(int day) => this.dayText.text = string.Concat((object) day);

  private void UpdatePlayerHp(int id)
  {
    if (!this.IdToHpBar.ContainsKey(id))
      return;
    RawImage rawImage = this.IdToHpBar[id];
    float x = 0.0f;
    if (id == LocalClient.instance.myId)
      x = (float) PlayerStatus.Instance.HpAndShield() / (float) PlayerStatus.Instance.MaxHpAndShield();
    else if ((Object) GameManager.players[id] != (Object) null)
    {
      x = Mathf.Clamp(GameManager.players[id].onlinePlayer.hpRatio, 0.0f, 1f);
      if (GameManager.players[id].dead || GameManager.players[id].disconnected)
        x = 0.0f;
    }
    rawImage.transform.localScale = new Vector3(x, 1f, 1f);
  }

  private string TimeToClock() => ((12 + (int) ((double) DayCycle.time * 24.0)) % 24).ToString() + ":" + "00";
}
