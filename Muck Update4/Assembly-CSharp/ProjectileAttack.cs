// Decompiled with JetBrains decompiler
// Type: ProjectileAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
  public GameObject projectile;
  public Transform spawnPos;
  public float launchAngle = 40f;
  public bool useLowestLaunchAngle;
  public Vector3 angularVel;
  public float disableColliderTime;

  public void SpawnProjectile()
  {
    Vector3 position = this.GetComponent<Mob>().target.position;
    GameObject newProjectile = UnityEngine.Object.Instantiate<GameObject>(this.projectile, this.spawnPos.position, Quaternion.identity);
    Rigidbody component = newProjectile.GetComponent<Rigidbody>();
    Vector3 launchVelocity = this.findLaunchVelocity(position, newProjectile);
    float mass = component.mass;
    float fixedDeltaTime = Time.fixedDeltaTime;
    float magnitude = launchVelocity.magnitude;
    component.AddForce(mass * (magnitude / fixedDeltaTime) * launchVelocity.normalized);
    component.angularVelocity = this.angularVel;
  }

  private Vector3 findLaunchVelocity(Vector3 targetPosition, GameObject newProjectile)
  {
    if (this.useLowestLaunchAngle)
    {
      Quaternion quaternion = Quaternion.LookRotation(targetPosition - this.spawnPos.position);
      float x = quaternion.eulerAngles.x;
      MonoBehaviour.print((object) ("raw ang: " + (object) x));
      float num = (double) x >= 180.0 ? 360f - x : -x;
      MonoBehaviour.print((object) ("ang: " + (object) (float) (360.0 - (double) quaternion.eulerAngles.x)));
      this.launchAngle = num + this.launchAngle;
    }
    Vector3 a = new Vector3(this.spawnPos.position.x, this.spawnPos.position.y, this.spawnPos.position.z);
    Vector3 worldPosition = new Vector3(targetPosition.x, this.spawnPos.position.y, targetPosition.z);
    Vector3 b = worldPosition;
    float num1 = Vector3.Distance(a, b);
    newProjectile.transform.LookAt(worldPosition);
    double y = (double) Physics.gravity.y;
    float num2 = Mathf.Tan(this.launchAngle * ((float) Math.PI / 180f));
    float num3 = targetPosition.y - this.spawnPos.position.y;
    double num4 = (double) num1;
    float z = Mathf.Sqrt((float) (y * num4 * (double) num1 / (2.0 * ((double) num3 - (double) num1 * (double) num2))));
    Vector3 direction = new Vector3(0.0f, num2 * z, z);
    return newProjectile.transform.TransformDirection(direction);
  }
}
