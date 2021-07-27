// Decompiled with JetBrains decompiler
// Type: VectorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class VectorExtensions : MonoBehaviour
{
  public static Vector3 XZVector(Vector3 v) => new Vector3(v.x, 0.0f, v.z);
}
