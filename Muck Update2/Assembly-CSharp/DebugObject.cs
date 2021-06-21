// Decompiled with JetBrains decompiler
// Type: DebugObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class DebugObject : MonoBehaviour
{
  public string text;
  public Vector3 offset;
  private Camera cam;

  private void Update()
  {
    if (!Object.op_Implicit((Object) ((Component) this).get_transform().get_parent()))
      return;
    ((Component) this).get_transform().set_rotation(Quaternion.get_identity());
    ((Component) this).get_transform().set_position(Vector3.op_Addition(((Component) this).get_transform().get_parent().get_position(), this.offset));
  }

  private void OnGUI()
  {
    if (!Object.op_Implicit((Object) PlayerMovement.Instance))
      return;
    if (!Object.op_Implicit((Object) this.cam))
    {
      this.cam = (Camera) ((Component) PlayerMovement.Instance.playerCam).GetComponentInChildren<Camera>();
    }
    else
    {
      Vector3 viewportPoint = this.cam.WorldToViewportPoint(((Component) this).get_transform().get_position());
      if (viewportPoint.x < 0.0 || viewportPoint.x > 1.0 || (viewportPoint.y < 0.0 || viewportPoint.y > 1.0) || viewportPoint.z <= 0.0)
        return;
      Vector3 screenPoint = Camera.get_main().WorldToScreenPoint(((Component) this).get_gameObject().get_transform().get_position());
      if ((double) Vector3.Distance(((Component) PlayerMovement.Instance).get_transform().get_position(), ((Component) this).get_transform().get_position()) > 30.0)
        return;
      Vector2 vector2 = GUI.get_skin().get_label().CalcSize(new GUIContent(this.text));
      GUI.Label(new Rect((float) screenPoint.x, (float) Screen.get_height() - (float) screenPoint.y, (float) vector2.x, (float) vector2.y), this.text);
    }
  }

  public DebugObject() => base.\u002Ector();
}
