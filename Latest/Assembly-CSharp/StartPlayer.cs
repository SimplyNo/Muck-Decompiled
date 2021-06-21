// Decompiled with JetBrains decompiler
// Type: StartPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class StartPlayer : MonoBehaviour
{
  private void Start()
  {
    for (int index = ((Component) this).get_transform().get_childCount() - 1; index >= 0; --index)
      ((Component) this).get_transform().GetChild(index).set_parent((Transform) null);
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }

  public StartPlayer() => base.\u002Ector();
}
