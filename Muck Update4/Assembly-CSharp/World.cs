// Decompiled with JetBrains decompiler
// Type: World
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class World : MonoBehaviour
{
  public Transform worldMesh;
  public Transform water;
  public static World Instance;

  private void Awake() => World.Instance = this;
}
