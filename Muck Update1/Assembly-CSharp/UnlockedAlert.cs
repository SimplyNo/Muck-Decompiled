// Decompiled with JetBrains decompiler
// Type: UnlockedAlert
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class UnlockedAlert : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
  public InventoryCell cell;
  public Transform alert;

  private void Start()
  {
    if ((Object) this.cell.currentItem == (Object) null)
      Debug.LogError((object) "Item is null");
    else if (UiEvents.Instance.alertCleared[this.cell.currentItem.id])
      Object.Destroy((Object) this.gameObject);
    else
      this.alert.transform.localScale = Vector3.one * (float) (1.0 + (double) Mathf.PingPong(Time.time, 0.25f) - 0.5);
  }

  private void Update() => this.alert.transform.localScale = Vector3.Lerp(this.alert.transform.localScale, Vector3.one * (float) (1.0 + (double) Mathf.PingPong(Time.time, 0.25f) - 0.5), Time.deltaTime * 10f);

  public void OnPointerEnter(PointerEventData eventData)
  {
    UiEvents.Instance.alertCleared[this.cell.currentItem.id] = true;
    Object.Destroy((Object) this.gameObject);
  }
}
