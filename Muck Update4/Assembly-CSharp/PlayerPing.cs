// Decompiled with JetBrains decompiler
// Type: PlayerPing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class PlayerPing : MonoBehaviour
{
  private float desiredScale;
  private float localScale;
  public TextMeshProUGUI pingText;

  private void Awake()
  {
    this.desiredScale = 1f;
    this.transform.localScale = Vector3.zero;
    this.Invoke("HidePing", 5f);
  }

  public void SetPing(string username, string item) => this.pingText.text = username + "\n<size=75>" + item;

  private void Update()
  {
    this.localScale = Mathf.Lerp(this.localScale, this.desiredScale, Time.deltaTime * 10f);
    float num = Vector3.Distance(this.transform.position, PlayerMovement.Instance.playerCam.position);
    if ((double) num < 7.0)
      num = 7f;
    if ((double) num > 100.0)
      num = 100f;
    this.transform.localScale = this.localScale * num * Vector3.one;
  }

  private void HidePing() => this.desiredScale = 0.0f;
}
