// Decompiled with JetBrains decompiler
// Type: Outline
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections;
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
  private Color outlineColor;
  [SerializeField]
  [Range(0.0f, 10f)]
  private float outlineWidth;
  [Header("Optional")]
  [SerializeField]
  [Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
  private bool precomputeOutline;
  [SerializeField]
  [HideInInspector]
  private List<Mesh> bakeKeys;
  [SerializeField]
  [HideInInspector]
  private List<Outline.ListVector3> bakeValues;
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
    this.renderers = (Renderer[]) ((Component) this).GetComponentsInChildren<Renderer>();
    this.outlineMaskMaterial = (Material) Object.Instantiate<Material>(Resources.Load<Material>("Materials/OutlineMask"));
    this.outlineFillMaterial = (Material) Object.Instantiate<Material>(Resources.Load<Material>("Materials/OutlineFill"));
    ((Object) this.outlineMaskMaterial).set_name("OutlineMask (Instance)");
    ((Object) this.outlineFillMaterial).set_name("OutlineFill (Instance)");
    this.LoadSmoothNormals();
    this.needsUpdate = true;
  }

  private void OnEnable()
  {
    foreach (Renderer renderer in this.renderers)
    {
      List<Material> list = ((IEnumerable<Material>) renderer.get_sharedMaterials()).ToList<Material>();
      list.Add(this.outlineMaskMaterial);
      list.Add(this.outlineFillMaterial);
      renderer.set_materials(list.ToArray());
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
      List<Material> list = ((IEnumerable<Material>) renderer.get_sharedMaterials()).ToList<Material>();
      list.Remove(this.outlineMaskMaterial);
      list.Remove(this.outlineFillMaterial);
      renderer.set_materials(list.ToArray());
    }
  }

  private void OnDestroy()
  {
    Object.Destroy((Object) this.outlineMaskMaterial);
    Object.Destroy((Object) this.outlineFillMaterial);
  }

  private void Bake()
  {
    HashSet<Mesh> meshSet = new HashSet<Mesh>();
    foreach (MeshFilter componentsInChild in (MeshFilter[]) ((Component) this).GetComponentsInChildren<MeshFilter>())
    {
      if (meshSet.Add(componentsInChild.get_sharedMesh()))
      {
        List<Vector3> vector3List = this.SmoothNormals(componentsInChild.get_sharedMesh());
        this.bakeKeys.Add(componentsInChild.get_sharedMesh());
        this.bakeValues.Add(new Outline.ListVector3()
        {
          data = vector3List
        });
      }
    }
  }

  private void LoadSmoothNormals()
  {
    foreach (MeshFilter componentsInChild in (MeshFilter[]) ((Component) this).GetComponentsInChildren<MeshFilter>())
    {
      if (Outline.registeredMeshes.Add(componentsInChild.get_sharedMesh()))
      {
        int index = this.bakeKeys.IndexOf(componentsInChild.get_sharedMesh());
        List<Vector3> vector3List = index >= 0 ? this.bakeValues[index].data : this.SmoothNormals(componentsInChild.get_sharedMesh());
        componentsInChild.get_sharedMesh().SetUVs(3, vector3List);
      }
    }
    foreach (SkinnedMeshRenderer componentsInChild in (SkinnedMeshRenderer[]) ((Component) this).GetComponentsInChildren<SkinnedMeshRenderer>())
    {
      if (Outline.registeredMeshes.Add(componentsInChild.get_sharedMesh()))
        componentsInChild.get_sharedMesh().set_uv4(new Vector2[componentsInChild.get_sharedMesh().get_vertexCount()]);
    }
  }

  private List<Vector3> SmoothNormals(Mesh mesh)
  {
    IEnumerable<IGrouping<Vector3, KeyValuePair<Vector3, int>>> groupings = ((IEnumerable<Vector3>) mesh.get_vertices()).Select<Vector3, KeyValuePair<Vector3, int>>((Func<Vector3, int, KeyValuePair<Vector3, int>>) ((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index))).GroupBy<KeyValuePair<Vector3, int>, Vector3>((Func<KeyValuePair<Vector3, int>, Vector3>) (pair => pair.Key));
    List<Vector3> vector3List = new List<Vector3>((IEnumerable<Vector3>) mesh.get_normals());
    using (IEnumerator<IGrouping<Vector3, KeyValuePair<Vector3, int>>> enumerator1 = groupings.GetEnumerator())
    {
      while (((IEnumerator) enumerator1).MoveNext())
      {
        IGrouping<Vector3, KeyValuePair<Vector3, int>> current1 = enumerator1.Current;
        if (((IEnumerable<KeyValuePair<Vector3, int>>) current1).Count<KeyValuePair<Vector3, int>>() != 1)
        {
          Vector3 vector3 = Vector3.get_zero();
          using (IEnumerator<KeyValuePair<Vector3, int>> enumerator2 = ((IEnumerable<KeyValuePair<Vector3, int>>) current1).GetEnumerator())
          {
            while (((IEnumerator) enumerator2).MoveNext())
            {
              KeyValuePair<Vector3, int> current2 = enumerator2.Current;
              vector3 = Vector3.op_Addition(vector3, mesh.get_normals()[current2.Value]);
            }
          }
          ((Vector3) ref vector3).Normalize();
          using (IEnumerator<KeyValuePair<Vector3, int>> enumerator2 = ((IEnumerable<KeyValuePair<Vector3, int>>) current1).GetEnumerator())
          {
            while (((IEnumerator) enumerator2).MoveNext())
            {
              KeyValuePair<Vector3, int> current2 = enumerator2.Current;
              vector3List[current2.Value] = vector3;
            }
          }
        }
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

  public Outline() => base.\u002Ector();

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
