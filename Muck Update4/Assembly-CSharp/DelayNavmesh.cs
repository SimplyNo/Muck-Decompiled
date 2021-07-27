// Decompiled with JetBrains decompiler
// Type: DelayNavmesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class DelayNavmesh : MonoBehaviour
{
  private void Awake() => this.Invoke("ResetObstacle", Random.Range(5f, 15f));

  private void ResetObstacle()
  {
    NavMeshObstacle component = this.GetComponent<NavMeshObstacle>();
    component.enabled = false;
    component.enabled = true;
  }
}
