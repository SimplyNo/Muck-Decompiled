// Decompiled with JetBrains decompiler
// Type: DebugOnlinePlayers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
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
