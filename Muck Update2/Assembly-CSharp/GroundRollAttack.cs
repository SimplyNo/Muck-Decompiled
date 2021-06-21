// Decompiled with JetBrains decompiler
// Type: GroundRollAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class GroundRollAttack : MonoBehaviour
{
  public Rigidbody rb;
  public float speed;
  public LayerMask whatIsGround;
  public Transform rollRock;
  public GameObject rollPrefab;
  private bool child;
  private Vector3 rollAxis;
  private float rollSpeed;

  private void Start()
  {
    Vector3 forward = ((Component) this).get_transform().get_forward();
    forward.y = (__Null) 0.0;
    this.rb.set_velocity(Vector3.op_Multiply(forward, this.speed));
    this.rollAxis = Vector3.Cross(this.rb.get_velocity(), Vector3.get_up());
    Debug.DrawLine(((Component) this).get_transform().get_position(), Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(forward, 10f)), Color.get_red(), 10f);
    Debug.LogError((object) ("collider: " + (object) ((Component) this).GetComponent<Collider>()));
    ((Collider) ((Component) this).GetComponent<Collider>()).set_enabled(true);
    if (this.child)
      return;
    int num1 = 2;
    int num2 = 25;
    Collider component = (Collider) ((Component) this).GetComponent<Collider>();
    for (int index = 0; index < num1; ++index)
    {
      Transform transform = ((GameObject) Object.Instantiate<GameObject>((M0) this.rollPrefab, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation())).get_transform();
      ((GroundRollAttack) ((Component) transform).GetComponent<GroundRollAttack>()).child = true;
      transform.set_eulerAngles(new Vector3((float) transform.get_eulerAngles().x, (float) transform.get_eulerAngles().y + (float) (num2 * 2 * index) - (float) num2, (float) transform.get_eulerAngles().z));
      Physics.IgnoreCollision(component, (Collider) ((Component) transform).GetComponent<Collider>());
    }
  }

  private void Update()
  {
    this.KeepRockGrounded();
    this.SpinRock();
  }

  private void KeepRockGrounded()
  {
    RaycastHit raycastHit;
    if (!Physics.Raycast(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 50f)), Vector3.get_down(), ref raycastHit, 100f, LayerMask.op_Implicit(this.whatIsGround)))
      return;
    Vector3 position = this.rb.get_position();
    position.y = ((RaycastHit) ref raycastHit).get_point().y;
    this.rb.MovePosition(position);
  }

  private void SpinRock() => ((Component) this.rollRock).get_transform().Rotate(Vector3.op_Multiply(Vector3.op_Multiply(this.rollAxis, this.rollSpeed), Time.get_deltaTime()));

  public GroundRollAttack() => base.\u002Ector();
}
