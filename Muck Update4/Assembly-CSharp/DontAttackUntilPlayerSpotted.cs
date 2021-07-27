// Decompiled with JetBrains decompiler
// Type: DontAttackUntilPlayerSpotted
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DontAttackUntilPlayerSpotted : MonoBehaviour
{
  private Mob mob;
  private Vector3 headOffset;
  public int mobZoneId;
  private MobServerNeutral neutral;

  private void Start()
  {
    this.mob = this.GetComponent<Mob>();
    Object.Destroy((Object) this.gameObject.GetComponent<MobServer>());
    this.GetComponent<MobServer>().enabled = false;
    this.neutral = this.gameObject.AddComponent<MobServerNeutral>();
    this.neutral.mobZoneId = this.mobZoneId;
    this.headOffset = Vector3.up * this.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.extents.y * 1.5f;
    this.InvokeRepeating("CheckForPlayers", 0.5f, 0.5f);
  }

  private void CheckForPlayers()
  {
    Vector3 forward = this.transform.forward;
    foreach (PlayerManager playerManager in GameManager.players.Values)
    {
      if ((bool) (Object) playerManager)
      {
        float maxDistance = Vector3.Distance(this.transform.position, playerManager.transform.position);
        if ((double) maxDistance < 5.0)
          this.FoundPlayer();
        if ((double) maxDistance < 40.0)
        {
          Vector3 vector3 = playerManager.transform.position - this.transform.position;
          if ((double) Mathf.Abs(Vector3.SignedAngle(VectorExtensions.XZVector(vector3), VectorExtensions.XZVector(forward), Vector3.up)) < 55.0)
          {
            Debug.DrawLine(this.transform.position + this.headOffset, this.transform.position + this.headOffset + vector3 * maxDistance, Color.black, 2f);
            RaycastHit hitInfo;
            if (Physics.Raycast(this.transform.position + this.headOffset, vector3, out hitInfo, maxDistance, (int) GameManager.instance.whatIsGroundAndObject))
            {
              if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
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
      this.gameObject.AddComponent<MobServerEnemy>();
    else
      this.gameObject.AddComponent<MobServerEnemyMeleeAndRanged>();
    Object.Destroy((Object) this);
  }
}
