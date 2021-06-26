// Decompiled with JetBrains decompiler
// Type: ShaderInteractor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ShaderInteractor : MonoBehaviour
{
  private void Update() => Shader.SetGlobalVector("_PositionMoving", (Vector4) this.transform.position);
}
