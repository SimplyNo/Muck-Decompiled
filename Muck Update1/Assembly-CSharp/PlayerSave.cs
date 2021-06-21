﻿// Decompiled with JetBrains decompiler
// Type: PlayerSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PlayerSave
{
  public bool cameraShake = true;
  public float sensMultiplier = 1f;
  public bool invertedMouse;
  public bool grass = true;
  public bool tutorial = true;
  public KeyCode forward = KeyCode.W;
  public KeyCode backwards = KeyCode.S;
  public KeyCode left = KeyCode.A;
  public KeyCode right = KeyCode.D;
  public KeyCode jump = KeyCode.Space;
  public KeyCode sprint = KeyCode.LeftShift;
  public KeyCode interact = KeyCode.E;
  public KeyCode inventory = KeyCode.Tab;
  public KeyCode map = KeyCode.M;
  public KeyCode leftClick = KeyCode.Mouse0;
  public KeyCode rightClick = KeyCode.Mouse1;
  public int shadowQuality = 2;
  public int shadowResolution = 2;
  public int shadowDistance = 2;
  public int shadowCascade = 1;
  public int textureQuality = 2;
  public int antiAliasing = 1;
  public bool softParticles = true;
  public int bloom = 2;
  public bool motionBlur;
  public bool ambientOcclusion = true;
  public Vector2 resolution;
  public int refreshRate = 144;
  public bool fullscreen = true;
  public int vSync;
  public int fpsLimit = 144;
  public int xp;
  public int money;

  public int volume { get; set; } = 4;

  public int music { get; set; } = 2;
}
