// Decompiled with JetBrains decompiler
// Type: BuildManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class BuildManager : MonoBehaviour
{
  public int gridSize;
  private int gridWidth;
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
  private int rotationAngle;
  private int id;

  private void Awake()
  {
    BuildManager.Instance = this;
    this.filter = (MeshFilter) this.ghostItem.GetComponent<MeshFilter>();
    this.renderer = (Renderer) this.ghostItem.GetComponent<Renderer>();
  }

  private void SetNewItem()
  {
    this.filter.set_mesh(this.currentItem.mesh);
    Material material = this.renderer.get_material();
    material.set_mainTexture(this.currentItem.material.get_mainTexture());
    this.renderer.set_material(material);
    Object.Destroy((Object) this.ghostItem.GetComponent<BoxCollider>());
    this.ghostCollider = (Collider) this.ghostItem.AddComponent<BoxCollider>();
    BuildSnappingInfo component = (BuildSnappingInfo) this.currentItem.prefab.GetComponent<BuildSnappingInfo>();
    this.ghostExtents = !Object.op_Implicit((Object) component) ? new Vector3[0] : component.position;
    this.ghostItem.get_transform().set_localScale(Vector3.op_Multiply(Vector3.get_one(), (float) this.gridSize));
    if (this.currentItem.grid)
      return;
    this.ghostItem.get_transform().set_localScale(Vector3.get_one());
  }

  private void Update() => this.NewestBuild();

  private void NewestBuild()
  {
    this.debugInfo = "";
    if (!Object.op_Implicit((Object) this.currentItem) || Object.op_Inequality((Object) this.currentItem, (Object) Hotbar.Instance.currentItem))
    {
      this.currentItem = Hotbar.Instance.currentItem;
      if (!Object.op_Implicit((Object) this.currentItem) || !this.canBuild)
      {
        if (!this.ghostItem.get_activeInHierarchy())
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
      if (!Object.op_Implicit((Object) this.playerCam))
      {
        if (!Object.op_Implicit((Object) PlayerMovement.Instance))
          return;
        this.playerCam = PlayerMovement.Instance.playerCam;
      }
      if (!this.ghostItem.get_activeInHierarchy())
      {
        this.ghostItem.SetActive(true);
        this.rotateText.SetActive(true);
      }
      this.SetNewItem();
      Bounds bounds1 = this.filter.get_mesh().get_bounds();
      Vector3 vector3_1 = ((Bounds) ref bounds1).get_extents();
      if (this.currentItem.grid)
        vector3_1 = Vector3.op_Multiply(vector3_1, (float) this.gridSize);
      this.ghostItem.get_transform().set_rotation(Quaternion.Euler(0.0f, (float) this.yRotation, 0.0f));
      RaycastHit raycastHit;
      if (Physics.Raycast(new Ray(this.playerCam.get_position(), this.playerCam.get_forward()), ref raycastHit, 12f, LayerMask.op_Implicit(this.whatIsGround)))
      {
        Vector3 point = ((RaycastHit) ref raycastHit).get_point();
        Vector3 up = Vector3.get_up();
        // ISSUE: variable of the null type
        __Null y1 = vector3_1.y;
        Bounds bounds2 = this.filter.get_mesh().get_bounds();
        // ISSUE: variable of the null type
        __Null y2 = ((Bounds) ref bounds2).get_center().y;
        // ISSUE: variable of the null type
        __Null local = y1 - y2;
        Vector3 vector3_2 = Vector3.op_Multiply(up, (float) local);
        Vector3 vector3_3 = Vector3.op_Addition(point, vector3_2);
        Bounds bounds3 = this.filter.get_mesh().get_bounds();
        ((Bounds) ref bounds3).get_center();
        BuildSnappingInfo component = (BuildSnappingInfo) ((Component) ((RaycastHit) ref raycastHit).get_collider()).GetComponent<BuildSnappingInfo>();
        if (((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().CompareTag("Build") && this.currentItem.grid && Object.op_Inequality((Object) component, (Object) null))
        {
          Vector3 vector3_4 = ((RaycastHit) ref raycastHit).get_point();
          float num1 = 3f;
          float num2 = float.PositiveInfinity;
          Vector3 vector3_5 = Vector3.get_zero();
          foreach (Vector3 vector3_6 in component.position)
          {
            Vector3 vector3_7 = this.RotateAroundPivot(Vector3.op_Addition(((Component) ((RaycastHit) ref raycastHit).get_collider()).get_transform().get_position(), Vector3.op_Multiply(vector3_6, (float) this.gridSize)), ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_transform().get_position(), new Vector3(0.0f, (float) ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_transform().get_eulerAngles().y, 0.0f));
            Vector3 vector3_8 = Vector3.op_Subtraction(vector3_7, ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_transform().get_position());
            Vector3 normalized = ((Vector3) ref vector3_8).get_normalized();
            Vector3 zero = Vector3.get_zero();
            if (zero.y > 0.0)
              zero.y = (__Null) 1.0;
            else if (zero.y < 0.0)
              zero.y = (__Null) -1.0;
            Vector3 vector3_9 = Vector3.op_Addition(normalized, ((RaycastHit) ref raycastHit).get_normal());
            Vector3 vector3_10 = Vector3.op_Division(Vector3.op_Multiply(((Vector3) ref vector3_9).get_normalized(), (float) this.gridSize), 2f);
            if ((double) Vector3.Distance(((RaycastHit) ref raycastHit).get_point(), vector3_7) < (double) num1)
            {
              this.ghostItem.get_transform().set_position(vector3_7);
              foreach (Vector3 ghostExtent in this.ghostExtents)
              {
                Vector3 vector3_11 = this.RotateAroundPivot(Vector3.op_Addition(this.ghostItem.get_transform().get_position(), Vector3.op_Multiply(ghostExtent, (float) this.gridSize)), ((Component) this.ghostCollider).get_transform().get_position(), new Vector3(0.0f, (float) this.yRotation, 0.0f));
                float num3 = Vector3.Distance(Vector3.op_Subtraction(vector3_11, vector3_10), vector3_7);
                if ((double) num3 < (double) num2)
                {
                  num2 = num3;
                  vector3_5 = Vector3.op_Subtraction(vector3_11, this.ghostItem.get_transform().get_position());
                  vector3_4 = vector3_7;
                }
              }
            }
          }
          vector3_3 = Vector3.op_Addition(vector3_4, vector3_5);
        }
        this.canBuild = true;
        this.lastPosition = vector3_3;
        this.ghostItem.get_transform().set_position(vector3_3);
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
      Gizmos.set_color(Color.get_blue());
      Gizmos.DrawCube(this.RotateAroundPivot(Vector3.op_Addition(this.ghostItem.get_transform().get_position(), Vector3.op_Multiply(ghostExtent, (float) this.gridSize)), ((Component) this.ghostCollider).get_transform().get_position(), new Vector3(0.0f, (float) this.yRotation, 0.0f)), Vector3.get_one());
    }
  }

  private Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
  {
    Vector3 vector3 = Vector3.op_Subtraction(point, pivot);
    point = Vector3.op_Addition(Quaternion.op_Multiply(Quaternion.Euler(angles), vector3), pivot);
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

  public bool CanBuild() => Object.op_Implicit((Object) this.currentItem) && this.currentItem.buildable && this.currentItem.amount > 0;

  public GameObject BuildItem(
    int buildOwner,
    int itemID,
    int objectId,
    Vector3 position,
    int yRotation)
  {
    InventoryItem allItem = ItemManager.Instance.allItems[itemID];
    GameObject o = (GameObject) Object.Instantiate<GameObject>((M0) allItem.prefab);
    o.get_transform().set_position(position);
    o.get_transform().set_rotation(Quaternion.Euler(0.0f, (float) yRotation, 0.0f));
    if (Object.op_Implicit((Object) this.buildFx))
      Object.Instantiate<GameObject>((M0) this.buildFx, position, Quaternion.get_identity());
    if (allItem.grid)
    {
      M0 component = o.GetComponent<HitableTree>();
      ((HitableResource) component).SetDefaultScale(Vector3.op_Multiply(Vector3.get_one(), (float) this.gridSize));
      ((HitableResource) component).PopIn();
    }
    ((Hitable) o.GetComponent<Hitable>()).SetId(objectId);
    ResourceManager.Instance.AddObject(objectId, o);
    ResourceManager.Instance.AddBuild(objectId, o);
    BuildDoor component1 = (BuildDoor) o.GetComponent<BuildDoor>();
    if (Object.op_Inequality((Object) component1, (Object) null))
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
      Chest componentInChildren = (Chest) o.GetComponentInChildren<Chest>();
      ChestManager.Instance.AddChest(componentInChildren, objectId);
    }
    if (buildOwner == LocalClient.instance.myId)
    {
      MonoBehaviour.print((object) "i built something");
      if (allItem.type == InventoryItem.ItemType.Station)
      {
        UiEvents.Instance.StationUnlock(itemID);
        if (Object.op_Implicit((Object) Tutorial.Instance) && allItem.name == "Workbench")
          Tutorial.Instance.stationPlaced = true;
      }
    }
    return o;
  }

  public int GetNextBuildId() => ResourceManager.Instance.GetNextId();

  public BuildManager() => base.\u002Ector();
}
