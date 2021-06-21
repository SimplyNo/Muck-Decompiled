// Decompiled with JetBrains decompiler
// Type: DontAttackUntilPlayerSpotted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class DontAttackUntilPlayerSpotted : MonoBehaviour
{
  private Mob mob;
  private Vector3 headOffset;
  public int mobZoneId;
  private MobServerNeutral neutral;

  private void Start()
  {
    this.mob = (Mob) ((Component) this).GetComponent<Mob>();
    Object.Destroy((Object) ((Component) this).get_gameObject().GetComponent<MobServer>());
    ((Behaviour) ((Component) this).GetComponent<MobServer>()).set_enabled(false);
    this.neutral = (MobServerNeutral) ((Component) this).get_gameObject().AddComponent<MobServerNeutral>();
    this.neutral.mobZoneId = this.mobZoneId;
    Mesh sharedMesh = ((SkinnedMeshRenderer) ((Component) this).GetComponentInChildren<SkinnedMeshRenderer>()).get_sharedMesh();
    Vector3 up = Vector3.get_up();
    Bounds bounds = sharedMesh.get_bounds();
    // ISSUE: variable of the null type
    __Null y = ((Bounds) ref bounds).get_extents().y;
    this.headOffset = Vector3.op_Multiply(Vector3.op_Multiply(up, (float) y), 1.5f);
    this.InvokeRepeating("CheckForPlayers", 0.5f, 0.5f);
  }

  private void CheckForPlayers()
  {
    Vector3 forward = ((Component) this).get_transform().get_forward();
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if (Object.op_Implicit((Object) playerManager))
      {
        float num = Vector3.Distance(((Component) this).get_transform().get_position(), ((Component) playerManager).get_transform().get_position());
        if ((double) num < 5.0)
          this.FoundPlayer();
        if ((double) num < 40.0)
        {
          Vector3 v = Vector3.op_Subtraction(((Component) playerManager).get_transform().get_position(), ((Component) this).get_transform().get_position());
          if ((double) Mathf.Abs(Vector3.SignedAngle(VectorExtensions.XZVector(v), VectorExtensions.XZVector(forward), Vector3.get_up())) < 55.0)
          {
            Debug.DrawLine(Vector3.op_Addition(((Component) this).get_transform().get_position(), this.headOffset), Vector3.op_Addition(Vector3.op_Addition(((Component) this).get_transform().get_position(), this.headOffset), Vector3.op_Multiply(v, num)), Color.get_black(), 2f);
            RaycastHit raycastHit;
            if (Physics.Raycast(Vector3.op_Addition(((Component) this).get_transform().get_position(), this.headOffset), v, ref raycastHit, num, LayerMask.op_Implicit(GameManager.instance.whatIsGroundAndObject)))
            {
              if (((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().get_layer() == LayerMask.NameToLayer("Player"))
                this.FoundPlayer();
            }
            else
              this.FoundPlayer();
          }
        }
        if (this.mob.hitable.hp < this.mob.hitable.maxHp)
          this.FoundPlayer();
      }
    }
  }

  private void FoundPlayer()
  {
    this.mob.ready = true;
    Object.Destroy((Object) this.neutral);
    if (this.mob.mobType.behaviour == MobType.MobBehaviour.Enemy)
      ((Component) this).get_gameObject().AddComponent<MobServerEnemy>();
    else
      ((Component) this).get_gameObject().AddComponent<MobServerEnemyMeleeAndRanged>();
    Object.Destroy((Object) this);
  }

  public DontAttackUntilPlayerSpotted() => base.\u002Ector();
}
