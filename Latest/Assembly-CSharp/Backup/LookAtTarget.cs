// Decompiled with JetBrains decompiler
// Type: LookAtTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class LookAtTarget : MonoBehaviour
{
  public Transform target;
  public Transform head;
  public float lookDistance = 30f;
  public bool yAxis;
  private Mob mob;

  private void Awake() => this.mob = this.transform.root.GetComponent<Mob>();

  private void LateUpdate()
  {
    if ((Object) this.mob.target == (Object) null || (double) Vector3.Distance(this.mob.target.position, this.transform.position) > (double) this.lookDistance)
      return;
    float num = Mathf.Clamp(Vector3.SignedAngle(this.transform.forward, VectorExtensions.XZVector(this.mob.target.position) - VectorExtensions.XZVector(this.transform.position), Vector3.up), -130f, 130f);
    Vector3 eulerAngles = this.head.transform.localRotation.eulerAngles;
    if (!this.yAxis)
      this.head.transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, num);
    else
      this.head.transform.localRotation = Quaternion.Euler(eulerAngles.x, num, eulerAngles.z);
  }
}
