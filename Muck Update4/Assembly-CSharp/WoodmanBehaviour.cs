// Decompiled with JetBrains decompiler
// Type: WoodmanBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class WoodmanBehaviour : MonoBehaviour
{
  private Mob mob;
  private Vector3 headOffset;
  public int mobZoneId;
  private MobServerNeutral neutral;
  private Hitable hitable;
  public GameObject interactObject;
  private bool aggressive;

  private void Awake() => this.interactObject = this.GetComponent<hahahayes>().interact;

  private void Start()
  {
    this.mob = this.GetComponent<Mob>();
    if (LocalClient.serverOwner)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject.GetComponent<MobServer>());
      this.GetComponent<MobServer>().enabled = false;
      this.neutral = this.gameObject.AddComponent<MobServerNeutral>();
      this.neutral.mobZoneId = this.mobZoneId;
    }
    this.hitable = this.GetComponent<Hitable>();
    this.InvokeRepeating("SlowUpdate", 0.25f, 0.25f);
    this.AssignRole(new ConsistentRandom(GameManager.GetSeed() + this.hitable.GetId()));
    this.mob.agent.speed /= 2f;
  }

  private void AssignRoles() => MobZoneManager.Instance.zones[this.mobZoneId].GetComponent<GenerateCamp>().AssignRoles();

  public void AssignRole(ConsistentRandom rand)
  {
    hahahayes component = this.transform.root.GetComponent<hahahayes>();
    component.SkinColor(rand);
    if (rand.NextDouble() < 0.4)
      return;
    WoodmanBehaviour.WoodmanType type = (WoodmanBehaviour.WoodmanType) rand.Next(1, Enum.GetValues(typeof (WoodmanBehaviour.WoodmanType)).Length);
    TraderInteract traderInteract = this.interactObject.AddComponent<TraderInteract>();
    int nextId = ResourceManager.Instance.GetNextId();
    traderInteract.SetId(nextId);
    ResourceManager.Instance.AddObject(nextId, traderInteract.gameObject);
    traderInteract.SetType(type, rand);
    component.SetType(type);
    component.Randomize(rand);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.neutral);
  }

  private void SlowUpdate()
  {
    if (this.hitable.hp >= this.hitable.maxHp)
      return;
    this.MakeAggressive(true);
  }

  public void MakeAggressive(bool first)
  {
    if (this.aggressive)
      return;
    this.aggressive = true;
    this.mob.ready = true;
    try
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.neutral);
    }
    catch (Exception ex)
    {
    }
    if (this.mob.mobType.behaviour == MobType.MobBehaviour.Enemy)
      this.gameObject.AddComponent<MobServerEnemy>();
    else
      this.gameObject.AddComponent<MobServerEnemyMeleeAndRanged>();
    if (first)
    {
      foreach (GameObject entity in MobZoneManager.Instance.zones[this.mobZoneId].entities)
        entity.GetComponent<WoodmanBehaviour>().MakeAggressive(false);
    }
    this.mob.agent.speed = this.mob.mobType.speed;
    UnityEngine.Object.Destroy((UnityEngine.Object) this);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.interactObject);
  }

  public enum WoodmanType
  {
    None,
    Archer,
    Smith,
    Woodcutter,
    Chef,
    Wildcard,
  }
}
