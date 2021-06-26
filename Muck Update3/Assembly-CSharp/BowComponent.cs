// Decompiled with JetBrains decompiler
// Type: BowComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
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
}
