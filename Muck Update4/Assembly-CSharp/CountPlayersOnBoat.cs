// Decompiled with JetBrains decompiler
// Type: CountPlayersOnBoat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CountPlayersOnBoat : MonoBehaviour
{
  public List<PlayerManager> players;

  private void OnTriggerEnter(Collider other)
  {
    GameObject gameObject = other.gameObject;
    if (gameObject.layer != LayerMask.NameToLayer("Player"))
      return;
    PlayerManager component = gameObject.GetComponent<PlayerManager>();
    if (!(bool) (Object) component)
      return;
    this.players.Add(component);
  }

  private void OnTriggerExit(Collider other)
  {
    GameObject gameObject = other.gameObject;
    if (gameObject.layer != LayerMask.NameToLayer("Player"))
      return;
    PlayerManager component = gameObject.GetComponent<PlayerManager>();
    if (!(bool) (Object) component)
      return;
    this.players.Remove(component);
  }
}
