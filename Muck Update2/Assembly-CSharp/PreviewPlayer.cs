// Decompiled with JetBrains decompiler
// Type: PreviewPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
      ((Component) this.armor[armorSlot]).get_gameObject().SetActive(false);
    }
    else
    {
      ((Component) this.armor[armorSlot]).get_gameObject().SetActive(true);
      InventoryItem allItem = ItemManager.Instance.allItems[itemId];
      ((Renderer) this.armor[armorSlot]).set_material(allItem.material);
    }
  }

  public void WeaponInHand(int itemId)
  {
    if (itemId == -1)
    {
      this.filter.set_mesh((Mesh) null);
    }
    else
    {
      InventoryItem allItem = ItemManager.Instance.allItems[itemId];
      this.filter.set_mesh(allItem.mesh);
      this.render.set_material(allItem.material);
    }
  }

  private void Update()
  {
  }

  public PreviewPlayer() => base.\u002Ector();
}
