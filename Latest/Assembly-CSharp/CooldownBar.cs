// Decompiled with JetBrains decompiler
// Type: CooldownBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class CooldownBar : MonoBehaviour
{
  public Transform cooldownBar;
  private float time;
  private float t;
  private float timeToReachTarget;
  public static CooldownBar Instance;
  private bool stayOnScreen;

  private void Awake()
  {
    CooldownBar.Instance = this;
    ((Component) this).get_gameObject().SetActive(false);
  }

  private void Update()
  {
    if ((double) this.timeToReachTarget == 0.0 || (double) this.t >= (double) this.timeToReachTarget)
      return;
    this.t += Time.get_deltaTime();
    ((Component) this.cooldownBar).get_transform().set_localScale(new Vector3(this.t / this.timeToReachTarget, 1f, 1f));
    if ((double) this.t < (double) this.timeToReachTarget)
      return;
    this.t = this.timeToReachTarget;
    if (this.stayOnScreen)
      return;
    ((Component) ((Component) this).get_transform()).get_gameObject().SetActive(false);
  }

  public void ResetCooldown(float speedMultiplier)
  {
    this.t = 0.0f;
    ((Component) this.cooldownBar).get_transform().set_localScale(new Vector3(0.0f, 1f, 1f));
    this.timeToReachTarget = this.time / speedMultiplier;
    ((Component) ((Component) this).get_transform()).get_gameObject().SetActive(true);
  }

  public void ResetCooldownTime(float time, bool stayOnScreen)
  {
    this.stayOnScreen = stayOnScreen;
    this.t = 0.0f;
    this.timeToReachTarget = time;
    ((Component) this.cooldownBar).get_transform().set_localScale(new Vector3(0.0f, 1f, 1f));
    ((Component) ((Component) this).get_transform()).get_gameObject().SetActive(true);
  }

  public void HideBar()
  {
    this.t = this.timeToReachTarget;
    ((Component) this).get_gameObject().SetActive(false);
  }

  public CooldownBar() => base.\u002Ector();
}
