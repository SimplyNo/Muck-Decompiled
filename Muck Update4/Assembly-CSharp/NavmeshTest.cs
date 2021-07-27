// Decompiled with JetBrains decompiler
// Type: NavmeshTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class NavmeshTest : MonoBehaviour
{
  private void OnDrawGizmos()
  {
    this.GetComponentInChildren<Renderer>();
    Gizmos.color = Color.red;
    Bounds bounds = this.GetComponent<BoxCollider>().bounds;
    Gizmos.DrawWireCube(bounds.center, bounds.size);
  }
}
