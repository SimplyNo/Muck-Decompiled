// Decompiled with JetBrains decompiler
// Type: GronkSwordProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GronkSwordProjectile : MonoBehaviour
{
  public bool noRotation;

  private void Start()
  {
    Vector3 forward = this.transform.forward;
    this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + new Vector3(0.0f, 0.0f, -90f));
    if (this.noRotation)
      return;
    Rigidbody component = this.GetComponent<Rigidbody>();
    component.maxAngularVelocity = 0.0f;
    component.maxAngularVelocity = 9999f;
    component.AddRelativeTorque(component.angularVelocity * 2000f);
    component.angularVelocity = Vector3.zero;
  }
}
