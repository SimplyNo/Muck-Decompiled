// Decompiled with JetBrains decompiler
// Type: TestShieldHp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class TestShieldHp : MonoBehaviour
{
  public int maxShield;
  public int maxHp;
  public int hp;
  public int shield;
  private int total;
  private float maxHpScale;
  private float maxShieldScale;
  private float hpRatio;
  private float shieldRatio;
  public RectTransform hpBar;
  public RectTransform shieldBar;

  private void Awake() => this.UpdateBar();

  private void UpdateBar()
  {
    this.total = this.maxShield + this.maxHp;
    this.maxHpScale = (float) this.maxHp / (float) this.total;
    this.maxShieldScale = (float) this.maxShield / (float) this.total;
    this.hpRatio = this.maxHp != 0 ? (float) this.hp / (float) this.maxHp : 0.0f;
    this.shieldRatio = this.maxShield != 0 ? (float) this.shield / (float) this.maxShield : 0.0f;
    ((Component) this.hpBar).get_transform().set_localScale(new Vector3(this.maxHpScale * this.hpRatio, 1f, 1f));
    ((Component) this.shieldBar).get_transform().set_localScale(new Vector3(this.maxShieldScale * this.shieldRatio, 1f, 1f));
  }

  private void Update() => this.UpdateBar();

  public TestShieldHp() => base.\u002Ector();
}
