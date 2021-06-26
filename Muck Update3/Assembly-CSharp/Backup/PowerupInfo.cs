// Decompiled with JetBrains decompiler
// Type: PowerupInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class PowerupInfo : 
  MonoBehaviour,
  IPointerEnterHandler,
  IEventSystemHandler,
  IPointerExitHandler
{
  public Powerup powerup { get; set; }

  public void OnPointerEnter(PointerEventData eventData) => ItemInfo.Instance.SetText(this.powerup.name + "\n<size=50%><i>" + this.powerup.description, true);

  public void OnPointerExit(PointerEventData eventData) => ItemInfo.Instance.Fade(0.0f);
}
