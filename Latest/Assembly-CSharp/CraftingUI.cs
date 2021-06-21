// Decompiled with JetBrains decompiler
// Type: CraftingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : InventoryExtensions
{
  public int nCells;
  public RectTransform cellsParent;
  public RectTransform cellsParentParent;
  public RectTransform cellsParentParentParent;
  private Rect rect;
  public GameObject cellPrefab;
  public GameObject requirementPrefab;
  private List<InventoryCell> cells = new List<InventoryCell>();
  private int tabSelected;
  public CraftingUI.Tab[] tabs;
  public Transform tabParent;
  private RawImage[] tabImgs;
  private TextMeshProUGUI[] tabTexts;
  public Color selectedTabColor;
  public Color unselectedTabColor;
  public Color selectedTextColor;
  public Color unselectedTextColor;
  public bool handCrafts;

  private void Awake()
  {
    if (this.handCrafts)
      return;
    this.tabImgs = new RawImage[this.tabs.Length];
    this.tabTexts = new TextMeshProUGUI[this.tabs.Length];
    for (int index = 0; index < this.tabs.Length; ++index)
    {
      this.tabImgs[index] = (RawImage) ((Component) this.tabParent.GetChild(index)).GetComponent<RawImage>();
      this.tabTexts[index] = (TextMeshProUGUI) ((Component) this.tabParent.GetChild(index)).GetComponentInChildren<TextMeshProUGUI>();
    }
  }

  private void Start()
  {
    this.UpdateCraftables();
    this.UpdateTabs();
  }

  private void OnEnable()
  {
  }

  public void OpenTab(int i)
  {
    this.tabSelected = i;
    this.UpdateTabs();
    this.UpdateCraftables();
  }

  private void UpdateTabs()
  {
    if (this.tabImgs == null)
      return;
    for (int index = 0; index < this.tabs.Length; ++index)
    {
      if (index == this.tabSelected)
      {
        ((Graphic) this.tabImgs[index]).set_color(this.selectedTabColor);
        ((Graphic) this.tabTexts[index]).set_color(this.selectedTextColor);
      }
      else
      {
        ((Graphic) this.tabImgs[index]).set_color(this.unselectedTabColor);
        ((Graphic) this.tabTexts[index]).set_color(this.unselectedTextColor);
      }
    }
  }

  public override void UpdateCraftables()
  {
    for (int index = 0; index < this.cells.Count; ++index)
    {
      if (Object.op_Implicit((Object) ((Component) this.cells[index]).get_gameObject()))
        Object.Destroy((Object) ((Component) this.cells[index]).get_gameObject());
    }
    this.cells = new List<InventoryCell>();
    foreach (InventoryItem inventoryItem in this.tabs[this.tabSelected].items)
    {
      if (UiEvents.Instance.IsSoftUnlocked(inventoryItem.id))
      {
        InventoryCell component = (InventoryCell) ((GameObject) Object.Instantiate<GameObject>((M0) this.cellPrefab, (Transform) this.cellsParent)).GetComponent<InventoryCell>();
        component.currentItem = (InventoryItem) ScriptableObject.CreateInstance<InventoryItem>();
        component.currentItem.Copy(inventoryItem, 1);
        component.cellType = InventoryCell.CellType.Crafting;
        foreach (InventoryItem.CraftRequirement requirement in component.currentItem.requirements)
        {
          M0 m0 = Object.Instantiate<GameObject>((M0) this.requirementPrefab);
          ((Image) ((GameObject) m0).GetComponent<Image>()).set_sprite(requirement.item.sprite);
          ((GameObject) m0).get_transform().SetParent(((Component) component).get_transform().GetChild(((Component) component).get_transform().get_childCount() - 2));
        }
        this.cells.Add(component);
        component.SetColor(component.idle);
        if (!InventoryUI.Instance.IsCraftable(inventoryItem))
          component.SetOverlayAlpha(0.6f);
      }
    }
    if (!this.handCrafts)
      return;
    this.nCells = this.cells.Count;
  }

  [Serializable]
  public class Tab
  {
    public InventoryItem[] items;
  }
}
