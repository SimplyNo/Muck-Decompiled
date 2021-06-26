// Decompiled with JetBrains decompiler
// Type: OtherInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class OtherInput : MonoBehaviour
{
  public InventoryExtensions handcrafts;
  public InventoryExtensions furnace;
  public InventoryExtensions workbench;
  public InventoryExtensions anvil;
  public InventoryExtensions fletch;
  public InventoryExtensions chest;
  public InventoryExtensions cauldron;
  public GameObject hotbar;
  public GameObject crosshair;
  public static bool lockCamera;
  private InventoryExtensions _currentCraftingUiMenu;
  public Chest currentChest;
  private Dictionary<int, bool> chestsOpened;
  public static OtherInput Instance;
  public GameObject pauseUi;
  public GameObject settingsUi;
  public UiSfx UiSfx;
  public RectTransform craftingOverlay;

  public OtherInput.CraftingState craftingState { get; set; }

  private void Awake()
  {
    OtherInput.Instance = this;
    this.chestsOpened = new Dictionary<int, bool>();
  }

  public void Unpause()
  {
    if (GameManager.gameSettings.multiplayer == GameSettings.Multiplayer.Off)
      Time.timeScale = 1f;
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
    this.paused = false;
    this.pauseUi.SetActive(false);
  }

  public void Pause()
  {
    if (GameManager.gameSettings.multiplayer == GameSettings.Multiplayer.Off)
      Time.timeScale = 0.0f;
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
    this.paused = true;
    this.pauseUi.SetActive(true);
  }

  public bool paused { get; set; }

  public bool OtherUiActive() => this.pauseUi.activeInHierarchy || this.settingsUi.activeInHierarchy || (ChatBox.Instance.typing || Map.Instance.active) || (RespawnTotemUI.Instance.root.activeInHierarchy || InventoryUI.Instance.gameObject.activeInHierarchy && this.craftingState != OtherInput.CraftingState.Inventory);

  private void Update()
  {
    if (GameManager.state == GameManager.GameState.GameOver)
      return;
    if (this.pauseUi.activeInHierarchy || this.settingsUi.activeInHierarchy)
    {
      if (!Input.GetKeyDown(KeyCode.Escape))
        return;
      if (this.settingsUi.activeInHierarchy)
      {
        this.settingsUi.SetActive(false);
        this.pauseUi.SetActive(true);
      }
      else
        this.Unpause();
    }
    else
    {
      if (RespawnTotemUI.Instance.root.activeInHierarchy || ChatBox.Instance.typing)
        return;
      if (Input.GetKeyDown(InputManager.map))
        Map.Instance.ToggleMap();
      if (Map.Instance.active)
        return;
      if (Input.GetKeyDown(InputManager.inventory) && !PlayerStatus.Instance.IsPlayerDead())
        this.ToggleInventory(OtherInput.CraftingState.Inventory);
      if (Input.GetButton("Cancel") && InventoryUI.Instance.gameObject.activeInHierarchy)
      {
        this.ToggleInventory(OtherInput.CraftingState.Inventory);
      }
      else
      {
        if (Input.GetKeyDown(InputManager.interact))
          DetectInteractables.Instance.currentInteractable?.Interact();
        if (!Input.GetKeyDown(KeyCode.Escape))
          return;
        this.Pause();
      }
    }
  }

  public void ToggleInventory(OtherInput.CraftingState state)
  {
    this.craftingState = state;
    InventoryUI.Instance.ToggleInventory();
    OtherInput.lockCamera = InventoryUI.Instance.gameObject.activeInHierarchy;
    if (InventoryUI.Instance.gameObject.activeInHierarchy)
    {
      this.UiSfx.PlayInventory(true);
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
      this.crosshair.SetActive(false);
      this.hotbar.SetActive(false);
      this.FindCurrentCraftingState();
      InventoryUI.Instance.CraftingUi = this._currentCraftingUiMenu;
      this._currentCraftingUiMenu.gameObject.SetActive(true);
      this._currentCraftingUiMenu.UpdateCraftables();
      this.CheckStationUnlock();
      this.CenterInventory();
    }
    else
    {
      this.UiSfx.PlayInventory(false);
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      this.crosshair.SetActive(true);
      this.hotbar.SetActive(true);
      InventoryUI.Instance.CraftingUi = (InventoryExtensions) null;
      this._currentCraftingUiMenu.gameObject.SetActive(false);
      this._currentCraftingUiMenu = (InventoryExtensions) null;
      if ((bool) (Object) this.currentChest)
      {
        MonoBehaviour.print((object) "closing chest");
        ClientSend.RequestChest(this.currentChest.id, false);
        state = OtherInput.CraftingState.Inventory;
        this.currentChest = (Chest) null;
      }
      ItemInfo.Instance.Fade(0.0f, 0.0f);
    }
    switch (state)
    {
      case OtherInput.CraftingState.Cauldron:
        ((CauldronUI) this.cauldron).CopyChest(this.currentChest);
        break;
      case OtherInput.CraftingState.Furnace:
        ((FurnaceUI) this.furnace).CopyChest(this.currentChest);
        break;
      case OtherInput.CraftingState.Chest:
        bool addMap = false;
        if ((bool) (Object) Boat.Instance && !this.chestsOpened.ContainsKey(this.currentChest.id) && Boat.Instance.status == Boat.BoatStatus.Hidden)
        {
          this.chestsOpened.Add(this.currentChest.id, true);
          if ((Object) this.currentChest.transform.root.GetComponent<BuildInfo>() == (Object) null)
            addMap = true;
          else
            Debug.LogError((object) "failed2");
        }
        ((ChestUI) this.chest).CopyChest(this.currentChest, addMap);
        break;
    }
  }

  private void CenterInventory()
  {
    if (!(bool) (Object) this._currentCraftingUiMenu)
      Debug.LogError((object) "no current ui menu");
    else
      this.craftingOverlay.offsetMax = new Vector2(0.0f, -(400f - this._currentCraftingUiMenu.GetComponent<RectTransform>().sizeDelta.y));
  }

  public bool IsAnyMenuOpen() => InventoryUI.Instance.gameObject.activeInHierarchy;

  private void CheckStationUnlock()
  {
    int stationId = this.GetStationId();
    if (stationId == -1)
      return;
    UiEvents.Instance.StationUnlock(stationId);
  }

  private int GetStationId()
  {
    switch (this.craftingState)
    {
      case OtherInput.CraftingState.Workbench:
        return ItemManager.Instance.GetItemByName("Workbench").id;
      case OtherInput.CraftingState.Anvil:
        return ItemManager.Instance.GetItemByName("Anvil").id;
      case OtherInput.CraftingState.Cauldron:
        return ItemManager.Instance.GetItemByName("Cauldron").id;
      case OtherInput.CraftingState.Fletch:
        return ItemManager.Instance.GetItemByName("Fletching Table").id;
      case OtherInput.CraftingState.Furnace:
        return ItemManager.Instance.GetItemByName("Furnace").id;
      default:
        return -1;
    }
  }

  private void FindCurrentCraftingState()
  {
    switch (this.craftingState)
    {
      case OtherInput.CraftingState.Inventory:
        this._currentCraftingUiMenu = this.handcrafts;
        break;
      case OtherInput.CraftingState.Workbench:
        this._currentCraftingUiMenu = this.workbench;
        break;
      case OtherInput.CraftingState.Anvil:
        this._currentCraftingUiMenu = this.anvil;
        break;
      case OtherInput.CraftingState.Cauldron:
        this._currentCraftingUiMenu = this.cauldron;
        break;
      case OtherInput.CraftingState.Fletch:
        this._currentCraftingUiMenu = this.fletch;
        break;
      case OtherInput.CraftingState.Furnace:
        this._currentCraftingUiMenu = this.furnace;
        break;
      case OtherInput.CraftingState.Chest:
        this._currentCraftingUiMenu = this.chest;
        break;
    }
  }

  public enum CraftingState
  {
    Inventory,
    Workbench,
    Anvil,
    Cauldron,
    Fletch,
    Furnace,
    Chest,
  }
}
