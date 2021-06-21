// Decompiled with JetBrains decompiler
// Type: PlayerLoading
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class PlayerLoading : MonoBehaviour
{
  public TextMeshProUGUI name;
  public TextMeshProUGUI status;

  public void SetStatus(string name, string status)
  {
    this.name.text = name;
    this.status.text = status;
  }

  public void ChangeStatus(string status) => this.status.text = status;
}
