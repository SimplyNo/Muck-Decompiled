// Decompiled with JetBrains decompiler
// Type: ObjectivePing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class ObjectivePing : MonoBehaviour
{
  public TextMeshProUGUI text;
  private float defaultScale = 0.4f;

  private void Awake() => this.transform.parent = (Transform) null;

  public void SetText(string s) => this.text.text = s;

  private void Update()
  {
    if (!(bool) (Object) PlayerMovement.Instance)
      return;
    float num = Vector3.Distance(this.transform.position, PlayerMovement.Instance.playerCam.position);
    if ((double) num < 5.0)
      num = 0.0f;
    if ((double) num > 5000.0)
      num = 5000f;
    this.transform.localScale = this.defaultScale * num * Vector3.one;
  }
}
