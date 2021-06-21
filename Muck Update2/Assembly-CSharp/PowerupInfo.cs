// Decompiled with JetBrains decompiler
// Type: PowerupInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class PowerupInfo : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
  public Powerup powerup { get; set; }

  public void OnPointerEnter(PointerEventData eventData) => ItemInfo.Instance.SetText(this.powerup.name + "\n<size=50%><i>" + this.powerup.description, true);

  public void OnPointerExit(PointerEventData eventData) => ItemInfo.Instance.Fade(0.0f);

  public PowerupInfo() => base.\u002Ector();
}
