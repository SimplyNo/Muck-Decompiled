// Decompiled with JetBrains decompiler
// Type: PlayerSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class PlayerSave
{
  public bool cameraShake = true;
  public int fov = 85;
  public float sensMultiplier = 1f;
  public bool invertedMouseHor;
  public bool invertedMouseVert;
  public bool grass = true;
  public bool tutorial = true;
  public KeyCode forward = (KeyCode) 119;
  public KeyCode backwards = (KeyCode) 115;
  public KeyCode left = (KeyCode) 97;
  public KeyCode right = (KeyCode) 100;
  public KeyCode jump = (KeyCode) 32;
  public KeyCode sprint = (KeyCode) 304;
  public KeyCode interact = (KeyCode) 101;
  public KeyCode inventory = (KeyCode) 9;
  public KeyCode map = (KeyCode) 109;
  public KeyCode leftClick = (KeyCode) 323;
  public KeyCode rightClick = (KeyCode) 324;
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
  public int fullscreenMode;
  public int vSync;
  public int fpsLimit = 144;
  public int xp;
  public int money;
  private bool[] bossesBeatEasy = new bool[20];
  private bool[] bossesBeatNormal = new bool[20];
  private bool[] bossesBeatHard = new bool[20];

  public int volume { get; set; } = 4;

  public int music { get; set; } = 2;

  public enum Bosses
  {
    BigChonk,
    Gronk,
  }
}
