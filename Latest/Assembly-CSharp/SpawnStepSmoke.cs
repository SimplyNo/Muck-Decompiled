// Decompiled with JetBrains decompiler
// Type: SpawnStepSmoke
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SpawnStepSmoke : MonoBehaviour
{
  public Transform leftFoot;
  public Transform rightFoot;
  public GameObject stepFx;

  public void LeftStep() => Object.Instantiate<GameObject>((M0) this.stepFx, this.leftFoot.get_position(), this.stepFx.get_transform().get_rotation());

  public void RightStep() => Object.Instantiate<GameObject>((M0) this.stepFx, this.rightFoot.get_position(), this.stepFx.get_transform().get_rotation());

  public SpawnStepSmoke() => base.\u002Ector();
}
