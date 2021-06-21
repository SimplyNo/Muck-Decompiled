// Decompiled with JetBrains decompiler
// Type: TestCenterDeleteThisLol
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class TestCenterDeleteThisLol : MonoBehaviour
{
  public MobType mob;

  private void Update()
  {
    if (!Input.GetKeyDown((KeyCode) 108))
      return;
    int nextId = MobManager.Instance.GetNextId();
    int id = this.mob.id;
    Vector3 position = ((Component) PlayerMovement.Instance).get_transform().get_position();
    MobSpawner.Instance.ServerSpawnNewMob(nextId, id, position, 1f, 1f);
  }

  public TestCenterDeleteThisLol() => base.\u002Ector();
}
