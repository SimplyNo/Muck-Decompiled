// Decompiled with JetBrains decompiler
// Type: NetworkManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class NetworkManager : MonoBehaviour
{
  public static NetworkManager instance;

  public static float Clock { get; set; }

  public static float CountDown { get; set; }

  private void Update() => NetworkManager.Clock += Time.get_deltaTime();

  public int GetSpawnPosition(int id) => id;

  private void Awake()
  {
    if (Object.op_Equality((Object) NetworkManager.instance, (Object) null))
    {
      NetworkManager.instance = this;
    }
    else
    {
      if (!Object.op_Inequality((Object) NetworkManager.instance, (Object) this))
        return;
      Debug.Log((object) "Instance already exists, destroying object");
      Object.Destroy((Object) this);
    }
  }

  private void Start()
  {
  }

  public void StartServer(int port) => Server.Start(40, port);

  private void OnApplicationQuit() => Server.Stop();

  public void DestroyPlayer(GameObject g) => Object.Destroy((Object) g);

  public NetworkManager() => base.\u002Ector();
}
