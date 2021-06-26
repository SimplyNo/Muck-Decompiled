﻿// Decompiled with JetBrains decompiler
// Type: GeometryGrassPainter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
}
