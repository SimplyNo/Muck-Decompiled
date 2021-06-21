// Decompiled with JetBrains decompiler
// Type: MoveCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
  private float bobSpeed = 15f;
  private float bobMultiplier = 1f;
  private readonly float bobConstant = 0.2f;
  public Camera mainCam;

  public static MoveCamera Instance { get; private set; }

  private void Start()
  {
    MoveCamera.Instance = this;
    this.cam = this.transform.GetChild(0).GetComponent<Camera>();
    this.rb = PlayerMovement.Instance.GetRb();
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
    this.transform.position = pos;
    this.state = MoveCamera.CameraState.Player;
    this.transform.parent = (Transform) null;
    this.CancelInvoke("SpectateCamera");
  }

  public void PlayerDied(Transform ragdoll)
  {
    this.target = ragdoll;
    this.state = MoveCamera.CameraState.PlayerDeath;
    this.desiredDeathPos = this.transform.position + Vector3.up * 3f;
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
    if (!(bool) (Object) this.target)
    {
      foreach (PlayerManager playerManager in GameManager.players.Values)
      {
        if (!((Object) playerManager == (Object) null) && !playerManager.dead)
        {
          this.target = new GameObject("cameraOrbit").transform;
          this.playerTarget = playerManager.transform;
          this.transform.parent = this.target;
          this.transform.localRotation = Quaternion.identity;
          this.transform.localPosition = new Vector3(0.0f, 0.0f, -10f);
          this.spectatingId = playerManager.id;
        }
      }
      if (!(bool) (Object) this.target)
        return;
    }
    Vector2 vector2 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    this.desiredSpectateRotation += new Vector3(-vector2.y, vector2.x, 0.0f) * 1.5f;
    if (Input.GetKeyDown(InputManager.rightClick))
      this.SpectateToggle(1);
    else if (Input.GetKeyDown(InputManager.leftClick))
      this.SpectateToggle(-1);
    this.target.position = this.playerTarget.position;
    this.target.rotation = Quaternion.Lerp(this.target.rotation, Quaternion.Euler(this.desiredSpectateRotation), Time.deltaTime * 10f);
    RaycastHit hitInfo;
    float num;
    if (Physics.Raycast(this.target.position, this.transform.position - this.target.position, out hitInfo, 10f, (int) this.whatIsGround))
    {
      Debug.DrawLine(this.target.position, hitInfo.point);
      num = Mathf.Clamp((float) (10.0 - (double) hitInfo.distance + 0.800000011920929), 0.0f, 10f);
    }
    else
      num = 0.0f;
    this.transform.localPosition = new Vector3(0.0f, 0.0f, num - 10f);
  }

  private void SpectateToggle(int dir)
  {
    int spectatingId = this.spectatingId;
    for (int key = 0; key < GameManager.players.Count; ++key)
    {
      if (!((Object) GameManager.players[key] == (Object) null))
      {
        PlayerManager player = GameManager.players[key];
        if (!((Object) player == (Object) null) && !player.dead)
        {
          if (dir > 0 && player.id > spectatingId)
          {
            this.spectatingId = player.id;
            this.playerTarget = player.transform;
            break;
          }
          if (dir < 0 && player.id < spectatingId)
          {
            this.spectatingId = player.id;
            this.playerTarget = player.transform;
            break;
          }
        }
      }
    }
  }

  private void PlayerDeathCamera()
  {
    this.transform.position = Vector3.Lerp(this.transform.position, this.desiredDeathPos, Time.deltaTime * 1f);
    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(this.target.position - this.transform.position), Time.deltaTime);
  }

  private void PlayerCamera()
  {
    this.UpdateBob();
    this.MoveGun();
    this.transform.position = this.player.transform.position + this.bobOffset + this.desyncOffset + this.vaultOffset + this.offset;
    if (this.cinematic)
      return;
    Vector3 cameraRot = this.playerInput.cameraRot;
    cameraRot.x = Mathf.Clamp(cameraRot.x, -90f, 90f);
    this.transform.rotation = Quaternion.Euler(cameraRot);
    this.desyncOffset = Vector3.Lerp(this.desyncOffset, Vector3.zero, Time.deltaTime * 15f);
    this.vaultOffset = Vector3.Slerp(this.vaultOffset, Vector3.zero, Time.deltaTime * 7f);
    this.desiredTilt = !PlayerMovement.Instance.IsCrouching() ? 0.0f : 6f;
    this.tilt = Mathf.Lerp(this.tilt, this.desiredTilt, Time.deltaTime * 8f);
    Vector3 eulerAngles = this.transform.rotation.eulerAngles;
    eulerAngles.z = this.tilt;
    this.transform.rotation = Quaternion.Euler(eulerAngles);
  }

  private void MoveGun()
  {
    if (!(bool) (Object) this.rb || ((double) Mathf.Abs(this.rb.velocity.magnitude) < 4.0 || !PlayerMovement.Instance.grounded))
      return;
    PlayerMovement.Instance.IsCrouching();
  }

  public void UpdateFov(float f) => this.mainCam.fieldOfView = f;

  public void BobOnce(Vector3 bobDirection) => this.desiredBob = this.ClampVector(bobDirection * 0.15f, -3f, 3f) * this.bobMultiplier;

  private void UpdateBob()
  {
    this.desiredBob = Vector3.Lerp(this.desiredBob, Vector3.zero, (float) ((double) Time.deltaTime * (double) this.bobSpeed * 0.5));
    this.bobOffset = Vector3.Lerp(this.bobOffset, this.desiredBob, Time.deltaTime * this.bobSpeed);
  }

  private Vector3 ClampVector(Vector3 vec, float min, float max) => new Vector3(Mathf.Clamp(vec.x, min, max), Mathf.Clamp(vec.y, min, max), Mathf.Clamp(vec.z, min, max));

  public enum CameraState
  {
    Player,
    PlayerDeath,
    Spectate,
  }
}
