// Decompiled with JetBrains decompiler
// Type: UiController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class UiController : MonoBehaviour
{
  public Canvas canvas;
  public static UiController Instance;
  private bool hudActive = true;

  private void Awake() => UiController.Instance = this;

  public void ToggleHud()
  {
    this.hudActive = !this.hudActive;
    this.canvas.enabled = this.hudActive;
  }
}
