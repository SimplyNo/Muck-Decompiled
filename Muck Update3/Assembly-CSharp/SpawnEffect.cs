// Decompiled with JetBrains decompiler
// Type: SpawnEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
  public GameObject spawnEffect;
  public float maxPlayerDistance = 40f;

  private void Awake()
  {
    if ((double) Vector3.Distance(PlayerMovement.Instance.playerCam.position, this.transform.position) < (double) this.maxPlayerDistance)
      Object.Instantiate<GameObject>(this.spawnEffect, this.transform.position, Quaternion.identity).GetComponent<AudioSource>().maxDistance = this.maxPlayerDistance;
    Object.Destroy((Object) this);
  }
}
