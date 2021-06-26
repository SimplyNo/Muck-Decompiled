// Decompiled with JetBrains decompiler
// Type: Grass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
  public float length;
  public int thicc = 10;
  public GameObject grass;
  private GameObject[,] grassPool;
  private Dictionary<(float, float), int> grassPositions;
  public LayerMask whatIsGround;
  private float updateRate = 0.02f;

  private void Awake()
  {
    this.grassPool = new GameObject[this.thicc, this.thicc];
    this.grassPositions = new Dictionary<(float, float), int>();
    for (int index1 = 0; index1 < this.thicc; ++index1)
    {
      for (int index2 = 0; index2 < this.thicc; ++index2)
        this.grassPool[index1, index2] = Object.Instantiate<GameObject>(this.grass);
    }
    this.InvokeRepeating("MakeGrass", 0.0f, this.updateRate);
  }

  private void MakeGrass()
  {
    float num1 = this.length / (float) this.thicc;
    for (int index1 = 0; index1 < this.thicc; ++index1)
    {
      for (int index2 = 0; index2 < this.thicc; ++index2)
      {
        double num2 = (double) this.transform.position.x - (double) this.transform.position.x % (double) num1;
        float num3 = this.transform.position.z - this.transform.position.z % num1;
        double num4 = (double) (index1 - this.thicc / 2) / (double) this.thicc * (double) this.length;
        Vector3 vector3 = new Vector3((float) (num2 + num4), this.transform.position.y + 50f, num3 + (float) (index2 - this.thicc / 2) / (float) this.thicc * this.length);
        RaycastHit hitInfo;
        if (Physics.Raycast(vector3, Vector3.down, out hitInfo, 60f, (int) this.whatIsGround))
        {
          Debug.DrawLine(vector3, hitInfo.point, Color.blue);
          vector3.y = hitInfo.point.y;
          Quaternion quaternion = Quaternion.LookRotation(Vector3.Cross(Vector3.up, hitInfo.normal));
          Transform transform = this.grassPool[index1, index2].transform;
          transform.gameObject.SetActive(true);
          transform.position = vector3;
          transform.rotation = quaternion;
          transform.localScale = Vector3.one * Random.Range(0.85f, 1.4f);
        }
        else
          this.grassPool[index1, index2].SetActive(false);
      }
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.green;
    Gizmos.DrawWireCube(this.transform.position, Vector3.one * this.length);
  }
}
