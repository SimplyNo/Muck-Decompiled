// Decompiled with JetBrains decompiler
// Type: HitableInspector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitableInspector : MonoBehaviour
{
  public LayerMask whatIsObject;
  private GameObject child;
  public TextMeshProUGUI hp;
  public TextMeshProUGUI info;
  public Image hpBar;
  public Image maxBar;
  public Image overlay;
  private Vector3 desiredPosition;
  private float speed;
  private Hitable currentResource;
  private bool show;
  private Vector3 offsetPos;
  private float maxResourceDistance;
  private float maxMobDistance;
  private float ratio;

  private void Awake()
  {
    this.child = ((Component) ((Component) this).get_transform().GetChild(0)).get_gameObject();
    ((Graphic) this.hpBar).CrossFadeAlpha(0.0f, 0.0f, true);
    ((Graphic) this.hpBar).CrossFadeAlpha(0.0f, 0.0f, true);
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) PlayerMovement.Instance))
      return;
    Transform playerCam = PlayerMovement.Instance.playerCam;
    RaycastHit raycastHit;
    if (Physics.Raycast(playerCam.get_position(), playerCam.get_forward(), ref raycastHit, this.maxMobDistance, LayerMask.op_Implicit(this.whatIsObject)))
    {
      Hitable component = (Hitable) ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().GetComponent<Hitable>();
      bool flag = true;
      if (Object.op_Inequality((Object) component, (Object) null) && ((Component) component).get_gameObject().get_layer() != LayerMask.NameToLayer("Enemy") && (double) ((RaycastHit) ref raycastHit).get_distance() > (double) this.maxResourceDistance)
      {
        flag = false;
        this.currentResource = (Hitable) null;
      }
      if (((!Object.op_Inequality((Object) this.currentResource, (Object) component) ? 0 : (Object.op_Inequality((Object) component, (Object) null) ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        this.currentResource = component;
        this.show = true;
        ((Graphic) this.maxBar).CrossFadeAlpha(1f, 0.25f, true);
        ((Graphic) this.hpBar).CrossFadeAlpha(1f, 0.25f, true);
        ((Graphic) this.info).CrossFadeAlpha(1f, 0.25f, true);
        ((Graphic) this.overlay).CrossFadeAlpha(1f, 0.25f, true);
        ((Component) this.hpBar).get_transform().set_localScale(new Vector3((float) this.currentResource.hp / (float) this.currentResource.maxHp, 1f, 1f));
        ((TMP_Text) this.info).set_text(component.entityName);
      }
      if (Object.op_Implicit((Object) this.currentResource))
      {
        float y = (float) ((RaycastHit) ref raycastHit).get_collider().ClosestPoint(Vector3.op_Addition(((Component) ((RaycastHit) ref raycastHit).get_collider()).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 10f))).y;
        float num = (float) (((RaycastHit) ref raycastHit).get_transform().get_position().y + 2.0);
        this.offsetPos.y = (double) y >= (double) num ? (__Null) ((double) num + 1.0) : (__Null) ((double) y + 1.0);
        if (((Component) this.currentResource).get_gameObject().get_layer() == LayerMask.NameToLayer("Enemy"))
          this.offsetPos.y = (__Null) ((double) y + 0.400000005960464);
        this.ratio = (float) this.currentResource.hp / (float) this.currentResource.maxHp;
        Vector3 position = ((Component) this.currentResource).get_transform().get_position();
        position.y = this.offsetPos.y;
        ((Component) this).get_transform().set_position(position);
      }
      else
      {
        this.show = false;
        ((Graphic) this.maxBar).CrossFadeAlpha(0.0f, 0.25f, true);
        ((Graphic) this.hpBar).CrossFadeAlpha(0.0f, 0.25f, true);
        ((Graphic) this.overlay).CrossFadeAlpha(0.0f, 0.25f, true);
        ((Graphic) this.info).CrossFadeAlpha(0.0f, 0.25f, true);
      }
    }
    else
    {
      if (Object.op_Implicit((Object) this.currentResource) || this.show)
      {
        this.show = false;
        ((Graphic) this.maxBar).CrossFadeAlpha(0.0f, 0.25f, true);
        ((Graphic) this.hpBar).CrossFadeAlpha(0.0f, 0.25f, true);
        ((Graphic) this.info).CrossFadeAlpha(0.0f, 0.25f, true);
        ((Graphic) this.overlay).CrossFadeAlpha(0.0f, 0.25f, true);
      }
      this.currentResource = (Hitable) null;
    }
    ((Component) this.hpBar).get_transform().set_localScale(Vector3.Lerp(((Component) this.hpBar).get_transform().get_localScale(), new Vector3(this.ratio, 1f, 1f), Time.get_deltaTime() * 4f));
  }

  public HitableInspector() => base.\u002Ector();
}
