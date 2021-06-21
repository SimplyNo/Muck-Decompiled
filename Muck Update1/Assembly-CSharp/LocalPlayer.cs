// Decompiled with JetBrains decompiler
// Type: LocalPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
  public GameObject[] objects;

  public void SwitchUserInterface(bool b)
  {
    foreach (GameObject gameObject in this.objects)
      gameObject.SetActive(b);
  }
}
