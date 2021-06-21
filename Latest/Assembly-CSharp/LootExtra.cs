// Decompiled with JetBrains decompiler
// Type: LootExtra
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class LootExtra : MonoBehaviour
{
  public static void CheckDrop(int fromClient, HitableResource hitable)
  {
    if (Object.op_Equality((Object) hitable.dropTable, (Object) null))
      return;
    Vector3 pos = ((Component) hitable).get_transform().get_position();
    Collider componentInChildren = (Collider) ((Component) hitable).GetComponentInChildren<Collider>();
    if (Object.op_Implicit((Object) componentInChildren))
    {
      Bounds bounds = componentInChildren.get_bounds();
      pos = ((Bounds) ref bounds).get_center();
    }
    LootDrop dropTable = hitable.dropTable;
    List<InventoryItem> inventoryItemList = dropTable.GetLoot();
    float num1 = PowerupInventory.Instance.GetLootMultiplier(Server.clients[fromClient].player.powerups);
    if (dropTable.dropOne)
    {
      inventoryItemList = new List<InventoryItem>();
      InventoryItem inventoryItem = (InventoryItem) Object.Instantiate<InventoryItem>((M0) hitable.dropItem);
      inventoryItem.amount = 1;
      num1 = 1f;
      inventoryItemList.Add(inventoryItem);
    }
    foreach (InventoryItem inventoryItem in inventoryItemList)
    {
      int nextId = ItemManager.Instance.GetNextId();
      int id = inventoryItem.id;
      inventoryItem.amount = (int) ((double) inventoryItem.amount * (double) num1);
      if (inventoryItem.amount > inventoryItem.max)
        inventoryItem.amount = inventoryItem.max;
      Vector3 vector3_1 = pos;
      Vector3 up = Vector3.get_up();
      Bounds bounds = inventoryItem.mesh.get_bounds();
      double num2 = ((Bounds) ref bounds).get_extents().y * 2.0;
      Vector3 vector3_2 = Vector3.op_Multiply(up, (float) num2);
      pos = Vector3.op_Addition(vector3_1, vector3_2);
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
    Vector3 pos = dropTransform.get_position();
    Collider component = (Collider) ((Component) dropTransform).GetComponent<Collider>();
    if (Object.op_Implicit((Object) component))
    {
      Bounds bounds = component.get_bounds();
      pos = ((Bounds) ref bounds).get_center();
    }
    List<InventoryItem> loot = lootTable.GetLoot();
    float num1 = PowerupInventory.Instance.GetLootMultiplier(Server.clients[fromClient].player.powerups) * buffMultiplier;
    foreach (InventoryItem inventoryItem in loot)
    {
      if (inventoryItem.rarity == InventoryItem.ItemRarity.Rare)
        ServerSend.SendChatMessage(-1, "", "<color=orange>" + Server.clients[fromClient].player.username + " received rare drop: <color=red>" + inventoryItem.name);
      int nextId = ItemManager.Instance.GetNextId();
      int id = inventoryItem.id;
      inventoryItem.amount = (int) ((double) inventoryItem.amount * (double) num1);
      if (inventoryItem.amount > inventoryItem.max)
        inventoryItem.amount = inventoryItem.max;
      Vector3 vector3_1 = pos;
      Vector3 up = Vector3.get_up();
      Bounds bounds = inventoryItem.mesh.get_bounds();
      double num2 = ((Bounds) ref bounds).get_extents().y * 2.0;
      Vector3 vector3_2 = Vector3.op_Multiply(up, (float) num2);
      pos = Vector3.op_Addition(vector3_1, vector3_2);
      ItemManager.Instance.DropItemAtPosition(id, inventoryItem.amount, pos, nextId);
      ServerSend.DropItemAtPosition(id, inventoryItem.amount, nextId, pos);
    }
  }

  public static void BossLoot(Transform dropPos, Mob.BossType mobType)
  {
    int num = GameManager.instance.GetPlayersInLobby();
    if (mobType == Mob.BossType.BossShrine)
      num = 1;
    Vector3 position = dropPos.get_position();
    int id = ItemManager.Instance.GetRandomPowerup(0.0f, 0.8f, 0.2f).id;
    for (int index = 0; index < num; ++index)
    {
      float angle = (float) (360 / num * index);
      Vector3 pos = LootExtra.RandomCircle(position, 10f, angle);
      int nextId = ItemManager.Instance.GetNextId();
      ItemManager.Instance.DropPowerupAtPosition(id, pos, nextId);
      ServerSend.DropPowerupAtPosition(id, nextId, dropPos.get_position());
    }
  }

  private static Vector3 RandomCircle(Vector3 center, float radius, float angle)
  {
    Vector3 vector3 = center;
    vector3.x = (__Null) (center.x + (double) radius * (double) Mathf.Sin(angle * ((float) Math.PI / 180f)));
    vector3.z = (__Null) (center.z + (double) radius * (double) Mathf.Cos(angle * ((float) Math.PI / 180f)));
    RaycastHit raycastHit;
    if (Physics.Raycast(Vector3.op_Addition(vector3, Vector3.op_Multiply(Vector3.get_up(), 20f)), Vector3.get_down(), ref raycastHit, 50f, LayerMask.op_Implicit(GameManager.instance.whatIsGround)))
      vector3 = ((RaycastHit) ref raycastHit).get_point();
    return Vector3.op_Addition(vector3, Vector3.op_Multiply(Vector3.get_up(), 1.5f));
  }

  public LootExtra() => base.\u002Ector();
}
