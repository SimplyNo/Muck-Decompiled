// Decompiled with JetBrains decompiler
// Type: SpawnEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
  public GameObject spawnEffect;
  public float maxPlayerDistance;

  private void Awake()
  {
    if ((double) Vector3.Distance(PlayerMovement.Instance.playerCam.get_position(), ((Component) this).get_transform().get_position()) < (double) this.maxPlayerDistance)
      ((AudioSource) ((GameObject) Object.Instantiate<GameObject>((M0) this.spawnEffect, ((Component) this).get_transform().get_position(), Quaternion.get_identity())).GetComponent<AudioSource>()).set_maxDistance(this.maxPlayerDistance);
    Object.Destroy((Object) this);
  }

  public SpawnEffect() => base.\u002Ector();
}
