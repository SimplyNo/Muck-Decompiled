// Decompiled with JetBrains decompiler
// Type: PlayerRagdoll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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

  public void SetRagdoll(int id, Vector3 dir)
  {
    this.ragdoll.MakeRagdoll(dir);
    if (LocalClient.instance.myId == id)
    {
      if ((Object) Hotbar.Instance.currentItem != (Object) null)
        this.WeaponInHand(Hotbar.Instance.currentItem.id);
      for (int armorSlot = 0; armorSlot < PlayerStatus.Instance.armor.Length; ++armorSlot)
      {
        if ((bool) (Object) PlayerStatus.Instance.armor[armorSlot])
          this.SetArmor(armorSlot, PlayerStatus.Instance.armor[armorSlot].id);
      }
    }
    else
    {
      OnlinePlayer onlinePlayer = GameManager.players[id].onlinePlayer;
      this.WeaponInHand(onlinePlayer.currentWeaponId);
      for (int index = 0; index < onlinePlayer.armor.Length; ++index)
      {
        if (onlinePlayer.armor[index].gameObject.activeInHierarchy)
        {
          this.armor[index].material = onlinePlayer.armor[index].material;
          this.armor[index].gameObject.SetActive(true);
        }
      }
    }
  }
}
