// Decompiled with JetBrains decompiler
// Type: Guardian
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Guardian : MonoBehaviour
{
  public Guardian.GuardianType type;
  public Material[] guardianMaterial;
  public Material[] fxMaterial;
  public InventoryItem[] gems;
  public SkinnedMeshRenderer rend;
  public ParticleSystem[] particles;
  public LineRenderer[] lines;
  public TrailRenderer[] trails;
  public Hitable hitable;
  public GameObject[] destroyOnDeath;

  private void Start()
  {
    this.rend.material = this.guardianMaterial[(int) this.type];
    Material material = this.fxMaterial[(int) this.type];
    foreach (Component particle in this.particles)
    {
      ParticleSystemRenderer component = particle.GetComponent<ParticleSystemRenderer>();
      component.material = material;
      component.trailMaterial = material;
    }
    foreach (Renderer line in this.lines)
      line.material = material;
    foreach (Renderer trail in this.trails)
      trail.material = material;
    if (this.type == Guardian.GuardianType.Basic)
      return;
    Hitable component1 = this.GetComponent<Hitable>();
    LootDrop lootDrop = Object.Instantiate<LootDrop>(component1.dropTable);
    LootDrop.LootItems[] lootItemsArray = new LootDrop.LootItems[lootDrop.loot.Length + 1];
    for (int index = 0; index < lootDrop.loot.Length; ++index)
      lootItemsArray[index] = lootDrop.loot[index];
    lootItemsArray[lootDrop.loot.Length] = new LootDrop.LootItems()
    {
      item = this.gems[(int) (this.type - 1)],
      amountMin = 1,
      amountMax = 1,
      dropChance = 1f
    };
    lootDrop.loot = lootItemsArray;
    component1.dropTable = lootDrop;
    this.hitable.entityName = this.type.ToString() + " " + this.hitable.entityName;
  }

  private void OnDestroy()
  {
    for (int index = 0; index < this.destroyOnDeath.Length; ++index)
      Object.Destroy((Object) this.destroyOnDeath[index]);
  }

  public static Color TypeToColor(Guardian.GuardianType t)
  {
    switch (t)
    {
      case Guardian.GuardianType.Basic:
        return Color.white;
      case Guardian.GuardianType.Red:
        return Color.red;
      case Guardian.GuardianType.Yellow:
        return Color.yellow;
      case Guardian.GuardianType.Green:
        return Color.green;
      case Guardian.GuardianType.Blue:
        return Color.blue;
      case Guardian.GuardianType.Pink:
        return Color.magenta;
      default:
        return Color.white;
    }
  }

  public enum GuardianType
  {
    Basic,
    Red,
    Yellow,
    Green,
    Blue,
    Pink,
  }
}
