// Decompiled with JetBrains decompiler
// Type: RespawnPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RespawnPrefab : MonoBehaviour
{
  public RawImage overlay;
  public Button button;
  public TextMeshProUGUI nameText;

  public int playerId { get; set; }

  public void Set(int id, bool active, string username)
  {
    this.playerId = id;
    ((Component) this.overlay).get_gameObject().SetActive(!active);
    ((Behaviour) this.button).set_enabled(active);
    ((TMP_Text) this.nameText).set_text(username);
  }

  public void RespawnPlayer()
  {
    Debug.LogError((object) "requesting revive");
    RespawnTotemUI.Instance.RequestRevive(this.playerId);
  }

  public RespawnPrefab() => base.\u002Ector();
}
