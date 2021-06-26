// Decompiled with JetBrains decompiler
// Type: MenuCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MenuCamera : MonoBehaviour
{
  public Transform startPos;
  public Transform lobbyPos;
  private Transform desiredPos;

  private void Awake()
  {
    this.desiredPos = this.startPos;
    Time.timeScale = 1f;
  }

  private void Start() => NetworkController.Instance.loading = false;

  public void Lobby() => this.desiredPos = this.lobbyPos;

  public void Menu() => this.desiredPos = this.startPos;

  private void Update()
  {
    this.transform.position = Vector3.Lerp(this.transform.position, this.desiredPos.position, Time.deltaTime * 5f);
    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.desiredPos.rotation, Time.deltaTime * 5f);
  }
}
