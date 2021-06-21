// Decompiled with JetBrains decompiler
// Type: CooldownBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CooldownBar : MonoBehaviour
{
  public Transform cooldownBar;
  private float time = 1f;
  private float t;
  private float timeToReachTarget;
  public static CooldownBar Instance;
  private bool stayOnScreen;

  private void Awake()
  {
    CooldownBar.Instance = this;
    this.gameObject.SetActive(false);
  }

  private void Update()
  {
    if ((double) this.timeToReachTarget == 0.0 || (double) this.t >= (double) this.timeToReachTarget)
      return;
    this.t += Time.deltaTime;
    this.cooldownBar.transform.localScale = new Vector3(this.t / this.timeToReachTarget, 1f, 1f);
    if ((double) this.t < (double) this.timeToReachTarget)
      return;
    this.t = this.timeToReachTarget;
    if (this.stayOnScreen)
      return;
    this.transform.gameObject.SetActive(false);
  }

  public void ResetCooldown(float speedMultiplier)
  {
    this.t = 0.0f;
    this.cooldownBar.transform.localScale = new Vector3(0.0f, 1f, 1f);
    this.timeToReachTarget = this.time / speedMultiplier;
    this.transform.gameObject.SetActive(true);
  }

  public void ResetCooldownTime(float time, bool stayOnScreen)
  {
    this.stayOnScreen = stayOnScreen;
    this.t = 0.0f;
    this.timeToReachTarget = time;
    this.cooldownBar.transform.localScale = new Vector3(0.0f, 1f, 1f);
    this.transform.gameObject.SetActive(true);
  }

  public void HideBar()
  {
    this.t = this.timeToReachTarget;
    this.gameObject.SetActive(false);
  }
}
