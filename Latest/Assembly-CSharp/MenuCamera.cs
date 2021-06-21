// Decompiled with JetBrains decompiler
// Type: MenuCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MenuCamera : MonoBehaviour
{
  public Transform startPos;
  public Transform lobbyPos;
  private Transform desiredPos;

  private void Awake() => this.desiredPos = this.startPos;

  public void Lobby() => this.desiredPos = this.lobbyPos;

  public void Menu() => this.desiredPos = this.startPos;

  private void Update()
  {
    this.transform.position = Vector3.Lerp(this.transform.position, this.desiredPos.position, Time.deltaTime * 5f);
    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.desiredPos.rotation, Time.deltaTime * 5f);
  }
}
