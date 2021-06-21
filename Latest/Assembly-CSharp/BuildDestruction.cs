// Decompiled with JetBrains decompiler
// Type: BuildDestruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BuildDestruction : MonoBehaviour
{
  public bool connectedToGround;
  public bool directlyGrounded;
  public bool started;
  public bool destroyed;
  private List<BuildDestruction> otherBuilds;
  private BoxCollider trigger;

  private void Awake() => this.Invoke("CheckDirectlyGrounded", 2f);

  private void Start()
  {
    foreach (BoxCollider component in (BoxCollider[]) ((Component) this).GetComponents<BoxCollider>())
    {
      if (((Collider) component).get_isTrigger())
      {
        this.trigger = component;
        break;
      }
    }
    BoxCollider trigger = this.trigger;
    trigger.set_size(Vector3.op_Multiply(trigger.get_size(), 1.1f));
  }

  private void Update()
  {
  }

  private void OnDestroy()
  {
    this.destroyed = true;
    List<BuildDestruction> alreadyChecked = new List<BuildDestruction>();
    alreadyChecked.Add(this);
    for (int index = this.otherBuilds.Count - 1; index >= 0; --index)
    {
      if (!Object.op_Equality((Object) this.otherBuilds[index], (Object) null) && !this.otherBuilds[index].IsDirectlyGrounded(alreadyChecked))
        this.otherBuilds[index].DestroyBuild();
    }
  }

  private void DestroyBuild()
  {
    M0 component = ((Component) this).GetComponent<Hitable>();
    ((Hitable) component).Hit(((Hitable) component).hp, 1f, 1, ((Component) this).get_transform().get_position());
  }

  public bool IsDirectlyGrounded(List<BuildDestruction> alreadyChecked)
  {
    if (this.directlyGrounded)
      return true;
    foreach (BuildDestruction otherBuild in this.otherBuilds)
    {
      if (!Object.op_Equality((Object) otherBuild, (Object) null) && !alreadyChecked.Contains(otherBuild))
      {
        alreadyChecked.Add(otherBuild);
        if (otherBuild.IsDirectlyGrounded(alreadyChecked))
          return true;
      }
    }
    return false;
  }

  private void CheckDirectlyGrounded()
  {
    M0 component = ((Component) this).GetComponent<Rigidbody>();
    Object.Destroy((Object) this.trigger);
    Object.Destroy((Object) component);
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (((Component) collision).get_gameObject().get_layer() == LayerMask.NameToLayer("Ground"))
    {
      this.directlyGrounded = true;
      this.connectedToGround = true;
    }
    if (!((Component) collision).CompareTag("Build"))
      return;
    BuildDestruction component = (BuildDestruction) ((Component) collision).GetComponent<BuildDestruction>();
    if (this.otherBuilds.Contains(component))
      return;
    MonoBehaviour.print((object) ("added a build: " + ((Object) ((Component) collision).get_gameObject()).get_name()));
    this.otherBuilds.Add(component);
  }

  private void OnDrawGizmos()
  {
  }

  public BuildDestruction() => base.\u002Ector();
}
