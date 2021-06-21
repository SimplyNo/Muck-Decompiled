// Decompiled with JetBrains decompiler
// Type: LoadingScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
    this.canvasGroup.alpha = 0.0f;
    this.background.gameObject.SetActive(false);
    this.players = new bool[10];
  }

  private void Start()
  {
    if (!this.loadingInGame)
      return;
    this.InitLoadingPlayers();
  }

  public void SetText(string s, float loadProgress)
  {
    this.background.gameObject.SetActive(true);
    this.text.text = s;
    this.desiredLoad = loadProgress;
  }

  public void Hide(float fadeTime = 1f)
  {
    this.desiredAlpha = 0.0f;
    this.totalFadeTime = fadeTime;
    this.currentFadeTime = 0.0f;
    if ((double) fadeTime == 0.0)
      this.canvasGroup.alpha = 0.0f;
    this.Invoke("HideStuff", this.totalFadeTime);
  }

  private void HideStuff() => this.background.gameObject.SetActive(false);

  public void FinishLoading()
  {
    foreach (GameObject gameObject in this.loadingObject)
      gameObject.SetActive(false);
    this.loadingParent.gameObject.SetActive(true);
  }

  public void UpdateStatuses(int id)
  {
    this.players[id] = true;
    if (this.loadingParent.childCount <= id)
      return;
    this.loadingParent.GetChild(id).GetComponent<PlayerLoading>().ChangeStatus("<color=green>Ready");
  }

  public void Show(float fadeTime = 1f)
  {
    this.desiredAlpha = 1f;
    this.currentFadeTime = 0.0f;
    this.totalFadeTime = fadeTime;
    if ((double) fadeTime == 0.0)
      this.canvasGroup.alpha = 1f;
    this.background.gameObject.SetActive(true);
  }

  public void InitLoadingPlayers()
  {
    this.loadingParent.gameObject.SetActive(false);
    for (int index = 0; index < NetworkController.Instance.playerNames.Length; ++index)
    {
      PlayerLoading component = Object.Instantiate<GameObject>(this.loadingPlayerPrefab, this.loadingParent).GetComponent<PlayerLoading>();
      string str = "<color=red>Loading";
      string playerName = NetworkController.Instance.playerNames[index];
      string status = str;
      component.SetStatus(playerName, status);
    }
  }

  public float totalFadeTime { get; set; } = 1f;

  private void Update()
  {
    this.loadingBar.transform.localScale = new Vector3(this.desiredLoad, 1f, 1f);
    if ((double) this.currentFadeTime >= (double) this.totalFadeTime || (double) this.totalFadeTime <= 0.0)
      return;
    this.currentFadeTime += Time.deltaTime;
    this.canvasGroup.alpha = Mathf.Lerp(this.canvasGroup.alpha, this.desiredAlpha, this.currentFadeTime / this.totalFadeTime);
  }
}
