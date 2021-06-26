// Decompiled with JetBrains decompiler
// Type: Bezier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Bezier
{
  public static Vector3 CalculateCubicBezierPoint(
    float t,
    Vector3 p0,
    Vector3 p1,
    Vector3 p2,
    Vector3 p3)
  {
    float num1 = 1f - t;
    float num2 = t * t;
    float num3 = num1 * num1;
    double num4 = (double) num3 * (double) num1;
    float num5 = num2 * t;
    Vector3 vector3 = p0;
    return (float) num4 * vector3 + 3f * num3 * t * p1 + 3f * num1 * num2 * p2 + num5 * p3;
  }
}
