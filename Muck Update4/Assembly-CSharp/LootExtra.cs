// Decompiled with JetBrains decompiler
// Type: LootExtra
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class LootExtra : MonoBehaviour
{
  public static void CheckDrop(int fromClient, HitableResource hitable)
  {
    if ((UnityEngine.Object) hitable.dropTable == (UnityEngine.Object) null)
      return;
    Vector3 pos = hitable.transform.position;
    Collider componentInChildren = hitable.GetComponentInChildren<Collider>();
    if ((bool) (UnityEngine.Object) componentInChildren)
      pos = componentInChildren.bounds.center;
    LootDrop dropTable = hitable.dropTable;
    List<InventoryItem> inventoryItemList = dropTable.GetLoot();
    float num = PowerupInventory.Instance.GetLootMultiplier(Server.clients[fromClient].player.powerups);
    if (dropTable.dropOne)
    {
      inventoryItemList = new List<InventoryItem>();
      InventoryItem inventoryItem = UnityEngine.Object.Instantiate<InventoryItem>(hitable.dropItem);
      inventoryItem.amount = 1;
      num = 1f;
      inventoryItemList.Add(inventoryItem);
    }
    foreach (InventoryItem inventoryItem in inventoryItemList)
    {
      int nextId = ItemManager.Instance.GetNextId();
      int id = inventoryItem.id;
      inventoryItem.amount = (int) ((double) inventoryItem.amount * (double) num);
      if (inventoryItem.amount > inventoryItem.max)
        inventoryItem.amount = inventoryItem.max;
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
    if ((bool) (UnityEngine.Object) component)
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
      if (inventoryItem.amount > inventoryItem.max)
        inventoryItem.amount = inventoryItem.max;
      pos += Vector3.up * (inventoryItem.mesh.bounds.extents.y * 2f);
      ItemManager.Instance.DropItemAtPosition(id, inventoryItem.amount, pos, nextId);
      ServerSend.DropItemAtPosition(id, inventoryItem.amount, nextId, pos);
      if (inventoryItem.name == "Coin")
        Server.clients[fromClient].player.stats["Gold collected"] += inventoryItem.amount;
    }
  }

  public static void BossLoot(Transform dropPos, Mob.BossType mobType)
  {
    GameManager.instance.GetPlayersInLobby();
    Vector3 position1 = dropPos.position;
    int id = ItemManager.Instance.GetRandomPowerup(0.0f, 0.8f, 0.2f).id;
    Vector3 position2 = dropPos.position;
    int nextId = ItemManager.Instance.GetNextId();
    ItemManager.Instance.DropPowerupAtPosition(id, position2, nextId);
    ServerSend.DropPowerupAtPosition(id, nextId, dropPos.position);
  }

  private static Vector3 RandomCircle(Vector3 center, float radius, float angle)
  {
    Vector3 vector3 = center;
    vector3.x = center.x + radius * Mathf.Sin(angle * ((float) Math.PI / 180f));
    vector3.z = center.z + radius * Mathf.Cos(angle * ((float) Math.PI / 180f));
    RaycastHit hitInfo;
    if (Physics.Raycast(vector3 + Vector3.up * 20f, Vector3.down, out hitInfo, 50f, (int) GameManager.instance.whatIsGround))
      vector3 = hitInfo.point;
    return vector3 + Vector3.up * 1.5f;
  }
}
