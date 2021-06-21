// Decompiled with JetBrains decompiler
// Type: MoveCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MoveCamera : MonoBehaviour
{
  public Transform player;
  public Vector3 offset;
  public Vector3 desyncOffset;
  public Vector3 vaultOffset;
  private Camera cam;
  private Rigidbody rb;
  public PlayerInput playerInput;
  public bool cinematic;
  private float desiredTilt;
  private float tilt;
  private Vector3 desiredDeathPos;
  private Transform target;
  private Vector3 desiredSpectateRotation;
  private Transform playerTarget;
  public LayerMask whatIsGround;
  private int spectatingId;
  private Vector3 desiredBob;
  private Vector3 bobOffset;
  private float bobSpeed;
  private float bobMultiplier;
  private readonly float bobConstant;
  public Camera mainCam;
  public Camera gunCamera;

  public static MoveCamera Instance { get; private set; }

  private void Start()
  {
    MoveCamera.Instance = this;
    this.cam = (Camera) ((Component) ((Component) this).get_transform().GetChild(0)).GetComponent<Camera>();
    this.rb = PlayerMovement.Instance.GetRb();
    this.UpdateFov((float) CurrentSettings.Instance.fov);
    Debug.LogError((object) ("updating fov: " + (object) CurrentSettings.Instance.fov));
  }

  private void LateUpdate()
  {
    if (this.state == MoveCamera.CameraState.Player)
      this.PlayerCamera();
    else if (this.state == MoveCamera.CameraState.PlayerDeath)
    {
      this.PlayerDeathCamera();
    }
    else
    {
      if (this.state != MoveCamera.CameraState.Spectate)
        return;
      this.SpectateCamera();
    }
  }

  public MoveCamera.CameraState state { get; set; }

  public void PlayerRespawn(Vector3 pos)
  {
    ((Component) this).get_transform().set_position(pos);
    this.state = MoveCamera.CameraState.Player;
    ((Component) this).get_transform().set_parent((Transform) null);
    this.CancelInvoke("SpectateCamera");
  }

  public void PlayerDied(Transform ragdoll)
  {
    this.target = ragdoll;
    this.state = MoveCamera.CameraState.PlayerDeath;
    this.desiredDeathPos = Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 3f));
    if (GameManager.state == GameManager.GameState.GameOver)
      return;
    this.Invoke("StartSpectating", 4f);
  }

  private void StartSpectating()
  {
    if (GameManager.state == GameManager.GameState.GameOver || !PlayerStatus.Instance.IsPlayerDead())
      return;
    this.target = (Transform) null;
    this.state = MoveCamera.CameraState.Spectate;
    PPController.Instance.Reset();
  }

  private void SpectateCamera()
  {
    if (!Object.op_Implicit((Object) this.target))
    {
      foreach (PlayerManager playerManager in GameManager.players.Values)
      {
        if (!Object.op_Equality((Object) playerManager, (Object) null) && !playerManager.dead)
        {
          this.target = new GameObject("cameraOrbit").get_transform();
          this.playerTarget = ((Component) playerManager).get_transform();
          ((Component) this).get_transform().set_parent(this.target);
          ((Component) this).get_transform().set_localRotation(Quaternion.get_identity());
          ((Component) this).get_transform().set_localPosition(new Vector3(0.0f, 0.0f, -10f));
          this.spectatingId = playerManager.id;
        }
      }
      if (!Object.op_Implicit((Object) this.target))
        return;
    }
    Vector2 vector2;
    ((Vector2) ref vector2).\u002Ector(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    this.desiredSpectateRotation = Vector3.op_Addition(this.desiredSpectateRotation, Vector3.op_Multiply(new Vector3((float) -vector2.y, (float) vector2.x, 0.0f), 1.5f));
    if (Input.GetKeyDown(InputManager.rightClick))
      this.SpectateToggle(1);
    else if (Input.GetKeyDown(InputManager.leftClick))
      this.SpectateToggle(-1);
    this.target.set_position(this.playerTarget.get_position());
    this.target.set_rotation(Quaternion.Lerp(this.target.get_rotation(), Quaternion.Euler(this.desiredSpectateRotation), Time.get_deltaTime() * 10f));
    Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), this.target.get_position());
    RaycastHit raycastHit;
    float num;
    if (Physics.Raycast(this.target.get_position(), vector3, ref raycastHit, 10f, LayerMask.op_Implicit(this.whatIsGround)))
    {
      Debug.DrawLine(this.target.get_position(), ((RaycastHit) ref raycastHit).get_point());
      num = Mathf.Clamp((float) (10.0 - (double) ((RaycastHit) ref raycastHit).get_distance() + 0.800000011920929), 0.0f, 10f);
    }
    else
      num = 0.0f;
    ((Component) this).get_transform().set_localPosition(new Vector3(0.0f, 0.0f, num - 10f));
  }

  private void SpectateToggle(int dir)
  {
    int spectatingId = this.spectatingId;
    for (int key = 0; key < GameManager.players.Count; ++key)
    {
      if (!Object.op_Equality((Object) GameManager.players[key], (Object) null))
      {
        PlayerManager player = GameManager.players[key];
        if (!Object.op_Equality((Object) player, (Object) null) && !player.dead)
        {
          if (dir > 0 && player.id > spectatingId)
          {
            this.spectatingId = player.id;
            this.playerTarget = ((Component) player).get_transform();
            break;
          }
          if (dir < 0 && player.id < spectatingId)
          {
            this.spectatingId = player.id;
            this.playerTarget = ((Component) player).get_transform();
            break;
          }
        }
      }
    }
  }

  private void PlayerDeathCamera()
  {
    if (Object.op_Equality((Object) this.target, (Object) null))
      return;
    ((Component) this).get_transform().set_position(Vector3.Lerp(((Component) this).get_transform().get_position(), this.desiredDeathPos, Time.get_deltaTime() * 1f));
    ((Component) this).get_transform().set_rotation(Quaternion.Lerp(((Component) this).get_transform().get_rotation(), Quaternion.LookRotation(Vector3.op_Subtraction(this.target.get_position(), ((Component) this).get_transform().get_position())), Time.get_deltaTime()));
  }

  private void PlayerCamera()
  {
    this.UpdateBob();
    this.MoveGun();
    ((Component) this).get_transform().set_position(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(Vector3.op_Addition(((Component) this.player).get_transform().get_position(), this.bobOffset), this.desyncOffset), this.vaultOffset), this.offset));
    if (this.cinematic)
      return;
    Vector3 cameraRot = this.playerInput.cameraRot;
    cameraRot.x = (__Null) (double) Mathf.Clamp((float) cameraRot.x, -90f, 90f);
    ((Component) this).get_transform().set_rotation(Quaternion.Euler(cameraRot));
    this.desyncOffset = Vector3.Lerp(this.desyncOffset, Vector3.get_zero(), Time.get_deltaTime() * 15f);
    this.vaultOffset = Vector3.Slerp(this.vaultOffset, Vector3.get_zero(), Time.get_deltaTime() * 7f);
    this.desiredTilt = !PlayerMovement.Instance.IsCrouching() ? 0.0f : 6f;
    this.tilt = Mathf.Lerp(this.tilt, this.desiredTilt, Time.get_deltaTime() * 8f);
    Quaternion rotation = ((Component) this).get_transform().get_rotation();
    Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
    eulerAngles.z = (__Null) (double) this.tilt;
    ((Component) this).get_transform().set_rotation(Quaternion.Euler(eulerAngles));
  }

  private void MoveGun()
  {
    if (!Object.op_Implicit((Object) this.rb))
      return;
    Vector3 velocity = this.rb.get_velocity();
    if ((double) Mathf.Abs(((Vector3) ref velocity).get_magnitude()) < 4.0 || !PlayerMovement.Instance.grounded)
      return;
    PlayerMovement.Instance.IsCrouching();
  }

  public void UpdateFov(float f)
  {
    this.mainCam.set_fieldOfView(f);
    this.gunCamera.set_fieldOfView(f);
  }

  public void BobOnce(Vector3 bobDirection) => this.desiredBob = Vector3.op_Multiply(this.ClampVector(Vector3.op_Multiply(bobDirection, 0.15f), -3f, 3f), this.bobMultiplier);

  private void UpdateBob()
  {
    this.desiredBob = Vector3.Lerp(this.desiredBob, Vector3.get_zero(), (float) ((double) Time.get_deltaTime() * (double) this.bobSpeed * 0.5));
    this.bobOffset = Vector3.Lerp(this.bobOffset, this.desiredBob, Time.get_deltaTime() * this.bobSpeed);
  }

  private Vector3 ClampVector(Vector3 vec, float min, float max) => new Vector3(Mathf.Clamp((float) vec.x, min, max), Mathf.Clamp((float) vec.y, min, max), Mathf.Clamp((float) vec.z, min, max));

  public MoveCamera() => base.\u002Ector();

  public enum CameraState
  {
    Player,
    PlayerDeath,
    Spectate,
  }
}
