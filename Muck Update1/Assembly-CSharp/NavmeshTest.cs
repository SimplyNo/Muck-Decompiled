// Decompiled with JetBrains decompiler
// Type: NavmeshTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
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
