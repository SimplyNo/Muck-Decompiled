// Decompiled with JetBrains decompiler
// Type: ItemFuel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[CreateAssetMenu]
public class ItemFuel : ScriptableObject
{
  public int maxUses = 1;
  public int currentUses;
  public float speedMultiplier = 1f;
}
