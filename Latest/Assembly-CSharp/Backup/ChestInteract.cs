// Decompiled with JetBrains decompiler
// Type: ChestInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ChestInteract : MonoBehaviour, Interactable
{
  public OtherInput.CraftingState state;
  private Chest chest;
  private float cooldownTime = 0.5f;
  private bool ready;

  private void Awake()
  {
    this.chest = this.GetComponent<Chest>();
    this.ready = true;
  }

  public void Interact()
  {
    if (!this.ready)
      return;
    this.ready = false;
    this.Invoke("GetReady", this.cooldownTime);
    ClientSend.RequestChest(this.chest.id, true);
  }

  public void LocalExecute()
  {
  }

  public void AllExecute()
  {
  }

  public void ServerExecute(int fromClient)
  {
  }

  public void RemoveObject()
  {
  }

  public string GetName() => this.chest.inUse ? this.state.ToString() + "\n<size=50%>(Someone is already using it..)" : string.Format("{0}\n<size=50%>(Press \"{1}\" to open", (object) this.state.ToString(), (object) InputManager.interact);

  public bool IsStarted() => false;

  private void GetReady() => this.ready = true;
}
