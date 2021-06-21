// Decompiled with JetBrains decompiler
// Type: NetworkController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using Steamworks.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour
{
  public NetworkController.NetworkType networkType;
  public GameObject steam;
  public GameObject classic;
  public Lobby lobby;
  public string[] playerNames;
  public static NetworkController Instance;

  public bool loading { get; set; }

  private void Awake()
  {
    if (Object.op_Implicit((Object) NetworkController.Instance))
    {
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
    else
    {
      NetworkController.Instance = this;
      Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
    }
  }

  public void LoadGame(string[] names)
  {
    this.loading = true;
    this.playerNames = names;
    LoadingScreen.Instance.Show();
    this.Invoke("StartLoadingScene", LoadingScreen.Instance.totalFadeTime);
  }

  private void StartLoadingScene() => SceneManager.LoadScene("GameAfterLobby");

  public NetworkController() => base.\u002Ector();

  public enum NetworkType
  {
    Steam,
    Classic,
  }
}
