// Decompiled with JetBrains decompiler
// Type: UpdateableData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using UnityEngine;

public class UpdateableData : ScriptableObject
{
  public bool autoUpdate;

  public event Action OnValuesUpdate;

  public UpdateableData() => base.\u002Ector();
}
