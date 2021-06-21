// Decompiled with JetBrains decompiler
// Type: LookAtTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
  public Transform target;
  public Transform head;
  public float lookDistance = 30f;
  private Mob mob;

  private void Awake() => this.mob = this.transform.root.GetComponent<Mob>();

  private void LateUpdate()
  {
    if ((Object) this.mob.target == (Object) null || (double) Vector3.Distance(this.mob.target.position, this.transform.position) > (double) this.lookDistance)
      return;
    float z = Mathf.Clamp(Vector3.SignedAngle(this.transform.forward, VectorExtensions.XZVector(this.mob.target.position) - VectorExtensions.XZVector(this.transform.position), Vector3.up), -130f, 130f);
    Vector3 eulerAngles = this.head.transform.localRotation.eulerAngles;
    this.head.transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, z);
  }
}
