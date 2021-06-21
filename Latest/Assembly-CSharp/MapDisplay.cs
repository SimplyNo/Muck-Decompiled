// Decompiled with JetBrains decompiler
// Type: MapDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class MapDisplay : MonoBehaviour
{
  public Renderer textureRender;
  public MeshFilter meshFilter;
  public MeshRenderer meshRenderer;
  public MeshCollider meshCollider;

  public void DrawTexture(Texture2D texture)
  {
    this.textureRender.get_sharedMaterial().set_mainTexture((Texture) texture);
    ((Component) this.textureRender).get_transform().set_localScale(new Vector3((float) ((Texture) texture).get_width(), 1f, (float) ((Texture) texture).get_height()));
  }

  public void DrawMesh(MeshData meshData)
  {
    this.meshFilter.set_sharedMesh(meshData.CreateMesh());
    this.meshCollider.set_sharedMesh(this.meshFilter.get_sharedMesh());
  }

  public MapDisplay() => base.\u002Ector();
}
