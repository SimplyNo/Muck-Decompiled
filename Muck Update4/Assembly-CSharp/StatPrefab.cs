// Decompiled with JetBrains decompiler
// Type: StatPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPrefab : MonoBehaviour
{
  public TextMeshProUGUI statName;
  public TextMeshProUGUI statValue;

  public void SetStat(KeyValuePair<string, int> s)
  {
    this.statName.text = s.Key + " |";
    this.statValue.text = string.Concat((object) s.Value);
  }
}
