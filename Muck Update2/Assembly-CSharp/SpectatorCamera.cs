// Decompiled with JetBrains decompiler
// Type: SpectatorCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
  public Transform target;
  private bool ready;
  private int targetId;
  private string targetName;

  private void OnEnable()
  {
    this.ready = false;
    this.Invoke("GetReady", 1f);
  }

  public static SpectatorCamera Instance { get; private set; }

  private void Awake() => SpectatorCamera.Instance = this;

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      ++this.targetId;
      this.target = (Transform) null;
      this.targetName = "";
    }
    if (!this.ready || !Object.op_Implicit((Object) this.target) || (!((Component) this.target).get_gameObject().get_activeInHierarchy() || !Object.op_Implicit((Object) this.target)))
      return;
    Transform child = this.target.GetChild(0);
    ((Component) this).get_transform().set_position(Vector3.op_Addition(Vector3.op_Subtraction(this.target.get_position(), Vector3.op_Multiply(child.get_forward(), 5f)), Vector3.op_Multiply(Vector3.get_up(), 2f)));
    ((Component) this).get_transform().LookAt(this.target);
  }

  public void SetTarget(Transform target, string name) => this.target = target;

  private void GetReady() => this.ready = true;

  public SpectatorCamera() => base.\u002Ector();
}
