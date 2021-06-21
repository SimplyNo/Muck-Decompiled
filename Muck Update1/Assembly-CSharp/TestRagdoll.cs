// Decompiled with JetBrains decompiler
// Type: TestRagdoll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TestRagdoll : MonoBehaviour
{
  private Transform cow;
  public PhysicMaterial mat;

  private void Awake() => this.cow = this.transform.GetChild(0);

  private void Update()
  {
  }

  private void Test()
  {
  }

  public void MakeRagdoll(Vector3 dir)
  {
    Animator component = this.GetComponent<Animator>();
    if ((bool) (Object) component)
      component.enabled = false;
    this.cow.SetParent((Transform) null);
    Transform child = this.cow.GetChild(0);
    this.AddComponents(child, (Rigidbody) null, dir);
    this.Ragdoll(child, dir);
    this.cow.gameObject.AddComponent<DestroyObject>().time = 10f;
    this.cow.gameObject.layer = LayerMask.NameToLayer("GroundAndObjectOnly");
    child.gameObject.layer = LayerMask.NameToLayer("GroundAndObjectOnly");
  }

  private void Ragdoll(Transform part, Vector3 dir)
  {
    part.gameObject.layer = LayerMask.NameToLayer("GroundAndObjectOnly");
    for (int index = 0; index < part.childCount; ++index)
    {
      Transform child = part.GetChild(index);
      if (!child.CompareTag("Ignore"))
      {
        this.AddComponents(child, part.GetComponent<Rigidbody>(), dir);
        this.Ragdoll(child, dir);
      }
    }
  }

  private void AddComponents(Transform p, Rigidbody parent, Vector3 dir)
  {
    p.gameObject.layer = LayerMask.NameToLayer("GroundAndObjectOnly");
    Rigidbody rigidbody = p.gameObject.AddComponent<Rigidbody>();
    if (!(bool) (Object) rigidbody)
    {
      rigidbody = p.GetComponent<Rigidbody>();
      rigidbody.isKinematic = false;
      rigidbody.useGravity = true;
    }
    rigidbody.velocity = -dir.normalized * 8f;
    rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    rigidbody.angularDrag = 1f;
    rigidbody.drag = 0.2f;
    MonoBehaviour.print((object) ("problem is here: " + p.name));
    p.gameObject.AddComponent<SphereCollider>().material = this.mat;
    if (!((Object) parent != (Object) null))
      return;
    CharacterJoint characterJoint = p.gameObject.AddComponent<CharacterJoint>();
    characterJoint.connectedBody = parent;
    characterJoint.enableProjection = true;
  }
}
