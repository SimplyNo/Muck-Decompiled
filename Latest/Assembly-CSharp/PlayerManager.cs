// Decompiled with JetBrains decompiler
// Type: PlayerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IComparable
{
  public int id;
  public string username;
  public bool dead;
  public Color color;
  public OnlinePlayer onlinePlayer;
  public int kills;
  public int deaths;
  public int ping;
  public bool disconnected;
  public bool loaded;
  public TextMeshProUGUI nameText;
  public HitableActor hitable;
  private Collider collider;
  public Transform spectateOrbit;

  public int graveId { get; set; }

  private void Awake()
  {
    this.hitable = (HitableActor) ((Component) this).GetComponent<HitableActor>();
    this.collider = (Collider) ((Component) this).GetComponent<Collider>();
  }

  public void DamagePlayer(int hpLeft)
  {
    if (Object.op_Implicit((Object) this.onlinePlayer))
      this.SetDesiredHpRatio((float) hpLeft / 100f);
    else
      PlayerStatus.Instance.Damage(hpLeft);
  }

  public void SetHpRatio(float hpRatio)
  {
    if (!Object.op_Implicit((Object) this.onlinePlayer))
      return;
    this.SetDesiredHpRatio(hpRatio);
  }

  public void RemoveGrave()
  {
    if (this.graveId == -1)
      return;
    ResourceManager.Instance.RemoveInteractItem(this.graveId);
    this.graveId = -1;
  }

  public void SetArmor(int armorSlot, int itemId)
  {
    if (!Object.op_Implicit((Object) this.onlinePlayer))
      return;
    if (itemId == -1)
    {
      ((Component) this.onlinePlayer.armor[armorSlot]).get_gameObject().SetActive(false);
      ((Renderer) this.onlinePlayer.armor[armorSlot]).set_material((Material) null);
    }
    else
    {
      ((Component) this.onlinePlayer.armor[armorSlot]).get_gameObject().SetActive(true);
      InventoryItem allItem = ItemManager.Instance.allItems[itemId];
      ((Renderer) this.onlinePlayer.armor[armorSlot]).set_material(allItem.material);
    }
  }

  private void Start()
  {
    if (Object.op_Implicit((Object) this.nameText))
    {
      ((TMP_Text) this.nameText).set_text("");
      TextMeshProUGUI nameText = this.nameText;
      ((TMP_Text) nameText).set_text(((TMP_Text) nameText).get_text() + "\n<size=100%>" + this.username);
    }
    this.hitable.SetId(this.id);
  }

  public void SetDesiredPosition(Vector3 position)
  {
    if (!Object.op_Implicit((Object) this.onlinePlayer))
      return;
    this.onlinePlayer.desiredPos = position;
  }

  public void SetDesiredRotation(float orientationY, float orientationX)
  {
    if (!Object.op_Implicit((Object) this.onlinePlayer))
      return;
    this.onlinePlayer.orientationY = orientationY;
    this.onlinePlayer.orientationX = orientationX;
  }

  public void SetDesiredHpRatio(float ratio) => this.onlinePlayer.hpRatio = ratio;

  public int CompareTo(object obj) => 0;

  public Collider GetCollider() => this.collider;

  public PlayerManager() => base.\u002Ector();
}
