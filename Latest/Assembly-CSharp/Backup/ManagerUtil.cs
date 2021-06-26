// Decompiled with JetBrains decompiler
// Type: ManagerUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ManagerUtil : MonoBehaviour
{
  public static int FindRandomUnusedID(
    InventoryItem.ItemType item,
    Dictionary<int, GameObject> list,
    System.Random random)
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
}
