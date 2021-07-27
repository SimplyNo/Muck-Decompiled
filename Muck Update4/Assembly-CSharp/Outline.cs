// Decompiled with JetBrains decompiler
// Type: Outline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Outline : MonoBehaviour
{
  private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();
  [SerializeField]
  private Outline.Mode outlineMode;
  [SerializeField]
  private Color outlineColor = Color.white;
  [SerializeField]
  [Range(0.0f, 10f)]
  private float outlineWidth = 2f;
  [Header("Optional")]
  [SerializeField]
  [Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
  private bool precomputeOutline;
  [SerializeField]
  [HideInInspector]
  private List<Mesh> bakeKeys = new List<Mesh>();
  [SerializeField]
  [HideInInspector]
  private List<Outline.ListVector3> bakeValues = new List<Outline.ListVector3>();
  private Renderer[] renderers;
  private Material outlineMaskMaterial;
  private Material outlineFillMaterial;
  private bool needsUpdate;

  public Outline.Mode OutlineMode
  {
    get => this.outlineMode;
    set
    {
      this.outlineMode = value;
      this.needsUpdate = true;
    }
  }

  public Color OutlineColor
  {
    get => this.outlineColor;
    set
    {
      this.outlineColor = value;
      this.needsUpdate = true;
    }
  }

  public float OutlineWidth
  {
    get => this.outlineWidth;
    set
    {
      this.outlineWidth = value;
      this.needsUpdate = true;
    }
  }

  private void Awake()
  {
    this.renderers = this.GetComponentsInChildren<Renderer>();
    this.outlineMaskMaterial = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/OutlineMask"));
    this.outlineFillMaterial = UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/OutlineFill"));
    this.outlineMaskMaterial.name = "OutlineMask (Instance)";
    this.outlineFillMaterial.name = "OutlineFill (Instance)";
    this.LoadSmoothNormals();
    this.needsUpdate = true;
  }

  private void OnEnable()
  {
    foreach (Renderer renderer in this.renderers)
    {
      List<Material> list = ((IEnumerable<Material>) renderer.sharedMaterials).ToList<Material>();
      list.Add(this.outlineMaskMaterial);
      list.Add(this.outlineFillMaterial);
      renderer.materials = list.ToArray();
    }
  }

  private void OnValidate()
  {
    this.needsUpdate = true;
    if (!this.precomputeOutline && this.bakeKeys.Count != 0 || this.bakeKeys.Count != this.bakeValues.Count)
    {
      this.bakeKeys.Clear();
      this.bakeValues.Clear();
    }
    if (!this.precomputeOutline || this.bakeKeys.Count != 0)
      return;
    this.Bake();
  }

  private void Update()
  {
    if (!this.needsUpdate)
      return;
    this.needsUpdate = false;
    this.UpdateMaterialProperties();
  }

  private void OnDisable()
  {
    foreach (Renderer renderer in this.renderers)
    {
      List<Material> list = ((IEnumerable<Material>) renderer.sharedMaterials).ToList<Material>();
      list.Remove(this.outlineMaskMaterial);
      list.Remove(this.outlineFillMaterial);
      renderer.materials = list.ToArray();
    }
  }

  private void OnDestroy()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.outlineMaskMaterial);
    UnityEngine.Object.Destroy((UnityEngine.Object) this.outlineFillMaterial);
  }

  private void Bake()
  {
    HashSet<Mesh> meshSet = new HashSet<Mesh>();
    foreach (MeshFilter componentsInChild in this.GetComponentsInChildren<MeshFilter>())
    {
      if (meshSet.Add(componentsInChild.sharedMesh))
      {
        List<Vector3> vector3List = this.SmoothNormals(componentsInChild.sharedMesh);
        this.bakeKeys.Add(componentsInChild.sharedMesh);
        this.bakeValues.Add(new Outline.ListVector3()
        {
          data = vector3List
        });
      }
    }
  }

  private void LoadSmoothNormals()
  {
    foreach (MeshFilter componentsInChild in this.GetComponentsInChildren<MeshFilter>())
    {
      if (Outline.registeredMeshes.Add(componentsInChild.sharedMesh))
      {
        int index = this.bakeKeys.IndexOf(componentsInChild.sharedMesh);
        List<Vector3> vector3List = index >= 0 ? this.bakeValues[index].data : this.SmoothNormals(componentsInChild.sharedMesh);
        componentsInChild.sharedMesh.SetUVs(3, vector3List);
      }
    }
    foreach (SkinnedMeshRenderer componentsInChild in this.GetComponentsInChildren<SkinnedMeshRenderer>())
    {
      if (Outline.registeredMeshes.Add(componentsInChild.sharedMesh))
        componentsInChild.sharedMesh.uv4 = new Vector2[componentsInChild.sharedMesh.vertexCount];
    }
  }

  private List<Vector3> SmoothNormals(Mesh mesh)
  {
    IEnumerable<IGrouping<Vector3, KeyValuePair<Vector3, int>>> groupings = ((IEnumerable<Vector3>) mesh.vertices).Select<Vector3, KeyValuePair<Vector3, int>>((Func<Vector3, int, KeyValuePair<Vector3, int>>) ((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index))).GroupBy<KeyValuePair<Vector3, int>, Vector3>((Func<KeyValuePair<Vector3, int>, Vector3>) (pair => pair.Key));
    List<Vector3> vector3List = new List<Vector3>((IEnumerable<Vector3>) mesh.normals);
    foreach (IGrouping<Vector3, KeyValuePair<Vector3, int>> source in groupings)
    {
      if (source.Count<KeyValuePair<Vector3, int>>() != 1)
      {
        Vector3 zero = Vector3.zero;
        foreach (KeyValuePair<Vector3, int> keyValuePair in (IEnumerable<KeyValuePair<Vector3, int>>) source)
          zero += mesh.normals[keyValuePair.Value];
        zero.Normalize();
        foreach (KeyValuePair<Vector3, int> keyValuePair in (IEnumerable<KeyValuePair<Vector3, int>>) source)
          vector3List[keyValuePair.Value] = zero;
      }
    }
    return vector3List;
  }

  private void UpdateMaterialProperties()
  {
    this.outlineFillMaterial.SetColor("_OutlineColor", this.outlineColor);
    switch (this.outlineMode)
    {
      case Outline.Mode.OutlineAll:
        this.outlineMaskMaterial.SetFloat("_ZTest", 8f);
        this.outlineFillMaterial.SetFloat("_ZTest", 8f);
        this.outlineFillMaterial.SetFloat("_OutlineWidth", this.outlineWidth);
        break;
      case Outline.Mode.OutlineVisible:
        this.outlineMaskMaterial.SetFloat("_ZTest", 8f);
        this.outlineFillMaterial.SetFloat("_ZTest", 4f);
        this.outlineFillMaterial.SetFloat("_OutlineWidth", this.outlineWidth);
        break;
      case Outline.Mode.OutlineHidden:
        this.outlineMaskMaterial.SetFloat("_ZTest", 8f);
        this.outlineFillMaterial.SetFloat("_ZTest", 5f);
        this.outlineFillMaterial.SetFloat("_OutlineWidth", this.outlineWidth);
        break;
      case Outline.Mode.OutlineAndSilhouette:
        this.outlineMaskMaterial.SetFloat("_ZTest", 4f);
        this.outlineFillMaterial.SetFloat("_ZTest", 8f);
        this.outlineFillMaterial.SetFloat("_OutlineWidth", this.outlineWidth);
        break;
      case Outline.Mode.SilhouetteOnly:
        this.outlineMaskMaterial.SetFloat("_ZTest", 4f);
        this.outlineFillMaterial.SetFloat("_ZTest", 5f);
        this.outlineFillMaterial.SetFloat("_OutlineWidth", 0.0f);
        break;
    }
  }

  public enum Mode
  {
    OutlineAll,
    OutlineVisible,
    OutlineHidden,
    OutlineAndSilhouette,
    SilhouetteOnly,
  }

  [Serializable]
  private class ListVector3
  {
    public List<Vector3> data;
  }
}
