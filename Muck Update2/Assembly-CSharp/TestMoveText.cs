// Decompiled with JetBrains decompiler
// Type: TestMoveText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TestMoveText : MonoBehaviour
{
  private List<int> toSurface;
  private List<int> notSurfacing;
  private Transform[] children;
  private Vector3 startHeight;
  public Material[] mats;
  public GameObject[] trees;
  private int a;
  public LayerMask whatIsGround;
  public Vector3 drawArea;

  private void Awake()
  {
    this.toSurface = new List<int>();
    this.notSurfacing = new List<int>();
    this.children = (Transform[]) ((Component) this).GetComponentsInChildren<Transform>();
    this.startHeight = new Vector3(1f, (float) ((Component) this).get_transform().GetChild(0).get_position().y, 1f);
    foreach (Transform child in this.children)
    {
      Transform transform = ((Component) child).get_transform();
      transform.set_position(Vector3.op_Addition(transform.get_position(), Vector3.op_Multiply(Vector3.get_down(), 50f)));
      this.notSurfacing.Add(child.GetSiblingIndex());
    }
    float num1 = 0.08f;
    float num2 = 2f;
    this.InvokeRepeating("SlowUpdate", num2, num1);
    this.InvokeRepeating("AddMaterial", (float) ((double) num1 * (double) ((Component) this).get_transform().get_childCount() + (double) num2 + 2.0), 0.05f);
  }

  private void AddMaterial()
  {
    ++this.a;
    if (this.a > 50)
      this.CancelInvoke();
    for (int index = 0; index < 100; ++index)
    {
      GameObject tree = this.trees[Random.Range(0, this.trees.Length)];
      Vector3 vector3_1;
      ((Vector3) ref vector3_1).\u002Ector(Random.Range((float) (-this.drawArea.x / 2.0), (float) (this.drawArea.x / 2.0)), 80f, Random.Range((float) (-this.drawArea.z / 2.0), (float) (this.drawArea.z / 2.0)));
      Vector3 vector3_2 = Vector3.op_Addition(((Component) this).get_transform().get_position(), vector3_1);
      Debug.DrawLine(vector3_2, Vector3.op_Addition(vector3_2, Vector3.op_Multiply(Vector3.get_down(), 120f)), Color.get_red(), 10f);
      Debug.DrawLine(Vector3.get_zero(), Vector3.op_Multiply(vector3_2, 50f), Color.get_black(), 10f);
      RaycastHit raycastHit;
      if (Physics.Raycast(vector3_2, Vector3.get_down(), ref raycastHit, 120f, LayerMask.op_Implicit(this.whatIsGround)) && (double) Vector3.Angle(((RaycastHit) ref raycastHit).get_normal(), Vector3.get_up()) <= 5.0)
      {
        Transform transform = ((GameObject) Object.Instantiate<GameObject>((M0) tree, ((RaycastHit) ref raycastHit).get_point(), tree.get_transform().get_rotation())).get_transform();
        HitableResource component = (HitableResource) ((Component) transform).GetComponent<HitableResource>();
        if (Object.op_Implicit((Object) component))
        {
          float num = 1f;
          if (((Component) transform).CompareTag("Count"))
            num = 3f;
          component.SetDefaultScale(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_one(), 0.2f), num));
          component.PopIn();
        }
      }
    }
  }

  private void SlowUpdate()
  {
    int num = this.notSurfacing[Random.Range(0, this.notSurfacing.Count)];
    this.toSurface.Add(num);
    this.notSurfacing.Remove(num);
    if (this.notSurfacing.Count > 0)
      return;
    this.CancelInvoke(nameof (SlowUpdate));
  }

  private void Update()
  {
    int num1 = 0;
    foreach (int num2 in this.toSurface)
    {
      ++num1;
      Transform child = ((Component) this).get_transform().GetChild(num2);
      child.set_position(Vector3.Lerp(child.get_position(), new Vector3((float) child.get_position().x, (float) this.startHeight.y, (float) child.get_position().z), Time.get_deltaTime() * 5f));
    }
  }

  public TestMoveText() => base.\u002Ector();
}
