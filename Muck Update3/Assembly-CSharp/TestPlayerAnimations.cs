// Decompiled with JetBrains decompiler
// Type: TestPlayerAnimations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.AI;

public class TestPlayerAnimations : MonoBehaviour
{
  public Animator animator;
  public Rigidbody rb;
  public Vector3 desiredPos;
  public float orientationY;
  public float orientationX;
  private float blendX;
  private float blendY;
  public bool grounded;
  public bool dashing;
  public LayerMask whatIsGround;
  public GameObject jumpSfx;
  public GameObject dashFx;
  private float moveSpeed = 15f;
  private float rotationSpeed = 13f;
  private float animationBlendSpeed = 8f;
  public GameObject weapon;
  private MeshFilter filter;
  private MeshRenderer renderer;
  public Transform hpBar;
  public Transform upperBody;
  public NavMeshAgent agent;
  private float fallSpeed;

  public float hpRatio { get; set; } = 1f;

  private void Start()
  {
    this.grounded = true;
    this.InvokeRepeating("FindRandomPosition", 1f, 5f);
    this.filter = this.weapon.GetComponent<MeshFilter>();
    this.renderer = this.weapon.GetComponent<MeshRenderer>();
  }

  private void FindRandomPosition()
  {
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.transform.position + new Vector3(Random.Range(-20f, 20f), 20f, Random.Range(-20f, 20f)), Vector3.down, out hitInfo, 70f, (int) this.whatIsGround))
      return;
    this.agent.destination = hitInfo.point;
    this.agent.isStopped = false;
  }

  private void FixedUpdate() => this.animator.SetFloat("Speed", this.agent.speed);

  private void Update()
  {
    this.grounded = Physics.Raycast(this.transform.position, Vector3.down, 2.4f, (int) this.whatIsGround);
    this.animator.SetFloat("FallSpeed", this.fallSpeed);
    this.Animate();
    this.Sfx();
    this.orientationX = -60f;
    this.upperBody.localRotation = Quaternion.Lerp(this.upperBody.localRotation, Quaternion.Euler(this.orientationX, this.upperBody.localRotation.y, this.upperBody.localRotation.z), Time.deltaTime * this.rotationSpeed);
  }

  private void LateUpdate()
  {
    this.fallSpeed = this.rb.velocity.y;
    MonoBehaviour.print((object) ("fallspeed: " + (object) this.fallSpeed));
  }

  public void AttackAnimation() => this.animator.Play("Attack");

  public void UpdateWeapon(int objectID)
  {
    if (objectID == -1)
    {
      this.filter.mesh = (Mesh) null;
    }
    else
    {
      InventoryItem allItem = ItemManager.Instance.allItems[objectID];
      this.filter.mesh = allItem.mesh;
      this.renderer.material = allItem.material;
      this.animator.SetFloat("AnimationSpeed", allItem.attackSpeed);
    }
  }

  private void Sfx()
  {
    double player = (double) this.DistToPlayer();
  }

  private void Animate() => this.animator.SetBool("Grounded", this.grounded);

  private float DistToPlayer() => 1f;
}
