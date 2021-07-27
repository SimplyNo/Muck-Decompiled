// Decompiled with JetBrains decompiler
// Type: SpawnStepSmoke
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SpawnStepSmoke : MonoBehaviour
{
  public Transform leftFoot;
  public Transform rightFoot;
  public GameObject stepFx;

  public void LeftStep() => Object.Instantiate<GameObject>(this.stepFx, this.leftFoot.position, this.stepFx.transform.rotation);

  public void RightStep() => Object.Instantiate<GameObject>(this.stepFx, this.rightFoot.position, this.stepFx.transform.rotation);
}
