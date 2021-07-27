// Decompiled with JetBrains decompiler
// Type: MobType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[CreateAssetMenu]
public class MobType : ScriptableObject
{
  public string name;
  public GameObject mobPrefab;
  public MobType.MobBehaviour behaviour;
  public bool ranged;
  public float rangedCooldown = 6f;
  public float startAttackDistance = 1f;
  public float startRangedAttackDistance = 5f;
  public float maxAttackDistance = 1f;
  public float speed;
  public float spawnTime = 1f;
  public float minAttackAngle = 20f;
  public float sharpDefense;
  public float defense;
  public float knockbackThreshold = 0.2f;
  public bool ignoreBuilds;
  public float followPlayerDistance = 1f;
  public float followPlayerAccuracy = 0.15f;
  public bool onlyRangedInRangedPattern;
  public MobType.Weakness[] weaknesses;
  public bool boss;

  public int id { get; set; }

  public enum MobBehaviour
  {
    Neutral,
    Enemy,
    EnemyMeleeAndRanged,
    Dragon,
  }

  [Serializable]
  public enum Weakness
  {
    Sharp,
    Blunt,
    Water,
    Fire,
    Lightning,
  }
}
