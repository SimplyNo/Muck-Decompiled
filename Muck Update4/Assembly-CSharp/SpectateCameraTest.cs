// Decompiled with JetBrains decompiler
// Type: SpectateCameraTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SpectateCameraTest : MonoBehaviour
{
  public Transform target;
  private Vector3 desiredSpectateRotation;

  private void Start()
  {
    this.transform.parent = this.target;
    this.transform.localRotation = Quaternion.identity;
    this.transform.localPosition = new Vector3(0.0f, 0.0f, -6f);
  }

  private void Update()
  {
    Vector2 vector2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    this.desiredSpectateRotation += new Vector3(vector2.y, -vector2.x, 0.0f) * 1.5f;
    this.target.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(this.desiredSpectateRotation), Time.deltaTime * 10f);
  }
}
