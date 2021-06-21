// Decompiled with JetBrains decompiler
// Type: GrassChunks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GrassChunks : MonoBehaviour
{
  public int nChunks;
  public int chunkSize;
  private int chunkLength;
  private float topLeftX;
  private float topLeftZ;
  public Dictionary<Vector3, GrassChunks.Chunk> chunks;
  [Header("Grass")]
  public int grassDensity;
  private Vector2 previousChunk;
  public Transform target;
  private int maxLOD;

  private void Awake()
  {
    this.InitChunks();
    this.UpdateChunkCenters(Vector2.get_zero());
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
    float num1 = (float) (((Component) this).get_transform().get_position().x - ((Component) this).get_transform().get_position().x % (double) this.chunkSize);
    float num2 = (float) (((Component) this).get_transform().get_position().z - ((Component) this).get_transform().get_position().z % (double) this.chunkSize);
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
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector((float) ((double) num3 + (double) num6 + (double) this.chunkSize / 2.0), 0.0f, (float) ((double) num4 - (double) num5 - (double) this.chunkSize / 2.0));
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
    if (!Vector2.op_Inequality(playerChunk, Vector2.get_zero()))
      return;
    ref __Null local1 = ref playerChunk.x;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    ^(float&) ref local1 = ^(float&) ref local1 / (float) this.chunkSize;
    ref __Null local2 = ref playerChunk.y;
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    ^(float&) ref local2 = ^(float&) ref local2 / (float) this.chunkSize;
    MonoBehaviour.print((object) playerChunk);
    this.UpdateChunkCenters(playerChunk);
  }

  private void UpdateChunkLOD()
  {
    this.target = ((Component) this).get_transform();
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
    float num1 = (float) (((Component) this).get_transform().get_position().x - ((Component) this).get_transform().get_position().x % (double) this.chunkSize);
    float num2 = (float) (((Component) this).get_transform().get_position().z - ((Component) this).get_transform().get_position().z % (double) this.chunkSize);
    if ((double) num1 < 0.0)
      num1 -= (float) this.chunkSize / 2f;
    if ((double) num1 > 0.0)
      num1 += (float) this.chunkSize / 2f;
    if ((double) num2 < 0.0)
      num2 -= (float) this.chunkSize / 2f;
    if ((double) num2 > 0.0)
      num2 += (float) this.chunkSize / 2f;
    Vector2 vector2_1;
    ((Vector2) ref vector2_1).\u002Ector(num1, num2);
    if (!Vector2.op_Inequality(this.previousChunk, vector2_1))
      return Vector2.get_zero();
    Vector2 vector2_2 = Vector2.op_Subtraction(vector2_1, this.previousChunk);
    this.previousChunk = vector2_1;
    return vector2_2;
  }

  private void OnDrawGizmos()
  {
    if (this.chunks == null)
      return;
    using (Dictionary<Vector3, GrassChunks.Chunk>.ValueCollection.Enumerator enumerator = this.chunks.Values.GetEnumerator())
    {
      while (enumerator.MoveNext())
        Gizmos.DrawWireCube(enumerator.Current.chunkCenter, Vector3.op_Multiply(Vector3.get_one(), (float) this.chunkSize));
    }
  }

  public GrassChunks() => base.\u002Ector();

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
