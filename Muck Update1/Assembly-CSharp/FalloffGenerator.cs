// Decompiled with JetBrains decompiler
// Type: FalloffGenerator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
        float f = (float) ((double) index2 / (double) size * 2.0 - 1.0);
        float num2 = Mathf.Max(Mathf.Abs((float) num1), Mathf.Abs(f));
        numArray[index1, index2] = FalloffGenerator.Evaluate(num2);
      }
    }
    return numArray;
  }

  private static float Evaluate(float value)
  {
    float p = 3f;
    float num = 2.2f;
    return Mathf.Pow(value, p) / (Mathf.Pow(value, p) + Mathf.Pow(num - num * value, p));
  }
}
