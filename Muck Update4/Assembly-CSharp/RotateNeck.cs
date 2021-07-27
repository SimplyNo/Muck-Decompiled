// Decompiled with JetBrains decompiler
// Type: RotateNeck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RotateNeck : MonoBehaviour
{
  public Mob mob;
  public Transform neck;
  public Transform neckForward;
  public Transform customTarget;
  private Quaternion desiredRot;
  private Quaternion desiredHeadRot;
  private Quaternion oldRot;
  private Quaternion oldHeadRot;
  private Transform currentBreath;
  private bool done;
  public GameObject fireBreathPrefab;
  public Transform head;
  public Transform realHead;

  private void OnEnable()
  {
    this.desiredRot = Quaternion.Euler(-8f, 2f, 0.0f);
    this.desiredHeadRot = Quaternion.Euler(-34f, 0.0f, 0.0f);
    this.oldRot = this.desiredRot;
    this.oldHeadRot = this.desiredHeadRot;
    this.Invoke("ResetNeck", 3f);
    this.done = false;
    this.currentBreath = Object.Instantiate<GameObject>(this.fireBreathPrefab).transform;
  }

  private void ResetNeck() => this.done = true;

  private void LateUpdate()
  {
    if (this.done)
    {
      this.desiredRot = Quaternion.Lerp(this.desiredRot, this.oldRot, Time.deltaTime * 3f);
      this.neck.transform.localRotation = this.desiredRot;
      this.currentBreath.rotation = Quaternion.LookRotation(this.head.transform.up);
      this.currentBreath.position = this.head.position;
      this.desiredHeadRot = Quaternion.Lerp(this.desiredHeadRot, this.oldHeadRot, Time.deltaTime * 3f);
      this.realHead.transform.localRotation = this.desiredHeadRot;
    }
    else
    {
      if ((Object) this.mob.target == (Object) null)
        return;
      Transform target = this.mob.target;
      Vector3 from = VectorExtensions.XZVector(this.neckForward.forward);
      Vector3 to = VectorExtensions.XZVector(target.position) - VectorExtensions.XZVector(this.neckForward.position);
      Debug.DrawLine(this.neckForward.position, this.neckForward.position + from * 5f, Color.green);
      Debug.DrawLine(this.neckForward.position, this.neckForward.position + to * 5f, Color.blue);
      float y = Mathf.Clamp(Vector3.SignedAngle(from, to, Vector3.up), -130f, 130f);
      Vector3 eulerAngles1 = this.neck.transform.localRotation.eulerAngles;
      Vector3 eulerAngles2 = this.oldHeadRot.eulerAngles;
      this.desiredRot = Quaternion.Lerp(this.desiredRot, Quaternion.Euler(eulerAngles1.x, y, eulerAngles1.z), Time.deltaTime * 2.5f);
      float num = Mathf.Clamp((float) ((double) Vector3.Distance(target.position, this.realHead.transform.position) * 1.5 - 40.0), -40f, 3f);
      this.desiredHeadRot = Quaternion.Lerp(this.desiredHeadRot, Quaternion.Euler(eulerAngles2.x + num, eulerAngles2.y, eulerAngles2.z), Time.deltaTime * 4f);
      this.neck.transform.localRotation = this.desiredRot;
      this.realHead.transform.localRotation = this.desiredHeadRot;
      this.currentBreath.rotation = Quaternion.LookRotation(this.head.transform.up);
      this.currentBreath.position = this.head.position;
    }
  }
}
