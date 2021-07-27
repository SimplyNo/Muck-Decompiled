// Decompiled with JetBrains decompiler
// Type: SpawnAttackFx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SpawnAttackFx : MonoBehaviour
{
  public GameObject[] attackFx;
  public Transform spawnPos;
  private Mob m;

  private void Awake() => this.m = this.GetComponent<Mob>();

  public void SpawnFx(int n)
  {
    ImpactDamage componentInChildren = Object.Instantiate<GameObject>(this.attackFx[n], this.spawnPos.position, this.attackFx[n].transform.rotation).GetComponentInChildren<ImpactDamage>();
    componentInChildren.baseDamage = (int) ((double) componentInChildren.baseDamage * (double) this.m.multiplier);
  }
}
