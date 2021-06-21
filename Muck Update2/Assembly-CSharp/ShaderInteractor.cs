// Decompiled with JetBrains decompiler
// Type: ShaderInteractor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class ShaderInteractor : MonoBehaviour
{
  private void Update() => Shader.SetGlobalVector("_PositionMoving", Vector4.op_Implicit(((Component) this).get_transform().get_position()));

  public ShaderInteractor() => base.\u002Ector();
}
