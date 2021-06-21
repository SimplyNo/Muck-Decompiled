// Decompiled with JetBrains decompiler
// Type: MobLookAtPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MobLookAtPlayer : MonoBehaviour
{
  public bool lookAtPlayer;
  public Transform torso;
  public Transform head;
  private Mob mob;
  private Vector3 defaultHeadRotation;
  private Vector3 defaultTorsoRotation;
  public Vector3 maxTorsoRotation;
  public Vector3 maxHeadRotation;

  private void Awake()
  {
    this.defaultHeadRotation = this.head.transform.eulerAngles;
    this.defaultTorsoRotation = this.torso.transform.eulerAngles;
  }

  private void LateUpdate() => this.LookAtPlayer();

  private void LookAtPlayer()
  {
    if (!this.lookAtPlayer || !(bool) (Object) this.mob.target)
      return;
    Vector3 to = VectorExtensions.XZVector(this.mob.target.position - this.transform.position);
    double num = (double) Vector3.SignedAngle(VectorExtensions.XZVector(this.transform.forward), to, Vector3.up);
  }
}
