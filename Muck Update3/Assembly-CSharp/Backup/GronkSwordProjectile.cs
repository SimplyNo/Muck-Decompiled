// Decompiled with JetBrains decompiler
// Type: GronkSwordProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GronkSwordProjectile : MonoBehaviour
{
  private void Start()
  {
    Vector3 forward = this.transform.forward;
    this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + new Vector3(0.0f, 0.0f, -90f));
    Rigidbody component = this.GetComponent<Rigidbody>();
    component.maxAngularVelocity = 9999f;
    component.AddRelativeTorque(component.angularVelocity * 2000f);
    component.angularVelocity = Vector3.zero;
  }
}
