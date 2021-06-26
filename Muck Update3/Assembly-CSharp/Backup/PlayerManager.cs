// Decompiled with JetBrains decompiler
// Type: PlayerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    this.hitable = this.GetComponent<HitableActor>();
    this.collider = this.GetComponent<Collider>();
  }

  public void DamagePlayer(int hpLeft)
  {
    if ((bool) (UnityEngine.Object) this.onlinePlayer)
      this.SetDesiredHpRatio((float) hpLeft / 100f);
    else
      PlayerStatus.Instance.Damage(hpLeft);
  }

  public void SetHpRatio(float hpRatio)
  {
    if (!(bool) (UnityEngine.Object) this.onlinePlayer)
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
    if (!(bool) (UnityEngine.Object) this.onlinePlayer)
      return;
    if (itemId == -1)
    {
      this.onlinePlayer.armor[armorSlot].gameObject.SetActive(false);
      this.onlinePlayer.armor[armorSlot].material = (Material) null;
    }
    else
    {
      this.onlinePlayer.armor[armorSlot].gameObject.SetActive(true);
      InventoryItem allItem = ItemManager.Instance.allItems[itemId];
      this.onlinePlayer.armor[armorSlot].material = allItem.material;
    }
  }

  private void Start()
  {
    if ((bool) (UnityEngine.Object) this.nameText)
    {
      this.nameText.text = "";
      TextMeshProUGUI nameText = this.nameText;
      nameText.text = nameText.text + "\n<size=100%>" + this.username;
    }
    this.hitable.SetId(this.id);
  }

  public void SetDesiredPosition(Vector3 position)
  {
    if (!(bool) (UnityEngine.Object) this.onlinePlayer)
      return;
    this.onlinePlayer.desiredPos = position;
  }

  public void SetDesiredRotation(float orientationY, float orientationX)
  {
    if (!(bool) (UnityEngine.Object) this.onlinePlayer)
      return;
    this.onlinePlayer.orientationY = orientationY;
    this.onlinePlayer.orientationX = orientationX;
  }

  public void SetDesiredHpRatio(float ratio) => this.onlinePlayer.hpRatio = ratio;

  public int CompareTo(object obj) => 0;

  public Collider GetCollider() => this.collider;
}
