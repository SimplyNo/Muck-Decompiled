// Decompiled with JetBrains decompiler
// Type: CinematicCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
  public Transform target;
  public float speed;

  private void Update()
  {
    this.transform.LookAt(this.target);
    this.transform.RotateAround(this.target.position, Vector3.up, this.speed);
  }
}
