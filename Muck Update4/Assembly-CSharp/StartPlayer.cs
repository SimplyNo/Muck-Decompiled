// Decompiled with JetBrains decompiler
// Type: StartPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class StartPlayer : MonoBehaviour
{
  private void Start()
  {
    for (int index = this.transform.childCount - 1; index >= 0; --index)
      this.transform.GetChild(index).parent = (Transform) null;
    Object.Destroy((Object) this.gameObject);
  }
}
