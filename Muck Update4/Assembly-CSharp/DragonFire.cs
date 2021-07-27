// Decompiled with JetBrains decompiler
// Type: DragonFire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DragonFire : MonoBehaviour
{
  private Collider c;
  public HitboxDamage hitbox;
  private float yHeight;

  private void Awake()
  {
    this.InvokeRepeating("UpdateHitbox", 0.1f, 0.1f);
    this.Invoke("StartHitbox", 1.35f);
    this.c = this.GetComponent<Collider>();
    this.c.enabled = false;
  }

  private void StartHitbox()
  {
    this.Invoke("StopHitbox", 1.5f);
    this.c.enabled = true;
  }

  private void StopHitbox() => this.c.enabled = false;

  private void UpdateHitbox() => this.hitbox.Reset();

  private void Update() => this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, this.transform.parent.rotation.eulerAngles.y, 0.0f));
}
