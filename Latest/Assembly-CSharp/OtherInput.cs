// Decompiled with JetBrains decompiler
// Type: OtherInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
  public static OtherInput Instance;
  public GameObject pauseUi;
  public GameObject settingsUi;
  public UiSfx UiSfx;
  public RectTransform craftingOverlay;

  public OtherInput.CraftingState craftingState { get; set; }

  private void Awake() => OtherInput.Instance = this;

  public void Unpause()
  {
    if (GameManager.gameSettings.multiplayer == GameSettings.Multiplayer.Off)
      Time.set_timeScale(1f);
    Cursor.set_visible(false);
    Cursor.set_lockState((CursorLockMode) 1);
    this.paused = false;
    this.pauseUi.SetActive(false);
  }

  public void Pause()
  {
    if (GameManager.gameSettings.multiplayer == GameSettings.Multiplayer.Off)
      Time.set_timeScale(0.0f);
    Cursor.set_visible(true);
    Cursor.set_lockState((CursorLockMode) 0);
    this.paused = true;
    this.pauseUi.SetActive(true);
  }

  public bool paused { get; set; }

  public bool OtherUiActive() => this.pauseUi.get_activeInHierarchy() || this.settingsUi.get_activeInHierarchy() || (ChatBox.Instance.typing || Map.Instance.active) || (RespawnTotemUI.Instance.root.get_activeInHierarchy() || ((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy() && this.craftingState != OtherInput.CraftingState.Inventory);

  private void Update()
  {
    if (this.pauseUi.get_activeInHierarchy() || this.settingsUi.get_activeInHierarchy())
    {
      if (!Input.GetKeyDown((KeyCode) 27))
        return;
      if (this.settingsUi.get_activeInHierarchy())
      {
        this.settingsUi.SetActive(false);
        this.pauseUi.SetActive(true);
      }
      else
        this.Unpause();
    }
    else
    {
      if (RespawnTotemUI.Instance.root.get_activeInHierarchy() || ChatBox.Instance.typing)
        return;
      if (Input.GetKeyDown(InputManager.map))
        Map.Instance.ToggleMap();
      if (Map.Instance.active)
        return;
      if (Input.GetKeyDown(InputManager.inventory) && !PlayerStatus.Instance.IsPlayerDead())
        this.ToggleInventory(OtherInput.CraftingState.Inventory);
      if (Input.GetButton("Cancel") && ((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy())
      {
        this.ToggleInventory(OtherInput.CraftingState.Inventory);
      }
      else
      {
        if (Input.GetKeyDown(InputManager.interact))
          DetectInteractables.Instance.currentInteractable?.Interact();
        if (!Input.GetKeyDown((KeyCode) 27))
          return;
        this.Pause();
      }
    }
  }

  public void ToggleInventory(OtherInput.CraftingState state)
  {
    this.craftingState = state;
    InventoryUI.Instance.ToggleInventory();
    OtherInput.lockCamera = ((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy();
    if (((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy())
    {
      this.UiSfx.PlayInventory(true);
      Cursor.set_visible(true);
      Cursor.set_lockState((CursorLockMode) 0);
      this.crosshair.SetActive(false);
      this.hotbar.SetActive(false);
      this.FindCurrentCraftingState();
      InventoryUI.Instance.CraftingUi = this._currentCraftingUiMenu;
      ((Component) this._currentCraftingUiMenu).get_gameObject().SetActive(true);
      this._currentCraftingUiMenu.UpdateCraftables();
      this.CheckStationUnlock();
      this.CenterInventory();
    }
    else
    {
      this.UiSfx.PlayInventory(false);
      Cursor.set_visible(false);
      Cursor.set_lockState((CursorLockMode) 1);
      this.crosshair.SetActive(true);
      this.hotbar.SetActive(true);
      InventoryUI.Instance.CraftingUi = (InventoryExtensions) null;
      ((Component) this._currentCraftingUiMenu).get_gameObject().SetActive(false);
      this._currentCraftingUiMenu = (InventoryExtensions) null;
      if (Object.op_Implicit((Object) this.currentChest))
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
        ((ChestUI) this.chest).CopyChest(this.currentChest);
        break;
    }
  }

  private void CenterInventory()
  {
    if (!Object.op_Implicit((Object) this._currentCraftingUiMenu))
      Debug.LogError((object) "no current ui menu");
    else
      this.craftingOverlay.set_offsetMax(new Vector2(0.0f, -(400f - (float) ((RectTransform) ((Component) this._currentCraftingUiMenu).GetComponent<RectTransform>()).get_sizeDelta().y)));
  }

  public bool IsAnyMenuOpen() => ((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy();

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

  public OtherInput() => base.\u002Ector();

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
