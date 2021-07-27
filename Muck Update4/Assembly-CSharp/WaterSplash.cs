// Decompiled with JetBrains decompiler
// Type: WaterSplash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class WaterSplash : MonoBehaviour
{
  public GameObject splashFx;
  private Rigidbody rb;
  private Vector3 lastPos;

  private void Awake() => this.InvokeRepeating("SlowUpdate", 0.15f, 0.15f);

  private void SlowUpdate()
  {
    float y = World.Instance.water.transform.position.y;
    Vector3 position = this.transform.position;
    if ((double) this.lastPos.y < (double) y)
    {
      if ((double) position.y > (double) y)
        Object.Instantiate<GameObject>(this.splashFx, this.transform.position, Quaternion.LookRotation(position - this.lastPos));
    }
    else if ((double) position.y < (double) y)
      Object.Instantiate<GameObject>(this.splashFx, this.transform.position, Quaternion.LookRotation(position - this.lastPos));
    this.lastPos = position;
  }
}
