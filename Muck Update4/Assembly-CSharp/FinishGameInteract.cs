// Decompiled with JetBrains decompiler
// Type: FinishGameInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FinishGameInteract : MonoBehaviour, Interactable, SharedObject
{
  private int id;

  private void Awake()
  {
    foreach (Collider component in this.GetComponents<Collider>())
    {
      if (component.isTrigger)
        component.enabled = true;
    }
    this.gameObject.layer = LayerMask.NameToLayer("Interact");
  }

  public void Interact()
  {
    int playersInLobby = GameManager.instance.GetPlayersInLobby();
    if (Boat.Instance.countPlayers.players.Count < playersInLobby)
      return;
    ClientSend.Interact(this.id);
  }

  public void LocalExecute()
  {
  }

  public void AllExecute() => Boat.Instance.LeaveIsland();

  public void ServerExecute(int fromClient = -1)
  {
  }

  public void RemoveObject()
  {
  }

  public string GetName()
  {
    int playersInLobby = GameManager.instance.GetPlayersInLobby();
    int count = Boat.Instance.countPlayers.players.Count;
    return count >= playersInLobby ? string.Format("Press {0} to leave Muck!", (object) InputManager.interact) + string.Format("\n({0}/{1})", (object) count, (object) playersInLobby) : "Get all players on the ship!" + string.Format("\n({0}/{1})", (object) count, (object) playersInLobby);
  }

  public bool IsStarted() => false;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;
}
