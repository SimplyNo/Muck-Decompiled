// Decompiled with JetBrains decompiler
// Type: SendToBossUi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SendToBossUi : MonoBehaviour
{
  public bool forceUI;

  private void Awake()
  {
    Mob component = this.GetComponent<Mob>();
    if (!this.forceUI)
      return;
    BossUI.Instance.SetBoss(component);
  }
}
