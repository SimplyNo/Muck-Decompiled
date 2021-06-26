// Decompiled with JetBrains decompiler
// Type: ShrineInteractable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
      this.lights[index].material = this.lightMat;
    if (num < 3)
      return;
    this.CancelInvoke(nameof (CheckLights));
    if (LocalClient.serverOwner)
      this.Invoke("DropPowerup", 1.33f);
    Object.Instantiate<GameObject>(this.destroyShrineFx, this.transform.position, this.destroyShrineFx.transform.rotation);
    this.Invoke("DestroyShrine", 1.33f);
  }

  private void DestroyShrine() => ResourceManager.Instance.RemoveInteractItem(this.id);

  private void DropPowerup()
  {
    Powerup randomPowerup = ItemManager.Instance.GetRandomPowerup(0.3f, 0.2f, 0.1f);
    int nextId = ItemManager.Instance.GetNextId();
    ItemManager.Instance.DropPowerupAtPosition(randomPowerup.id, this.transform.position, nextId);
    ServerSend.DropPowerupAtPosition(randomPowerup.id, nextId, this.transform.position);
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
    Object.Destroy((Object) this.GetComponent<Collider>());
  }

  public void ServerExecute(int fromClient)
  {
    if (this.started)
      return;
    this.mobIds = new int[3];
    MobType spawn = GameLoop.Instance.SelectMobToSpawn(true);
    int num = 3;
    for (int index = 0; index < num; ++index)
    {
      int nextId = MobManager.Instance.GetNextId();
      int id = spawn.id;
      RaycastHit hitInfo;
      if (Physics.Raycast(this.transform.position + new Vector3(Random.Range(-1f, 1f) * 10f, 100f, Random.Range(-1f, 1f) * 10f), Vector3.down, out hitInfo, 200f, (int) this.whatIsGround))
      {
        MobSpawner.Instance.ServerSpawnNewMob(nextId, id, hitInfo.point, 1.75f, 1f);
        this.mobIds[index] = nextId;
      }
    }
    this.StartShrine(this.mobIds);
    ServerSend.ShrineStart(this.mobIds, this.id);
  }

  public void RemoveObject() => Object.Destroy((Object) this.gameObject.transform.root.gameObject);

  public string GetName() => "Start battle";

  public bool IsStarted() => this.started;

  public void SetId(int id) => this.id = id;

  public int GetId() => this.id;
}
