// Decompiled with JetBrains decompiler
// Type: PowerupUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUI : MonoBehaviour
{
  public GameObject uiPrefab;
  private GridLayout gridLayout;
  private Dictionary<int, GameObject> powerups;
  public static PowerupUI Instance;

  private void Awake()
  {
    PowerupUI.Instance = this;
    this.powerups = new Dictionary<int, GameObject>();
    this.gridLayout = (GridLayout) ((Component) this).GetComponent<GridLayout>();
  }

  public void AddPowerup(int powerupId)
  {
    if (this.powerups.ContainsKey(powerupId))
    {
      M0 componentInChildren = this.powerups[powerupId].GetComponentInChildren<TextMeshProUGUI>();
      ((TMP_Text) componentInChildren).set_text(string.Concat((object) (int.Parse(((TMP_Text) componentInChildren).get_text()) + 1)));
    }
    else
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.uiPrefab, ((Component) this).get_transform());
      Powerup allPowerup = ItemManager.Instance.allPowerups[powerupId];
      ((Image) gameObject.GetComponent<Image>()).set_sprite(allPowerup.sprite);
      ((TMP_Text) ((Component) gameObject.get_transform().GetChild(0)).GetComponent<TextMeshProUGUI>()).set_text(string.Concat((object) 1));
      ((PowerupInfo) gameObject.GetComponent<PowerupInfo>()).powerup = allPowerup;
      this.powerups.Add(powerupId, gameObject);
    }
  }

  public PowerupUI() => base.\u002Ector();
}
