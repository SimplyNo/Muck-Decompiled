// Decompiled with JetBrains decompiler
// Type: DetectInteractables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetectInteractables : MonoBehaviour
{
  public LayerMask whatIsInteractable;
  public GameObject interactUiPrefab;
  private Transform interactUi;
  private TextMeshProUGUI interactText;
  private Transform playerCam;
  public static DetectInteractables Instance;
  private Collider currentCollider;

  private void Awake()
  {
    DetectInteractables.Instance = this;
    this.interactUi = ((GameObject) Object.Instantiate<GameObject>((M0) this.interactUiPrefab)).get_transform();
    this.interactText = (TextMeshProUGUI) ((Component) this.interactUi).GetComponentInChildren<TextMeshProUGUI>();
    ((Component) this.interactUi).get_gameObject().SetActive(false);
  }

  private void Start() => this.playerCam = PlayerMovement.Instance.playerCam;

  public Interactable currentInteractable { get; private set; }

  private void Update()
  {
    RaycastHit raycastHit;
    if (Physics.SphereCast(this.playerCam.get_position(), 1.5f, this.playerCam.get_forward(), ref raycastHit, 4f, LayerMask.op_Implicit(this.whatIsInteractable)))
    {
      if (Object.op_Equality((Object) this.currentCollider, (Object) ((RaycastHit) ref raycastHit).get_collider()))
      {
        ((TMP_Text) this.interactText).set_text(this.currentInteractable.GetName() ?? "");
      }
      else
      {
        if (!((RaycastHit) ref raycastHit).get_collider().get_isTrigger())
          return;
        this.currentInteractable = (Interactable) ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().GetComponent<Interactable>();
        if (this.currentInteractable == null)
          return;
        if (this.currentInteractable != null)
          this.currentCollider = ((RaycastHit) ref raycastHit).get_collider();
        ((Component) this.interactUi).get_gameObject().SetActive(true);
        ((TMP_Text) this.interactText).set_text(this.currentInteractable.GetName() ?? "");
        Transform transform = ((Component) this.interactUi).get_transform();
        Vector3 position = ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().get_transform().get_position();
        Vector3 up = Vector3.get_up();
        Bounds bounds = ((RaycastHit) ref raycastHit).get_collider().get_bounds();
        // ISSUE: variable of the null type
        __Null y = ((Bounds) ref bounds).get_extents().y;
        Vector3 vector3_1 = Vector3.op_Multiply(up, (float) y);
        Vector3 vector3_2 = Vector3.op_Addition(position, vector3_1);
        transform.set_position(vector3_2);
        ((Graphic) this.interactText).CrossFadeAlpha(1f, 0.25f, false);
      }
    }
    else
    {
      this.currentCollider = (Collider) null;
      this.currentInteractable = (Interactable) null;
      ((Graphic) this.interactText).CrossFadeAlpha(0.0f, 0.15f, false);
    }
  }

  public DetectInteractables() => base.\u002Ector();
}
