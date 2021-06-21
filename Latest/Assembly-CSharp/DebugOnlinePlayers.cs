// Decompiled with JetBrains decompiler
// Type: DebugOnlinePlayers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class DebugOnlinePlayers : MonoBehaviour
{
  private void Start()
  {
    GameManager.instance.SpawnPlayer(2, "a", Color.get_black(), new Vector3(0.0f, 30f, 0.0f), 50f);
    GameManager.instance.SpawnPlayer(3, "b", Color.get_black(), new Vector3(5f, 30f, 2f), 150f);
  }

  public DebugOnlinePlayers() => base.\u002Ector();
}
