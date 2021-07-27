// Decompiled with JetBrains decompiler
// Type: Item
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Item : MonoBehaviour
{
  public float pickupDelay = 0.85f;
  public int objectID;
  private bool pickedUp;
  private bool readyToPickUp;
  private Material outlineMat;
  public GameObject powerupParticles;

  public InventoryItem item { get; set; }

  public Powerup powerup { get; set; }

  private void Awake()
  {
    this.outlineMat = this.GetComponent<MeshRenderer>().material;
    this.Invoke("ReadyToPickup", this.pickupDelay);
    if (!LocalClient.serverOwner)
      return;
    this.Invoke("DespawnItem", 300f);
  }

  private void Start()
  {
    if (!(bool) (Object) this.item || this.item.tag != InventoryItem.ItemTag.Gem)
      return;
    Map.Instance.AddMarker(this.transform, Map.MarkerType.Gem, (Texture) this.item.sprite.texture, Color.white);
  }

  public void UpdateMesh()
  {
    if ((bool) (Object) this.powerup)
    {
      this.outlineMat.mainTexture = this.powerup.material.mainTexture;
      this.outlineMat.SetColor("_Color", this.powerup.material.color);
      this.GetComponent<MeshFilter>().mesh = this.powerup.mesh;
      Renderer component = Object.Instantiate<GameObject>(this.powerupParticles, this.transform).GetComponent<ParticleSystem>().GetComponent<Renderer>();
      Material material = component.material;
      material.color = this.powerup.GetOutlineColor();
      material.SetColor("_EmissionColor", this.powerup.GetOutlineColor() * 3f);
      component.material = material;
      this.gameObject.AddComponent<FloatItem>();
      this.GetComponent<Rigidbody>().isKinematic = true;
    }
    if ((bool) (Object) this.item)
    {
      this.outlineMat.mainTexture = this.item.material.mainTexture;
      if (this.item.material.HasProperty("_Color"))
        this.outlineMat.SetColor("_Color", this.item.material.color);
      this.GetComponent<MeshFilter>().mesh = this.item.mesh;
    }
    this.outlineMat.SetFloat("_OutlineWidth", 0.06f);
    this.FindOutlineColor();
  }

  private void FindOutlineColor()
  {
    if ((bool) (Object) this.powerup)
    {
      this.outlineMat.SetColor("_OutlineColor", this.powerup.GetOutlineColor());
    }
    else
    {
      if (!(bool) (Object) this.item)
        return;
      this.outlineMat.SetColor("_OutlineColor", this.item.GetOutlineColor());
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (this.pickedUp || !this.readyToPickUp || (InventoryUI.Instance.pickupCooldown || other.gameObject.layer != LayerMask.NameToLayer("Player")) || (!other.gameObject.CompareTag("Local") || (bool) (Object) this.item && !InventoryUI.Instance.CanPickup(this.item)))
      return;
    this.pickedUp = true;
    ClientSend.PickupItem(this.objectID);
    InventoryUI.Instance.CheckInventoryAlmostFull();
  }

  private void ReadyToPickup() => this.readyToPickUp = true;

  private void DespawnItem()
  {
    if ((Object) this.item != (Object) null && this.item.important)
      return;
    ItemManager.Instance.PickupItem(this.objectID);
    ServerSend.PickupItem(-1, this.objectID);
  }
}
