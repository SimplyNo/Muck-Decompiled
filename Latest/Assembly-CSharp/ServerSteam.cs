// Decompiled with JetBrains decompiler
// Type: ServerSteam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using Steamworks;
using TMPro;
using UnityEngine;

public class ServerSteam : MonoBehaviour
{
  public TMP_InputField steamIdField;
  public GameObject lobbyCamera;

  public void HostServer()
  {
  }

  public void ConnectToServer()
  {
    if (this.steamIdField.get_text() == "")
      return;
    ((Object) LocalClient.instance).set_name(SteamClient.get_Name());
    ((SteamId) null).Value = (__Null) (long) ulong.Parse(this.steamIdField.get_text());
    MonoBehaviour.print((object) "sending join lobby request to server");
    ClientSend.JoinLobby();
    this.HideCamera();
  }

  public void HideCamera() => this.lobbyCamera.SetActive(false);

  public ServerSteam() => base.\u002Ector();
}
