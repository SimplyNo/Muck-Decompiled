// Decompiled with JetBrains decompiler
// Type: GraveInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GraveInteract : MonoBehaviour, SharedObject, Interactable
{
  private int id;
  private float timeLeft = 30f;

  public int playerId { get; set; }

  public string username { get; set; }

  private void Update()
  {
    if ((double) this.timeLeft <= 0.0)
      return;
    this.timeLeft -= Time.deltaTime;
    if ((double) this.timeLeft > 0.0)
      return;
    this.timeLeft = 0.0f;
  }

  public void Interact()
  {
    if (!this.IsDay() || (double) this.timeLeft > 0.0)
      return;
    ClientSend.RevivePlayer(this.playerId, this.id, true);
  }

  public void LocalExecute()
  {
  }

  public void AllExecute() => Object.Destroy((Object) this.gameObject.transform.parent.gameObject);

  public void ServerExecute()
  {
  }

  public void RemoveObject() => Object.Destroy((Object) this.gameObject.transform.parent.gameObject);

  public string GetName()
  {
    if ((double) this.timeLeft > 0.0)
      return string.Format("Can revive {0} in {1} seconds", (object) this.username, (object) Mathf.CeilToInt(this.timeLeft));
    return this.IsDay() ? string.Format("Press {0} to revive", (object) InputManager.interact) : "Can only revive during day..";
  }

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  public bool IsDay() => (double) DayCycle.time > 0.0 && (double) DayCycle.time < 0.5;
}
