// Decompiled with JetBrains decompiler
// Type: JoinLobbyBtn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    if (ulong.TryParse(this.inputField.get_text(), out result))
    {
      Lobby lobby;
      ((Lobby) ref lobby).\u002Ector(SteamId.op_Implicit(result));
      SteamManager.Instance.JoinLobby(lobby);
    }
    else
      StatusMessage.Instance.DisplayMessage("Couldn't find lobby. Make sure it's a valid lobbyID from someone");
  }

  public JoinLobbyBtn() => base.\u002Ector();
}
