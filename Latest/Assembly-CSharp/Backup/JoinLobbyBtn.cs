// Decompiled with JetBrains decompiler
// Type: JoinLobbyBtn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine;

public class JoinLobbyBtn : MonoBehaviour
{
  public TMP_InputField inputField;

  public void JoinLobby()
  {
    ulong result;
    if (ulong.TryParse(this.inputField.text, out result))
    {
      Lobby lobby = new Lobby((SteamId) result);
      SteamManager.Instance.JoinLobby(lobby);
    }
    else
      StatusMessage.Instance.DisplayMessage("Couldn't find lobby. Make sure it's a valid lobbyID from someone");
  }
}
