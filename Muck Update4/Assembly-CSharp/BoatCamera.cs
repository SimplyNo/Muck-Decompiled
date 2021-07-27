// Decompiled with JetBrains decompiler
// Type: BoatCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BoatCamera : MonoBehaviour
{
  private Transform target;
  private Transform dragonTransform;

  private void Awake()
  {
    this.target = Boat.Instance.rbTransform;
    this.dragonTransform = Dragon.Instance.transform;
    this.Invoke("StopCamera", 5f);
  }

  private void StopCamera() => this.gameObject.SetActive(false);

  private void Update()
  {
    if ((Object) this.transform != (Object) this.dragonTransform && (double) this.dragonTransform.position.y > (double) this.target.transform.position.y)
      this.target = this.dragonTransform;
    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(this.target.position - this.transform.position), Time.deltaTime * 6f);
  }
}
