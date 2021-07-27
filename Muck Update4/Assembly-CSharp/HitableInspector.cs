// Decompiled with JetBrains decompiler
// Type: HitableInspector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
  private float speed = 2f;
  private Hitable currentResource;
  private bool show;
  private Vector3 offsetPos = Vector3.zero;
  private float maxResourceDistance = 15f;
  private float maxMobDistance = 100f;
  private float ratio;

  private void Awake()
  {
    this.child = this.transform.GetChild(0).gameObject;
    this.hpBar.CrossFadeAlpha(0.0f, 0.0f, true);
    this.hpBar.CrossFadeAlpha(0.0f, 0.0f, true);
  }

  private void Update()
  {
    if (!(bool) (Object) PlayerMovement.Instance)
      return;
    Transform playerCam = PlayerMovement.Instance.playerCam;
    RaycastHit hitInfo;
    if (Physics.Raycast(playerCam.position, playerCam.forward, out hitInfo, this.maxMobDistance, (int) this.whatIsObject))
    {
      Hitable component = hitInfo.collider.gameObject.GetComponent<Hitable>();
      bool flag = true;
      if ((Object) component != (Object) null && component.gameObject.layer != LayerMask.NameToLayer("Enemy") && (double) hitInfo.distance > (double) this.maxResourceDistance)
      {
        flag = false;
        this.currentResource = (Hitable) null;
      }
      if (((!((Object) this.currentResource != (Object) component) ? 0 : ((Object) component != (Object) null ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        this.currentResource = component;
        this.show = true;
        this.maxBar.CrossFadeAlpha(1f, 0.25f, true);
        this.hpBar.CrossFadeAlpha(1f, 0.25f, true);
        this.info.CrossFadeAlpha(1f, 0.25f, true);
        this.overlay.CrossFadeAlpha(1f, 0.25f, true);
        this.hpBar.transform.localScale = new Vector3((float) this.currentResource.hp / (float) this.currentResource.maxHp, 1f, 1f);
        this.info.text = component.entityName;
      }
      if ((bool) (Object) this.currentResource)
      {
        float y = hitInfo.collider.ClosestPoint(hitInfo.collider.transform.position + Vector3.up * 10f).y;
        float num = hitInfo.transform.position.y + 2f;
        this.offsetPos.y = (double) y >= (double) num ? num + 1f : y + 1f;
        if (this.currentResource.gameObject.layer == LayerMask.NameToLayer("Enemy"))
          this.offsetPos.y = y + 0.4f;
        this.ratio = (float) this.currentResource.hp / (float) this.currentResource.maxHp;
        Vector3 position = this.currentResource.transform.position;
        position.y = this.offsetPos.y;
        this.transform.position = position;
      }
      else
      {
        this.show = false;
        this.maxBar.CrossFadeAlpha(0.0f, 0.25f, true);
        this.hpBar.CrossFadeAlpha(0.0f, 0.25f, true);
        this.overlay.CrossFadeAlpha(0.0f, 0.25f, true);
        this.info.CrossFadeAlpha(0.0f, 0.25f, true);
      }
    }
    else
    {
      if ((bool) (Object) this.currentResource || this.show)
      {
        this.show = false;
        this.maxBar.CrossFadeAlpha(0.0f, 0.25f, true);
        this.hpBar.CrossFadeAlpha(0.0f, 0.25f, true);
        this.info.CrossFadeAlpha(0.0f, 0.25f, true);
        this.overlay.CrossFadeAlpha(0.0f, 0.25f, true);
      }
      this.currentResource = (Hitable) null;
    }
    this.hpBar.transform.localScale = Vector3.Lerp(this.hpBar.transform.localScale, new Vector3(this.ratio, 1f, 1f), Time.deltaTime * 4f);
  }
}
