// Decompiled with JetBrains decompiler
// Type: TestCenterDeleteThisLol
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TestCenterDeleteThisLol : MonoBehaviour
{
  public MobType mob;

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.L))
      return;
    int nextId = MobManager.Instance.GetNextId();
    int id = this.mob.id;
    Vector3 position = PlayerMovement.Instance.transform.position;
    MobSpawner.Instance.ServerSpawnNewMob(nextId, id, position, 1f, 1f);
  }
}
