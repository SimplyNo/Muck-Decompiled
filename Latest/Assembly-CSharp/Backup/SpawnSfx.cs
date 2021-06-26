// Decompiled with JetBrains decompiler
// Type: SpawnSfx
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SpawnSfx : MonoBehaviour
{
  public GameObject startCharge;
  public Transform pos;

  public void SpawnSound() => Object.Instantiate<GameObject>(this.startCharge, this.pos.position, this.startCharge.transform.rotation);
}
