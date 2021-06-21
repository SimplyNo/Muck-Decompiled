// Decompiled with JetBrains decompiler
// Type: Item
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class Item : MonoBehaviour
{
  public float pickupDelay;
  public int objectID;
  private bool pickedUp;
  private bool readyToPickUp;
  private Material outlineMat;
  public GameObject powerupParticles;

  public InventoryItem item { get; set; }

  public Powerup powerup { get; set; }

  private void Awake()
  {
    this.outlineMat = ((Renderer) ((Component) this).GetComponent<MeshRenderer>()).get_material();
    this.Invoke("ReadyToPickup", this.pickupDelay);
    if (!LocalClient.serverOwner)
      return;
    this.Invoke("DespawnItem", 300f);
  }

  public void UpdateMesh()
  {
    if (Object.op_Implicit((Object) this.powerup))
    {
      this.outlineMat.set_mainTexture(this.powerup.material.get_mainTexture());
      this.outlineMat.SetColor("_Color", this.powerup.material.get_color());
      ((MeshFilter) ((Component) this).GetComponent<MeshFilter>()).set_mesh(this.powerup.mesh);
      M0 component = ((Component) ((GameObject) Object.Instantiate<GameObject>((M0) this.powerupParticles, ((Component) this).get_transform())).GetComponent<ParticleSystem>()).GetComponent<Renderer>();
      Material material = ((Renderer) component).get_material();
      material.set_color(this.powerup.GetOutlineColor());
      material.SetColor("_EmissionColor", Color.op_Multiply(this.powerup.GetOutlineColor(), 3f));
      ((Renderer) component).set_material(material);
      ((Component) this).get_gameObject().AddComponent<FloatItem>();
      ((Rigidbody) ((Component) this).GetComponent<Rigidbody>()).set_isKinematic(true);
    }
    if (Object.op_Implicit((Object) this.item))
    {
      this.outlineMat.set_mainTexture(this.item.material.get_mainTexture());
      if (this.item.material.HasProperty("_Color"))
        this.outlineMat.SetColor("_Color", this.item.material.get_color());
      ((MeshFilter) ((Component) this).GetComponent<MeshFilter>()).set_mesh(this.item.mesh);
    }
    this.outlineMat.SetFloat("_OutlineWidth", 0.06f);
    this.FindOutlineColor();
  }

  private void FindOutlineColor()
  {
    if (Object.op_Implicit((Object) this.powerup))
    {
      this.outlineMat.SetColor("_OutlineColor", this.powerup.GetOutlineColor());
    }
    else
    {
      if (!Object.op_Implicit((Object) this.item))
        return;
      this.outlineMat.SetColor("_OutlineColor", this.item.GetOutlineColor());
    }
  }

  private void OnTriggerStay(Collider other)
  {
    if (this.pickedUp || !this.readyToPickUp || (InventoryUI.Instance.pickupCooldown || ((Component) other).get_gameObject().get_layer() != LayerMask.NameToLayer("Player")) || (!((Component) other).get_gameObject().CompareTag("Local") || Object.op_Implicit((Object) this.item) && !InventoryUI.Instance.CanPickup(this.item)))
      return;
    this.pickedUp = true;
    ClientSend.PickupItem(this.objectID);
    InventoryUI.Instance.CheckInventoryAlmostFull();
  }

  private void ReadyToPickup() => this.readyToPickUp = true;

  private void DespawnItem()
  {
    ItemManager.Instance.PickupItem(this.objectID);
    ServerSend.PickupItem(-1, this.objectID);
  }

  public Item() => base.\u002Ector();
}
