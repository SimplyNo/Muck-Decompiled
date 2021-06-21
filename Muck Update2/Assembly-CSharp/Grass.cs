// Decompiled with JetBrains decompiler
// Type: Grass
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
  public float length;
  public int thicc;
  public GameObject grass;
  private GameObject[,] grassPool;
  private Dictionary<(float, float), int> grassPositions;
  public LayerMask whatIsGround;
  private float updateRate;

  private void Awake()
  {
    this.grassPool = new GameObject[this.thicc, this.thicc];
    this.grassPositions = new Dictionary<(float, float), int>();
    for (int index1 = 0; index1 < this.thicc; ++index1)
    {
      for (int index2 = 0; index2 < this.thicc; ++index2)
        this.grassPool[index1, index2] = (GameObject) Object.Instantiate<GameObject>((M0) this.grass);
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
        double num2 = ((Component) this).get_transform().get_position().x - ((Component) this).get_transform().get_position().x % (double) num1;
        float num3 = (float) (((Component) this).get_transform().get_position().z - ((Component) this).get_transform().get_position().z % (double) num1);
        double num4 = (double) (index1 - this.thicc / 2) / (double) this.thicc * (double) this.length;
        float num5 = (float) (num2 + num4);
        float num6 = num3 + (float) (index2 - this.thicc / 2) / (float) this.thicc * this.length;
        Vector3 vector3;
        ((Vector3) ref vector3).\u002Ector(num5, (float) (((Component) this).get_transform().get_position().y + 50.0), num6);
        RaycastHit raycastHit;
        if (Physics.Raycast(vector3, Vector3.get_down(), ref raycastHit, 60f, LayerMask.op_Implicit(this.whatIsGround)))
        {
          Debug.DrawLine(vector3, ((RaycastHit) ref raycastHit).get_point(), Color.get_blue());
          vector3.y = ((RaycastHit) ref raycastHit).get_point().y;
          Quaternion quaternion = Quaternion.LookRotation(Vector3.Cross(Vector3.get_up(), ((RaycastHit) ref raycastHit).get_normal()));
          Transform transform = this.grassPool[index1, index2].get_transform();
          ((Component) transform).get_gameObject().SetActive(true);
          transform.set_position(vector3);
          transform.set_rotation(quaternion);
          transform.set_localScale(Vector3.op_Multiply(Vector3.get_one(), Random.Range(0.85f, 1.4f)));
        }
        else
          this.grassPool[index1, index2].SetActive(false);
      }
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.set_color(Color.get_green());
    Gizmos.DrawWireCube(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_one(), this.length));
  }

  public Grass() => base.\u002Ector();
}
