// Decompiled with JetBrains decompiler
// Type: MoveCameraTowards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MoveCameraTowards : MonoBehaviour
{
  public float speed = 1f;
  public Transform target;
  private bool ready;

  private void Awake() => this.Invoke("SetReady", 1f);

  private void SetReady() => this.ready = true;

  private void Update()
  {
    if (!this.ready)
      return;
    this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, Time.deltaTime * this.speed);
  }
}
