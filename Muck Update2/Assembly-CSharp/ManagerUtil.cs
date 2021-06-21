// Decompiled with JetBrains decompiler
// Type: ManagerUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUtil : MonoBehaviour
{
  public static int FindRandomUnusedID(
    InventoryItem.ItemType item,
    Dictionary<int, GameObject> list,
    Random random)
  {
    Vector2 idRange = ManagerUtil.GetIdRange(item);
    int x = (int) idRange.x;
    int y = (int) idRange.y;
    int key = random.Next(x, y);
    int num = 0;
    while (list.ContainsKey(key))
    {
      key = random.Next(x, y);
      ++num;
      if (num > 1000)
        return -1;
    }
    return key;
  }

  public static Vector2 GetIdRange(InventoryItem.ItemType item)
  {
    int num = (int) item;
    return new Vector2((float) (num * 10000), (float) (10000 * (num + 1)));
  }

  public ManagerUtil() => base.\u002Ector();
}
