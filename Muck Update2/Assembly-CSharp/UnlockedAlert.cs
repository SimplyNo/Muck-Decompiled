// Decompiled with JetBrains decompiler
// Type: UnlockedAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class UnlockedAlert : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
  public InventoryCell cell;
  public Transform alert;

  private void Start()
  {
    if (Object.op_Equality((Object) this.cell.currentItem, (Object) null))
      Debug.LogError((object) "Item is null");
    else if (UiEvents.Instance.alertCleared[this.cell.currentItem.id])
      Object.Destroy((Object) ((Component) this).get_gameObject());
    else
      ((Component) this.alert).get_transform().set_localScale(Vector3.op_Multiply(Vector3.get_one(), (float) (1.0 + (double) Mathf.PingPong(Time.get_time(), 0.25f) - 0.5)));
  }

  private void Update()
  {
    float num = (float) (1.0 + (double) Mathf.PingPong(Time.get_time(), 0.25f) - 0.5);
    ((Component) this.alert).get_transform().set_localScale(Vector3.Lerp(((Component) this.alert).get_transform().get_localScale(), Vector3.op_Multiply(Vector3.get_one(), num), Time.get_deltaTime() * 10f));
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    UiEvents.Instance.alertCleared[this.cell.currentItem.id] = true;
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }

  public UnlockedAlert() => base.\u002Ector();
}
