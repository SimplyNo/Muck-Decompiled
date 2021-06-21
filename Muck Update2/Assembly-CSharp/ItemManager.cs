// Decompiled with JetBrains decompiler
// Type: ItemManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.dropItem);
    InventoryItem instance = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
    instance.Copy(this.allItems[itemId], amount);
    M0 component = gameObject.GetComponent<Item>();
    ((Item) component).item = instance;
    ((Item) component).UpdateMesh();
    gameObject.AddComponent<BoxCollider>();
    Vector3 position = ((Component) GameManager.players[fromClient]).get_transform().get_position();
    Transform transform = ((Component) GameManager.players[fromClient]).get_transform();
    if (fromClient == LocalClient.instance.myId)
      transform = ((Component) transform).get_transform().GetChild(0);
    Vector3 vector3 = Vector3.op_Addition(transform.get_forward(), Vector3.op_Multiply(Vector3.get_up(), 0.35f));
    Vector3 normalized = ((Vector3) ref vector3).get_normalized();
    gameObject.get_transform().set_position(position);
    ((Rigidbody) gameObject.GetComponent<Rigidbody>()).AddForce(Vector3.op_Multiply(normalized, InventoryUI.throwForce));
    if (this.attatchDebug)
    {
      M0 m0 = Object.Instantiate<GameObject>((M0) this.debug, gameObject.get_transform());
      ((DebugObject) ((GameObject) m0).GetComponent<DebugObject>()).text = string.Concat((object) key);
      ((GameObject) m0).get_transform().set_localPosition(Vector3.op_Multiply(Vector3.get_up(), 1.25f));
    }
    ((Item) gameObject.GetComponent<Item>()).objectID = key;
    this.list.Add(key, gameObject);
  }

  public void DropItemAtPosition(int itemId, int amount, Vector3 pos, int objectID)
  {
    int key = objectID;
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.dropItem);
    InventoryItem instance = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
    instance.Copy(this.allItems[itemId], amount);
    M0 component = gameObject.GetComponent<Item>();
    ((Item) component).item = instance;
    ((Item) component).UpdateMesh();
    gameObject.AddComponent<BoxCollider>();
    Vector3 vector3 = pos;
    gameObject.get_transform().set_position(vector3);
    if (this.attatchDebug)
    {
      M0 m0 = Object.Instantiate<GameObject>((M0) this.debug, gameObject.get_transform());
      ((DebugObject) ((GameObject) m0).GetComponent<DebugObject>()).text = string.Concat((object) key);
      ((GameObject) m0).get_transform().set_localPosition(Vector3.op_Multiply(Vector3.get_up(), 1.25f));
    }
    ((Item) gameObject.GetComponent<Item>()).objectID = key;
    this.list.Add(key, gameObject);
  }

  public void DropResource(int fromClient, int dropTableId, int droppedObjectID)
  {
    int key = droppedObjectID;
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.dropItem);
    InventoryItem instance = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
    M0 component = gameObject.GetComponent<Item>();
    ((Item) component).item = instance;
    ((Item) component).UpdateMesh();
    gameObject.AddComponent<BoxCollider>();
    if (this.attatchDebug)
    {
      M0 m0 = Object.Instantiate<GameObject>((M0) this.debug, gameObject.get_transform());
      ((DebugObject) ((GameObject) m0).GetComponent<DebugObject>()).text = string.Concat((object) key);
      ((GameObject) m0).get_transform().set_localPosition(Vector3.op_Multiply(Vector3.get_up(), 1.25f));
    }
    ((Item) gameObject.GetComponent<Item>()).objectID = key;
    this.list.Add(key, gameObject);
  }

  public void DropPowerupAtPosition(int powerupId, Vector3 pos, int objectID)
  {
    int key = objectID;
    GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.dropItem);
    Powerup powerup = (Powerup) Object.Instantiate<Powerup>((M0) this.allPowerups[powerupId]);
    M0 component = gameObject.GetComponent<Item>();
    ((Item) component).powerup = powerup;
    ((Item) component).UpdateMesh();
    gameObject.AddComponent<BoxCollider>();
    gameObject.get_transform().set_position(pos);
    if (this.attatchDebug)
    {
      M0 m0 = Object.Instantiate<GameObject>((M0) this.debug, gameObject.get_transform());
      ((DebugObject) ((GameObject) m0).GetComponent<DebugObject>()).text = string.Concat((object) key);
      ((GameObject) m0).get_transform().set_localPosition(Vector3.op_Multiply(Vector3.get_up(), 1.25f));
    }
    ((Item) gameObject.GetComponent<Item>()).objectID = key;
    this.list.Add(key, gameObject);
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

  public ItemManager() => base.\u002Ector();
}
