// Decompiled with JetBrains decompiler
// Type: ChatBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
  public Image overlay;
  public TMP_InputField inputField;
  public TextMeshProUGUI messages;
  public Color localPlayer;
  public Color onlinePlayer;
  public Color deadPlayer;
  private Color console = Color.cyan;
  private int maxMsgLength = 120;
  private int maxChars = 800;
  private int purgeAmount = 400;
  public static ChatBox Instance;
  public TextAsset profanityList;
  private List<string> profanity;

  public bool typing { get; set; }

  private void Awake()
  {
    ChatBox.Instance = this;
    this.HideChat();
    this.profanity = new List<string>();
    string text = this.profanityList.text;
    char[] chArray = new char[1]{ '\n' };
    foreach (string input in text.Split(chArray))
      this.profanity.Add(ChatBox.RemoveWhitespace(input));
  }

  public static string RemoveWhitespace(string input) => new string(((IEnumerable<char>) input.ToCharArray()).Where<char>((Func<char, bool>) (c => !char.IsWhiteSpace(c))).ToArray<char>());

  public void AppendMessage(int fromUser, string message, string fromUsername)
  {
    string str1 = this.TrimMessage(message);
    string str2 = "\n";
    if (fromUser != -1)
    {
      string str3 = "<color=";
      string str4 = fromUser != LocalClient.instance.myId ? (!GameManager.players[fromUser].dead ? str3 + "#" + ColorUtility.ToHtmlStringRGB(this.onlinePlayer) + ">" : str3 + "#" + ColorUtility.ToHtmlStringRGB(this.deadPlayer) + ">") : str3 + "#" + ColorUtility.ToHtmlStringRGB(this.localPlayer) + ">";
      str2 += str4;
    }
    if (GameManager.gameSettings.gameMode == GameSettings.GameMode.Versus && GameManager.players[fromUser].dead && !PlayerStatus.Instance.IsPlayerDead())
      return;
    if (fromUser != -1 || fromUser == -1 && fromUsername != "")
      str2 = str2 + fromUsername + ": ";
    this.messages.text += str2 + str1;
    int length = this.messages.text.Length;
    if (length > this.maxChars)
      this.messages.text = this.messages.text.Substring(length - this.purgeAmount);
    this.ShowChat();
    if (this.typing)
      return;
    this.CancelInvoke("HideChat");
    this.Invoke("HideChat", 5f);
  }

  public new void SendMessage(string message)
  {
    this.typing = false;
    message = this.TrimMessage(message);
    if (message == "")
      return;
    if (message[0] == '/')
    {
      this.ChatCommand(message);
    }
    else
    {
      foreach (string pattern in this.profanity)
        message = Regex.Replace(message, pattern, "muck");
      this.AppendMessage(0, message, GameManager.players[LocalClient.instance.myId].username);
      ClientSend.SendChatMessage(message);
      this.ClearMessage();
    }
  }

  private void ClearMessage()
  {
    this.inputField.text = "";
    this.inputField.interactable = false;
  }

  private void ChatCommand(string message)
  {
    string str1 = message.Substring(1);
    this.ClearMessage();
    string str2 = "#" + ColorUtility.ToHtmlStringRGB(this.console);
    int num = str1 == "seed" ? 1 : 0;
    if (!(str1 == "seed"))
    {
      if (!(str1 == "ping"))
      {
        if (!(str1 == "debug"))
        {
          if (!(str1 == "kill"))
            return;
          PlayerStatus.Instance.Damage(0);
        }
        else
          DebugNet.Instance.ToggleConsole();
      }
      else
        this.AppendMessage(-1, "<color=" + str2 + ">pong<color=white>", "");
    }
    else
    {
      int seed = GameManager.gameSettings.Seed;
      this.AppendMessage(-1, "<color=" + str2 + ">Seed: " + (object) seed + " (copied to clipboard)<color=white>", "");
      GUIUtility.systemCopyBuffer = string.Concat((object) seed);
    }
  }

  private string TrimMessage(string message) => string.IsNullOrEmpty(message) ? "" : message.Substring(0, Mathf.Min(message.Length, this.maxMsgLength));

  private void UserInput()
  {
    if (Input.GetKeyDown(KeyCode.Return))
    {
      if (this.typing)
      {
        this.SendMessage(this.inputField.text);
      }
      else
      {
        this.ShowChat();
        this.inputField.interactable = true;
        this.inputField.Select();
        this.typing = true;
      }
    }
    if (this.typing && !this.inputField.isFocused)
      this.inputField.Select();
    if (!Input.GetKeyDown(KeyCode.Escape) || !this.typing)
      return;
    this.ClearMessage();
    this.typing = false;
    this.CancelInvoke("HideChat");
    this.Invoke("HideChat", 5f);
  }

  private void Update() => this.UserInput();

  private void HideChat()
  {
    if (this.typing)
      return;
    this.typing = false;
    this.overlay.CrossFadeAlpha(0.0f, 1f, true);
    this.messages.CrossFadeAlpha(0.0f, 1f, true);
    this.inputField.GetComponent<Image>().CrossFadeAlpha(0.0f, 1f, true);
    this.inputField.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0.0f, 1f, true);
  }

  private void ShowChat()
  {
    this.overlay.CrossFadeAlpha(1f, 0.2f, true);
    this.messages.CrossFadeAlpha(1f, 0.2f, true);
    this.inputField.GetComponent<Image>().CrossFadeAlpha(0.2f, 1f, true);
    this.inputField.GetComponentInChildren<TextMeshProUGUI>().CrossFadeAlpha(0.2f, 1f, true);
  }
}
