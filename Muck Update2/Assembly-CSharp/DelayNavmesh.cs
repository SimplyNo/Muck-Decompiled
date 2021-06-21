// Decompiled with JetBrains decompiler
// Type: DelayNavmesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class DelayNavmesh : MonoBehaviour
{
  private void Awake() => this.Invoke("ResetObstacle", Random.Range(5f, 15f));

  private void ResetObstacle()
  {
    M0 component = ((Component) this).GetComponent<NavMeshObstacle>();
    ((Behaviour) component).set_enabled(false);
    ((Behaviour) component).set_enabled(true);
  }

  public DelayNavmesh() => base.\u002Ector();
}
