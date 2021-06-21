// Decompiled with JetBrains decompiler
// Type: TestCenterDeleteThisLol
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
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
    MobSpawner.Instance.ServerSpawnNewMob(nextId, id, position, 1.5f, 1f);
  }
}
