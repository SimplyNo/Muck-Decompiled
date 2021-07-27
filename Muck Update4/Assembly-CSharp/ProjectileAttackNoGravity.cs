// Decompiled with JetBrains decompiler
// Type: ProjectileAttackNoGravity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ProjectileAttackNoGravity : MonoBehaviour
{
  public InventoryItem projectile;
  public InventoryItem predictionProjectile;
  public InventoryItem warningAttack;
  public Transform spawnPos;
  public Transform predictionPos;
  public float attackForce = 1000f;
  public float launchAngle = 40f;
  public bool useLowestLaunchAngle;
  public Vector3 angularVel;
  private Mob mob;

  private void Awake() => this.mob = this.GetComponent<Mob>();

  private void SpawnProjectile()
  {
    if (!LocalClient.serverOwner || (UnityEngine.Object) this.mob.target == (UnityEngine.Object) null)
      return;
    Vector3 position = this.spawnPos.position;
    Vector3 normalized = (this.mob.target.position + Vector3.up * 1f - this.spawnPos.position).normalized;
    float force = this.projectile.bowComponent.projectileSpeed * this.attackForce;
    int id1 = this.projectile.id;
    int id2 = this.mob.id;
    ServerSend.MobSpawnProjectile(position, normalized, force, id1, id2);
    ProjectileController.Instance.SpawnMobProjectile(position, normalized, force, id1, id2);
  }

  public void SpawnPredictedWarningAttack()
  {
    if (!LocalClient.serverOwner || (UnityEngine.Object) this.mob.target == (UnityEngine.Object) null)
      return;
    Rigidbody component = this.mob.target.GetComponent<Rigidbody>();
    Vector3 position = this.mob.target.position;
    Vector3 vector3_1 = Vector3.zero;
    if ((bool) (UnityEngine.Object) component)
      vector3_1 = VectorExtensions.XZVector(component.velocity);
    float timeToImpact = this.warningAttack.bowComponent.timeToImpact;
    Vector3 vector3_2 = vector3_1 * timeToImpact;
    Vector3 vector3_3;
    Vector3 vector3_4 = (vector3_3 = position + vector3_2) - vector3_3;
    float force = 0.0f;
    int id1 = this.warningAttack.id;
    int id2 = this.mob.id;
    ServerSend.MobSpawnProjectile(vector3_3, vector3_4, force, id1, id2);
    ProjectileController.Instance.SpawnMobProjectile(vector3_3, vector3_4, force, id1, id2);
  }

  public void SpawnProjectilePredictionTrajectory()
  {
    if (!LocalClient.serverOwner || (UnityEngine.Object) this.mob.target == (UnityEngine.Object) null)
      return;
    Vector3 position1 = this.mob.target.position;
    Rigidbody component = this.projectile.prefab.GetComponent<Rigidbody>();
    Vector3 launchVelocity = this.findLaunchVelocity(position1, this.spawnPos.gameObject);
    double mass = (double) component.mass;
    float fixedDeltaTime = Time.fixedDeltaTime;
    float magnitude = launchVelocity.magnitude;
    Vector3 position2 = this.spawnPos.position;
    Vector3 normalized = launchVelocity.normalized;
    double num = (double) magnitude / (double) fixedDeltaTime;
    float force = (float) (mass * num);
    int id1 = this.projectile.id;
    int id2 = this.mob.id;
    ServerSend.MobSpawnProjectile(position2, normalized, force, id1, id2);
    ProjectileController.Instance.SpawnMobProjectile(position2, normalized, force, id1, id2);
  }

  public void SpawnProjectilePredictNextPosition()
  {
    if (!LocalClient.serverOwner || (UnityEngine.Object) this.mob.target == (UnityEngine.Object) null)
      return;
    Rigidbody component = this.mob.target.GetComponent<Rigidbody>();
    Vector3 position1 = this.mob.target.position;
    Vector3 vector3_1 = Vector3.zero;
    if ((bool) (UnityEngine.Object) component)
      vector3_1 = VectorExtensions.XZVector(component.velocity);
    float projectileSpeed = this.predictionProjectile.bowComponent.projectileSpeed;
    float num = Vector3.Distance(position1, this.predictionPos.position) / projectileSpeed;
    Vector3 vector3_2 = position1 + vector3_1 * num;
    Vector3 position2 = this.predictionPos.position;
    Vector3 vector3_3 = position2;
    Vector3 vector3_4 = vector3_2 - vector3_3;
    int id1 = this.predictionProjectile.id;
    int id2 = this.mob.id;
    float force = 0.0f;
    ServerSend.MobSpawnProjectile(position2, vector3_4, force, id1, id2);
    ProjectileController.Instance.SpawnMobProjectile(position2, vector3_4, force, id1, id2);
  }

  private Vector3 findLaunchVelocity(Vector3 targetPosition, GameObject newProjectile)
  {
    if (this.useLowestLaunchAngle)
    {
      Vector3 normalized = (targetPosition - newProjectile.transform.position).normalized;
      if ((double) normalized.x > 1.0)
        normalized.x = 1f;
      if ((double) normalized.y > 1.0)
        normalized.y = 1f;
      if ((double) normalized.z > 1.0)
        normalized.z = 1f;
      MonoBehaviour.print((object) ("y component: " + (object) normalized.y));
      this.launchAngle = Mathf.Asin(normalized.y) + this.launchAngle;
      MonoBehaviour.print((object) ("launch angle: " + (object) this.launchAngle));
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
    Vector3 vector3 = newProjectile.transform.TransformDirection(direction);
    if (float.IsNaN(vector3.x))
      vector3 = (targetPosition - newProjectile.transform.position).normalized;
    return vector3;
  }
}
