// Decompiled with JetBrains decompiler
// Type: StatusMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class StatusMessage : MonoBehaviour
{
  public TextMeshProUGUI statusText;
  public GameObject status;
  private Vector3 defaultScale;
  public static StatusMessage Instance;

  private void Awake()
  {
    StatusMessage.Instance = this;
    this.defaultScale = this.status.transform.localScale;
  }

  private void Update() => this.status.transform.localScale = Vector3.Lerp(this.status.transform.localScale, this.defaultScale, Time.deltaTime * 25f);

  public void DisplayMessage(string message)
  {
    this.status.transform.parent.gameObject.SetActive(true);
    this.status.transform.localScale = Vector3.zero;
    this.statusText.text = message;
  }

  public void OkayDokay() => this.status.transform.parent.gameObject.SetActive(false);
}
