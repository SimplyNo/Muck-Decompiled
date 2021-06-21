// Decompiled with JetBrains decompiler
// Type: SpawnObjectTimed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SpawnObjectTimed : MonoBehaviour
{
  public float time;
  public GameObject objectToSpawn;

  private void Awake() => this.Invoke("SpawnObject", this.time);

  private void SpawnObject()
  {
    Object.Instantiate<GameObject>((M0) this.objectToSpawn, ((Component) this).get_transform().get_position(), this.objectToSpawn.get_transform().get_rotation());
    Object.Destroy((Object) this);
  }

  public SpawnObjectTimed() => base.\u002Ector();
}
