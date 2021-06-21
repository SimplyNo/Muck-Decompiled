// Decompiled with JetBrains decompiler
// Type: TestRagdoll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class TestRagdoll : MonoBehaviour
{
  private Transform cow;
  public PhysicMaterial mat;

  private void Awake() => this.cow = ((Component) this).get_transform().GetChild(0);

  private void Update()
  {
  }

  private void Test()
  {
  }

  public void MakeRagdoll(Vector3 dir)
  {
    Animator component = (Animator) ((Component) this).GetComponent<Animator>();
    if (Object.op_Implicit((Object) component))
      ((Behaviour) component).set_enabled(false);
    this.cow.SetParent((Transform) null);
    Transform child = this.cow.GetChild(0);
    this.AddComponents(child, (Rigidbody) null, dir);
    this.Ragdoll(child, dir);
    ((DestroyObject) ((Component) this.cow).get_gameObject().AddComponent<DestroyObject>()).time = 10f;
    ((Component) this.cow).get_gameObject().set_layer(LayerMask.NameToLayer("GroundAndObjectOnly"));
    ((Component) child).get_gameObject().set_layer(LayerMask.NameToLayer("GroundAndObjectOnly"));
  }

  private void Ragdoll(Transform part, Vector3 dir)
  {
    ((Component) part).get_gameObject().set_layer(LayerMask.NameToLayer("GroundAndObjectOnly"));
    for (int index = 0; index < part.get_childCount(); ++index)
    {
      Transform child = part.GetChild(index);
      if (!((Component) child).CompareTag("Ignore"))
      {
        this.AddComponents(child, (Rigidbody) ((Component) part).GetComponent<Rigidbody>(), dir);
        this.Ragdoll(child, dir);
      }
    }
  }

  private void AddComponents(Transform p, Rigidbody parent, Vector3 dir)
  {
    ((Component) p).get_gameObject().set_layer(LayerMask.NameToLayer("GroundAndObjectOnly"));
    Rigidbody rigidbody = (Rigidbody) ((Component) p).get_gameObject().AddComponent<Rigidbody>();
    if (!Object.op_Implicit((Object) rigidbody))
    {
      rigidbody = (Rigidbody) ((Component) p).GetComponent<Rigidbody>();
      rigidbody.set_isKinematic(false);
      rigidbody.set_useGravity(true);
    }
    rigidbody.set_velocity(Vector3.op_Multiply(Vector3.op_UnaryNegation(((Vector3) ref dir).get_normalized()), 8f));
    rigidbody.set_interpolation((RigidbodyInterpolation) 1);
    rigidbody.set_angularDrag(1f);
    rigidbody.set_drag(0.2f);
    ((Collider) ((Component) p).get_gameObject().AddComponent<SphereCollider>()).set_material(this.mat);
    if (!Object.op_Inequality((Object) parent, (Object) null))
      return;
    M0 m0 = ((Component) p).get_gameObject().AddComponent<CharacterJoint>();
    ((Joint) m0).set_connectedBody(parent);
    ((CharacterJoint) m0).set_enableProjection(true);
  }

  public TestRagdoll() => base.\u002Ector();
}
