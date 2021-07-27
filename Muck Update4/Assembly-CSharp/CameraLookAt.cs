// Decompiled with JetBrains decompiler
// Type: CameraLookAt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
  public Transform target;

  private void Update() => this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(this.target.position - this.transform.position), Time.deltaTime * 6.4f);
}
