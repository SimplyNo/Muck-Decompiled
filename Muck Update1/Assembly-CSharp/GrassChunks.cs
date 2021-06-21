// Decompiled with JetBrains decompiler
// Type: GrassChunks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GrassChunks : MonoBehaviour
{
  public int nChunks = 25;
  public int chunkSize = 5;
  private int chunkLength;
  private float topLeftX;
  private float topLeftZ;
  public Dictionary<Vector3, GrassChunks.Chunk> chunks;
  [Header("Grass")]
  public int grassDensity;
  private Vector2 previousChunk;
  public Transform target;
  private int maxLOD = 20;

  private void Awake()
  {
    this.InitChunks();
    this.UpdateChunkCenters(Vector2.zero);
  }

  private void InitChunks()
  {
    this.chunkLength = Mathf.FloorToInt(Mathf.Sqrt((float) this.nChunks));
    this.topLeftX = (float) (this.chunkSize * this.chunkLength) / -2f;
    this.topLeftZ = (float) (this.chunkSize * this.chunkLength) / 2f;
    this.chunks = new Dictionary<Vector3, GrassChunks.Chunk>();
  }

  private void UpdateChunkCenters(Vector2 dir)
  {
    Dictionary<Vector3, GrassChunks.Chunk> dictionary = new Dictionary<Vector3, GrassChunks.Chunk>();
    float num1 = this.transform.position.x - this.transform.position.x % (float) this.chunkSize;
    float num2 = this.transform.position.z - this.transform.position.z % (float) this.chunkSize;
    if ((double) num1 < 0.0)
      num1 -= (float) this.chunkSize / 2f;
    if ((double) num1 > 0.0)
      num1 += (float) this.chunkSize / 2f;
    if ((double) num2 < 0.0)
      num2 -= (float) this.chunkSize / 2f;
    if ((double) num2 > 0.0)
      num2 += (float) this.chunkSize / 2f;
    float num3 = num1 + this.topLeftX;
    float num4 = num2 + this.topLeftZ;
    MonoBehaviour.print((object) "updaying chunk centers");
    for (int index = 0; index < this.nChunks; ++index)
    {
      int num5 = Mathf.FloorToInt((float) index / (float) this.chunkLength) * this.chunkSize;
      int num6 = index % this.chunkLength * this.chunkSize;
      Vector3 vector3 = new Vector3((float) ((double) num3 + (double) num6 + (double) this.chunkSize / 2.0), 0.0f, (float) ((double) num4 - (double) num5 - (double) this.chunkSize / 2.0));
      if (this.chunks.ContainsKey(vector3))
      {
        dictionary.Add(vector3, this.chunks[vector3]);
      }
      else
      {
        GrassChunks.Chunk chunk = new GrassChunks.Chunk(1, vector3);
        dictionary.Add(vector3, chunk);
      }
    }
    this.chunks = dictionary;
  }

  private void Update()
  {
    Vector2 playerChunk = this.FindPLayerChunk();
    if (!(playerChunk != Vector2.zero))
      return;
    playerChunk.x /= (float) this.chunkSize;
    playerChunk.y /= (float) this.chunkSize;
    MonoBehaviour.print((object) playerChunk);
    this.UpdateChunkCenters(playerChunk);
  }

  private void UpdateChunkLOD()
  {
    this.target = this.transform;
    int num = 0;
    while (num < this.nChunks)
      ++num;
  }

  public int FindLOD(float distanceFromChunk)
  {
    for (int index = 1; index < this.maxLOD + 1; ++index)
    {
      if ((double) distanceFromChunk < (double) this.chunkSize * 1.5 * (double) index)
        return index * index;
    }
    return this.maxLOD * this.maxLOD;
  }

  private Vector2 FindPLayerChunk()
  {
    float x = this.transform.position.x - this.transform.position.x % (float) this.chunkSize;
    float y = this.transform.position.z - this.transform.position.z % (float) this.chunkSize;
    if ((double) x < 0.0)
      x -= (float) this.chunkSize / 2f;
    if ((double) x > 0.0)
      x += (float) this.chunkSize / 2f;
    if ((double) y < 0.0)
      y -= (float) this.chunkSize / 2f;
    if ((double) y > 0.0)
      y += (float) this.chunkSize / 2f;
    Vector2 vector2_1 = new Vector2(x, y);
    if (!(this.previousChunk != vector2_1))
      return Vector2.zero;
    Vector2 vector2_2 = vector2_1 - this.previousChunk;
    this.previousChunk = vector2_1;
    return vector2_2;
  }

  private void OnDrawGizmos()
  {
    if (this.chunks == null)
      return;
    foreach (GrassChunks.Chunk chunk in this.chunks.Values)
      Gizmos.DrawWireCube(chunk.chunkCenter, Vector3.one * (float) this.chunkSize);
  }

  public class Chunk
  {
    public int lod;
    public Vector3 chunkCenter;

    public Chunk(int lod, Vector3 chunkCenter)
    {
      this.lod = lod;
      this.chunkCenter = chunkCenter;
    }
  }
}
