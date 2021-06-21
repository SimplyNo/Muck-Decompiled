// Decompiled with JetBrains decompiler
// Type: GenerateAllResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
}
