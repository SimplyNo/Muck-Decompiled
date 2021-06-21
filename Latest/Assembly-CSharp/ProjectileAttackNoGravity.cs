// Decompiled with JetBrains decompiler
// Type: ProjectileAttackNoGravity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using UnityEngine;

public class ProjectileAttackNoGravity : MonoBehaviour
{
  public InventoryItem projectile;
  public InventoryItem predictionProjectile;
  public Transform spawnPos;
  public Transform predictionPos;
  public float attackForce;
  public float launchAngle;
  public bool useLowestLaunchAngle;
  public Vector3 angularVel;
  private Mob mob;

  private void Awake() => this.mob = (Mob) ((Component) this).GetComponent<Mob>();

  private void SpawnProjectile()
  {
    if (!LocalClient.serverOwner || Object.op_Equality((Object) this.mob.target, (Object) null))
      return;
    Vector3 position = this.spawnPos.get_position();
    Vector3 vector3 = Vector3.op_Subtraction(Vector3.op_Addition(this.mob.target.get_position(), Vector3.op_Multiply(Vector3.get_up(), 1f)), this.spawnPos.get_position());
    Vector3 normalized = ((Vector3) ref vector3).get_normalized();
    float force = this.projectile.bowComponent.projectileSpeed * this.attackForce;
    int id1 = this.projectile.id;
    int id2 = this.mob.id;
    ServerSend.MobSpawnProjectile(position, normalized, force, id1, id2);
    ProjectileController.Instance.SpawnMobProjectile(position, normalized, force, id1, id2);
  }

  public void SpawnProjectilePredictionTrajectory()
  {
    if (!LocalClient.serverOwner || Object.op_Equality((Object) this.mob.target, (Object) null))
      return;
    Vector3 position1 = this.mob.target.get_position();
    M0 component = this.projectile.prefab.GetComponent<Rigidbody>();
    Vector3 launchVelocity = this.findLaunchVelocity(position1, ((Component) this.spawnPos).get_gameObject());
    double mass = (double) ((Rigidbody) component).get_mass();
    float fixedDeltaTime = Time.get_fixedDeltaTime();
    float magnitude = ((Vector3) ref launchVelocity).get_magnitude();
    Vector3 position2 = this.spawnPos.get_position();
    Vector3 normalized = ((Vector3) ref launchVelocity).get_normalized();
    double num = (double) magnitude / (double) fixedDeltaTime;
    float force = (float) (mass * num);
    int id1 = this.projectile.id;
    int id2 = this.mob.id;
    ServerSend.MobSpawnProjectile(position2, normalized, force, id1, id2);
    ProjectileController.Instance.SpawnMobProjectile(position2, normalized, force, id1, id2);
  }

  public void SpawnProjectilePredictNextPosition()
  {
    if (!LocalClient.serverOwner || Object.op_Equality((Object) this.mob.target, (Object) null))
      return;
    Rigidbody component = (Rigidbody) ((Component) this.mob.target).GetComponent<Rigidbody>();
    Vector3 position1 = this.mob.target.get_position();
    Vector3 vector3_1 = Vector3.get_zero();
    if (Object.op_Implicit((Object) component))
      vector3_1 = VectorExtensions.XZVector(component.get_velocity());
    float projectileSpeed = this.predictionProjectile.bowComponent.projectileSpeed;
    float num = Vector3.Distance(position1, this.predictionPos.get_position()) / projectileSpeed;
    Vector3 vector3_2 = Vector3.op_Addition(position1, Vector3.op_Multiply(vector3_1, num));
    Debug.DrawLine(position1, vector3_2, Color.get_black(), 10f);
    Vector3 position2 = this.predictionPos.get_position();
    Vector3 vector3_3 = Vector3.op_Subtraction(vector3_2, position2);
    int id1 = this.predictionProjectile.id;
    int id2 = this.mob.id;
    float force = 0.0f;
    ServerSend.MobSpawnProjectile(position2, vector3_3, force, id1, id2);
    ProjectileController.Instance.SpawnMobProjectile(position2, vector3_3, force, id1, id2);
  }

  private Vector3 findLaunchVelocity(Vector3 targetPosition, GameObject newProjectile)
  {
    if (this.useLowestLaunchAngle)
    {
      Vector3 vector3 = Vector3.op_Subtraction(targetPosition, newProjectile.get_transform().get_position());
      Vector3 normalized = ((Vector3) ref vector3).get_normalized();
      if (normalized.x > 1.0)
        normalized.x = (__Null) 1.0;
      if (normalized.y > 1.0)
        normalized.y = (__Null) 1.0;
      if (normalized.z > 1.0)
        normalized.z = (__Null) 1.0;
      MonoBehaviour.print((object) ("y component: " + (object) (float) normalized.y));
      this.launchAngle = Mathf.Asin((float) normalized.y) + this.launchAngle;
      MonoBehaviour.print((object) ("launch angle: " + (object) this.launchAngle));
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
    Vector3 vector3_5 = newProjectile.get_transform().TransformDirection(vector3_4);
    if (float.IsNaN((float) vector3_5.x))
    {
      Vector3 vector3_6 = Vector3.op_Subtraction(targetPosition, newProjectile.get_transform().get_position());
      vector3_5 = ((Vector3) ref vector3_6).get_normalized();
    }
    return vector3_5;
  }

  public ProjectileAttackNoGravity() => base.\u002Ector();
}
