// Decompiled with JetBrains decompiler
// Type: AchievementDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using Steamworks.Data;
using System.Linq;
using UnityEngine;

public class AchievementDisplay : MonoBehaviour
{
  public GameObject achievementPrefab;
  public Transform achievementParent;
  private int achievementsPerPage = 8;
  private int nAchievements;
  private int nPages;
  private int currentPage;
  private Achievement[] achievements = new Achievement[0];

  private void OnEnable()
  {
    this.currentPage = 0;
    if (this.achievements.Length < 1)
      this.achievements = SteamUserStats.Achievements.ToArray<Achievement>();
    this.nAchievements = this.achievements.Length;
    this.nPages = Mathf.FloorToInt((float) this.nAchievements / (float) this.achievementsPerPage);
    this.LoadPage(this.currentPage);
  }

  private void LoadPage(int page)
  {
    for (int index = this.achievementParent.childCount - 1; index >= 0; --index)
      Object.Destroy((Object) this.achievementParent.GetChild(index).gameObject);
    int num = this.achievementsPerPage * page;
    for (int index = num; index < this.achievements.Length; ++index)
    {
      Object.Instantiate<GameObject>(this.achievementPrefab, this.achievementParent).GetComponent<AchievementPrefab>().SetAchievement(this.achievements[index]);
      if (index >= num + this.achievementsPerPage - 1)
        break;
    }
  }

  public void NextPage(int dir)
  {
    if (dir < 0 && this.currentPage == 0 || dir > 0 && this.currentPage >= this.nPages)
      return;
    this.currentPage += dir;
    this.LoadPage(this.currentPage);
  }

  public enum WinState
  {
    Won = -3, // 0xFFFFFFFD
    Lost = -2, // 0xFFFFFFFE
    Draw = -1, // 0xFFFFFFFF
  }
}
