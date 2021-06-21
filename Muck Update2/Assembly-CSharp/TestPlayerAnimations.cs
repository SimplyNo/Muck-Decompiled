// Decompiled with JetBrains decompiler
// Type: TestPlayerAnimations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
  private float moveSpeed;
  private float rotationSpeed;
  private float animationBlendSpeed;
  public GameObject weapon;
  private MeshFilter filter;
  private MeshRenderer renderer;
  public Transform hpBar;
  public Transform upperBody;
  public NavMeshAgent agent;
  private float fallSpeed;

  public float hpRatio { get; set; }

  private void Start()
  {
    this.grounded = true;
    this.InvokeRepeating("FindRandomPosition", 1f, 5f);
    this.filter = (MeshFilter) this.weapon.GetComponent<MeshFilter>();
    this.renderer = (MeshRenderer) this.weapon.GetComponent<MeshRenderer>();
  }

  private void FindRandomPosition()
  {
    Vector3 vector3;
    ((Vector3) ref vector3).\u002Ector(Random.Range(-20f, 20f), 20f, Random.Range(-20f, 20f));
    RaycastHit raycastHit;
    if (!Physics.Raycast(Vector3.op_Addition(((Component) this).get_transform().get_position(), vector3), Vector3.get_down(), ref raycastHit, 70f, LayerMask.op_Implicit(this.whatIsGround)))
      return;
    this.agent.set_destination(((RaycastHit) ref raycastHit).get_point());
    this.agent.set_isStopped(false);
  }

  private void FixedUpdate() => this.animator.SetFloat("Speed", this.agent.get_speed());

  private void Update()
  {
    this.grounded = Physics.Raycast(((Component) this).get_transform().get_position(), Vector3.get_down(), 2.4f, LayerMask.op_Implicit(this.whatIsGround));
    this.animator.SetFloat("FallSpeed", this.fallSpeed);
    this.Animate();
    this.Sfx();
    this.orientationX = -60f;
    this.upperBody.set_localRotation(Quaternion.Lerp(this.upperBody.get_localRotation(), Quaternion.Euler(this.orientationX, (float) this.upperBody.get_localRotation().y, (float) this.upperBody.get_localRotation().z), Time.get_deltaTime() * this.rotationSpeed));
  }

  private void LateUpdate()
  {
    this.fallSpeed = (float) this.rb.get_velocity().y;
    MonoBehaviour.print((object) ("fallspeed: " + (object) this.fallSpeed));
  }

  public void AttackAnimation() => this.animator.Play("Attack");

  public void UpdateWeapon(int objectID)
  {
    if (objectID == -1)
    {
      this.filter.set_mesh((Mesh) null);
    }
    else
    {
      InventoryItem allItem = ItemManager.Instance.allItems[objectID];
      this.filter.set_mesh(allItem.mesh);
      ((Renderer) this.renderer).set_material(allItem.material);
      this.animator.SetFloat("AnimationSpeed", allItem.attackSpeed);
    }
  }

  private void Sfx()
  {
    double player = (double) this.DistToPlayer();
  }

  private void Animate() => this.animator.SetBool("Grounded", this.grounded);

  private float DistToPlayer() => 1f;

  public TestPlayerAnimations() => base.\u002Ector();
}
