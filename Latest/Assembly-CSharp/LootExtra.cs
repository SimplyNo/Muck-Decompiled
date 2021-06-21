// Decompiled with JetBrains decompiler
// Type: LootExtra
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class LootExtra : MonoBehaviour
{
  public static void CheckDrop(int fromClient, HitableResource hitable)
  {
    if ((Object) hitable.dropTable == (Object) null)
      return;
    Vector3 pos = hitable.transform.position;
    Collider componentInChildren = hitable.GetComponentInChildren<Collider>();
    if ((bool) (Object) componentInChildren)
      pos = componentInChildren.bounds.center;
    LootDrop dropTable = hitable.dropTable;
    List<InventoryItem> inventoryItemList = dropTable.GetLoot();
    float num = PowerupInventory.Instance.GetLootMultiplier(Server.clients[fromClient].player.powerups);
    if (dropTable.dropOne)
    {
      inventoryItemList = new List<InventoryItem>();
      InventoryItem inventoryItem = Object.Instantiate<InventoryItem>(hitable.dropItem);
      inventoryItem.amount = 1;
      num = 1f;
      inventoryItemList.Add(inventoryItem);
    }
    foreach (InventoryItem inventoryItem in inventoryItemList)
    {
      int nextId = ItemManager.Instance.GetNextId();
      int id = inventoryItem.id;
      inventoryItem.amount = (int) ((double) inventoryItem.amount * (double) num);
      pos += Vector3.up * (inventoryItem.mesh.bounds.extents.y * 2f);
      ItemManager.Instance.DropItemAtPosition(id, inventoryItem.amount, pos, nextId);
      ServerSend.DropItemAtPosition(id, inventoryItem.amount, nextId, pos);
    }
  }

  public static void DropMobLoot(
    Transform dropTransform,
    LootDrop lootTable,
    int fromClient,
    float buffMultiplier)
  {
    Vector3 pos = dropTransform.position;
    Collider component = dropTransform.GetComponent<Collider>();
    if ((bool) (Object) component)
      pos = component.bounds.center;
    List<InventoryItem> loot = lootTable.GetLoot();
    float num = PowerupInventory.Instance.GetLootMultiplier(Server.clients[fromClient].player.powerups) * buffMultiplier;
    foreach (InventoryItem inventoryItem in loot)
    {
      if (inventoryItem.rarity == InventoryItem.ItemRarity.Rare)
        ServerSend.SendChatMessage(-1, "", "<color=orange>" + Server.clients[fromClient].player.username + " received rare drop: <color=red>" + inventoryItem.name);
      int nextId = ItemManager.Instance.GetNextId();
      int id = inventoryItem.id;
      inventoryItem.amount = (int) ((double) inventoryItem.amount * (double) num);
      pos += Vector3.up * (inventoryItem.mesh.bounds.extents.y * 2f);
      ItemManager.Instance.DropItemAtPosition(id, inventoryItem.amount, pos, nextId);
      ServerSend.DropItemAtPosition(id, inventoryItem.amount, nextId, pos);
    }
  }
}
