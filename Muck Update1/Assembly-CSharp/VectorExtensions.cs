// Decompiled with JetBrains decompiler
// Type: VectorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class VectorExtensions : MonoBehaviour
{
  public static Vector3 XZVector(Vector3 v) => new Vector3(v.x, 0.0f, v.z);
}
