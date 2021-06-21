// Decompiled with JetBrains decompiler
// Type: TutorialArrow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
  private void Update()
  {
    ((Component) this).get_transform().Rotate(Vector3.get_forward(), 22f * Time.get_deltaTime());
    float num = (float) (1.0 + (double) Mathf.PingPong(Time.get_time() * 0.25f, 0.3f) - 0.150000005960464);
    ((Component) this).get_transform().set_localScale(Vector3.Lerp(((Component) this).get_transform().get_localScale(), Vector3.op_Multiply(Vector3.get_one(), num), Time.get_deltaTime() * 2f));
  }

  public TutorialArrow() => base.\u002Ector();
}
