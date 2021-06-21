// Decompiled with JetBrains decompiler
// Type: ZoneController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class ZoneController : MonoBehaviour
{
  private int baseDamage;
  private float threshold;
  private float updateRate;
  private bool inZone;
  public Transform audio;
  public AudioSource transition;
  public AudioSource damageAudio;
  private float maxScale;
  public static ZoneController Instance;
  public LayerMask whatIsGround;
  private int currentDay;
  private float desiredZoneScale;
  private float zoneSpeed;

  private void Awake()
  {
    ZoneController.Instance = this;
    this.maxScale = (float) ((Component) this).get_transform().get_localScale().x;
    this.desiredZoneScale = this.maxScale;
    this.InvokeRepeating("SlowUpdate", this.updateRate, this.updateRate);
  }

  private void Start() => this.AdjustZoneHeight();

  private void AdjustZoneHeight()
  {
    RaycastHit raycastHit;
    if (!Physics.Raycast(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 500f)), Vector3.get_down(), ref raycastHit, 1000f, LayerMask.op_Implicit(this.whatIsGround)))
      return;
    Vector3 position = ((Component) this).get_transform().get_position();
    position.y = ((RaycastHit) ref raycastHit).get_point().y;
    ((Component) this).get_transform().set_position(position);
  }

  public void NextDay(int day)
  {
    int gameLength = (int) GameManager.gameSettings.gameLength;
    this.currentDay = day;
    this.desiredZoneScale = (float) (1.0 - (double) this.currentDay / (double) gameLength);
    this.desiredZoneScale = Mathf.Clamp(this.desiredZoneScale, 0.0f, 1f);
    this.desiredZoneScale *= this.maxScale;
  }

  private void SlowUpdate()
  {
    if (!Object.op_Implicit((Object) PlayerMovement.Instance) || PlayerStatus.Instance.IsPlayerDead())
      return;
    Vector3 position = ((Component) PlayerMovement.Instance).get_transform().get_position();
    if ((double) Vector3.Distance(Vector3.get_zero(), position) > ((Component) this).get_transform().get_localScale().x)
    {
      PlayerStatus.Instance.DealDamage(this.baseDamage * GameManager.instance.currentDay + 1);
      ((Component) this.audio).get_transform().set_position(position);
      this.damageAudio.Play();
      if (this.inZone)
        return;
      this.transition.Play();
      this.inZone = true;
      PPController.Instance.SetChromaticAberration(1f);
      ZoneVignette.Instance.SetVignette(true);
    }
    else
    {
      if (!this.inZone)
        return;
      this.transition.Play();
      this.inZone = false;
      ZoneVignette.Instance.SetVignette(false);
      PPController.Instance.SetChromaticAberration(0.0f);
    }
  }

  private void Update()
  {
    if (!Object.op_Implicit((Object) PlayerMovement.Instance))
      return;
    Vector3 position = ((Component) PlayerMovement.Instance).get_transform().get_position();
    if (!this.inZone)
    {
      Vector3 vector3 = Vector3.op_Subtraction(position, Vector3.get_zero());
      Vector3 normalized = ((Vector3) ref vector3).get_normalized();
      ((Component) this.audio).get_transform().set_position(Vector3.op_Addition(Vector3.get_zero(), Vector3.op_Multiply(normalized, (float) ((Component) this).get_transform().get_localScale().x)));
    }
    else
      ((Component) this.audio).get_transform().set_position(position);
    if (((Component) this).get_transform().get_localScale().x <= (double) this.desiredZoneScale || ((Component) this).get_transform().get_localScale().x <= 0.0)
      return;
    if (((Component) this).get_transform().get_localScale().x < 40.0)
      this.zoneSpeed = 1f;
    Transform transform = ((Component) this).get_transform();
    transform.set_localScale(Vector3.op_Subtraction(transform.get_localScale(), Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), this.zoneSpeed), Time.get_deltaTime())));
  }

  public ZoneController() => base.\u002Ector();
}
