// Decompiled with JetBrains decompiler
// Type: VectorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class VectorExtensions : MonoBehaviour
{
  public static Vector3 XZVector(Vector3 v) => new Vector3(v.x, 0.0f, v.z);
}
