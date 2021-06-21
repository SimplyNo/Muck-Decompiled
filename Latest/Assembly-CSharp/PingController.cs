// Decompiled with JetBrains decompiler
// Type: PingController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class PingController : MonoBehaviour
{
  public LayerMask whatIsPingable;
  public GameObject pingPrefab;
  private float pingCooldown;
  private bool readyToPing;
  public static PingController Instance;

  private void Awake()
  {
    PingController.Instance = this;
    this.readyToPing = true;
  }

  private void Update()
  {
    if (!Input.GetMouseButtonDown(2))
      return;
    this.LocalPing();
  }

  private void LocalPing()
  {
    if (!this.readyToPing)
      return;
    this.readyToPing = false;
    this.Invoke("PingCooldown", this.pingCooldown);
    Vector3 pingPos = this.FindPingPos();
    if (Vector3.op_Equality(pingPos, Vector3.get_zero()))
      return;
    this.MakePing(pingPos, GameManager.players[LocalClient.instance.myId].username, "");
    ClientSend.PlayerPing(pingPos);
  }

  private Vector3 FindPingPos()
  {
    Transform playerCam = PlayerMovement.Instance.playerCam;
    RaycastHit raycastHit;
    if (!Physics.Raycast(playerCam.get_position(), playerCam.get_forward(), ref raycastHit, 1500f))
      return Vector3.get_zero();
    Vector3 vector3 = Vector3.get_zero();
    if (((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().get_layer() == LayerMask.NameToLayer("Ground"))
      vector3 = Vector3.get_one();
    return Vector3.op_Addition(((RaycastHit) ref raycastHit).get_point(), vector3);
  }

  public void MakePing(Vector3 pos, string name, string pingedName) => ((PlayerPing) ((GameObject) Object.Instantiate<GameObject>((M0) this.pingPrefab, pos, Quaternion.get_identity())).GetComponent<PlayerPing>()).SetPing(name, pingedName);

  private void PingCooldown() => this.readyToPing = true;

  public PingController() => base.\u002Ector();
}
