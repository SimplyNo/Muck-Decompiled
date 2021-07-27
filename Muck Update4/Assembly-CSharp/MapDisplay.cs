// Decompiled with JetBrains decompiler
// Type: MapDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MapDisplay : MonoBehaviour
{
  public Renderer textureRender;
  public MeshFilter meshFilter;
  public MeshRenderer meshRenderer;
  public MeshCollider meshCollider;

  public void DrawTexture(Texture2D texture)
  {
    this.textureRender.sharedMaterial.mainTexture = (Texture) texture;
    this.textureRender.transform.localScale = new Vector3((float) texture.width, 1f, (float) texture.height);
  }

  public void DrawMesh(MeshData meshData)
  {
    this.meshFilter.sharedMesh = meshData.CreateMesh();
    this.meshCollider.sharedMesh = this.meshFilter.sharedMesh;
  }
}
