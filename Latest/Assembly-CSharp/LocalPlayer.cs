// Decompiled with JetBrains decompiler
// Type: LocalPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class LocalPlayer : MonoBehaviour
{
  public GameObject[] objects;

  public void SwitchUserInterface(bool b)
  {
    foreach (GameObject gameObject in this.objects)
      gameObject.SetActive(b);
  }

  public LocalPlayer() => base.\u002Ector();
}
