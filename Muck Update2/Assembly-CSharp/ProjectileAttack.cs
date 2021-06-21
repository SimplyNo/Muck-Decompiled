// Decompiled with JetBrains decompiler
// Type: ProjectileAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
  public GameObject projectile;
  public Transform spawnPos;
  public float launchAngle;
  public bool useLowestLaunchAngle;
  public Vector3 angularVel;
  public float disableColliderTime;

  public void SpawnProjectile()
  {
    Vector3 position = ((Mob) ((Component) this).GetComponent<Mob>()).target.get_position();
    GameObject newProjectile = (GameObject) Object.Instantiate<GameObject>((M0) this.projectile, this.spawnPos.get_position(), Quaternion.get_identity());
    M0 component = newProjectile.GetComponent<Rigidbody>();
    Vector3 launchVelocity = this.findLaunchVelocity(position, newProjectile);
    float mass = ((Rigidbody) component).get_mass();
    float fixedDeltaTime = Time.get_fixedDeltaTime();
    float magnitude = ((Vector3) ref launchVelocity).get_magnitude();
    ((Rigidbody) component).AddForce(Vector3.op_Multiply(mass * (magnitude / fixedDeltaTime), ((Vector3) ref launchVelocity).get_normalized()));
    ((Rigidbody) component).set_angularVelocity(this.angularVel);
  }

  private Vector3 findLaunchVelocity(Vector3 targetPosition, GameObject newProjectile)
  {
    if (this.useLowestLaunchAngle)
    {
      Quaternion quaternion = Quaternion.LookRotation(Vector3.op_Subtraction(targetPosition, this.spawnPos.get_position()));
      float x = (float) ((Quaternion) ref quaternion).get_eulerAngles().x;
      MonoBehaviour.print((object) ("raw ang: " + (object) x));
      float num = (double) x >= 180.0 ? 360f - x : -x;
      MonoBehaviour.print((object) ("ang: " + (object) (float) (360.0 - ((Quaternion) ref quaternion).get_eulerAngles().x)));
      this.launchAngle = num + this.launchAngle;
    }
    Vector3 vector3_1 = new Vector3((float) this.spawnPos.get_position().x, (float) this.spawnPos.get_position().y, (float) this.spawnPos.get_position().z);
    Vector3 vector3_2;
    ((Vector3) ref vector3_2).\u002Ector((float) targetPosition.x, (float) this.spawnPos.get_position().y, (float) targetPosition.z);
    Vector3 vector3_3 = vector3_2;
    float num1 = Vector3.Distance(vector3_1, vector3_3);
    newProjectile.get_transform().LookAt(vector3_2);
    // ISSUE: variable of the null type
    __Null y = Physics.get_gravity().y;
    float num2 = Mathf.Tan(this.launchAngle * ((float) Math.PI / 180f));
    float num3 = (float) (targetPosition.y - this.spawnPos.get_position().y);
    double num4 = (double) num1;
    float num5 = Mathf.Sqrt((float) (y * num4 * (double) num1 / (2.0 * ((double) num3 - (double) num1 * (double) num2))));
    float num6 = num2 * num5;
    Vector3 vector3_4;
    ((Vector3) ref vector3_4).\u002Ector(0.0f, num6, num5);
    return newProjectile.get_transform().TransformDirection(vector3_4);
  }

  public ProjectileAttack() => base.\u002Ector();
}
