// Decompiled with JetBrains decompiler
// Type: TestMoveText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    this.children = this.GetComponentsInChildren<Transform>();
    this.startHeight = new Vector3(1f, this.transform.GetChild(0).position.y, 1f);
    foreach (Transform child in this.children)
    {
      child.transform.position += Vector3.down * 50f;
      this.notSurfacing.Add(child.GetSiblingIndex());
    }
    float repeatRate = 0.08f;
    float time = 2f;
    this.InvokeRepeating("SlowUpdate", time, repeatRate);
    this.InvokeRepeating("AddMaterial", (float) ((double) repeatRate * (double) this.transform.childCount + (double) time + 2.0), 0.05f);
  }

  private void AddMaterial()
  {
    ++this.a;
    if (this.a > 50)
      this.CancelInvoke();
    for (int index = 0; index < 100; ++index)
    {
      GameObject tree = this.trees[Random.Range(0, this.trees.Length)];
      Vector3 vector3 = this.transform.position + new Vector3(Random.Range((float) (-(double) this.drawArea.x / 2.0), this.drawArea.x / 2f), 80f, Random.Range((float) (-(double) this.drawArea.z / 2.0), this.drawArea.z / 2f));
      Debug.DrawLine(vector3, vector3 + Vector3.down * 120f, Color.red, 10f);
      Debug.DrawLine(Vector3.zero, vector3 * 50f, Color.black, 10f);
      RaycastHit hitInfo;
      if (Physics.Raycast(vector3, Vector3.down, out hitInfo, 120f, (int) this.whatIsGround) && (double) Vector3.Angle(hitInfo.normal, Vector3.up) <= 5.0)
      {
        Transform transform = Object.Instantiate<GameObject>(tree, hitInfo.point, tree.transform.rotation).transform;
        HitableResource component = transform.GetComponent<HitableResource>();
        if ((bool) (Object) component)
        {
          float num = 1f;
          if (transform.CompareTag("Count"))
            num = 3f;
          component.SetDefaultScale(Vector3.one * 0.2f * num);
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
    int num = 0;
    foreach (int index in this.toSurface)
    {
      ++num;
      Transform child = this.transform.GetChild(index);
      child.position = Vector3.Lerp(child.position, new Vector3(child.position.x, this.startHeight.y, child.position.z), Time.deltaTime * 5f);
    }
  }
}
