// Decompiled with JetBrains decompiler
// Type: GeometryGrassPainter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (MeshFilter))]
[RequireComponent(typeof (MeshRenderer))]
[ExecuteInEditMode]
public class GeometryGrassPainter : MonoBehaviour
{
  private Mesh mesh;
  private MeshFilter filter;
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

  public GeometryGrassPainter() => base.\u002Ector();
}
