// Decompiled with JetBrains decompiler
// Type: MainController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MainController : MonoBehaviour
{
  public static bool isHost;
  public static MainController.MainState state;

  public MainController() => base.\u002Ector();

  public enum MainState
  {
    None,
    Lobby,
    Loading,
    Playing,
    EndScreen,
  }
}
