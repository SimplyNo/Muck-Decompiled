// Decompiled with JetBrains decompiler
// Type: PickupInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class PickupInteract : MonoBehaviour, Interactable, SharedObject
{
  public InventoryItem item;
  public int amount;
  public int id;
  private Vector3 defaultScale;
  private Vector3 desiredScale;

  private void Awake()
  {
    this.defaultScale = ((Component) this).get_transform().get_localScale();
    this.desiredScale = this.defaultScale;
  }

  public void Interact() => ClientSend.PickupInteract(this.id);

  public void LocalExecute()
  {
    InventoryItem instance = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
    instance.Copy(this.item, this.amount);
    InventoryUI.Instance.AddItemToInventory(instance);
  }

  public void AllExecute() => this.RemoveObject();

  public void ServerExecute(int fromClient)
  {
  }

  public void RemoveObject()
  {
    Object.Destroy((Object) ((Component) this).get_gameObject());
    ResourceManager.Instance.RemoveInteractItem(this.id);
  }

  public string GetName() => this.item.name + "\n<size=50%>(Press \"E\" to pickup";

  public bool IsStarted() => false;

  public void SetId(int id) => this.id = id;

  private void Update()
  {
    this.desiredScale = Vector3.Lerp(this.desiredScale, this.defaultScale, Time.get_deltaTime() * 15f);
    this.desiredScale.y = this.defaultScale.y;
    ((Component) this).get_transform().set_localScale(Vector3.Lerp(((Component) this).get_transform().get_localScale(), this.desiredScale, Time.get_deltaTime() * 15f));
  }

  public int GetId() => this.id;

  public PickupInteract() => base.\u002Ector();
}
