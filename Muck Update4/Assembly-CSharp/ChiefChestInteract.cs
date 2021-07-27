// Decompiled with JetBrains decompiler
// Type: ChiefChestInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ChiefChestInteract : ChestInteract
{
  public bool alreadyOpened;
  public int mobZoneId;

  protected override void WhenOpened()
  {
    if (this.alreadyOpened)
      return;
    this.alreadyOpened = true;
    foreach (GameObject entity in MobZoneManager.Instance.zones[this.mobZoneId].entities)
      entity.GetComponent<WoodmanBehaviour>().MakeAggressive(false);
    if (!(bool) (Object) AchievementManager.Instance)
      return;
    AchievementManager.Instance.OpenChiefChest();
  }
}
