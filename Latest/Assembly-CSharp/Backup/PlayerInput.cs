// Decompiled with JetBrains decompiler
// Type: PlayerInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PlayerInput : MonoBehaviour
{
  private float xRotation;
  private float sensitivity = 50f;
  public static float sensMultiplier = 1f;
  private float desiredX;
  private float x;
  private float y;
  private bool jumping;
  private bool crouching;
  private bool sprinting;
  private float mouseScroll;
  private Transform playerCam;
  private Transform orientation;
  private PlayerMovement playerMovement;
  public bool active = true;
  private float actualWallRotation;
  private float wallRotationVel;
  public Vector3 cameraRot;
  private float wallRunRotation;
  public float mouseOffsetY;

  public static PlayerInput Instance { get; set; }

  private void Awake()
  {
    PlayerInput.Instance = this;
    this.playerMovement = (PlayerMovement) this.GetComponent("PlayerMovement");
    this.playerCam = this.playerMovement.playerCam;
    this.orientation = this.playerMovement.orientation;
  }

  private void Update()
  {
    if (!this.active)
      return;
    if (GameManager.state == GameManager.GameState.GameOver)
    {
      this.StopInput();
    }
    else
    {
      this.MyInput();
      this.Look();
    }
  }

  private void FixedUpdate()
  {
    if (!this.active)
      return;
    this.playerMovement.Movement(this.x, this.y);
  }

  private void StopInput()
  {
    this.x = 0.0f;
    this.y = 0.0f;
    this.jumping = false;
    this.sprinting = false;
    this.mouseScroll = 0.0f;
    this.playerMovement.SetInput(new Vector2(this.x, this.y), this.crouching, this.jumping, this.sprinting);
  }

  private void MyInput()
  {
    if (OtherInput.Instance.OtherUiActive() && !Map.Instance.active)
    {
      this.StopInput();
    }
    else
    {
      if (!(bool) (Object) this.playerMovement)
        return;
      this.x = 0.0f;
      this.y = 0.0f;
      if (Input.GetKey(InputManager.forward))
        ++this.y;
      else if (Input.GetKey(InputManager.backwards))
        --this.y;
      if (Input.GetKey(InputManager.left))
        --this.x;
      if (Input.GetKey(InputManager.right))
        ++this.x;
      this.jumping = Input.GetKey(InputManager.jump);
      this.sprinting = Input.GetKey(InputManager.sprint);
      this.mouseScroll = Input.mouseScrollDelta.y;
      if (Input.GetKeyDown(InputManager.jump))
        this.playerMovement.Jump();
      if (Input.GetKey(InputManager.leftClick))
        UseInventory.Instance.Use();
      if (Input.GetKeyUp(InputManager.leftClick))
        UseInventory.Instance.UseButtonUp();
      if (Input.GetKeyDown(InputManager.rightClick))
        BuildManager.Instance.RequestBuildItem();
      if ((double) this.mouseScroll != 0.0)
        BuildManager.Instance.RotateBuild((int) Mathf.Sign(this.mouseScroll));
      if (Input.GetKeyDown(KeyCode.R))
        BuildManager.Instance.RotateBuild(1);
      if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.U) && Input.GetKeyDown(KeyCode.I))
        UiController.Instance.ToggleHud();
      this.playerMovement.SetInput(new Vector2(this.x, this.y), this.crouching, this.jumping, this.sprinting);
    }
  }

  private void Look()
  {
    if (Cursor.lockState == CursorLockMode.None || OtherInput.lockCamera)
      return;
    float num1 = this.GetMouseX();
    float num2 = (float) ((double) Input.GetAxis("Mouse Y") * (double) this.sensitivity * 0.0199999995529652) * PlayerInput.sensMultiplier;
    if (CurrentSettings.invertedHor)
      num1 = -num1;
    if (CurrentSettings.invertedVer)
      num2 = -num2;
    this.desiredX = this.playerCam.transform.localRotation.eulerAngles.y + num1;
    this.xRotation -= num2;
    this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
    this.actualWallRotation = Mathf.SmoothDamp(this.actualWallRotation, this.wallRunRotation, ref this.wallRotationVel, 0.2f);
    this.cameraRot = new Vector3(this.xRotation, this.desiredX, this.actualWallRotation);
    this.orientation.transform.localRotation = Quaternion.Euler(0.0f, this.desiredX, 0.0f);
  }

  public Vector2 GetAxisInput() => new Vector2(this.x, this.y);

  public float GetMouseX() => (float) ((double) Input.GetAxis("Mouse X") * (double) this.sensitivity * 0.0199999995529652) * PlayerInput.sensMultiplier;

  public void SetMouseOffset(float o) => this.xRotation = o;

  public float GetMouseOffset() => this.xRotation;
}
