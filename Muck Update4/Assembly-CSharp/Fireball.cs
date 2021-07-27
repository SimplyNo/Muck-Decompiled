// Decompiled with JetBrains decompiler
// Type: Fireball
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Fireball : MonoBehaviour
{
  public InventoryItem fireball;
  public GameObject warningFx;

  private void Start()
  {
    Vector3 forward = this.transform.forward;
    this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + new Vector3(0.0f, 0.0f, -90f));
    Rigidbody component = this.GetComponent<Rigidbody>();
    component.velocity = forward * this.fireball.bowComponent.projectileSpeed;
    component.maxAngularVelocity = 9999f;
    component.AddRelativeTorque(component.angularVelocity * 2000f);
    component.angularVelocity = Vector3.zero;
    RaycastHit hitInfo;
    if (!Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, 5000f, (int) GameManager.instance.whatIsGround))
      return;
    Object.Instantiate<GameObject>(this.warningFx, hitInfo.point, this.warningFx.transform.rotation).GetComponent<EnemyAttackIndicator>().SetWarning(Vector3.Distance(this.transform.position, hitInfo.point) / component.velocity.magnitude, 5f);
  }
}
