// Decompiled with JetBrains decompiler
// Type: ZoneController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ZoneController : MonoBehaviour
{
  private int baseDamage = 1;
  private float threshold = 1.5f;
  private float updateRate = 0.5f;
  private bool inZone;
  public Transform audio;
  public AudioSource transition;
  public AudioSource damageAudio;
  private float maxScale;
  public static ZoneController Instance;
  public LayerMask whatIsGround;
  private int currentDay;
  private float desiredZoneScale;
  private float zoneSpeed = 5f;

  private void Awake()
  {
    ZoneController.Instance = this;
    this.maxScale = this.transform.localScale.x;
    this.desiredZoneScale = this.maxScale;
    this.InvokeRepeating("SlowUpdate", this.updateRate, this.updateRate);
  }

  private void Start() => this.AdjustZoneHeight();

  private void AdjustZoneHeight()
  {
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.transform.position + Vector3.up * 500f, Vector3.down, out hitInfo, 1000f, (int) this.whatIsGround))
      return;
    Vector3 position = this.transform.position;
    position.y = hitInfo.point.y;
    this.transform.position = position;
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
    if (!(bool) (Object) PlayerMovement.Instance || PlayerStatus.Instance.IsPlayerDead())
      return;
    Vector3 position = PlayerMovement.Instance.transform.position;
    if ((double) Vector3.Distance(Vector3.zero, position) > (double) this.transform.localScale.x)
    {
      PlayerStatus.Instance.DealDamage(this.baseDamage * GameManager.instance.currentDay + 1);
      this.audio.transform.position = position;
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
    if (!(bool) (Object) PlayerMovement.Instance)
      return;
    Vector3 position = PlayerMovement.Instance.transform.position;
    if (!this.inZone)
      this.audio.transform.position = Vector3.zero + (position - Vector3.zero).normalized * this.transform.localScale.x;
    else
      this.audio.transform.position = position;
    if ((double) this.transform.localScale.x <= (double) this.desiredZoneScale || (double) this.transform.localScale.x <= 0.0)
      return;
    if ((double) this.transform.localScale.x < 40.0)
      this.zoneSpeed = 1f;
    this.transform.localScale -= Vector3.one * this.zoneSpeed * Time.deltaTime;
  }
}
