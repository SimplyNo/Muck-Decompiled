// Decompiled with JetBrains decompiler
// Type: ItemManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
  public GameObject dropItem;
  public Dictionary<int, GameObject> list;
  public GameObject debug;
  public InventoryItem[] allScriptableItems;
  public LootDrop[] allScriptableDropTables;
  public Powerup[] powerupsWhite;
  public Powerup[] powerupsBlue;
  public Powerup[] powerupsOrange;
  public Dictionary<int, InventoryItem> allItems;
  public Dictionary<int, Powerup> allPowerups;
  public Dictionary<int, LootDrop> allDropTables;
  public Dictionary<string, int> stringToPowerupId;
  private ConsistentRandom random;
  public bool attatchDebug;
  public static ItemManager Instance;
  public static int currentId;

  private void Awake()
  {
    ItemManager.Instance = this;
    this.list = new Dictionary<int, GameObject>();
    this.allItems = new Dictionary<int, InventoryItem>();
    this.allPowerups = new Dictionary<int, Powerup>();
    this.stringToPowerupId = new Dictionary<string, int>();
    this.random = new ConsistentRandom();
    this.InitAllItems();
    this.InitAllPowerups();
    this.InitAllDropTables();
  }

  private void InitAllItems()
  {
    for (int key = 0; key < this.allScriptableItems.Length; ++key)
    {
      this.allScriptableItems[key].id = key;
      this.allItems.Add(key, this.allScriptableItems[key]);
    }
  }

  private void InitAllDropTables()
  {
    for (int key = 0; key < this.allScriptableDropTables.Length; ++key)
    {
      this.allScriptableDropTables[key].id = key;
      this.allDropTables.Add(key, this.allScriptableDropTables[key]);
    }
  }

  private void InitAllPowerups() => this.AddPowerupsToList(this.powerupsOrange, this.AddPowerupsToList(this.powerupsBlue, this.AddPowerupsToList(this.powerupsWhite, 0)));

  private int AddPowerupsToList(Powerup[] powerups, int id)
  {
    foreach (Powerup powerup in powerups)
    {
      this.allPowerups.Add(id, powerup);
      this.stringToPowerupId.Add(powerup.name, id);
      powerup.id = id;
      ++id;
    }
    return id;
  }

  public InventoryItem GetItemByName(string name)
  {
    foreach (InventoryItem inventoryItem in this.allItems.Values)
    {
      if (inventoryItem.name == name)
        return inventoryItem;
    }
    return (InventoryItem) null;
  }

  public int GetNextId() => ItemManager.currentId++;

  public void DropItem(int fromClient, int itemId, int amount, int objectID)
  {
    int key = objectID;
    GameObject gameObject1 = Object.Instantiate<GameObject>(this.dropItem);
    InventoryItem instance = ScriptableObject.CreateInstance<InventoryItem>();
    instance.Copy(this.allItems[itemId], amount);
    Item component = gameObject1.GetComponent<Item>();
    component.item = instance;
    component.UpdateMesh();
    gameObject1.AddComponent<BoxCollider>();
    Vector3 position = GameManager.players[fromClient].transform.position;
    Transform transform = GameManager.players[fromClient].transform;
    if (fromClient == LocalClient.instance.myId)
      transform = transform.transform.GetChild(0);
    Vector3 normalized = (transform.forward + Vector3.up * 0.35f).normalized;
    gameObject1.transform.position = position;
    gameObject1.GetComponent<Rigidbody>().AddForce(normalized * InventoryUI.throwForce);
    if (this.attatchDebug)
    {
      GameObject gameObject2 = Object.Instantiate<GameObject>(this.debug, gameObject1.transform);
      gameObject2.GetComponent<DebugObject>().text = string.Concat((object) key);
      gameObject2.transform.localPosition = Vector3.up * 1.25f;
    }
    gameObject1.GetComponent<Item>().objectID = key;
    this.list.Add(key, gameObject1);
  }

  public void DropItemAtPosition(int itemId, int amount, Vector3 pos, int objectID)
  {
    int key = objectID;
    GameObject gameObject1 = Object.Instantiate<GameObject>(this.dropItem);
    InventoryItem instance = ScriptableObject.CreateInstance<InventoryItem>();
    instance.Copy(this.allItems[itemId], amount);
    Item component = gameObject1.GetComponent<Item>();
    component.item = instance;
    component.UpdateMesh();
    gameObject1.AddComponent<BoxCollider>();
    Vector3 vector3 = pos;
    gameObject1.transform.position = vector3;
    if (this.attatchDebug)
    {
      GameObject gameObject2 = Object.Instantiate<GameObject>(this.debug, gameObject1.transform);
      gameObject2.GetComponent<DebugObject>().text = string.Concat((object) key);
      gameObject2.transform.localPosition = Vector3.up * 1.25f;
    }
    gameObject1.GetComponent<Item>().objectID = key;
    this.list.Add(key, gameObject1);
  }

  public void DropResource(int fromClient, int dropTableId, int droppedObjectID)
  {
    int key = droppedObjectID;
    GameObject gameObject1 = Object.Instantiate<GameObject>(this.dropItem);
    InventoryItem instance = ScriptableObject.CreateInstance<InventoryItem>();
    Item component = gameObject1.GetComponent<Item>();
    component.item = instance;
    component.UpdateMesh();
    gameObject1.AddComponent<BoxCollider>();
    if (this.attatchDebug)
    {
      GameObject gameObject2 = Object.Instantiate<GameObject>(this.debug, gameObject1.transform);
      gameObject2.GetComponent<DebugObject>().text = string.Concat((object) key);
      gameObject2.transform.localPosition = Vector3.up * 1.25f;
    }
    gameObject1.GetComponent<Item>().objectID = key;
    this.list.Add(key, gameObject1);
  }

  public void DropPowerupAtPosition(int powerupId, Vector3 pos, int objectID)
  {
    int key = objectID;
    GameObject gameObject1 = Object.Instantiate<GameObject>(this.dropItem);
    Powerup powerup = Object.Instantiate<Powerup>(this.allPowerups[powerupId]);
    Item component = gameObject1.GetComponent<Item>();
    component.powerup = powerup;
    component.UpdateMesh();
    gameObject1.AddComponent<BoxCollider>();
    gameObject1.transform.position = pos;
    if (this.attatchDebug)
    {
      GameObject gameObject2 = Object.Instantiate<GameObject>(this.debug, gameObject1.transform);
      gameObject2.GetComponent<DebugObject>().text = string.Concat((object) key);
      gameObject2.transform.localPosition = Vector3.up * 1.25f;
    }
    gameObject1.GetComponent<Item>().objectID = key;
    this.list.Add(key, gameObject1);
  }

  public Powerup GetRandomPowerup(float whiteWeight, float blueWeight, float orangeWeight)
  {
    float num1 = whiteWeight + blueWeight + orangeWeight;
    float num2 = (float) this.random.NextDouble();
    if ((double) num2 < (double) whiteWeight / (double) num1)
      return this.powerupsWhite[Random.Range(0, this.powerupsWhite.Length)];
    return (double) num2 < ((double) whiteWeight + (double) blueWeight) / (double) num1 ? this.powerupsBlue[Random.Range(0, this.powerupsBlue.Length)] : this.powerupsOrange[Random.Range(0, this.powerupsOrange.Length)];
  }

  public bool PickupItem(int objectID)
  {
    Object.Destroy((Object) this.list[objectID]);
    this.list.Remove(objectID);
    return true;
  }
}
