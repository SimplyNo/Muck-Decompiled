// Decompiled with JetBrains decompiler
// Type: GraveInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class GraveInteract : MonoBehaviour, SharedObject, Interactable
{
  private int id;
  private bool holding;
  private float holdTime;
  private float requiredHoldTime;

  public int playerId { get; set; }

  public string username { get; set; }

  public float timeLeft { get; set; }

  public void SetTime(float time) => this.timeLeft = time;

  private void Update()
  {
    if ((double) this.timeLeft > 0.0)
    {
      this.timeLeft -= Time.get_deltaTime();
      if ((double) this.timeLeft <= 0.0)
        this.timeLeft = 0.0f;
    }
    if (!this.holding)
      return;
    if (!Object.op_Implicit((Object) PlayerMovement.Instance) || (double) Vector3.Distance(((Component) PlayerMovement.Instance).get_transform().get_position(), ((Component) this).get_transform().get_position()) > 6.0 || (!Input.GetKey(InputManager.interact) || PlayerStatus.Instance.IsPlayerDead()))
      this.StopHolding();
    this.holdTime += Time.get_deltaTime();
    if ((double) this.holdTime < (double) this.requiredHoldTime)
      return;
    ClientSend.RevivePlayer(this.playerId, this.id, true);
    this.StopHolding();
  }

  private void StartHolding()
  {
    CooldownBar.Instance.ResetCooldownTime(this.requiredHoldTime, true);
    this.holding = true;
    this.holdTime = 0.0f;
  }

  private void StopHolding()
  {
    this.holding = false;
    CooldownBar.Instance.HideBar();
    this.holdTime = 0.0f;
  }

  public void Interact()
  {
    if (!this.IsDay() || (double) this.timeLeft > 0.0)
      return;
    this.StartHolding();
  }

  public void LocalExecute()
  {
  }

  public void AllExecute() => Object.Destroy((Object) ((Component) ((Component) this).get_gameObject().get_transform().get_parent()).get_gameObject());

  public void ServerExecute(int fromClient)
  {
  }

  public void RemoveObject() => Object.Destroy((Object) ((Component) ((Component) this).get_gameObject().get_transform().get_parent()).get_gameObject());

  public string GetName()
  {
    if ((double) this.timeLeft > 0.0)
      return string.Format("Can revive {0} in {1} seconds", (object) this.username, (object) Mathf.CeilToInt(this.timeLeft));
    return this.IsDay() ? string.Format("Hold {0} to revive", (object) InputManager.interact) : "Can only revive during day..";
  }

  public bool IsStarted() => false;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  public bool IsDay() => (double) DayCycle.time > 0.0 && (double) DayCycle.time < 0.5;

  public GraveInteract() => base.\u002Ector();
}
