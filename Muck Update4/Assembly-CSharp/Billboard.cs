// Decompiled with JetBrains decompiler
// Type: Billboard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Billboard : MonoBehaviour
{
  private Vector3 defaultScale;
  public bool xz;
  public bool affectScale;
  private Transform t;

  private void Awake() => this.defaultScale = this.transform.localScale;

  private void Update()
  {
    if (!(bool) (Object) this.t)
    {
      if (!((Object) this.t == (Object) null) && this.t.gameObject.activeInHierarchy)
        return;
      if ((bool) (Object) PlayerMovement.Instance)
      {
        this.t = PlayerMovement.Instance.playerCam;
      }
      else
      {
        if (!(bool) (Object) Camera.main)
          return;
        this.t = Camera.main.transform;
      }
    }
    else
    {
      this.transform.LookAt(this.t);
      if (!this.xz)
        this.transform.rotation = Quaternion.Euler(0.0f, this.transform.rotation.eulerAngles.y + 180f, 0.0f);
      if (!this.affectScale)
        return;
      this.transform.localScale = this.defaultScale;
    }
  }
}
