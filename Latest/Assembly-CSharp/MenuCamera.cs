// Decompiled with JetBrains decompiler
// Type: MenuCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MenuCamera : MonoBehaviour
{
  public Transform startPos;
  public Transform lobbyPos;
  private Transform desiredPos;

  private void Awake()
  {
    this.desiredPos = this.startPos;
    Time.set_timeScale(1f);
  }

  private void Start() => NetworkController.Instance.loading = false;

  public void Lobby() => this.desiredPos = this.lobbyPos;

  public void Menu() => this.desiredPos = this.startPos;

  private void Update()
  {
    ((Component) this).get_transform().set_position(Vector3.Lerp(((Component) this).get_transform().get_position(), this.desiredPos.get_position(), Time.get_deltaTime() * 5f));
    ((Component) this).get_transform().set_rotation(Quaternion.Lerp(((Component) this).get_transform().get_rotation(), this.desiredPos.get_rotation(), Time.get_deltaTime() * 5f));
  }

  public MenuCamera() => base.\u002Ector();
}
