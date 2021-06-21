// Decompiled with JetBrains decompiler
// Type: EnableDepthBuffer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
public class EnableDepthBuffer : MonoBehaviour
{
  private Camera m_camera;

  private void Update()
  {
    if (Object.op_Equality((Object) this.m_camera, (Object) null))
      this.m_camera = (Camera) ((Component) this).GetComponent<Camera>();
    if (this.m_camera.get_depthTextureMode() != null)
      return;
    this.m_camera.set_depthTextureMode((DepthTextureMode) 1);
  }

  public EnableDepthBuffer() => base.\u002Ector();
}
