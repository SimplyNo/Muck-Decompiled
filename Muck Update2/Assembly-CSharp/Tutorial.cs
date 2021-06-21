// Decompiled with JetBrains decompiler
// Type: Tutorial
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
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
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }

  private void Update()
  {
    if (!this.started)
    {
      if (!Object.op_Implicit((Object) PlayerMovement.Instance))
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
      if (!Object.op_Equality((Object) hotkeyCell.currentItem, (Object) null) && hotkeyCell.currentItem.id == this.currentStep.item.id)
      {
        this.ContinueTutorial();
        ((Component) this.tutorialArrow).get_gameObject().SetActive(false);
      }
    }
  }

  private void Build()
  {
    if (!this.stationPlaced)
      return;
    this.ContinueTutorial();
    ((Component) this.tutorialArrow).get_gameObject().SetActive(false);
  }

  private void Inventory()
  {
    if (!((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy())
      return;
    this.ContinueTutorial();
  }

  private void TargetFollowUI()
  {
    if (!Object.op_Implicit((Object) this.currentStep.arrowTargetPos) || !((Component) InventoryUI.Instance).get_gameObject().get_activeInHierarchy())
    {
      ((Component) this.tutorialArrow).get_gameObject().SetActive(false);
    }
    else
    {
      ((Component) this.tutorialArrow).get_gameObject().SetActive(true);
      this.tutorialArrow.set_position(this.currentStep.arrowTargetPos.get_position());
      foreach (InventoryCell hotkeyCell in InventoryUI.Instance.hotkeyCells)
      {
        if (!Object.op_Equality((Object) hotkeyCell.currentItem, (Object) null) && hotkeyCell.currentItem.id == this.currentStep.item.id)
        {
          this.ContinueTutorial();
          ((Component) this.tutorialArrow).get_gameObject().SetActive(false);
        }
      }
    }
  }

  private void TargetFollowItem()
  {
    if (UiEvents.Instance.IsHardUnlocked(this.currentStep.item.id))
    {
      this.ContinueTutorial();
      ((Component) this.tutorialArrow).get_gameObject().SetActive(false);
    }
    else if (!Object.op_Implicit((Object) this.target))
    {
      ((Component) this.tutorialArrow).get_gameObject().SetActive(false);
    }
    else
    {
      ((Component) this.tutorialArrow).get_gameObject().SetActive(true);
      Vector3 worldPosition = this.calculateWorldPosition(this.target.get_position(), Camera.get_main());
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector((float) worldPosition.x, (float) worldPosition.y, (float) worldPosition.z);
      Vector2 vector2_1;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasRect, Vector2.op_Implicit(Camera.get_main().WorldToScreenPoint(vector3_1)), (Camera) null, ref vector2_1);
      float num = 0.85f;
      Vector3 vector3_2 = Vector2.op_Implicit(this.canvasRect.get_sizeDelta());
      Vector2 vector2_2 = Vector2.op_Multiply(new Vector2((float) (vector3_2.x / 2.0), (float) (vector3_2.y / 2.0)), num);
      Vector2 vector2_3 = Vector2.op_Multiply(new Vector2((float) (-vector3_2.x / 2.0), (float) (-vector3_2.y / 2.0)), num);
      if (vector2_1.x > vector2_2.x)
        vector2_1.x = vector2_2.x;
      if (vector2_1.x < vector2_3.x)
        vector2_1.x = vector2_3.x;
      if (vector2_1.y > vector2_2.y)
        vector2_1.y = vector2_2.y;
      if (vector2_1.y < vector2_3.y)
        vector2_1.y = vector2_3.y;
      this.tutorialArrow.set_localPosition(Vector2.op_Implicit(vector2_1));
    }
  }

  private Vector3 calculateWorldPosition(Vector3 position, Camera camera)
  {
    Vector3 forward = ((Component) camera).get_transform().get_forward();
    Vector3 vector3_1 = Vector3.op_Subtraction(position, ((Component) camera).get_transform().get_position());
    if ((double) Vector3.Dot(forward, ((Vector3) ref vector3_1).get_normalized()) <= 0.0)
    {
      float num = Vector3.Dot(forward, vector3_1);
      Vector3 vector3_2 = Vector3.op_Multiply(Vector3.op_Multiply(forward, num), 1.01f);
      position = Vector3.op_Addition(((Component) camera).get_transform().get_position(), Vector3.op_Subtraction(vector3_1, vector3_2));
    }
    return position;
  }

  private void FindItem(InventoryItem item)
  {
    float num1 = float.PositiveInfinity;
    GameObject gameObject = (GameObject) null;
    using (Dictionary<int, GameObject>.ValueCollection.Enumerator enumerator = ResourceManager.Instance.list.Values.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        GameObject current = enumerator.Current;
        PickupInteract component = (PickupInteract) current.GetComponent<PickupInteract>();
        if (Object.op_Inequality((Object) component, (Object) null) && component.item.id == item.id)
        {
          float num2 = Vector3.Distance(current.get_transform().get_position(), ((Component) PlayerMovement.Instance).get_transform().get_position());
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            gameObject = current;
          }
        }
      }
    }
    if (!Object.op_Inequality((Object) gameObject, (Object) null))
      return;
    this.target = gameObject.get_transform();
  }

  public Tutorial.TutorialStep currentStep { get; set; }

  public void ContinueTutorial()
  {
    if (Object.op_Implicit((Object) this.currentTaskUi))
      this.currentTaskUi.StartFade();
    this.currentTaskUi = (TutorialTaskUI) null;
    UiSfx.Instance.PlayTaskComplete();
    if (this.progress >= this.steps.Length)
    {
      this.currentStep = (Tutorial.TutorialStep) null;
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
    else
    {
      this.currentStep = this.steps[this.progress++];
      this.currentTaskUi = (TutorialTaskUI) ((GameObject) Object.Instantiate<GameObject>((M0) this.taskPrefab, this.taskParent)).GetComponent<TutorialTaskUI>();
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
    GameObject gameObject = (GameObject) null;
    using (Dictionary<int, GameObject>.ValueCollection.Enumerator enumerator = ResourceManager.Instance.list.Values.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        GameObject current = enumerator.Current;
        HitableTree component = (HitableTree) current.GetComponent<HitableTree>();
        if (Object.op_Inequality((Object) component, (Object) null) && component.entityName == "Tree")
        {
          float num2 = Vector3.Distance(current.get_transform().get_position(), ((Component) PlayerMovement.Instance).get_transform().get_position());
          if ((double) num2 < (double) num1)
          {
            num1 = num2;
            gameObject = current;
          }
        }
      }
    }
    if (Object.op_Inequality((Object) gameObject, (Object) null))
      this.target = gameObject.get_transform();
    else
      Debug.LogError((object) "didnt find tree");
  }

  public Tutorial() => base.\u002Ector();

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
