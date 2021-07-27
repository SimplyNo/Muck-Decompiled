// Decompiled with JetBrains decompiler
// Type: GenerateBoat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GenerateBoat : MonoBehaviour
{
  public int mapWidth;
  public int mapHeight;
  public float worldScale;
  public LayerMask whatIsWater;
  public LayerMask whatIsLand;
  private Vector3 randomPos;
  private ConsistentRandom randomGen;
  public GameObject boatPrefab;
  private float waterHeight;
  public int seed;

  private void Awake()
  {
    this.mapWidth = MapGenerator.mapChunkSize;
    this.mapHeight = MapGenerator.mapChunkSize;
    this.worldScale = (float) MapGenerator.worldScale;
    this.randomGen = new ConsistentRandom(GameManager.GetSeed() + ResourceManager.GetNextGenOffset());
    int num = 0;
    while (this.randomPos == Vector3.zero)
    {
      this.randomPos = this.FindRandomPointAroundWorld();
      ++num;
      if (num > 10000)
        break;
    }
    Object.Instantiate<GameObject>(this.boatPrefab, this.randomPos, this.boatPrefab.transform.rotation).GetComponent<Boat>().waterHeight = this.waterHeight;
  }

  private Vector3 FindRandomPointAroundWorld()
  {
    double num1 = this.randomGen.NextDouble() - 0.5;
    float num2 = (float) (this.randomGen.NextDouble() - 0.5);
    this.waterHeight = 6f;
    double num3 = (double) num2;
    Vector3 origin = new Vector3((float) num1, 0.0f, (float) num3).normalized * this.worldScale * ((float) this.mapWidth / 2f);
    RaycastHit hitInfo1;
    if (!Physics.Raycast(new Vector3(origin.x, 200f, origin.z), Vector3.down, out hitInfo1, 1000f, (int) this.whatIsWater))
      return Vector3.zero;
    this.waterHeight = hitInfo1.point.y;
    origin.y = this.waterHeight;
    Vector3 normalized = VectorExtensions.XZVector(Vector3.zero - origin).normalized;
    origin += Vector3.up;
    RaycastHit hitInfo2;
    return Physics.Raycast(origin, normalized, out hitInfo2, 5000f, (int) this.whatIsLand) ? hitInfo2.point : Vector3.zero;
  }

  private void OnDrawGizmos()
  {
  }
}
