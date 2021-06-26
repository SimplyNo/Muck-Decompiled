// Decompiled with JetBrains decompiler
// Type: ServerSteam
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    if (this.steamIdField.text == "")
      return;
    LocalClient.instance.name = SteamClient.Name;
    new SteamId().Value = ulong.Parse(this.steamIdField.text);
    MonoBehaviour.print((object) "sending join lobby request to server");
    ClientSend.JoinLobby();
    this.HideCamera();
  }

  public void HideCamera() => this.lobbyCamera.SetActive(false);
}
