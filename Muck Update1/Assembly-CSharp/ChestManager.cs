// Decompiled with JetBrains decompiler
// Type: ChestManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
      inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
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
    Object.Destroy((Object) chest.transform.parent.gameObject);
    if ((Object) OtherInput.Instance.currentChest == (Object) chest)
      OtherInput.Instance.ToggleInventory(OtherInput.CraftingState.Inventory);
    if (!LocalClient.serverOwner)
      return;
    this.DropChest(chest);
  }

  private void DropChest(Chest chest)
  {
    Vector3 position = chest.transform.position;
    foreach (InventoryItem cell in chest.cells)
    {
      if ((Object) cell != (Object) null)
      {
        position += Vector3.up * (cell.mesh.bounds.extents.y * 2f);
        int nextId = ItemManager.Instance.GetNextId();
        ItemManager.Instance.DropItemAtPosition(cell.id, cell.amount, position, nextId);
        ServerSend.DropItemAtPosition(cell.id, cell.amount, nextId, position);
      }
    }
  }
}
