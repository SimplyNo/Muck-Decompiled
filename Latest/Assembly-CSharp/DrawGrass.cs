// Decompiled with JetBrains decompiler
// Type: DrawGrass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DrawGrass : MonoBehaviour
{
  public Mesh mesh;
  public MeshFilter filter;
  public Color AdjustedColor;
  [Range(1f, 600000f)]
  public int grassLimit = 50000;
  private Vector3 lastPosition = Vector3.zero;
  public int toolbarInt;
  [SerializeField]
  private List<Vector3> positions = new List<Vector3>();
  [SerializeField]
  private List<Color> colors = new List<Color>();
  [SerializeField]
  private List<int> indicies = new List<int>();
  [SerializeField]
  private List<Vector3> normals = new List<Vector3>();
  [SerializeField]
  private List<Vector2> length = new List<Vector2>();
  public bool painting;
  public bool removing;
  public bool editing;
  public int i;
  public float sizeWidth = 1f;
  public float sizeLength = 1f;
  public float density = 1f;
  public float normalLimit = 1f;
  public float rangeR;
  public float rangeG;
  public float rangeB;
  public LayerMask hitMask = (LayerMask) 1;
  public LayerMask paintMask = (LayerMask) 1;
  public float brushSize;
  private Vector3 mousePos;
  [HideInInspector]
  public Vector3 hitPosGizmo;
  private Vector3 hitPos;
  [HideInInspector]
  public Vector3 hitNormal;
  private int[] indi;
  private float updateRate = 0.5f;
  public Transform grassObject;
  public float chunkLength = 20f;
  public float chunkDensity = 10f;
  public int nChunks = 16;
  public int iterations;
  private Dictionary<Vector3, bool> currentPositions;
  public Transform target;
  private Vector3 currentGridPos;
  public int gridSize = 20;

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
      Object.Destroy((Object) this.grassObject.gameObject);
      Object.Destroy((Object) this.gameObject);
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
      Object.Destroy((Object) this.grassObject.gameObject);
      Object.Destroy((Object) this.gameObject);
    }
    else
      this.UpdateGrass();
  }

  private void UpdateGrass()
  {
    if (!(bool) (Object) this.target)
    {
      if (!(bool) (Object) PlayerMovement.Instance)
        return;
      this.target = PlayerMovement.Instance.playerCam;
    }
    Vector3 gridPos = this.posToGridPos(this.target.position);
    if (!(this.currentGridPos != gridPos))
      return;
    Vector3 vector3 = (gridPos - this.currentGridPos) / this.chunkLength;
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
        Vector3 start = this.currentGridPos + new Vector3((float) index1, 0.0f, (float) index2) * (float) this.gridSize;
        this.CreateNewMesh(new Vector3((float) index1, 0.0f, (float) index2) * (float) this.gridSize, d);
        ++num2;
        Debug.DrawLine(start, start + Vector3.up * 50f, Color.red, 50f);
      }
    }
    try
    {
      MonoBehaviour.print((object) "setting grass mesh");
      this.mesh = new Mesh();
      this.mesh.SetVertices(this.positions);
      this.indi = this.indicies.ToArray();
      this.mesh.SetIndices(this.indi, MeshTopology.Points, 0);
      this.mesh.SetUVs(0, this.length);
      this.mesh.SetColors(this.colors);
      this.mesh.SetNormals(this.normals);
      this.filter.mesh = this.mesh;
    }
    catch
    {
      Debug.LogError((object) "Failed to draw grass");
    }
  }

  public Vector3 posToGridPos(Vector3 point)
  {
    float x = point.x;
    float z = point.z;
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
    if ((bool) (Object) PlayerMovement.Instance)
      this.transform.position = PlayerMovement.Instance.transform.position;
    float num1 = this.chunkDensity / (float) d;
    float num2 = this.chunkLength / num1;
    for (int index1 = 0; (double) index1 < (double) num1; ++index1)
    {
      for (int index2 = 0; (double) index2 < (double) num1; ++index2)
      {
        double num3 = (double) this.currentGridPos.x + (double) offset.x - (double) this.currentGridPos.x % (double) num2;
        float num4 = (float) ((double) this.currentGridPos.z + (double) offset.z - (double) this.currentGridPos.z % (double) num2);
        double num5 = ((double) index1 - (double) num1 / 2.0) / (double) num1 * (double) this.chunkLength;
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(new Vector3((float) (num3 + num5), this.transform.position.y + 50f, num4 + ((float) index2 - num1 / 2f) / num1 * this.chunkLength), Vector3.down), out hitInfo, 200f, this.hitMask.value) && this.i < this.grassLimit && ((double) hitInfo.normal.y <= 1.0 + (double) this.normalLimit && (double) hitInfo.normal.y >= 1.0 - (double) this.normalLimit) && (WorldUtility.WorldHeightToBiome(hitInfo.point.y) == TextureData.TerrainType.Grass && hitInfo.collider.gameObject.CompareTag("Terrain")))
        {
          this.hitPos = hitInfo.point;
          this.hitNormal = hitInfo.normal;
          this.hitPos -= this.grassObject.transform.position;
          this.positions.Add(this.hitPos);
          this.indicies.Add(this.i);
          this.length.Add(new Vector2(this.sizeWidth, this.sizeLength));
          this.colors.Add(new Color(this.AdjustedColor.r + Random.Range(0.0f, 1f) * this.rangeR, this.AdjustedColor.g + Random.Range(0.0f, 1f) * this.rangeG, this.AdjustedColor.b + Random.Range(0.0f, 1f) * this.rangeB, 1f));
          this.normals.Add(hitInfo.normal);
          ++this.i;
        }
      }
    }
  }
}
