// Decompiled with JetBrains decompiler
// Type: GroundRollAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GroundRollAttack : MonoBehaviour
{
  public Rigidbody rb;
  public float speed = 60f;
  public LayerMask whatIsGround;
  public Transform rollRock;
  public GameObject rollPrefab;
  private bool child;
  private Vector3 rollAxis;
  private float rollSpeed = 10f;

  private void Start()
  {
    Vector3 forward = this.transform.forward;
    forward.y = 0.0f;
    this.rb.velocity = forward * this.speed;
    this.rollAxis = Vector3.Cross(this.rb.velocity, Vector3.up);
    Debug.DrawLine(this.transform.position, this.transform.position + forward * 10f, Color.red, 10f);
    Debug.LogError((object) ("collider: " + (object) this.GetComponent<Collider>()));
    this.GetComponent<Collider>().enabled = true;
    if (this.child)
      return;
    int num1 = 2;
    int num2 = 25;
    Collider component = this.GetComponent<Collider>();
    for (int index = 0; index < num1; ++index)
    {
      Transform transform = Object.Instantiate<GameObject>(this.rollPrefab, this.transform.position, this.transform.rotation).transform;
      transform.GetComponent<GroundRollAttack>().child = true;
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (float) (num2 * 2 * index) - (float) num2, transform.eulerAngles.z);
      Physics.IgnoreCollision(component, transform.GetComponent<Collider>());
    }
  }

  private void Update()
  {
    this.KeepRockGrounded();
    this.SpinRock();
  }

  private void KeepRockGrounded()
  {
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.transform.position + Vector3.up * 50f, Vector3.down, out hitInfo, 100f, (int) this.whatIsGround))
      return;
    Vector3 position = this.rb.position;
    position.y = hitInfo.point.y;
    this.rb.MovePosition(position);
  }

  private void SpinRock() => this.rollRock.transform.Rotate(this.rollAxis * this.rollSpeed * Time.deltaTime);
}
