// Decompiled with JetBrains decompiler
// Type: SpawnObjectTimed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SpawnObjectTimed : MonoBehaviour
{
  public float time;
  public GameObject objectToSpawn;

  private void Awake() => this.Invoke("SpawnObject", this.time);

  private void SpawnObject()
  {
    Object.Instantiate<GameObject>(this.objectToSpawn, this.transform.position, this.objectToSpawn.transform.rotation);
    Object.Destroy((Object) this);
  }
}
