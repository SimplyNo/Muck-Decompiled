// Decompiled with JetBrains decompiler
// Type: CraftingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
      this.tabImgs[index] = this.tabParent.GetChild(index).GetComponent<RawImage>();
      this.tabTexts[index] = this.tabParent.GetChild(index).GetComponentInChildren<TextMeshProUGUI>();
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
        this.tabImgs[index].color = this.selectedTabColor;
        this.tabTexts[index].color = this.selectedTextColor;
      }
      else
      {
        this.tabImgs[index].color = this.unselectedTabColor;
        this.tabTexts[index].color = this.unselectedTextColor;
      }
    }
  }

  public override void UpdateCraftables()
  {
    for (int index = 0; index < this.cells.Count; ++index)
    {
      if ((bool) (UnityEngine.Object) this.cells[index].gameObject)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.cells[index].gameObject);
    }
    this.cells = new List<InventoryCell>();
    foreach (InventoryItem inventoryItem in this.tabs[this.tabSelected].items)
    {
      if (UiEvents.Instance.IsSoftUnlocked(inventoryItem.id))
      {
        InventoryCell component = UnityEngine.Object.Instantiate<GameObject>(this.cellPrefab, (Transform) this.cellsParent).GetComponent<InventoryCell>();
        component.currentItem = ScriptableObject.CreateInstance<InventoryItem>();
        component.currentItem.Copy(inventoryItem, 1);
        component.cellType = InventoryCell.CellType.Crafting;
        foreach (InventoryItem.CraftRequirement requirement in component.currentItem.requirements)
        {
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.requirementPrefab);
          gameObject.GetComponent<Image>().sprite = requirement.item.sprite;
          gameObject.transform.SetParent(component.transform.GetChild(component.transform.childCount - 2));
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
