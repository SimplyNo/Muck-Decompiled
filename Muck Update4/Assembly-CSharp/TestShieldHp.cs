// Decompiled with JetBrains decompiler
// Type: TestShieldHp
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    this.hpBar.transform.localScale = new Vector3(this.maxHpScale * this.hpRatio, 1f, 1f);
    this.shieldBar.transform.localScale = new Vector3(this.maxShieldScale * this.shieldRatio, 1f, 1f);
  }

  private void Update() => this.UpdateBar();
}
