// Decompiled with JetBrains decompiler
// Type: FloatItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class FloatItem : MonoBehaviour
{
  private LayerMask whatIsGround;
  private float floatHeight;
  private Vector3 desiredScale;
  private float yPos;
  private float yOffset;
  public float maxOffset;

  private void Start()
  {
    this.PositionItem();
    this.yPos = (float) ((Component) this).get_transform().get_position().y;
    this.desiredScale = ((Component) this).get_transform().get_localScale();
    ((Component) this).get_transform().set_localScale(Vector3.get_zero());
  }

  private void Update()
  {
    ((Component) this).get_transform().set_localScale(Vector3.Lerp(((Component) this).get_transform().get_localScale(), this.desiredScale, Time.get_deltaTime() * 7f));
    ((Component) this).get_transform().Rotate(Vector3.get_up(), 20f * Time.get_deltaTime());
    this.yOffset = Mathf.Lerp(this.yOffset, Mathf.PingPong(Time.get_time() * 0.5f, this.maxOffset) - this.maxOffset / 2f, Time.get_deltaTime() * 2f);
    ((Component) this).get_transform().set_position(new Vector3((float) ((Component) this).get_transform().get_position().x, this.yPos + this.yOffset, (float) ((Component) this).get_transform().get_position().z));
  }

  private void PositionItem()
  {
    this.whatIsGround = LayerMask.op_Implicit(LayerMask.GetMask(new string[1]
    {
      "Ground"
    }));
    RaycastHit raycastHit;
    if (!Physics.Raycast(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 20f)), Vector3.get_down(), ref raycastHit, 50f, LayerMask.op_Implicit(this.whatIsGround)))
      return;
    ((Component) this).get_transform().set_position(Vector3.op_Addition(((RaycastHit) ref raycastHit).get_point(), Vector3.op_Multiply(Vector3.get_up(), this.floatHeight)));
  }

  public FloatItem() => base.\u002Ector();
}
