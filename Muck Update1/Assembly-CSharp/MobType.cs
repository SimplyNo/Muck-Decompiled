// Decompiled with JetBrains decompiler
// Type: MobType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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

  public int id { get; set; }

  public enum MobBehaviour
  {
    Neutral,
    Enemy,
    EnemyMeleeAndRanged,
  }
}
