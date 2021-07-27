// Decompiled with JetBrains decompiler
// Type: OnlyActivateForHost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using UnityEngine;

public class OnlyActivateForHost : MonoBehaviour
{
  public GameObject kickBtn;
  public SteamId steamId;

  public void Kick()
  {
    using (Packet packet = new Packet(6))
      ServerSend.SendTCPDataToSteamId(this.steamId, packet);
  }

  private void OnEnable()
  {
  }
}
