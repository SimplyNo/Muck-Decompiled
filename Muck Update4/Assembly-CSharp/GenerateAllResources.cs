// Decompiled with JetBrains decompiler
// Type: GenerateAllResources
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

public class GenerateAllResources : MonoBehaviour
{
  public GameObject[] spawnersFirst;
  public GameObject[] spawners;
  public static int seedOffset;

  private void Awake() => this.StartCoroutine(this.GenerateResources());

  private IEnumerator GenerateResources()
  {
    for (int index = 0; index < this.spawnersFirst.Length; ++index)
      this.spawnersFirst[index].SetActive(true);
    yield return (object) 3000;
    for (int index = 0; index < this.spawners.Length; ++index)
      this.spawners[index].SetActive(true);
  }
}
