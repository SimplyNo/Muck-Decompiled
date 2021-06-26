// Decompiled with JetBrains decompiler
// Type: DrawChunks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class DrawChunks : MonoBehaviour
{
  public ResourceGenerator resourceGen;
  [Header("Chunks")]
  public int nChunks = 256;
  private int chunkLength;
  private int chunkSize;
  public float updateRate = 1f;
  [Header("Render distance")]
  [Range(0.0f, 2000f)]
  public float drawDistance;
  public float minRenderDistance;
  public static readonly float maxRenderDistance = 1500f;
  public int drawnTrees;
  public int totalTrees;
  [Header("LOD")]
  public int maxLOD = 10;
  private bool[] visibleChunks;
  private int[] chunkLOD;
  public List<GameObject>[] chunks;
  private float topLeftX;
  private float topLeftZ;
  public Transform player;
  private int a;
  private Vector3[] chunkCenters;

  private void Awake()
  {
    this.InvokeRepeating("UpdateChunks", 0.0f, this.updateRate);
    this.visibleChunks = new bool[this.nChunks];
    this.chunkLOD = new int[this.nChunks];
    this.chunkLength = Mathf.FloorToInt(Mathf.Sqrt((float) this.nChunks));
    this.chunkSize = MapGenerator.mapChunkSize / this.chunkLength;
    this.minRenderDistance = (float) ((double) Mathf.Sqrt((float) (this.chunkLength * this.chunkLength)) * (double) this.resourceGen.worldScale * 1.25);
    this.topLeftX = (float) (MapGenerator.mapChunkSize - 1) / -2f;
    this.topLeftZ = (float) (MapGenerator.mapChunkSize - 1) / 2f;
    this.InitChunkCenters();
  }

  public void InitChunks(List<GameObject>[] chunks) => this.chunks = chunks;

  public int FindChunk(int x, int y)
  {
    int num1 = Mathf.FloorToInt((float) x / (float) this.chunkSize);
    int num2 = Mathf.FloorToInt((float) (Mathf.FloorToInt((float) y / (float) this.chunkSize) * this.chunkLength));
    if (num2 > this.chunkLength * (this.chunkLength - 1))
      num2 = this.chunkLength * this.chunkLength - 1;
    if (num1 > this.chunkLength - 1)
      num1 = this.chunkLength - 1;
    return num1 + num2;
  }

  private void UpdateChunks()
  {
    if (!(bool) (Object) this.player)
    {
      if ((bool) (Object) PlayerMovement.Instance)
      {
        this.player = PlayerMovement.Instance.playerCam.transform;
      }
      else
      {
        if (!(bool) (Object) Camera.main)
          return;
        this.player = Camera.main.transform;
      }
    }
    if (this.chunks == null)
      return;
    for (int index = 0; index < this.chunks.Length; ++index)
    {
      float distanceFromChunk = this.DistanceFromChunk(index);
      if ((double) distanceFromChunk < (double) this.drawDistance)
      {
        int lod = this.FindLOD(distanceFromChunk);
        this.DrawChunk(index, true, lod);
      }
      else
        this.DrawChunk(index, false, 1);
    }
  }

  private void DrawChunk(int c, bool draw, int lod)
  {
    if (this.a >= this.chunks.Length || draw == this.visibleChunks[c] && this.chunkLOD[c] == lod)
      return;
    this.visibleChunks[c] = draw;
    this.chunkLOD[c] = lod;
    if (this.chunks[c].Count < 1)
      return;
    for (int index = 0; index < this.chunks[c].Count; ++index)
    {
      if (!((Object) this.chunks[c][index] == (Object) null))
      {
        if (index % lod == 0)
        {
          if (draw)
          {
            if (!this.chunks[c][index].activeInHierarchy)
              ++this.drawnTrees;
          }
          else if (this.chunks[c][index].activeInHierarchy)
            --this.drawnTrees;
          this.chunks[c][index].SetActive(draw);
        }
        else if (this.chunks[c][index].activeInHierarchy)
        {
          --this.drawnTrees;
          this.chunks[c][index].SetActive(false);
        }
      }
    }
  }

  private void InitChunkCenters()
  {
    this.chunkCenters = new Vector3[this.nChunks];
    for (int index = 0; index < this.nChunks; ++index)
    {
      int num1 = Mathf.FloorToInt((float) index / (float) this.chunkLength) * this.chunkSize;
      int num2 = index % this.chunkLength * this.chunkSize;
      this.chunkCenters[index] = new Vector3((float) ((double) this.topLeftX + (double) num2 + (double) this.chunkSize / 2.0), 0.0f, (float) ((double) this.topLeftZ - (double) num1 - (double) this.chunkSize / 2.0)) * this.resourceGen.worldScale;
    }
  }

  public float DistanceFromChunk(int chunk) => Vector3.Distance(this.player.position, this.chunkCenters[chunk]);

  public int FindLOD(float distanceFromChunk)
  {
    for (int index = 1; index < this.maxLOD + 1; ++index)
    {
      if ((double) distanceFromChunk < (double) this.minRenderDistance * (double) index)
        return index * index;
    }
    return this.maxLOD * this.maxLOD;
  }
}
