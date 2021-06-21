// Decompiled with JetBrains decompiler
// Type: GravePing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class GravePing : MonoBehaviour
{
  public TextMeshProUGUI pingText;
  private float defaultScale = 0.6f;
  private GameObject child;

  private void Awake() => this.child = this.transform.GetChild(0).gameObject;

  public void SetPing(string name) => this.pingText.text = "Revive " + name;

  private void Update()
  {
    if ((double) DayCycle.time <= 0.5)
    {
      this.child.SetActive(true);
      float num = Vector3.Distance(this.transform.position, PlayerMovement.Instance.playerCam.position);
      if ((double) num < 5.0)
        num = 0.0f;
      if ((double) num > 5000.0)
        num = 5000f;
      this.transform.localScale = this.defaultScale * num * Vector3.one;
    }
    else
      this.child.SetActive(false);
  }
}
