// Decompiled with JetBrains decompiler
// Type: MoveCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
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
  private float yRotation;
  private float desiredX;
  public Vector3 cameraRot;
  private Vector3 desiredBob;
  private Vector3 bobOffset;
  private float bobSpeed = 15f;
  private float bobMultiplier = 1f;
  private readonly float bobConstant = 0.2f;
  public Camera mainCam;
  public Camera gunCamera;

  public static MoveCamera Instance { get; private set; }

  private void Start()
  {
    MoveCamera.Instance = this;
    this.cam = this.transform.GetChild(0).GetComponent<Camera>();
    this.rb = PlayerMovement.Instance.GetRb();
    this.UpdateFov((float) CurrentSettings.Instance.fov);
  }

  private void LateUpdate()
  {
    switch (this.state)
    {
      case MoveCamera.CameraState.Player:
        this.PlayerCamera();
        break;
      case MoveCamera.CameraState.PlayerDeath:
        this.PlayerDeathCamera();
        break;
      case MoveCamera.CameraState.Spectate:
        this.SpectateCamera();
        break;
      case MoveCamera.CameraState.Freecam:
        this.FreeCam();
        break;
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
    if (this.TryStartFreecam())
    {
      this.state = MoveCamera.CameraState.Freecam;
      this.desiredX = this.transform.rotation.x;
      this.yRotation = this.transform.rotation.y;
      this.target = (Transform) null;
    }
    else
    {
      if (Input.GetKeyDown(InputManager.rightClick))
        this.SpectateToggle(1);
      else if (Input.GetKeyDown(InputManager.leftClick))
        this.SpectateToggle(-1);
      if (!(bool) (Object) this.target || !(bool) (Object) this.playerTarget)
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
  }

  private void FreeCam()
  {
    if (this.TryStartLockedCam())
    {
      this.state = MoveCamera.CameraState.Spectate;
    }
    else
    {
      this.FreeCamRotation();
      int num1 = Input.GetKey(InputManager.forward) ? 1 : 0;
      bool key1 = Input.GetKey(InputManager.backwards);
      bool key2 = Input.GetKey(InputManager.right);
      bool key3 = Input.GetKey(InputManager.left);
      bool key4 = Input.GetKey(InputManager.sprint);
      bool key5 = Input.GetKey(InputManager.jump);
      float num2 = 0.0f;
      float num3 = 0.0f;
      float num4 = 1f;
      float num5 = 0.0f;
      if (num1 != 0)
        num2 = 1f;
      else if (key1)
        num2 = -1f;
      if (key2)
        num3 = 0.5f;
      else if (key3)
        num3 = -0.5f;
      if (key4)
        num4 = 4f;
      if (key5)
        num5 = 1f;
      this.transform.position += ((this.transform.forward * num2 + this.transform.right * num3) * num4 + Vector3.up * num5) * Time.deltaTime * 15f;
    }
  }

  private void FreeCamRotation()
  {
    float num1 = this.playerInput.GetMouseX();
    float num2 = (float) ((double) Input.GetAxis("Mouse Y") * (double) this.playerInput.sensitivity * 0.0199999995529652) * PlayerInput.sensMultiplier;
    if (CurrentSettings.invertedHor)
      num1 = -num1;
    if (CurrentSettings.invertedVer)
      num2 = -num2;
    Debug.LogError((object) ("mouseX: " + (object) num1 + ", mouseY: " + (object) num2));
    this.desiredX += num1;
    this.yRotation -= num2;
    this.yRotation = Mathf.Clamp(this.yRotation, -90f, 90f);
    this.cameraRot = new Vector3(this.yRotation, this.desiredX, 0.0f);
    this.transform.rotation = Quaternion.Euler(this.cameraRot);
  }

  private bool TryStartFreecam() => Input.GetKey(InputManager.left) || Input.GetKey(InputManager.right) || (Input.GetKey(InputManager.forward) || Input.GetKey(InputManager.backwards)) || Input.GetKey(InputManager.jump);

  private bool TryStartLockedCam() => Input.GetKey(InputManager.rightClick) || Input.GetKey(InputManager.leftClick);

  private void SpectateToggle(int dir)
  {
    int spectatingId = this.spectatingId;
    List<int> intList = new List<int>();
    for (int key = 0; key < GameManager.players.Count; ++key)
    {
      if (GameManager.players.ContainsKey(key) && !((Object) GameManager.players[key] == (Object) null))
      {
        PlayerManager player = GameManager.players[key];
        if (!((Object) player == (Object) null) && !player.dead)
        {
          if (dir > 0 && player.id > spectatingId)
            intList.Add(key);
          if (dir < 0 && player.id < spectatingId)
            intList.Add(key);
        }
      }
    }
    if (intList.Count < 1)
      return;
    intList.Sort();
    PlayerManager player1 = GameManager.players[intList[0]];
    if (dir > 0)
      player1 = GameManager.players[intList[0]];
    if (dir < 0)
      player1 = GameManager.players[intList[intList.Count - 1]];
    this.spectatingId = player1.id;
    this.playerTarget = player1.transform;
    Debug.LogError((object) ("nextId: " + (object) this.spectatingId));
  }

  private void PlayerDeathCamera()
  {
    if ((Object) this.target == (Object) null)
      return;
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

  public void UpdateFov(float f)
  {
    this.mainCam.fieldOfView = f;
    this.gunCamera.fieldOfView = f;
  }

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
    Freecam,
  }
}
