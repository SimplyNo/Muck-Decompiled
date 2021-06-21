// Decompiled with JetBrains decompiler
// Type: DestroyObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DestroyObject : MonoBehaviour
{
  public float time;

  private void Start() => this.Invoke("DestroySelf", this.time);

  private void DestroySelf() => Object.Destroy((Object) this.gameObject);
}
