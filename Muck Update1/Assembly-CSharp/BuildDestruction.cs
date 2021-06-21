// Decompiled with JetBrains decompiler
// Type: BuildDestruction
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BuildDestruction : MonoBehaviour
{
  public bool connectedToGround;
  public bool directlyGrounded;
  public bool started;
  public bool destroyed;
  private List<BuildDestruction> otherBuilds = new List<BuildDestruction>();
  private BoxCollider trigger;

  private void Awake() => this.Invoke("CheckDirectlyGrounded", 2f);

  private void Start()
  {
    foreach (BoxCollider component in this.GetComponents<BoxCollider>())
    {
      if (component.isTrigger)
      {
        this.trigger = component;
        break;
      }
    }
    this.trigger.size *= 1.1f;
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
      if (!((Object) this.otherBuilds[index] == (Object) null) && !this.otherBuilds[index].IsDirectlyGrounded(alreadyChecked))
        this.otherBuilds[index].DestroyBuild();
    }
  }

  private void DestroyBuild()
  {
    Hitable component = this.GetComponent<Hitable>();
    component.Hit(component.hp, 1f, 1, this.transform.position);
  }

  public bool IsDirectlyGrounded(List<BuildDestruction> alreadyChecked)
  {
    if (this.directlyGrounded)
      return true;
    foreach (BuildDestruction otherBuild in this.otherBuilds)
    {
      if (!((Object) otherBuild == (Object) null) && !alreadyChecked.Contains(otherBuild))
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
    Rigidbody component = this.GetComponent<Rigidbody>();
    Object.Destroy((Object) this.trigger);
    Object.Destroy((Object) component);
  }

  private void OnTriggerEnter(Collider collision)
  {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
      this.directlyGrounded = true;
      this.connectedToGround = true;
    }
    if (!collision.CompareTag("Build"))
      return;
    BuildDestruction component = collision.GetComponent<BuildDestruction>();
    if (this.otherBuilds.Contains(component))
      return;
    MonoBehaviour.print((object) ("added a build: " + collision.gameObject.name));
    this.otherBuilds.Add(component);
  }

  private void OnDrawGizmos()
  {
  }
}
