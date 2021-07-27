// Decompiled with JetBrains decompiler
// Type: TestGlobalStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class TestGlobalStats : MonoBehaviour
{
  private Task<Result> a;

  private void Start()
  {
    if (SteamUserStats.RequestCurrentStats())
    {
      this.a = SteamUserStats.RequestGlobalStatsAsync(60);
      Debug.LogError((object) "REquesting global stats");
    }
    SteamUserStats.OnUserStatsReceived += new Action<SteamId, Result>(this.ReceivedStats);
  }

  private void ReceivedStats(SteamId id, Result r)
  {
  }
}
