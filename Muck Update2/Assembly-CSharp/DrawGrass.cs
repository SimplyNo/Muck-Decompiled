// Decompiled with JetBrains decompiler
// Type: DrawGrass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DrawGrass : MonoBehaviour
{
  public Mesh mesh;
  public MeshFilter filter;
  public Color AdjustedColor;
  [Range(1f, 600000f)]
  public int grassLimit;
  private Vector3 lastPosition;
  public int toolbarInt;
  [SerializeField]
  private List<Vector3> positions;
  [SerializeField]
  private List<Color> colors;
  [SerializeField]
  private List<int> indicies;
  [SerializeField]
  private List<Vector3> normals;
  [SerializeField]
  private List<Vector2> length;
  public bool painting;
  public bool removing;
  public bool editing;
  public int i;
  public float sizeWidth;
  public float sizeLength;
  public float density;
  public float normalLimit;
  public float rangeR;
  public float rangeG;
  public float rangeB;
  public LayerMask hitMask;
  public LayerMask paintMask;
  public float brushSize;
  private Vector3 mousePos;
  [HideInInspector]
  public Vector3 hitPosGizmo;
  private Vector3 hitPos;
  [HideInInspector]
  public Vector3 hitNormal;
  private int[] indi;
  private float updateRate;
  public Transform grassObject;
  public float chunkLength;
  public float chunkDensity;
  public int nChunks;
  public int iterations;
  private Dictionary<Vector3, bool> currentPositions;
  public Transform target;
  private Vector3 currentGridPos;
  public int gridSize;

  public void ClearMesh()
  {
    this.positions.Clear();
    this.indicies.Clear();
    this.normals.Clear();
    this.length.Clear();
    Object.Destroy((Object) this.mesh);
    this.i = 0;
    this.positions = new List<Vector3>();
    this.indicies = new List<int>();
    this.colors = new List<Color>();
    this.normals = new List<Vector3>();
    this.length = new List<Vector2>();
  }

  private void Awake()
  {
    if (!CurrentSettings.grass)
    {
      Object.Destroy((Object) ((Component) this.grassObject).get_gameObject());
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
    else
    {
      this.ClearMesh();
      this.InvokeRepeating("SlowUpdate", 0.0f, this.updateRate);
      this.currentPositions = new Dictionary<Vector3, bool>();
    }
  }

  private void SlowUpdate()
  {
    if (!CurrentSettings.grass)
    {
      Object.Destroy((Object) ((Component) this.grassObject).get_gameObject());
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
    else
      this.UpdateGrass();
  }

  private void UpdateGrass()
  {
    if (!Object.op_Implicit((Object) this.target))
    {
      if (!Object.op_Implicit((Object) PlayerMovement.Instance))
        return;
      this.target = PlayerMovement.Instance.playerCam;
    }
    Vector3 gridPos = this.posToGridPos(this.target.get_position());
    if (!Vector3.op_Inequality(this.currentGridPos, gridPos))
      return;
    Vector3.op_Division(Vector3.op_Subtraction(gridPos, this.currentGridPos), this.chunkLength);
    this.ClearMesh();
    this.currentGridPos = gridPos;
    int num1 = 5;
    int num2 = 0;
    int num3 = Mathf.FloorToInt(5f / 2f);
    int num4 = num3 + 1;
    int num5 = Mathf.FloorToInt((float) num1 / 2f);
    int num6 = num5 + 1;
    for (int index1 = -num3; index1 < num4; ++index1)
    {
      for (int index2 = -num3; index2 < num4; ++index2)
      {
        int d = 1;
        if (index1 <= -num5 || index1 >= num6 || (index2 <= -num5 || index2 >= num6))
          d = 2;
        Vector3 vector3 = Vector3.op_Addition(this.currentGridPos, Vector3.op_Multiply(new Vector3((float) index1, 0.0f, (float) index2), (float) this.gridSize));
        this.CreateNewMesh(Vector3.op_Multiply(new Vector3((float) index1, 0.0f, (float) index2), (float) this.gridSize), d);
        ++num2;
        Debug.DrawLine(vector3, Vector3.op_Addition(vector3, Vector3.op_Multiply(Vector3.get_up(), 50f)), Color.get_red(), 50f);
      }
    }
    try
    {
      MonoBehaviour.print((object) "setting grass mesh");
      this.mesh = new Mesh();
      this.mesh.SetVertices(this.positions);
      this.indi = this.indicies.ToArray();
      this.mesh.SetIndices(this.indi, (MeshTopology) 5, 0);
      this.mesh.SetUVs(0, this.length);
      this.mesh.SetColors(this.colors);
      this.mesh.SetNormals(this.normals);
      this.filter.set_mesh(this.mesh);
    }
    catch
    {
      Debug.LogError((object) "Failed to draw grass");
    }
  }

  public Vector3 posToGridPos(Vector3 point)
  {
    float x = (float) point.x;
    float z = (float) point.z;
    if ((double) x < 0.0)
      x -= (float) this.gridSize;
    if ((double) z < 0.0)
      z -= (float) this.gridSize;
    double num1 = (double) x - (double) x % (double) this.gridSize;
    float num2 = z - z % (float) this.gridSize;
    double num3 = (double) this.gridSize / 2.0;
    return new Vector3((float) (num1 + num3), 0.0f, num2 + (float) this.gridSize / 2f);
  }

  private void CreateNewMesh(Vector3 offset, int d)
  {
    if (Object.op_Implicit((Object) PlayerMovement.Instance))
      ((Component) this).get_transform().set_position(((Component) PlayerMovement.Instance).get_transform().get_position());
    float num1 = this.chunkDensity / (float) d;
    float num2 = this.chunkLength / num1;
    for (int index1 = 0; (double) index1 < (double) num1; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) num1; ++index2)
      {
        double num3 = this.currentGridPos.x + offset.x - this.currentGridPos.x % (double) num2;
        float num4 = (float) (this.currentGridPos.z + offset.z - this.currentGridPos.z % (double) num2);
        double num5 = ((double) index1 - (double) num1 / 2.0) / (double) num1 * (double) this.chunkLength;
        float num6 = (float) (num3 + num5);
        float num7 = num4 + ((float) index2 - num1 / 2f) / num1 * this.chunkLength;
        Vector3 vector3;
        ((Vector3) ref vector3).\u002Ector(num6, (float) (((Component) this).get_transform().get_position().y + 50.0), num7);
        RaycastHit raycastHit;
        if (Physics.Raycast(new Ray(vector3, Vector3.get_down()), ref raycastHit, 200f, ((LayerMask) ref this.hitMask).get_value()) && this.i < this.grassLimit && (((RaycastHit) ref raycastHit).get_normal().y <= 1.0 + (double) this.normalLimit && ((RaycastHit) ref raycastHit).get_normal().y >= 1.0 - (double) this.normalLimit) && (WorldUtility.WorldHeightToBiome((float) ((RaycastHit) ref raycastHit).get_point().y) == TextureData.TerrainType.Grass && ((Component) ((RaycastHit) ref raycastHit).get_collider()).get_gameObject().CompareTag("Terrain")))
        {
          this.hitPos = ((RaycastHit) ref raycastHit).get_point();
          this.hitNormal = ((RaycastHit) ref raycastHit).get_normal();
          this.hitPos = Vector3.op_Subtraction(this.hitPos, ((Component) this.grassObject).get_transform().get_position());
          this.positions.Add(this.hitPos);
          this.indicies.Add(this.i);
          this.length.Add(new Vector2(this.sizeWidth, this.sizeLength));
          this.colors.Add(new Color((float) (this.AdjustedColor.r + (double) Random.Range(0.0f, 1f) * (double) this.rangeR), (float) (this.AdjustedColor.g + (double) Random.Range(0.0f, 1f) * (double) this.rangeG), (float) (this.AdjustedColor.b + (double) Random.Range(0.0f, 1f) * (double) this.rangeB), 1f));
          this.normals.Add(((RaycastHit) ref raycastHit).get_normal());
          ++this.i;
        }
      }
    }
  }

  public DrawGrass() => base.\u002Ector();
}
