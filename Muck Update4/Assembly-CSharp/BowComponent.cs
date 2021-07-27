// Decompiled with JetBrains decompiler
// Type: BowComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[CreateAssetMenu]
public class BowComponent : ScriptableObject
{
  public float projectileSpeed;
  public int nArrows;
  public int angleDelta;
  public float timeToImpact = 1.2f;
  public float attackSize = 10f;
  public float colliderDisabledTime = 0.1f;
}
