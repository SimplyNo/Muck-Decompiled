// Decompiled with JetBrains decompiler
// Type: AchievementPrefab
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPrefab : MonoBehaviour
{
  public RawImage img;
  public TextMeshProUGUI title;
  public TextMeshProUGUI desc;

  public void SetAchievement(Achievement a)
  {
    if (!a.GetIcon().HasValue)
      Debug.LogError((object) "no img");
    else
      this.img.texture = (Texture) AchievementPrefab.GetSteamImageAsTexture2D(a.GetIcon().Value);
    this.title.text = a.Name;
    this.desc.text = a.Description;
  }

  public static Texture2D GetSteamImageAsTexture2D(Steamworks.Data.Image img)
  {
    Texture2D texture2D = new Texture2D((int) img.Width, (int) img.Height, TextureFormat.RGBA32, false, true);
    texture2D.LoadRawTextureData(img.Data);
    texture2D.Apply();
    return texture2D;
  }
}
