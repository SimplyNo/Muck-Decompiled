// Decompiled with JetBrains decompiler
// Type: DestroyObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class DestroyObject : MonoBehaviour
{
  public float time;

  private void Start() => this.Invoke("DestroySelf", this.time);

  private void DestroySelf() => Object.Destroy((Object) ((Component) this).get_gameObject());

  public DestroyObject() => base.\u002Ector();
}
