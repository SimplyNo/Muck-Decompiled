// Decompiled with JetBrains decompiler
// Type: MoveToPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class MoveToPosition : MonoBehaviour
{
  public Transform target;

  public void LateUpdate() => this.transform.position = this.target.position;
}
