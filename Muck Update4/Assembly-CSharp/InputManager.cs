// Decompiled with JetBrains decompiler
// Type: InputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class InputManager : MonoBehaviour
{
  public static KeyCode forward;
  public static KeyCode backwards;
  public static KeyCode left;
  public static KeyCode right;
  public static KeyCode jump;
  public static KeyCode sprint;
  public static KeyCode interact;
  public static KeyCode inventory;
  public static KeyCode map;
  public static KeyCode leftClick;
  public static KeyCode rightClick;

  private void Start() => this.Init();

  private void Init()
  {
    InputManager.forward = SaveManager.Instance.state.forward;
    InputManager.backwards = SaveManager.Instance.state.backwards;
    InputManager.left = SaveManager.Instance.state.left;
    InputManager.right = SaveManager.Instance.state.right;
    InputManager.jump = SaveManager.Instance.state.jump;
    InputManager.sprint = SaveManager.Instance.state.sprint;
    InputManager.interact = SaveManager.Instance.state.interact;
    InputManager.inventory = SaveManager.Instance.state.inventory;
    InputManager.map = SaveManager.Instance.state.map;
    InputManager.leftClick = SaveManager.Instance.state.leftClick;
    InputManager.rightClick = SaveManager.Instance.state.rightClick;
  }
}
