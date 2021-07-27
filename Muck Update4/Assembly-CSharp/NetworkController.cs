// Decompiled with JetBrains decompiler
// Type: NetworkController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using Steamworks.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviour
{
  public NetworkController.NetworkType networkType;
  public GameObject steam;
  public GameObject classic;
  public int nPlayers;
  public static int maxPlayers = 10;
  public Lobby lobby;
  public string[] playerNames;
  public static NetworkController Instance;

  public bool loading { get; set; }

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
    this.loading = true;
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
