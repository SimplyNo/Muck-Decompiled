// Decompiled with JetBrains decompiler
// Type: FallIfNotGrounded
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FallIfNotGrounded : MonoBehaviour
{
  private Rigidbody rb;
  public float x;
  public float z;
  private bool falling;
  private Vector3 bottomOffset;
  public LayerMask whatIsLandable;
  private Mesh mesh;
  private Collider c;

  private void Start()
  {
    this.x = this.transform.position.x;
    this.z = this.transform.position.z;
    MeshFilter componentInChildren1 = this.GetComponentInChildren<MeshFilter>();
    if ((bool) (Object) componentInChildren1)
    {
      this.mesh = componentInChildren1.mesh;
    }
    else
    {
      SkinnedMeshRenderer componentInChildren2 = this.GetComponentInChildren<SkinnedMeshRenderer>();
      if ((bool) (Object) componentInChildren2)
        this.mesh = componentInChildren2.sharedMesh;
    }
    this.c = this.GetComponent<Collider>();
    if ((bool) (Object) this.c)
      this.bottomOffset = new Vector3(0.0f, this.c.bounds.extents.y, 0.0f);
    this.InvokeRepeating("CheckFalling", 1f, 1f);
  }

  private void CheckFalling()
  {
    if (this.falling)
      return;
    bool flag = false;
    foreach (RaycastHit raycastHit in Physics.RaycastAll(this.transform.position, Vector3.down, 2f, (int) this.whatIsLandable))
    {
      if (raycastHit.collider.gameObject.layer != LayerMask.NameToLayer("Pickup"))
        flag = true;
    }
    if (flag)
      return;
    this.StartFalling();
  }

  private void StartFalling()
  {
    Hitable component = this.GetComponent<Hitable>();
    component.Hit(component.maxHp, 0.0f, 0, this.transform.position);
    this.falling = true;
    this.rb = this.gameObject.AddComponent<Rigidbody>();
    this.rb.isKinematic = false;
    this.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
  }

  private void Land()
  {
    Object.Destroy((Object) this.rb);
    this.falling = false;
  }
}
