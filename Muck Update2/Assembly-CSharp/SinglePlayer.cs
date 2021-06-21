// Decompiled with JetBrains decompiler
// Type: SinglePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SinglePlayer : MonoBehaviour
{
  private Transform playerCam;
  public LayerMask whatIsGrabbable;
  private Rigidbody objectGrabbing;
  private Vector3 previousLookdir;
  private Vector3 grabPoint;
  private float dragForce;
  private SpringJoint grabJoint;
  public LineRenderer grabLr;
  private Vector3 myGrabPoint;
  private Vector3 myHandPoint;
  public static SinglePlayer Instance;
  private float oldDrag;

  private void Start()
  {
    SinglePlayer.Instance = this;
    if (!Object.op_Implicit((Object) PlayerMovement.Instance))
      return;
    this.playerCam = PlayerMovement.Instance.playerCam;
  }

  private void Update() => this.DrawGrabbing();

  private void DrawGrabbing()
  {
  }

  private void FindNewGrabLerp()
  {
    this.myGrabPoint = Vector3.Lerp(this.myGrabPoint, this.objectGrabbing.get_position(), Time.get_deltaTime() * 45f);
    this.myHandPoint = Vector3.Lerp(this.myHandPoint, ((Joint) this.grabJoint).get_connectedAnchor(), Time.get_deltaTime() * 45f);
    this.grabLr.SetPosition(0, this.myGrabPoint);
    this.grabLr.SetPosition(1, this.myHandPoint);
  }

  private void HoldGrab()
  {
    ((Joint) this.grabJoint).set_connectedAnchor(Vector3.op_Addition(((Component) this.playerCam).get_transform().get_position(), Vector3.op_Multiply(((Component) this.playerCam).get_transform().get_forward(), 6.5f)));
    this.grabLr.set_startWidth(0.05f);
    this.grabLr.set_endWidth(0.05f);
    this.previousLookdir = ((Component) this.playerCam).get_transform().get_forward();
  }

  public void StopGrab()
  {
    ((Renderer) this.grabLr).set_enabled(false);
    if (!Object.op_Implicit((Object) this.objectGrabbing))
      return;
    Object.Destroy((Object) this.grabJoint);
    this.objectGrabbing.set_angularDrag(0.05f);
    this.objectGrabbing.set_drag(this.oldDrag);
    this.objectGrabbing = (Rigidbody) null;
  }

  public SinglePlayer() => base.\u002Ector();
}
