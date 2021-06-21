// Decompiled with JetBrains decompiler
// Type: OnlinePlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class OnlinePlayer : MonoBehaviour
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
  public SkinnedMeshRenderer[] armor;
  private float currentTorsoRotation;
  private float lastFallSpeed;
  public GameObject footstepFx;
  private float distance;
  private float fallSpeed;
  public GameObject smokeFx;
  public Transform jumpSmokeFxPos;
  private float speed;

  public float hpRatio { get; set; }

  private void Start()
  {
    this.grounded = true;
    this.filter = (MeshFilter) this.weapon.GetComponent<MeshFilter>();
    this.renderer = (MeshRenderer) this.weapon.GetComponent<MeshRenderer>();
  }

  private void FixedUpdate()
  {
    this.fallSpeed = Mathf.Abs((float) this.rb.get_velocity().y);
    this.rb.MovePosition(Vector3.Lerp(this.rb.get_position(), this.desiredPos, Time.get_deltaTime() * this.moveSpeed));
  }

  private void Update()
  {
    this.grounded = Physics.Raycast(((Component) this).get_transform().get_position(), Vector3.get_down(), 2.4f, LayerMask.op_Implicit(this.whatIsGround));
    this.Animate();
    this.Sfx();
    this.FootSteps();
    ((Component) this).get_transform().set_rotation(Quaternion.Lerp(((Component) this).get_transform().get_rotation(), Quaternion.Euler(0.0f, this.orientationY, 0.0f), Time.get_deltaTime() * this.rotationSpeed));
    this.hpBar.set_localScale(new Vector3(Mathf.Lerp((float) this.hpBar.get_localScale().x, this.hpRatio, Time.get_deltaTime() * 10f), 1f, 1f));
  }

  private void LateUpdate()
  {
    this.currentTorsoRotation = Mathf.Lerp(this.currentTorsoRotation, this.orientationX, Time.get_deltaTime() * this.rotationSpeed);
    this.upperBody.set_localRotation(Quaternion.Euler(this.currentTorsoRotation, (float) this.upperBody.get_localRotation().y, (float) this.upperBody.get_localRotation().z));
    this.lastFallSpeed = (float) this.rb.get_velocity().y;
  }

  private void FootSteps()
  {
    if ((double) this.DistToPlayer() > 30.0 || !this.grounded)
      return;
    float num1 = 1f;
    Vector3 velocity = this.rb.get_velocity();
    float num2 = ((Vector3) ref velocity).get_magnitude();
    if ((double) num2 > 20.0)
      num2 = 20f;
    this.distance += (float) ((double) num2 * (double) Time.get_deltaTime() * 50.0);
    if ((double) this.distance <= 300.0 / (double) num1)
      return;
    Object.Instantiate<GameObject>((M0) this.footstepFx, ((Component) this).get_transform().get_position(), Quaternion.get_identity());
    this.distance = 0.0f;
  }

  public int currentWeaponId { get; set; }

  public void UpdateWeapon(int objectID)
  {
    this.currentWeaponId = objectID;
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

  public void SpawnSmoke()
  {
    if ((double) this.DistToPlayer() > 30.0)
      return;
    Object.Instantiate<GameObject>((M0) this.smokeFx, this.jumpSmokeFxPos.get_position(), Quaternion.LookRotation(Vector3.get_up()));
  }

  private void Animate()
  {
    Vector3 velocity = this.rb.get_velocity();
    this.speed = Mathf.Lerp(this.speed, Mathf.Clamp(((Vector3) ref velocity).get_magnitude() * 0.1f, 0.0f, 1f), Time.get_deltaTime() * 10f);
    this.animator.SetBool("Grounded", this.grounded);
    this.animator.SetFloat("FallSpeed", this.lastFallSpeed);
    this.animator.SetFloat("Speed", this.speed);
  }

  private float DistToPlayer() => !Object.op_Implicit((Object) PlayerMovement.Instance) ? 1000f : Vector3.Distance(((Component) PlayerMovement.Instance).get_transform().get_position(), ((Component) this).get_transform().get_position());

  public void NewAnimation(int animation, bool b)
  {
    switch ((OnlinePlayer.SharedAnimation) animation)
    {
      case OnlinePlayer.SharedAnimation.Attack:
        this.animator.Play("Attack");
        break;
      case OnlinePlayer.SharedAnimation.Eat:
        this.animator.SetBool("Eating", b);
        break;
      case OnlinePlayer.SharedAnimation.Charge:
        this.animator.SetBool("Charging", b);
        break;
    }
  }

  public OnlinePlayer() => base.\u002Ector();

  public enum SharedAnimation
  {
    Attack,
    Eat,
    Charge,
  }
}
