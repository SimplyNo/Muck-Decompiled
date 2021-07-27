// Decompiled with JetBrains decompiler
// Type: BillyInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BillyInteract : MonoBehaviour, SharedObject, Interactable
{
  public int id;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  public void Interact()
  {
    Application.OpenURL("https://store.steampowered.com/app/1228610/KARLSON/");
    AchievementManager.Instance.Karlson();
  }

  public void LocalExecute()
  {
  }

  public void AllExecute()
  {
  }

  public void ServerExecute(int fromClient = -1)
  {
  }

  public void RemoveObject()
  {
  }

  public string GetName() => string.Format("<size=40%>Press {0} to wishlist KARLSON now gamer!", (object) InputManager.interact);

  public bool IsStarted() => false;
}
