// Decompiled with JetBrains decompiler
// Type: FalloffGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public static class FalloffGenerator
{
  public static float[,] GenerateFalloffMap(int size)
  {
    float[,] numArray = new float[size, size];
    for (int index1 = 0; index1 < size; ++index1)
    {
      for (int index2 = 0; index2 < size; ++index2)
      {
        double num1 = (double) index1 / (double) size * 2.0 - 1.0;
        float num2 = (float) ((double) index2 / (double) size * 2.0 - 1.0);
        float num3 = Mathf.Max(Mathf.Abs((float) num1), Mathf.Abs(num2));
        numArray[index1, index2] = FalloffGenerator.Evaluate(num3);
      }
    }
    return numArray;
  }

  private static float Evaluate(float value)
  {
    float num1 = 3f;
    float num2 = 2.2f;
    return Mathf.Pow(value, num1) / (Mathf.Pow(value, num1) + Mathf.Pow(num2 - num2 * value, num1));
  }
}
