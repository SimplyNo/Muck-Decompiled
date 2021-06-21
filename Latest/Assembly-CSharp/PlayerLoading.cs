// Decompiled with JetBrains decompiler
// Type: PlayerLoading
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class PlayerLoading : MonoBehaviour
{
  public TextMeshProUGUI name;
  public TextMeshProUGUI status;

  public void SetStatus(string name, string status)
  {
    ((TMP_Text) this.name).set_text(name);
    ((TMP_Text) this.status).set_text(status);
  }

  public void ChangeStatus(string status) => ((TMP_Text) this.status).set_text(status);

  public PlayerLoading() => base.\u002Ector();
}
