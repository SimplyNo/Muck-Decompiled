// Decompiled with JetBrains decompiler
// Type: EnableDepthBuffer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (Camera))]
[ExecuteInEditMode]
public class EnableDepthBuffer : MonoBehaviour
{
  private Camera m_camera;

  private void Update()
  {
    if ((Object) this.m_camera == (Object) null)
      this.m_camera = this.GetComponent<Camera>();
    if (this.m_camera.depthTextureMode != DepthTextureMode.None)
      return;
    this.m_camera.depthTextureMode = DepthTextureMode.Depth;
  }
}
