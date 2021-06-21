// Decompiled with JetBrains decompiler
// Type: GenerateNavmesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class GenerateNavmesh : MonoBehaviour
{
  public NavMeshSurface surface;

  public void GenerateNavMesh() => this.surface.BuildNavMesh();

  public GenerateNavmesh() => base.\u002Ector();
}
