// Decompiled with JetBrains decompiler
// Type: FloatItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FloatItem : MonoBehaviour
{
  private LayerMask whatIsGround;
  private float floatHeight = 2f;
  private Vector3 desiredScale;
  private float yPos;
  private float yOffset;
  public float maxOffset = 0.5f;

  private void Start()
  {
    this.PositionItem();
    this.yPos = this.transform.position.y;
    this.desiredScale = this.transform.localScale;
    this.transform.localScale = Vector3.zero;
  }

  private void Update()
  {
    this.transform.localScale = Vector3.Lerp(this.transform.localScale, this.desiredScale, Time.deltaTime * 7f);
    this.transform.Rotate(Vector3.up, 20f * Time.deltaTime);
    this.yOffset = Mathf.Lerp(this.yOffset, Mathf.PingPong(Time.time * 0.5f, this.maxOffset) - this.maxOffset / 2f, Time.deltaTime * 2f);
    this.transform.position = new Vector3(this.transform.position.x, this.yPos + this.yOffset, this.transform.position.z);
  }

  private void PositionItem()
  {
    this.whatIsGround = (LayerMask) LayerMask.GetMask("Ground");
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.transform.position + Vector3.up * 20f, Vector3.down, out hitInfo, 50f, (int) this.whatIsGround))
      return;
    this.transform.position = hitInfo.point + Vector3.up * this.floatHeight;
  }
}
