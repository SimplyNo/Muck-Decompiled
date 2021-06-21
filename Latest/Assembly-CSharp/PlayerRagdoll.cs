// Decompiled with JetBrains decompiler
// Type: PlayerRagdoll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
  public TestRagdoll ragdoll;
  public SkinnedMeshRenderer[] armor;
  public MeshFilter filter;
  public Renderer render;

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

  public void SetRagdoll(int id, Vector3 dir)
  {
    this.ragdoll.MakeRagdoll(dir);
    if (LocalClient.instance.myId == id)
    {
      if (Object.op_Inequality((Object) Hotbar.Instance.currentItem, (Object) null))
        this.WeaponInHand(Hotbar.Instance.currentItem.id);
      for (int armorSlot = 0; armorSlot < PlayerStatus.Instance.armor.Length; ++armorSlot)
      {
        if (Object.op_Implicit((Object) PlayerStatus.Instance.armor[armorSlot]))
          this.SetArmor(armorSlot, PlayerStatus.Instance.armor[armorSlot].id);
      }
    }
    else
    {
      OnlinePlayer onlinePlayer = GameManager.players[id].onlinePlayer;
      this.WeaponInHand(onlinePlayer.currentWeaponId);
      for (int index = 0; index < onlinePlayer.armor.Length; ++index)
      {
        if (((Component) onlinePlayer.armor[index]).get_gameObject().get_activeInHierarchy())
        {
          ((Renderer) this.armor[index]).set_material(((Renderer) onlinePlayer.armor[index]).get_material());
          ((Component) this.armor[index]).get_gameObject().SetActive(true);
        }
      }
    }
  }

  public PlayerRagdoll() => base.\u002Ector();
}
