// Decompiled with JetBrains decompiler
// Type: SinglePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SinglePlayer : MonoBehaviour
{
  private Transform playerCam;
  public LayerMask whatIsGrabbable;
  private Rigidbody objectGrabbing;
  private Vector3 previousLookdir;
  private Vector3 grabPoint;
  private float dragForce = 700000f;
  private SpringJoint grabJoint;
  public LineRenderer grabLr;
  private Vector3 myGrabPoint;
  private Vector3 myHandPoint;
  public static SinglePlayer Instance;
  private float oldDrag;

  private void Start()
  {
    SinglePlayer.Instance = this;
    if (!(bool) (Object) PlayerMovement.Instance)
      return;
    this.playerCam = PlayerMovement.Instance.playerCam;
  }

  private void Update() => this.DrawGrabbing();

  private void DrawGrabbing()
  {
  }

  private void FindNewGrabLerp()
  {
    this.myGrabPoint = Vector3.Lerp(this.myGrabPoint, this.objectGrabbing.position, Time.deltaTime * 45f);
    this.myHandPoint = Vector3.Lerp(this.myHandPoint, this.grabJoint.connectedAnchor, Time.deltaTime * 45f);
    this.grabLr.SetPosition(0, this.myGrabPoint);
    this.grabLr.SetPosition(1, this.myHandPoint);
  }

  private void HoldGrab()
  {
    this.grabJoint.connectedAnchor = this.playerCam.transform.position + this.playerCam.transform.forward * 6.5f;
    this.grabLr.startWidth = 0.05f;
    this.grabLr.endWidth = 0.05f;
    this.previousLookdir = this.playerCam.transform.forward;
  }

  public void StopGrab()
  {
    this.grabLr.enabled = false;
    if (!(bool) (Object) this.objectGrabbing)
      return;
    Object.Destroy((Object) this.grabJoint);
    this.objectGrabbing.angularDrag = 0.05f;
    this.objectGrabbing.drag = this.oldDrag;
    this.objectGrabbing = (Rigidbody) null;
  }
}
