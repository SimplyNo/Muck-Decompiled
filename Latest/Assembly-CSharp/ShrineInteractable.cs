// Decompiled with JetBrains decompiler
// Type: ShrineInteractable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class ShrineInteractable : MonoBehaviour, SharedObject, Interactable
{
  private int id;
  public MeshRenderer[] lights;
  public Material lightMat;
  private int[] mobIds;
  public bool started;
  public LayerMask whatIsGround;
  public GameObject destroyShrineFx;

  private void Start()
  {
  }

  private void CheckLights()
  {
    int num = 0;
    foreach (int mobId in this.mobIds)
    {
      if (!MobManager.Instance.mobs.ContainsKey(mobId))
        ++num;
    }
    for (int index = 0; index < num; ++index)
      ((Renderer) this.lights[index]).set_material(this.lightMat);
    if (num < 3)
      return;
    this.CancelInvoke(nameof (CheckLights));
    if (LocalClient.serverOwner)
      this.Invoke("DropPowerup", 1.33f);
    Object.Instantiate<GameObject>((M0) this.destroyShrineFx, ((Component) this).get_transform().get_position(), this.destroyShrineFx.get_transform().get_rotation());
    this.Invoke("DestroyShrine", 1.33f);
  }

  private void DestroyShrine() => ResourceManager.Instance.RemoveInteractItem(this.id);

  private void DropPowerup()
  {
    Powerup randomPowerup = ItemManager.Instance.GetRandomPowerup(0.3f, 0.2f, 0.1f);
    int nextId = ItemManager.Instance.GetNextId();
    ItemManager.Instance.DropPowerupAtPosition(randomPowerup.id, ((Component) this).get_transform().get_position(), nextId);
    ServerSend.DropPowerupAtPosition(randomPowerup.id, nextId, ((Component) this).get_transform().get_position());
  }

  public void Interact()
  {
    if (this.started)
      return;
    ClientSend.StartCombatShrine(this.id);
  }

  public void LocalExecute()
  {
  }

  public void AllExecute()
  {
  }

  public void StartShrine(int[] mobIds)
  {
    this.mobIds = mobIds;
    this.started = true;
    this.InvokeRepeating("CheckLights", 0.5f, 0.5f);
    Object.Destroy((Object) ((Component) this).GetComponent<Collider>());
  }

  public void ServerExecute(int fromClient)
  {
    if (this.started)
      return;
    this.mobIds = new int[3];
    MobType spawn = GameLoop.Instance.SelectMobToSpawn(true);
    int num = 3;
    if (spawn.boss)
      num = 2;
    for (int index = 0; index < num; ++index)
    {
      int nextId = MobManager.Instance.GetNextId();
      int id = spawn.id;
      RaycastHit raycastHit;
      if (Physics.Raycast(Vector3.op_Addition(((Component) this).get_transform().get_position(), new Vector3(Random.Range(-1f, 1f) * 10f, 100f, Random.Range(-1f, 1f) * 10f)), Vector3.get_down(), ref raycastHit, 200f, LayerMask.op_Implicit(this.whatIsGround)))
      {
        MobSpawner.Instance.ServerSpawnNewMob(nextId, id, ((RaycastHit) ref raycastHit).get_point(), 1.75f, 1f);
        this.mobIds[index] = nextId;
      }
    }
    this.StartShrine(this.mobIds);
    ServerSend.ShrineStart(this.mobIds, this.id);
  }

  public void RemoveObject() => Object.Destroy((Object) ((Component) ((Component) this).get_gameObject().get_transform().get_root()).get_gameObject());

  public string GetName() => "Start battle";

  public bool IsStarted() => this.started;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;

  public ShrineInteractable() => base.\u002Ector();
}
