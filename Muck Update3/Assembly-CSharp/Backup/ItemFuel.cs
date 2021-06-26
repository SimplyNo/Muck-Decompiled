// Decompiled with JetBrains decompiler
// Type: ItemFuel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[CreateAssetMenu]
public class ItemFuel : ScriptableObject
{
  public int maxUses = 1;
  public int currentUses;
  public float speedMultiplier = 1f;
}
