// Decompiled with JetBrains decompiler
// Type: PreviewPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PreviewPlayer : MonoBehaviour
{
  public SkinnedMeshRenderer[] armor;
  public static PreviewPlayer Instance;
  public MeshFilter filter;
  public Renderer render;

  private void Awake() => PreviewPlayer.Instance = this;

  public void SetArmor(int armorSlot, int itemId)
  {
    MonoBehaviour.print((object) ("armor slot: " + (object) armorSlot + ", item id: " + (object) itemId));
    if (itemId == -1)
    {
      this.armor[armorSlot].gameObject.SetActive(false);
    }
    else
    {
      this.armor[armorSlot].gameObject.SetActive(true);
      InventoryItem allItem = ItemManager.Instance.allItems[itemId];
      this.armor[armorSlot].material = allItem.material;
    }
  }

  public void WeaponInHand(int itemId)
  {
    if (itemId == -1)
    {
      this.filter.mesh = (Mesh) null;
    }
    else
    {
      InventoryItem allItem = ItemManager.Instance.allItems[itemId];
      this.filter.mesh = allItem.mesh;
      this.render.material = allItem.material;
    }
  }

  private void Update()
  {
  }
}
