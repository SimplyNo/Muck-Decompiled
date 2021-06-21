// Decompiled with JetBrains decompiler
// Type: NetStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
    if (!Object.op_Implicit((Object) GameManager.instance))
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

  public NetStatus() => base.\u002Ector();
}
