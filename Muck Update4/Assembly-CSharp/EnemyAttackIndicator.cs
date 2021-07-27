// Decompiled with JetBrains decompiler
// Type: EnemyAttackIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EnemyAttackIndicator : MonoBehaviour
{
  public Vector3 offset = new Vector3(0.0f, -0.25f, 0.0f);
  public LayerMask whatIsGround;
  public Projector projector;
  private Vector3 desiredScale;

  private void Awake()
  {
    RaycastHit hitInfo;
    if (Physics.Raycast(this.transform.position + Vector3.up * 10f, Vector3.down, out hitInfo, 50f, (int) this.whatIsGround))
      this.transform.position = hitInfo.point + this.offset * this.transform.localScale.x;
    this.desiredScale = this.transform.localScale;
    this.transform.localScale = Vector3.zero;
  }

  public void SetWarning(float time, float scale)
  {
    this.desiredScale = Vector3.one * scale;
    this.Invoke("DestroySelf", time);
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.transform.position + Vector3.up * 10f, Vector3.down, out hitInfo, 50f, (int) this.whatIsGround))
      return;
    this.transform.position = hitInfo.point + this.offset * this.transform.localScale.x;
  }

  private void Update()
  {
    this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.desiredScale, Time.deltaTime * 7f);
    this.projector.orthographicSize = this.transform.localScale.x / 2f;
    this.transform.Rotate(new Vector3(0.0f, 0.0f, 100f * Time.deltaTime), Space.Self);
  }

  private void DestroySelf() => Object.Destroy((Object) this.gameObject);
}
