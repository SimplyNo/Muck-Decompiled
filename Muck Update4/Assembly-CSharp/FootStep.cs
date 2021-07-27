// Decompiled with JetBrains decompiler
// Type: FootStep
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FootStep : MonoBehaviour
{
  public LayerMask whatIsGround;
  public RandomSfx randomSfx;
  public AudioClip[] woodSfx;

  private void Start() => this.FindGroundType();

  private void FindGroundType()
  {
    RaycastHit hitInfo;
    if (Physics.Raycast(this.transform.position, Vector3.down, out hitInfo, 5f, (int) this.whatIsGround) && hitInfo.collider.gameObject.CompareTag("Build"))
      this.randomSfx.sounds = this.woodSfx;
    this.randomSfx.Randomize(0.0f);
  }
}
