// Decompiled with JetBrains decompiler
// Type: MoveToPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class MoveToPosition : MonoBehaviour
{
  public Transform target;

  public void LateUpdate() => this.transform.position = this.target.position;
}
