// Decompiled with JetBrains decompiler
// Type: MobFlying
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MobFlying : Mob
{
  private float defaultHeight = 8f;
  public LayerMask whatIsGround;

  public override void ExtraUpdate()
  {
    RaycastHit hitInfo;
    if (!(bool) (Object) this.target || !Physics.Raycast(this.target.transform.position, Vector3.down, out hitInfo, 5000f, (int) this.whatIsGround))
      return;
    this.agent.baseOffset = Mathf.Lerp(this.agent.baseOffset, this.defaultHeight + hitInfo.distance, Time.deltaTime * 0.3f);
  }
}
