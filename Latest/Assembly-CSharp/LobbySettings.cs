// Decompiled with JetBrains decompiler
// Type: LobbySettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class LobbySettings : MonoBehaviour
{
  public UiSettings difficultySetting;
  public UiSettings friendlyFireSetting;
  public UiSettings gamemodeSetting;
  public TMP_InputField seed;
  public GameObject startButton;
  public static LobbySettings Instance;

  private void Awake() => LobbySettings.Instance = this;

  private void Start()
  {
    this.difficultySetting.AddSettings(1, Enum.GetNames(typeof (GameSettings.Difficulty)));
    this.friendlyFireSetting.AddSettings(0, Enum.GetNames(typeof (GameSettings.FriendlyFire)));
    this.gamemodeSetting.AddSettings(0, Enum.GetNames(typeof (GameSettings.GameMode)));
  }

  public LobbySettings() => base.\u002Ector();
}
