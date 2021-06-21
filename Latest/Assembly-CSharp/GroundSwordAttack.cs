// Decompiled with JetBrains decompiler
// Type: GroundSwordAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class GroundSwordAttack : MonoBehaviour
{
  public Rigidbody rb;
  public float speed;
  public LayerMask whatIsGround;
  public Transform rollRock;
  public GameObject rollPrefab;
  public InventoryItem projectile;
  private bool child;
  private Vector3 rollAxis;
  private float rollSpeed;

  private void Start()
  {
    Debug.LogError((object) "Spawned");
    Vector3 forward = ((Component) this).get_transform().get_forward();
    forward.y = (__Null) 0.0;
    this.rb.set_velocity(Vector3.op_Multiply(((Vector3) ref forward).get_normalized(), this.projectile.bowComponent.projectileSpeed));
    this.rb.set_angularVelocity(Vector3.get_zero());
    this.rollAxis = Vector3.Cross(this.rb.get_velocity(), Vector3.get_up());
    Debug.DrawLine(((Component) this).get_transform().get_position(), Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(forward, 10f)), Color.get_red(), 10f);
    ((Collider) ((Component) this).GetComponent<Collider>()).set_enabled(true);
    if (this.child)
      return;
    int num = 25;
    Collider component = (Collider) ((Component) this).GetComponent<Collider>();
    for (int index = 0; index < 2; ++index)
    {
      Transform transform = ((GameObject) Object.Instantiate<GameObject>((M0) this.rollPrefab, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation())).get_transform();
      ((GroundSwordAttack) ((Component) transform).GetComponent<GroundSwordAttack>()).child = true;
      transform.set_eulerAngles(new Vector3((float) transform.get_eulerAngles().x, (float) transform.get_eulerAngles().y + (float) (num * 2 * index) - (float) num, (float) transform.get_eulerAngles().z));
      Physics.IgnoreCollision(component, (Collider) ((Component) transform).GetComponent<Collider>());
    }
    for (int index = 0; index < 2; ++index)
    {
      Transform transform = ((GameObject) Object.Instantiate<GameObject>((M0) this.rollPrefab, ((Component) this).get_transform().get_position(), ((Component) this).get_transform().get_rotation())).get_transform();
      ((GroundSwordAttack) ((Component) transform).GetComponent<GroundSwordAttack>()).child = true;
      transform.set_eulerAngles(new Vector3((float) transform.get_eulerAngles().x, (float) transform.get_eulerAngles().y + (float) (num * 4 * index) - (float) (num * 2), (float) transform.get_eulerAngles().z));
      Physics.IgnoreCollision(component, (Collider) ((Component) transform).GetComponent<Collider>());
    }
  }

  private void Update()
  {
    this.rb.set_velocity(Vector3.op_Multiply(((Component) this).get_transform().get_forward(), this.projectile.bowComponent.projectileSpeed));
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

  private void SpinRock()
  {
    RaycastHit raycastHit;
    if (!Physics.Raycast(Vector3.op_Addition(((Component) this).get_transform().get_position(), Vector3.op_Multiply(Vector3.get_up(), 2f)), Vector3.get_down(), ref raycastHit, 4f, LayerMask.op_Implicit(this.whatIsGround)))
      return;
    float num = Vector3.SignedAngle(Vector3.get_up(), ((RaycastHit) ref raycastHit).get_normal(), ((Component) this).get_transform().get_right());
    Quaternion rotation = ((Component) this.rollRock).get_transform().get_rotation();
    Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
    ((Component) this.rollRock).get_transform().set_rotation(Quaternion.Lerp(this.rollRock.get_rotation(), Quaternion.Euler(new Vector3(num, (float) eulerAngles.y, (float) eulerAngles.z)), Time.get_deltaTime() * 15f));
  }

  public GroundSwordAttack() => base.\u002Ector();
}
