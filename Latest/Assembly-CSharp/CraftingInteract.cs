// Decompiled with JetBrains decompiler
// Type: CraftingInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using UnityEngine;

public class CraftingInteract : MonoBehaviour, Interactable
{
  public OtherInput.CraftingState state;

  public void Interact() => OtherInput.Instance.ToggleInventory(this.state);

  public void LocalExecute() => throw new NotImplementedException();

  public void AllExecute() => throw new NotImplementedException();

  public void ServerExecute(int fromClient) => throw new NotImplementedException();

  public void RemoveObject() => throw new NotImplementedException();

  public string GetName() => string.Format("{0}\n<size=50%>(Press \"{1}\" to use)", (object) this.state.ToString(), (object) InputManager.interact);

  public bool IsStarted() => false;

  public CraftingInteract() => base.\u002Ector();
}
