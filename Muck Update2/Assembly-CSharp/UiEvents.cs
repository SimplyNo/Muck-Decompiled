// Decompiled with JetBrains decompiler
// Type: UiEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class UiEvents : MonoBehaviour
{
  public GameObject pickupPrefab;
  public Transform pickupParent;
  public GameObject unlockPrefab;
  public Transform unlockParent;
  private bool[] unlockedHard;
  private bool[] unlockedSoft;
  private bool[] stationsUnlocked;
  public bool[] alertCleared;
  public static UiEvents Instance;
  private Queue<int> idsToUnlock;

  private void Awake()
  {
    UiEvents.Instance = this;
    this.idsToUnlock = new Queue<int>();
  }

  private void Start()
  {
    this.unlockedHard = new bool[ItemManager.Instance.allItems.Count];
    this.unlockedSoft = new bool[ItemManager.Instance.allItems.Count];
    this.alertCleared = new bool[ItemManager.Instance.allItems.Count];
    this.stationsUnlocked = new bool[ItemManager.Instance.allItems.Count];
  }

  public bool IsSoftUnlocked(int id) => this.unlockedSoft != null && this.unlockedSoft[id];

  public bool IsHardUnlocked(int id) => this.unlockedHard != null && this.unlockedHard[id];

  public bool IsStationUnlocked(int id) => this.stationsUnlocked != null && this.stationsUnlocked[id];

  public void StationUnlock(int id)
  {
    MonoBehaviour.print((object) ("unlocked station: " + (object) id));
    this.stationsUnlocked[id] = true;
    this.CheckNewUnlocks(id);
  }

  public void AddPowerup(Powerup p)
  {
    M0 m0 = Object.Instantiate<GameObject>((M0) this.pickupPrefab, this.pickupParent);
    ((ItemPickedupUI) ((GameObject) m0).GetComponent<ItemPickedupUI>()).SetPowerup(p);
    ((GameObject) m0).get_transform().SetSiblingIndex(0);
  }

  public void AddPickup(InventoryItem item)
  {
    Hotbar.Instance.UpdateHotbar();
    M0 m0 = Object.Instantiate<GameObject>((M0) this.pickupPrefab, this.pickupParent);
    ((ItemPickedupUI) ((GameObject) m0).GetComponent<ItemPickedupUI>()).SetItem(item);
    ((GameObject) m0).get_transform().SetSiblingIndex(0);
    MonoBehaviour.print((object) "checking");
    if (this.unlockedHard[item.id])
      return;
    MonoBehaviour.print((object) "Unlocking hard");
    this.UnlockItemHard(item.id);
    this.CheckNewUnlocks(item.id);
  }

  public void PlaceInInventory(InventoryItem item)
  {
    if (this.unlockedHard[item.id])
      return;
    this.UnlockItemHard(item.id);
    this.CheckNewUnlocks(item.id);
  }

  private bool CanUnlock(
    InventoryItem.CraftRequirement[] requirements,
    bool unlockWithFirstRequirement)
  {
    if (requirements.Length < 1)
      return false;
    if (unlockWithFirstRequirement && this.unlockedHard[requirements[0].item.id])
      return true;
    for (int index = 0; index < requirements.Length; ++index)
    {
      if (!this.unlockedHard[requirements[index].item.id])
        return false;
    }
    return true;
  }

  public void CheckProcessedItem(int id)
  {
    if (this.unlockedHard[id])
      return;
    this.UnlockItemHard(id);
    this.CheckNewUnlocks(id);
  }

  public void CheckNewUnlocks(int id)
  {
    List<int> intList = new List<int>();
    for (int key = 0; key < this.unlockedHard.Length; ++key)
    {
      if (!this.unlockedSoft[key])
      {
        InventoryItem allItem = ItemManager.Instance.allItems[key];
        InventoryItem.CraftRequirement[] requirements = allItem.requirements;
        if ((!Object.op_Inequality((Object) allItem.stationRequirement, (Object) null) || this.stationsUnlocked[allItem.stationRequirement.id]) && this.CanUnlock(requirements, allItem.unlockWithFirstRequirementOnly))
          intList.Add(key);
      }
    }
    foreach (int id1 in intList)
      this.UnlockItemSoft(id1);
    this.Unlock();
  }

  private void UnlockItemHard(int id)
  {
    this.unlockedHard[id] = true;
    this.unlockedSoft[id] = true;
    this.idsToUnlock.Enqueue(id);
  }

  private void UnlockItemSoft(int id)
  {
    this.unlockedSoft[id] = true;
    this.idsToUnlock.Enqueue(id);
  }

  private void Unlock()
  {
    if (this.idsToUnlock.Count < 1 || this.IsInvoking(nameof (Unlock)))
      return;
    int key = this.idsToUnlock.Dequeue();
    M0 m0 = Object.Instantiate<GameObject>((M0) this.unlockPrefab, this.unlockParent);
    ((ItemUnlcokedUI) ((GameObject) m0).GetComponent<ItemUnlcokedUI>()).SetItem(ItemManager.Instance.allItems[key]);
    ((GameObject) m0).get_transform().SetSiblingIndex(0);
    if (this.idsToUnlock.Count <= 0)
      return;
    this.Invoke(nameof (Unlock), 2f);
  }

  public UiEvents() => base.\u002Ector();
}
