// Decompiled with JetBrains decompiler
// Type: NetStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetStatus : MonoBehaviour
{
  private static LinkedList<int> pings = new LinkedList<int>();
  private static int pingBuffer = 2;

  private void Awake() => this.InvokeRepeating("SlowUpdate", 1f, 1f);

  private void SlowUpdate()
  {
    if (!(bool) (Object) GameManager.instance)
      return;
    ClientSend.PingServer();
  }

  public static void AddPing(int p)
  {
    NetStatus.pings.AddFirst(p);
    if (NetStatus.pings.Count <= NetStatus.pingBuffer)
      return;
    NetStatus.pings.RemoveLast();
  }

  public static int GetPing() => NetStatus.pings.Count > 0 ? (int) NetStatus.pings.Average() : 0;
}
