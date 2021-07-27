// Decompiled with JetBrains decompiler
// Type: Ladder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Ladder : MonoBehaviour
{
  private bool onLadder;

  private void FixedUpdate()
  {
    if (!this.onLadder)
      return;
    Vector3 force = Vector3.up * -Physics.gravity.y * PlayerMovement.Instance.GetRb().mass;
    if ((double) PlayerMovement.Instance.GetInput().y > 0.0)
      force *= 6f;
    PlayerMovement.Instance.GetRb().AddForce(force);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (!other.gameObject.CompareTag("Local"))
      return;
    PlayerMovement.Instance.GetRb().drag = 3f;
    this.onLadder = true;
  }

  private void OnTriggerExit(Collider other)
  {
    if (!other.gameObject.CompareTag("Local"))
      return;
    PlayerMovement.Instance.GetRb().drag = 0.0f;
    this.onLadder = false;
  }
}
