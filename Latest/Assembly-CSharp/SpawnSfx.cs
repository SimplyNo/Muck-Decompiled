// Decompiled with JetBrains decompiler
// Type: SpawnSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SpawnSfx : MonoBehaviour
{
  public GameObject startCharge;
  public Transform pos;

  public void SpawnSound() => Object.Instantiate<GameObject>((M0) this.startCharge, this.pos.get_position(), this.startCharge.get_transform().get_rotation());

  public SpawnSfx() => base.\u002Ector();
}
