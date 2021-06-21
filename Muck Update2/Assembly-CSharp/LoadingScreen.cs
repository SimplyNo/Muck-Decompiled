// Decompiled with JetBrains decompiler
// Type: LoadingScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
  public TextMeshProUGUI text;
  public RawImage loadingBar;
  public RawImage background;
  private float desiredLoad;
  private Graphic[] allGraphics;
  public CanvasGroup canvasGroup;
  public Transform loadingParent;
  public GameObject loadingPlayerPrefab;
  public static LoadingScreen Instance;
  public bool[] players;
  public CanvasGroup loadBar;
  public CanvasGroup playerStatuses;
  public GameObject[] loadingObject;
  public bool loadingInGame;
  private float currentFadeTime;
  private float desiredAlpha;

  private void Awake()
  {
    LoadingScreen.Instance = this;
    this.canvasGroup.set_alpha(0.0f);
    ((Component) this.background).get_gameObject().SetActive(false);
    this.players = new bool[10];
    if (!LocalClient.serverOwner)
      return;
    this.InvokeRepeating("CheckAllPlayersLoading", 10f, 10f);
  }

  private void CheckAllPlayersLoading()
  {
    if (GameManager.state == GameManager.GameState.Playing)
    {
      this.CancelInvoke(nameof (CheckAllPlayersLoading));
    }
    else
    {
      Debug.LogError((object) "Checking all players");
      foreach (Client client in Server.clients.Values)
      {
        if (client?.player != null)
        {
          Debug.LogError((object) "Checking players");
          if (!client.player.loading)
          {
            ServerSend.StartGame(client.player.id, GameManager.gameSettings);
            Debug.LogError((object) (client.player.username + " failed to load, trying to get him to load again..."));
          }
        }
      }
    }
  }

  private void Start()
  {
    if (!this.loadingInGame)
      return;
    this.InitLoadingPlayers();
  }

  public void SetText(string s, float loadProgress)
  {
    ((Component) this.background).get_gameObject().SetActive(true);
    ((TMP_Text) this.text).set_text(s);
    this.desiredLoad = loadProgress;
  }

  public void Hide(float fadeTime = 1f)
  {
    this.desiredAlpha = 0.0f;
    this.totalFadeTime = fadeTime;
    this.currentFadeTime = 0.0f;
    if ((double) fadeTime == 0.0)
      this.canvasGroup.set_alpha(0.0f);
    this.Invoke("HideStuff", this.totalFadeTime);
  }

  private void HideStuff() => ((Component) this.background).get_gameObject().SetActive(false);

  public void FinishLoading()
  {
    foreach (GameObject gameObject in this.loadingObject)
      gameObject.SetActive(false);
    ((Component) this.loadingParent).get_gameObject().SetActive(true);
  }

  public void UpdateStatuses(int id)
  {
    this.players[id] = true;
    if (this.loadingParent.get_childCount() <= id)
      return;
    ((PlayerLoading) ((Component) this.loadingParent.GetChild(id)).GetComponent<PlayerLoading>()).ChangeStatus("<color=green>Ready");
  }

  public void Show(float fadeTime = 1f)
  {
    this.desiredAlpha = 1f;
    this.currentFadeTime = 0.0f;
    this.totalFadeTime = fadeTime;
    if ((double) fadeTime == 0.0)
      this.canvasGroup.set_alpha(1f);
    ((Component) this.background).get_gameObject().SetActive(true);
  }

  public void InitLoadingPlayers()
  {
    ((Component) this.loadingParent).get_gameObject().SetActive(false);
    for (int index = 0; index < NetworkController.Instance.playerNames.Length; ++index)
    {
      M0 component = ((GameObject) Object.Instantiate<GameObject>((M0) this.loadingPlayerPrefab, this.loadingParent)).GetComponent<PlayerLoading>();
      string str = "<color=red>Loading";
      string playerName = NetworkController.Instance.playerNames[index];
      string status = str;
      ((PlayerLoading) component).SetStatus(playerName, status);
    }
  }

  public float totalFadeTime { get; set; }

  private void Update()
  {
    ((Component) this.loadingBar).get_transform().set_localScale(new Vector3(this.desiredLoad, 1f, 1f));
    if ((double) this.currentFadeTime >= (double) this.totalFadeTime || (double) this.totalFadeTime <= 0.0)
      return;
    this.currentFadeTime += Time.get_deltaTime();
    this.canvasGroup.set_alpha(Mathf.Lerp(this.canvasGroup.get_alpha(), this.desiredAlpha, this.currentFadeTime / this.totalFadeTime));
  }

  public LoadingScreen() => base.\u002Ector();
}
