// Decompiled with JetBrains decompiler
// Type: DetectInteractables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

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
    this.interactUi = Object.Instantiate<GameObject>(this.interactUiPrefab).transform;
    this.interactText = this.interactUi.GetComponentInChildren<TextMeshProUGUI>();
    this.interactUi.gameObject.SetActive(false);
  }

  private void Start() => this.playerCam = PlayerMovement.Instance.playerCam;

  public Interactable currentInteractable { get; private set; }

  private void Update()
  {
    RaycastHit hitInfo;
    if (Physics.SphereCast(this.playerCam.position, 1.5f, this.playerCam.forward, out hitInfo, 4f, (int) this.whatIsInteractable))
    {
      if ((Object) this.currentCollider == (Object) hitInfo.collider)
      {
        this.interactText.text = this.currentInteractable.GetName() ?? "";
      }
      else
      {
        if (!hitInfo.collider.isTrigger)
          return;
        this.currentInteractable = hitInfo.collider.gameObject.GetComponent<Interactable>();
        if (this.currentInteractable == null)
          return;
        if (this.currentInteractable != null)
          this.currentCollider = hitInfo.collider;
        this.interactUi.gameObject.SetActive(true);
        this.interactText.text = this.currentInteractable.GetName() ?? "";
        this.interactUi.transform.position = hitInfo.collider.gameObject.transform.position + Vector3.up * hitInfo.collider.bounds.extents.y;
        this.interactText.CrossFadeAlpha(1f, 0.25f, false);
      }
    }
    else
    {
      this.currentCollider = (Collider) null;
      this.currentInteractable = (Interactable) null;
      this.interactText.CrossFadeAlpha(0.0f, 0.15f, false);
    }
  }
}
