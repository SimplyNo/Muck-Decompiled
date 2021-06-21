// Decompiled with JetBrains decompiler
// Type: TutorialArrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
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
