// Decompiled with JetBrains decompiler
// Type: DebugObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DebugObject : MonoBehaviour
{
  public string text;
  public Vector3 offset = new Vector3(0.0f, 1.5f, 0.0f);
  private Camera cam;

  private void Update()
  {
    if (!(bool) (Object) this.transform.parent)
      return;
    this.transform.rotation = Quaternion.identity;
    this.transform.position = this.transform.parent.position + this.offset;
  }

  private void OnGUI()
  {
    if (!(bool) (Object) PlayerMovement.Instance)
      return;
    if (!(bool) (Object) this.cam)
    {
      this.cam = PlayerMovement.Instance.playerCam.GetComponentInChildren<Camera>();
    }
    else
    {
      Vector3 viewportPoint = this.cam.WorldToViewportPoint(this.transform.position);
      if ((double) viewportPoint.x < 0.0 || (double) viewportPoint.x > 1.0 || ((double) viewportPoint.y < 0.0 || (double) viewportPoint.y > 1.0) || (double) viewportPoint.z <= 0.0)
        return;
      Vector3 screenPoint = Camera.main.WorldToScreenPoint(this.gameObject.transform.position);
      if ((double) Vector3.Distance(PlayerMovement.Instance.transform.position, this.transform.position) > 30.0)
        return;
      Vector2 vector2 = GUI.skin.label.CalcSize(new GUIContent(this.text));
      GUI.Label(new Rect(screenPoint.x, (float) Screen.height - screenPoint.y, vector2.x, vector2.y), this.text);
    }
  }
}
