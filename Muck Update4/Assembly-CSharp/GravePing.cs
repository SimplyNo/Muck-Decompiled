// Decompiled with JetBrains decompiler
// Type: GravePing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class GravePing : MonoBehaviour
{
  public TextMeshProUGUI pingText;
  private float defaultScale = 0.4f;
  private GraveInteract grave;
  private GameObject child;

  private void Awake()
  {
    this.child = this.transform.GetChild(0).gameObject;
    this.grave = this.transform.root.GetComponentInChildren<GraveInteract>();
  }

  public void SetPing(string name) => this.pingText.text = string.Format("Revive {0} ({1}", (object) this.grave.username, (object) this.grave.timeLeft);

  private void Update()
  {
    if ((double) DayCycle.time <= 0.5)
    {
      this.child.SetActive(true);
      string str = "";
      if ((double) this.grave.timeLeft > 0.0)
        str = string.Format("({0})", (object) (int) this.grave.timeLeft);
      this.pingText.text = "Revive " + this.grave.username + " " + str;
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
