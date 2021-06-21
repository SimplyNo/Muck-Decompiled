// Decompiled with JetBrains decompiler
// Type: FallIfNotGrounded
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    this.x = (float) ((Component) this).get_transform().get_position().x;
    this.z = (float) ((Component) this).get_transform().get_position().z;
    MeshFilter componentInChildren1 = (MeshFilter) ((Component) this).GetComponentInChildren<MeshFilter>();
    if (Object.op_Implicit((Object) componentInChildren1))
    {
      this.mesh = componentInChildren1.get_mesh();
    }
    else
    {
      SkinnedMeshRenderer componentInChildren2 = (SkinnedMeshRenderer) ((Component) this).GetComponentInChildren<SkinnedMeshRenderer>();
      if (Object.op_Implicit((Object) componentInChildren2))
        this.mesh = componentInChildren2.get_sharedMesh();
    }
    this.c = (Collider) ((Component) this).GetComponent<Collider>();
    if (Object.op_Implicit((Object) this.c))
    {
      Bounds bounds = this.c.get_bounds();
      this.bottomOffset = new Vector3(0.0f, (float) ((Bounds) ref bounds).get_extents().y, 0.0f);
    }
    this.InvokeRepeating("CheckFalling", 1f, 1f);
  }

  private void CheckFalling()
  {
    if (this.falling)
      return;
    bool flag = false;
    foreach (RaycastHit raycastHit in Physics.RaycastAll(((Component) this).get_transform().get_position(), Vector3.get_down(), 2f, LayerMask.op_Implicit(this.whatIsLandable)))
    {
      if (((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().get_layer() != LayerMask.NameToLayer("Pickup"))
        flag = true;
    }
    if (flag)
      return;
    this.StartFalling();
  }

  private void StartFalling()
  {
    M0 component = ((Component) this).GetComponent<Hitable>();
    ((Hitable) component).Hit(((Hitable) component).maxHp, 0.0f, 0, ((Component) this).get_transform().get_position());
    this.falling = true;
    this.rb = (Rigidbody) ((Component) this).get_gameObject().AddComponent<Rigidbody>();
    this.rb.set_isKinematic(false);
    this.rb.set_constraints((RigidbodyConstraints) 122);
  }

  private void Land()
  {
    Object.Destroy((Object) this.rb);
    this.falling = false;
  }

  public FallIfNotGrounded() => base.\u002Ector();
}
