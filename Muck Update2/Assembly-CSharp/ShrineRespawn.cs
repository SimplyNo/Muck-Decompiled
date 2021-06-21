// Decompiled with JetBrains decompiler
// Type: ShrineRespawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class ShrineRespawn : MonoBehaviour, SharedObject, Interactable
{
  private int id;

  private void Start()
  {
  }

  public void Interact() => RespawnTotemUI.Instance.Show();

  public void LocalExecute()
  {
  }

  public void AllExecute()
  {
  }

  public void ServerExecute(int fromClient)
  {
  }

  public void RemoveObject() => Object.Destroy((Object) ((Component) this).get_gameObject());

  public string GetName() => "Revive the homies";

  public bool IsStarted() => false;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  public ShrineRespawn() => base.\u002Ector();
}
