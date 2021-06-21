// Decompiled with JetBrains decompiler
// Type: MobType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using UnityEngine;

[CreateAssetMenu]
public class MobType : ScriptableObject
{
  public string name;
  public GameObject mobPrefab;
  public MobType.MobBehaviour behaviour;
  public bool ranged;
  public float rangedCooldown;
  public float startAttackDistance;
  public float startRangedAttackDistance;
  public float maxAttackDistance;
  public float speed;
  public float spawnTime;
  public float minAttackAngle;
  public float sharpDefense;
  public float defense;
  public float knockbackThreshold;
  public bool ignoreBuilds;
  public float followPlayerDistance;
  public float followPlayerAccuracy;
  public bool onlyRangedInRangedPattern;
  public MobType.Weakness[] weaknesses;
  public bool boss;

  public int id { get; set; }

  public MobType() => base.\u002Ector();

  public enum MobBehaviour
  {
    Neutral,
    Enemy,
    EnemyMeleeAndRanged,
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
