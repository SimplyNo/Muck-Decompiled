// Decompiled with JetBrains decompiler
// Type: BuildManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BuildManager : MonoBehaviour
{
  public int gridSize = 2;
  private int gridWidth = 10;
  public LayerMask whatIsGround;
  private Transform playerCam;
  private InventoryItem currentItem;
  public GameObject buildFx;
  public GameObject ghostItem;
  private Renderer renderer;
  private MeshFilter filter;
  public int yRotation;
  public GameObject rotateText;
  public static BuildManager Instance;
  private Vector3 lastPosition;
  private bool canBuild;
  private Vector3[] ghostExtents;
  private Collider ghostCollider;
  private string debugInfo;
  private int rotationAngle = 45;
  private int id;

  private void Awake()
  {
    BuildManager.Instance = this;
    this.filter = this.ghostItem.GetComponent<MeshFilter>();
    this.renderer = this.ghostItem.GetComponent<Renderer>();
  }

  private void SetNewItem()
  {
    this.filter.mesh = this.currentItem.mesh;
    Material material = this.renderer.material;
    material.mainTexture = this.currentItem.material.mainTexture;
    this.renderer.material = material;
    Object.Destroy((Object) this.ghostItem.GetComponent<BoxCollider>());
    this.ghostCollider = (Collider) this.ghostItem.AddComponent<BoxCollider>();
    BuildSnappingInfo component = this.currentItem.prefab.GetComponent<BuildSnappingInfo>();
    this.ghostExtents = !(bool) (Object) component ? new Vector3[0] : component.position;
    this.ghostItem.transform.localScale = Vector3.one * (float) this.gridSize;
    if (this.currentItem.grid)
      return;
    this.ghostItem.transform.localScale = Vector3.one;
  }

  private void Update() => this.NewestBuild();

  private void NewestBuild()
  {
    this.debugInfo = "";
    if (!(bool) (Object) this.currentItem || (Object) this.currentItem != (Object) Hotbar.Instance.currentItem)
    {
      this.currentItem = Hotbar.Instance.currentItem;
      if (!(bool) (Object) this.currentItem || !this.canBuild)
      {
        if (!this.ghostItem.activeInHierarchy)
          return;
        this.ghostItem.SetActive(false);
        this.rotateText.SetActive(false);
        return;
      }
    }
    if (!this.currentItem.buildable)
    {
      this.ghostItem.SetActive(false);
      this.rotateText.SetActive(false);
      this.canBuild = false;
    }
    else
    {
      if (!(bool) (Object) this.playerCam)
      {
        if (!(bool) (Object) PlayerMovement.Instance)
          return;
        this.playerCam = PlayerMovement.Instance.playerCam;
      }
      if (!this.ghostItem.activeInHierarchy)
      {
        this.ghostItem.SetActive(true);
        this.rotateText.SetActive(true);
      }
      this.SetNewItem();
      Vector3 extents = this.filter.mesh.bounds.extents;
      if (this.currentItem.grid)
        extents *= (float) this.gridSize;
      this.ghostItem.transform.rotation = Quaternion.Euler(0.0f, (float) this.yRotation, 0.0f);
      RaycastHit hitInfo;
      if (Physics.Raycast(new Ray(this.playerCam.position, this.playerCam.forward), out hitInfo, 12f, (int) this.whatIsGround))
      {
        if (hitInfo.collider.CompareTag("Ignore"))
        {
          this.canBuild = false;
          this.ghostItem.SetActive(false);
        }
        else
        {
          Vector3 vector3_1 = hitInfo.point + Vector3.up * (extents.y - this.filter.mesh.bounds.center.y);
          Vector3 center = this.filter.mesh.bounds.center;
          BuildSnappingInfo component = hitInfo.collider.GetComponent<BuildSnappingInfo>();
          if (hitInfo.collider.gameObject.CompareTag("Build") && this.currentItem.grid && (Object) component != (Object) null)
          {
            Vector3 vector3_2 = hitInfo.point;
            float num1 = 3f;
            float num2 = float.PositiveInfinity;
            Vector3 vector3_3 = Vector3.zero;
            foreach (Vector3 vector3_4 in component.position)
            {
              Vector3 b = this.RotateAroundPivot(hitInfo.collider.transform.position + vector3_4 * (float) this.gridSize, hitInfo.collider.transform.position, new Vector3(0.0f, hitInfo.collider.transform.eulerAngles.y, 0.0f));
              Vector3 normalized = (b - hitInfo.collider.transform.position).normalized;
              Vector3 zero = Vector3.zero;
              if ((double) zero.y > 0.0)
                zero.y = 1f;
              else if ((double) zero.y < 0.0)
                zero.y = -1f;
              Vector3 vector3_5 = (normalized + hitInfo.normal).normalized * (float) this.gridSize / 2f;
              if ((double) Vector3.Distance(hitInfo.point, b) < (double) num1)
              {
                this.ghostItem.transform.position = b;
                foreach (Vector3 ghostExtent in this.ghostExtents)
                {
                  Vector3 vector3_6 = this.RotateAroundPivot(this.ghostItem.transform.position + ghostExtent * (float) this.gridSize, this.ghostCollider.transform.position, new Vector3(0.0f, (float) this.yRotation, 0.0f));
                  float num3 = Vector3.Distance(vector3_6 - vector3_5, b);
                  if ((double) num3 < (double) num2)
                  {
                    num2 = num3;
                    vector3_3 = vector3_6 - this.ghostItem.transform.position;
                    vector3_2 = b;
                  }
                }
              }
            }
            vector3_1 = vector3_2 + vector3_3;
          }
          this.canBuild = true;
          this.lastPosition = vector3_1;
          this.ghostItem.transform.position = vector3_1;
        }
      }
      else
      {
        this.ghostItem.SetActive(false);
        this.canBuild = false;
      }
    }
  }

  private void OnDrawGizmos()
  {
    if (this.ghostExtents == null)
      return;
    foreach (Vector3 ghostExtent in this.ghostExtents)
    {
      Gizmos.color = Color.blue;
      Gizmos.DrawCube(this.RotateAroundPivot(this.ghostItem.transform.position + ghostExtent * (float) this.gridSize, this.ghostCollider.transform.position, new Vector3(0.0f, (float) this.yRotation, 0.0f)), Vector3.one);
    }
  }

  private Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
  {
    Vector3 vector3 = point - pivot;
    point = Quaternion.Euler(angles) * vector3 + pivot;
    return point;
  }

  public void RotateBuild(int dir) => this.yRotation -= dir * this.rotationAngle;

  public void RequestBuildItem()
  {
    if (!this.CanBuild() || !this.canBuild)
      return;
    Hotbar.Instance.UseItem(1);
    Gun.Instance.Build();
    ClientSend.RequestBuild(this.currentItem.id, this.lastPosition, this.yRotation);
  }

  public bool CanBuild() => (bool) (Object) this.currentItem && this.currentItem.buildable && this.currentItem.amount > 0;

  public GameObject BuildItem(
    int buildOwner,
    int itemID,
    int objectId,
    Vector3 position,
    int yRotation)
  {
    InventoryItem allItem = ItemManager.Instance.allItems[itemID];
    GameObject o = Object.Instantiate<GameObject>(allItem.prefab);
    o.transform.position = position;
    o.transform.rotation = Quaternion.Euler(0.0f, (float) yRotation, 0.0f);
    o.AddComponent<BuildInfo>().ownerId = buildOwner;
    if ((bool) (Object) this.buildFx)
      Object.Instantiate<GameObject>(this.buildFx, position, Quaternion.identity);
    if (allItem.grid)
    {
      HitableTree component = o.GetComponent<HitableTree>();
      component.SetDefaultScale(Vector3.one * (float) this.gridSize);
      component.PopIn();
    }
    o.GetComponent<Hitable>().SetId(objectId);
    ResourceManager.Instance.AddObject(objectId, o);
    ResourceManager.Instance.AddBuild(objectId, o);
    BuildDoor component1 = o.GetComponent<BuildDoor>();
    if ((Object) component1 != (Object) null)
    {
      foreach (BuildDoor.Door door in component1.doors)
      {
        if (LocalClient.serverOwner)
          door.SetId(ResourceManager.Instance.GetNextId());
        else
          door.SetId(objectId++);
      }
    }
    if (allItem.type == InventoryItem.ItemType.Storage)
    {
      Chest componentInChildren = o.GetComponentInChildren<Chest>();
      ChestManager.Instance.AddChest(componentInChildren, objectId);
    }
    if (buildOwner == LocalClient.instance.myId)
    {
      MonoBehaviour.print((object) "i built something");
      if (allItem.type == InventoryItem.ItemType.Station)
      {
        UiEvents.Instance.StationUnlock(itemID);
        if ((bool) (Object) Tutorial.Instance && allItem.name == "Workbench")
          Tutorial.Instance.stationPlaced = true;
      }
    }
    return o;
  }

  public int GetNextBuildId() => ResourceManager.Instance.GetNextId();
}
