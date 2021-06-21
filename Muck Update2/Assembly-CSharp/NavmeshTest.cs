// Decompiled with JetBrains decompiler
// Type: NavmeshTest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class NavmeshTest : MonoBehaviour
{
  private void OnDrawGizmos()
  {
    ((Component) this).GetComponentInChildren<Renderer>();
    Gizmos.set_color(Color.get_red());
    Bounds bounds = ((Collider) ((Component) this).GetComponent<BoxCollider>()).get_bounds();
    Gizmos.DrawWireCube(((Bounds) ref bounds).get_center(), ((Bounds) ref bounds).get_size());
  }

  public NavmeshTest() => base.\u002Ector();
}
