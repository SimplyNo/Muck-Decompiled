// Decompiled with JetBrains decompiler
// Type: TutorialArrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
  private void Update()
  {
    this.transform.Rotate(Vector3.forward, 22f * Time.deltaTime);
    this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one * (float) (1.0 + (double) Mathf.PingPong(Time.time * 0.25f, 0.3f) - 0.150000005960464), Time.deltaTime * 2f);
  }
}
