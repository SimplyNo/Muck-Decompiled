// Decompiled with JetBrains decompiler
// Type: SpectatorCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    if (!this.ready || !(bool) (Object) this.target || (!this.target.gameObject.activeInHierarchy || !(bool) (Object) this.target))
      return;
    this.transform.position = this.target.position - this.target.GetChild(0).forward * 5f + Vector3.up * 2f;
    this.transform.LookAt(this.target);
  }

  public void SetTarget(Transform target, string name) => this.target = target;

  private void GetReady() => this.ready = true;
}
