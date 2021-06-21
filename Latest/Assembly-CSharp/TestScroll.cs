// Decompiled with JetBrains decompiler
// Type: TestScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class TestScroll : MonoBehaviour
{
  public NoiseData noise;
  public TerrainData terrain;
  public bool ready;
  public static TestScroll Instance;

  private void Awake()
  {
    TestScroll.Instance = this;
    this.Invoke("GetReady", 4f);
  }

  private void GetReady() => this.ready = true;

  private void Update()
  {
    if (!this.ready || (double) this.terrain.heightMultiplier > 300.0)
      return;
    this.terrain.heightMultiplier += 20f * Time.get_deltaTime();
  }

  public TestScroll() => base.\u002Ector();
}
