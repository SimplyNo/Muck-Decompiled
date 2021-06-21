// Decompiled with JetBrains decompiler
// Type: PowerupUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    this.gridLayout = this.GetComponent<GridLayout>();
  }

  public void AddPowerup(int powerupId)
  {
    if (this.powerups.ContainsKey(powerupId))
    {
      TextMeshProUGUI componentInChildren = this.powerups[powerupId].GetComponentInChildren<TextMeshProUGUI>();
      componentInChildren.text = string.Concat((object) (int.Parse(componentInChildren.text) + 1));
    }
    else
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.uiPrefab, this.transform);
      Powerup allPowerup = ItemManager.Instance.allPowerups[powerupId];
      gameObject.GetComponent<Image>().sprite = allPowerup.sprite;
      gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Concat((object) 1);
      gameObject.GetComponent<PowerupInfo>().powerup = allPowerup;
      this.powerups.Add(powerupId, gameObject);
    }
  }
}
