// Decompiled with JetBrains decompiler
// Type: RepairInteract
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

public class RepairInteract : MonoBehaviour, Interactable, SharedObject
{
  public string name;
  private int id;
  public bool replace;
  public GameObject fixedObject;
  public GameObject repairFx;
  public Material outlineMat;
  private Material defaultMat;
  private MeshRenderer render;
  public GameObject[] toActive;
  public GameObject[] toInactive;
  public Material fixedMaterial;
  public InventoryItem[] requirements;
  public int[] amounts;

  private void Start()
  {
    float num = Mathf.Clamp(1f + (float) GameManager.instance.GetPlayersInLobby() * 0.15f, 1f, 2f);
    for (int index = 0; index < this.requirements.Length; ++index)
    {
      this.requirements[index] = Object.Instantiate<InventoryItem>(this.requirements[index]);
      this.requirements[index].amount = (int) ((double) this.amounts[index] * (double) num);
    }
    this.render = this.GetComponent<MeshRenderer>();
    this.InvokeRepeating("SlowUpdate", 1f, 1f);
  }

  private void SlowUpdate()
  {
    if (!(bool) (Object) PlayerMovement.Instance)
      return;
    double num = (double) Vector3.Distance(PlayerMovement.Instance.transform.position, this.transform.position);
  }

  public void Interact()
  {
    if (!InventoryUI.Instance.CanRepair(this.requirements))
      return;
    ClientSend.Interact(this.id);
  }

  public void LocalExecute() => InventoryUI.Instance.Repair(this.requirements);

  public void AllExecute()
  {
    Object.Instantiate<GameObject>(this.repairFx, this.transform.position, Quaternion.identity);
    if (this.replace)
    {
      if ((bool) (Object) this.fixedObject)
        this.fixedObject.SetActive(true);
      this.gameObject.SetActive(false);
    }
    else
    {
      this.render = this.GetComponent<MeshRenderer>();
      this.render.material = this.fixedMaterial;
      this.render.shadowCastingMode = ShadowCastingMode.On;
      this.gameObject.layer = LayerMask.NameToLayer("Object");
      Collider[] components = this.GetComponents<Collider>();
      for (int index = 0; index < components.Length; ++index)
      {
        if (components[index].isTrigger)
          Object.Destroy((Object) components[index]);
        else
          components[index].enabled = true;
      }
    }
    foreach (GameObject gameObject in this.toActive)
      gameObject.SetActive(true);
    foreach (GameObject gameObject in this.toInactive)
      gameObject.SetActive(false);
    Object.Destroy((Object) this);
  }

  public void ServerExecute(int fromClient = -1)
  {
  }

  public void RemoveObject()
  {
  }

  public string GetName()
  {
    string str = this.name + "<size=75%>";
    foreach (InventoryItem requirement in this.requirements)
      str += string.Format("\n{0} ({1})", (object) requirement.name, (object) requirement.amount);
    return str;
  }

  public bool IsStarted() => false;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;
}
