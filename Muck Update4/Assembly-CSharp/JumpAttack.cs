// Decompiled with JetBrains decompiler
// Type: JumpAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class JumpAttack : MonoBehaviour
{
  public RotateWhenRangedAttack rangedRotation;
  public NavMeshAgent agent;
  public Mob mob;
  public GameObject warningPrefab;
  public GameObject jumpFx;
  public GameObject landingFx;
  public float jumpTime = 1f;
  private float currentTime;
  private Vector3 startJumpPos;
  private Vector3 desiredPos;
  public Transform raycastPos;
  public LayerMask whatIsHittable;
  public LayerMask whatIsGroundOnly;

  private void Update()
  {
    if (this.agent.enabled)
      return;
    this.currentTime += Time.deltaTime;
    float currentTime = this.currentTime;
    float y = Physics.gravity.y;
    float num1 = 2f;
    float num2 = (float) (10.0 * (double) currentTime + (double) y * (double) Mathf.Pow(currentTime, 2f)) * num1;
    this.transform.position = Vector3.Lerp(this.startJumpPos, this.desiredPos, this.currentTime / this.jumpTime);
    this.transform.position += Vector3.up * num2;
  }

  private void Jump()
  {
    this.startJumpPos = this.transform.position;
    RaycastHit hitInfo1;
    RaycastHit hitInfo2;
    this.desiredPos = (bool) (Object) this.mob.target ? (!Physics.Raycast(this.raycastPos.position, this.mob.target.position - this.raycastPos.position, out hitInfo1, 200f, (int) this.whatIsHittable) ? this.transform.position : (!Physics.Raycast(hitInfo1.point, Vector3.down, out hitInfo2, 500f, (int) this.whatIsGroundOnly) ? this.transform.position : hitInfo2.point - Vector3.up * this.agent.baseOffset)) : this.transform.position;
    this.agent.enabled = false;
    this.rangedRotation.enabled = false;
    this.currentTime = 0.0f;
    Object.Instantiate<GameObject>(this.warningPrefab, this.desiredPos, this.warningPrefab.transform.rotation).GetComponent<EnemyAttackIndicator>().SetWarning(1f, 13.5f);
    Object.Instantiate<GameObject>(this.jumpFx, this.transform.position, this.landingFx.transform.rotation);
  }

  private void Land()
  {
    this.agent.enabled = true;
    this.rangedRotation.enabled = true;
    Object.Instantiate<GameObject>(this.landingFx, this.transform.position, this.landingFx.transform.rotation);
  }
}
