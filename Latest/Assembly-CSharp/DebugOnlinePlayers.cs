// Decompiled with JetBrains decompiler
// Type: DebugOnlinePlayers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugOnlinePlayers : MonoBehaviour
{
  private void Start()
  {
    GameManager.instance.SpawnPlayer(2, "a", Color.black, new Vector3(0.0f, 30f, 0.0f), 50f);
    GameManager.instance.SpawnPlayer(3, "b", Color.black, new Vector3(5f, 30f, 2f), 150f);
  }
}
