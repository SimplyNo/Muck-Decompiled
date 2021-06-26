// Decompiled with JetBrains decompiler
// Type: PlayerLoading
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
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
