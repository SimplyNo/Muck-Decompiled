// Decompiled with JetBrains decompiler
// Type: Tutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
  public Transform tutorialArrow;
  public RectTransform canvasRect;
  private bool started;
  public Tutorial.TutorialStep[] steps;
  public Transform taskParent;
  public GameObject taskPrefab;
  public static Tutorial Instance;
  public bool stationPlaced;
  private TutorialTaskUI currentTaskUi;
  private int progress;

  public Transform target { get; set; }

  private void Awake() => Tutorial.Instance = this;

  private void Start()
  {
    if (CurrentSettings.Instance.tutorial)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  private void Update()
  {
    if (!this.started)
    {
      if (!(bool) (UnityEngine.Object) PlayerMovement.Instance)
        return;
      this.started = true;
      this.Invoke("ContinueTutorial", 5f);
    }
    else
    {
      if (this.currentStep == null)
        return;
      if (this.currentStep.state == Tutorial.TutorialState.Unlock || this.currentStep.state == Tutorial.TutorialState.Tree)
        this.TargetFollowItem();
      else if (this.currentStep.state == Tutorial.TutorialState.Hotbar)
        this.TargetFollowUI();
      else if (this.currentStep.state == Tutorial.TutorialState.Inventory)
        this.Inventory();
      else if (this.currentStep.state == Tutorial.TutorialState.Workbench)
      {
        this.Workbench();
      }
      else
      {
        if (this.currentStep.state != Tutorial.TutorialState.Build)
          return;
        this.Build();
      }
    }
  }

  private void Tree()
  {
  }

  private void Workbench()
  {
    foreach (InventoryCell hotkeyCell in InventoryUI.Instance.hotkeyCells)
    {
      if (!((UnityEngine.Object) hotkeyCell.currentItem == (UnityEngine.Object) null) && hotkeyCell.currentItem.id == this.currentStep.item.id)
      {
        this.ContinueTutorial();
        this.tutorialArrow.gameObject.SetActive(false);
      }
    }
  }

  private void Build()
  {
    if (!this.stationPlaced)
      return;
    this.ContinueTutorial();
    this.tutorialArrow.gameObject.SetActive(false);
  }

  private void Inventory()
  {
    if (!InventoryUI.Instance.gameObject.activeInHierarchy)
      return;
    this.ContinueTutorial();
  }

  private void TargetFollowUI()
  {
    if (!(bool) (UnityEngine.Object) this.currentStep.arrowTargetPos || !InventoryUI.Instance.gameObject.activeInHierarchy)
    {
      this.tutorialArrow.gameObject.SetActive(false);
    }
    else
    {
      this.tutorialArrow.gameObject.SetActive(true);
      this.tutorialArrow.position = this.currentStep.arrowTargetPos.position;
      foreach (InventoryCell hotkeyCell in InventoryUI.Instance.hotkeyCells)
      {
        if (!((UnityEngine.Object) hotkeyCell.currentItem == (UnityEngine.Object) null) && hotkeyCell.currentItem.id == this.currentStep.item.id)
        {
          this.ContinueTutorial();
          this.tutorialArrow.gameObject.SetActive(false);
        }
      }
    }
  }

  private void TargetFollowItem()
  {
    if (UiEvents.Instance.IsHardUnlocked(this.currentStep.item.id))
    {
      this.ContinueTutorial();
      this.tutorialArrow.gameObject.SetActive(false);
    }
    else if (!(bool) (UnityEngine.Object) this.target)
    {
      this.tutorialArrow.gameObject.SetActive(false);
    }
    else
    {
      this.tutorialArrow.gameObject.SetActive(true);
      Vector3 worldPosition = this.calculateWorldPosition(this.target.position, Camera.main);
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasRect, (Vector2) Camera.main.WorldToScreenPoint(new Vector3(worldPosition.x, worldPosition.y, worldPosition.z)), (Camera) null, out localPoint);
      float num = 0.85f;
      Vector3 sizeDelta = (Vector3) this.canvasRect.sizeDelta;
      Vector2 vector2_1 = new Vector2(sizeDelta.x / 2f, sizeDelta.y / 2f) * num;
      Vector2 vector2_2 = new Vector2((float) (-(double) sizeDelta.x / 2.0), (float) (-(double) sizeDelta.y / 2.0)) * num;
      if ((double) localPoint.x > (double) vector2_1.x)
        localPoint.x = vector2_1.x;
      if ((double) localPoint.x < (double) vector2_2.x)
        localPoint.x = vector2_2.x;
      if ((double) localPoint.y > (double) vector2_1.y)
        localPoint.y = vector2_1.y;
      if ((double) localPoint.y < (double) vector2_2.y)
        localPoint.y = vector2_2.y;
      this.tutorialArrow.localPosition = (Vector3) localPoint;
    }
  }

  private Vector3 calculateWorldPosition(Vector3 position, Camera camera)
  {
    Vector3 forward = camera.transform.forward;
    Vector3 rhs = position - camera.transform.position;
    if ((double) Vector3.Dot(forward, rhs.normalized) <= 0.0)
    {
      float num = Vector3.Dot(forward, rhs);
      Vector3 vector3 = forward * num * 1.01f;
      position = camera.transform.position + (rhs - vector3);
    }
    return position;
  }

  private void FindItem(InventoryItem item)
  {
    float num1 = float.PositiveInfinity;
    GameObject gameObject1 = (GameObject) null;
    foreach (GameObject gameObject2 in ResourceManager.Instance.list.Values)
    {
      if (!((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null))
      {
        PickupInteract component = gameObject2.GetComponent<PickupInteract>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.item.id == item.id)
        {
          float num2 = Vector3.Distance(gameObject2.transform.position, PlayerMovement.Instance.transform.position);
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            gameObject1 = gameObject2;
          }
        }
      }
    }
    if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null))
      return;
    this.target = gameObject1.transform;
  }

  public Tutorial.TutorialStep currentStep { get; set; }

  public void ContinueTutorial()
  {
    if ((bool) (UnityEngine.Object) this.currentTaskUi)
      this.currentTaskUi.StartFade();
    this.currentTaskUi = (TutorialTaskUI) null;
    UiSfx.Instance.PlayTaskComplete();
    if (this.progress >= this.steps.Length)
    {
      this.currentStep = (Tutorial.TutorialStep) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
    {
      this.currentStep = this.steps[this.progress++];
      this.currentTaskUi = UnityEngine.Object.Instantiate<GameObject>(this.taskPrefab, this.taskParent).GetComponent<TutorialTaskUI>();
      this.currentTaskUi.SetItem(this.currentStep.item, this.currentStep.text);
      if (this.currentStep.state == Tutorial.TutorialState.Unlock)
      {
        this.FindItem(this.currentStep.item);
      }
      else
      {
        if (this.currentStep.state != Tutorial.TutorialState.Tree)
          return;
        this.FindTree();
      }
    }
  }

  private void FindTree()
  {
    float num1 = float.PositiveInfinity;
    GameObject gameObject1 = (GameObject) null;
    foreach (GameObject gameObject2 in ResourceManager.Instance.list.Values)
    {
      HitableTree component = gameObject2.GetComponent<HitableTree>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.entityName == "Tree")
      {
        float num2 = Vector3.Distance(gameObject2.transform.position, PlayerMovement.Instance.transform.position);
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          gameObject1 = gameObject2;
        }
      }
    }
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
      this.target = gameObject1.transform;
    else
      Debug.LogError((object) "didnt find tree");
  }

  [Serializable]
  public class TutorialStep
  {
    public Tutorial.TutorialState state;
    public string text;
    public InventoryItem item;
    public Transform arrowTargetPos;
  }

  [Serializable]
  public enum TutorialState
  {
    Unlock,
    Hotbar,
    Inventory,
    Tree,
    Workbench,
    Build,
  }
}
