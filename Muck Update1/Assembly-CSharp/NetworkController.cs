// Decompiled with JetBrains decompiler
// Type: NetworkController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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

  private void Awake()
  {
    if ((bool) (Object) NetworkController.Instance)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      NetworkController.Instance = this;
      Object.DontDestroyOnLoad((Object) this.gameObject);
    }
  }

  public void LoadGame(string[] names)
  {
    this.playerNames = names;
    LoadingScreen.Instance.Show();
    this.Invoke("StartLoadingScene", LoadingScreen.Instance.totalFadeTime);
  }

  private void StartLoadingScene() => SceneManager.LoadScene("GameAfterLobby");

  public enum NetworkType
  {
    Steam,
    Classic,
  }
}
