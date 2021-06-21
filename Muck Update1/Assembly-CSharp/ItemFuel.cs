// Decompiled with JetBrains decompiler
// Type: ItemFuel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[CreateAssetMenu]
public class ItemFuel : ScriptableObject
{
  public int maxUses = 1;
  public int currentUses;
  public float speedMultiplier = 1f;
}
