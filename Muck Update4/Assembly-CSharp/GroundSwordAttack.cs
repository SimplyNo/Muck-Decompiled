// Decompiled with JetBrains decompiler
// Type: GroundSwordAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GroundSwordAttack : MonoBehaviour
{
  public Rigidbody rb;
  public float speed = 60f;
  public LayerMask whatIsGround;
  public Transform rollRock;
  public GameObject rollPrefab;
  public InventoryItem projectile;
  private bool child;
  private Vector3 rollAxis;
  private float rollSpeed = 10f;

  private void Start()
  {
    Debug.LogError((object) "Spawned");
    Vector3 forward = this.transform.forward;
    forward.y = 0.0f;
    this.rb.velocity = forward.normalized * this.projectile.bowComponent.projectileSpeed;
    this.rb.angularVelocity = Vector3.zero;
    this.rollAxis = Vector3.Cross(this.rb.velocity, Vector3.up);
    Debug.DrawLine(this.transform.position, this.transform.position + forward * 10f, Color.red, 10f);
    this.GetComponent<Collider>().enabled = true;
    if (this.child)
      return;
    int num = 25;
    Collider component = this.GetComponent<Collider>();
    for (int index = 0; index < 2; ++index)
    {
      Transform transform = Object.Instantiate<GameObject>(this.rollPrefab, this.transform.position, this.transform.rotation).transform;
      transform.GetComponent<GroundSwordAttack>().child = true;
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (float) (num * 2 * index) - (float) num, transform.eulerAngles.z);
      Physics.IgnoreCollision(component, transform.GetComponent<Collider>());
    }
    for (int index = 0; index < 2; ++index)
    {
      Transform transform = Object.Instantiate<GameObject>(this.rollPrefab, this.transform.position, this.transform.rotation).transform;
      transform.GetComponent<GroundSwordAttack>().child = true;
      transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (float) (num * 4 * index) - (float) (num * 2), transform.eulerAngles.z);
      Physics.IgnoreCollision(component, transform.GetComponent<Collider>());
    }
  }

  private void Update()
  {
    this.rb.velocity = this.transform.forward * this.projectile.bowComponent.projectileSpeed;
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

  private void SpinRock()
  {
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.transform.position + Vector3.up * 2f, Vector3.down, out hitInfo, 4f, (int) this.whatIsGround))
      return;
    float x = Vector3.SignedAngle(Vector3.up, hitInfo.normal, this.transform.right);
    Vector3 eulerAngles = this.rollRock.transform.rotation.eulerAngles;
    this.rollRock.transform.rotation = Quaternion.Lerp(this.rollRock.rotation, Quaternion.Euler(new Vector3(x, eulerAngles.y, eulerAngles.z)), Time.deltaTime * 15f);
  }
}
