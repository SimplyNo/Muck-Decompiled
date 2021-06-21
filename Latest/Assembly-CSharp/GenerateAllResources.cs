// Decompiled with JetBrains decompiler
// Type: GenerateAllResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class GenerateAllResources : MonoBehaviour
{
  public GameObject[] spawners;
  public static int seedOffset;

  private void Awake()
  {
    for (int index = 0; index < this.spawners.Length; ++index)
      this.spawners[index].SetActive(true);
  }

  public GenerateAllResources() => base.\u002Ector();
}
