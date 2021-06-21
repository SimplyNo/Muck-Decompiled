// Decompiled with JetBrains decompiler
// Type: ChestManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
  public Dictionary<int, Chest> chests;
  private int chestId;
  public static ChestManager Instance;

  private void Awake()
  {
    ChestManager.Instance = this;
    this.chests = new Dictionary<int, Chest>();
    this.chestId = (int) IdOffsets.chestIdRange.x;
  }

  public void AddChest(Chest c, int id)
  {
    c.id = id;
    this.chests.Add(id, c);
  }

  public int GetNextId() => this.chestId++;

  public void UseChest(int chestId, bool inUse) => this.chests[chestId].Use(inUse);

  public void SendChestUpdate(int chestId, int cellId) => this.chests[chestId].locked[cellId] = true;

  public void UpdateChest(int chestId, int cellId, int itemId, int amount)
  {
    InventoryItem inventoryItem = (InventoryItem) null;
    if (itemId != -1)
    {
      inventoryItem = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
      inventoryItem.Copy(ItemManager.Instance.allItems[itemId], amount);
    }
    this.chests[chestId].cells[cellId] = inventoryItem;
    this.chests[chestId].locked[cellId] = false;
    this.chests[chestId].UpdateCraftables();
  }

  public bool IsChestOpen(int chestId) => this.chests[chestId].IsUsed();

  public void RemoveChest(int chestId)
  {
    Chest chest = this.chests[chestId];
    this.chests.Remove(chestId);
    Object.Destroy((Object) ((Component) ((Component) chest).get_transform().get_parent()).get_gameObject());
    if (Object.op_Equality((Object) OtherInput.Instance.currentChest, (Object) chest))
      OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
    if (!LocalClient.serverOwner)
      return;
    this.DropChest(chest);
  }

  private void DropChest(Chest chest)
  {
    Vector3 pos = ((Component) chest).get_transform().get_position();
    foreach (InventoryItem cell in chest.cells)
    {
      if (Object.op_Inequality((Object) cell, (Object) null))
      {
        Vector3 vector3_1 = pos;
        Vector3 up = Vector3.get_up();
        Bounds bounds = cell.mesh.get_bounds();
        double num = ((Bounds) ref bounds).get_extents().y * 2.0;
        Vector3 vector3_2 = Vector3.op_Multiply(up, (float) num);
        pos = Vector3.op_Addition(vector3_1, vector3_2);
        int nextId = ItemManager.Instance.GetNextId();
        ItemManager.Instance.DropItemAtPosition(cell.id, cell.amount, pos, nextId);
        ServerSend.DropItemAtPosition(cell.id, cell.amount, nextId, pos);
      }
    }
  }

  public ChestManager() => base.\u002Ector();
}
