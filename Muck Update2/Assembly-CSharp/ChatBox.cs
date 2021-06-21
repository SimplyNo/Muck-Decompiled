// Decompiled with JetBrains decompiler
// Type: ChatBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
  private Color console;
  private int maxMsgLength;
  private int maxChars;
  private int purgeAmount;
  public static ChatBox Instance;
  public TextAsset profanityList;
  private List<string> profanity;

  public bool typing { get; set; }

  private void Awake()
  {
    ChatBox.Instance = this;
    this.HideChat();
    this.profanity = new List<string>();
    string text = this.profanityList.get_text();
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
    string str5 = str2 + str1;
    TextMeshProUGUI messages = this.messages;
    ((TMP_Text) messages).set_text(((TMP_Text) messages).get_text() + str5);
    int length = ((TMP_Text) this.messages).get_text().Length;
    if (length > this.maxChars)
    {
      int startIndex = length - this.purgeAmount;
      ((TMP_Text) this.messages).set_text(((TMP_Text) this.messages).get_text().Substring(startIndex));
    }
    this.ShowChat();
    if (this.typing)
      return;
    this.CancelInvoke("HideChat");
    this.Invoke("HideChat", 5f);
  }

  public void SendMessage(string message)
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
    this.inputField.set_text("");
    ((Selectable) this.inputField).set_interactable(false);
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
          PlayerStatus.Instance.Damage(0, true);
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
      GUIUtility.set_systemCopyBuffer(string.Concat((object) seed));
    }
  }

  private string TrimMessage(string message) => string.IsNullOrEmpty(message) ? "" : message.Substring(0, Mathf.Min(message.Length, this.maxMsgLength));

  private void UserInput()
  {
    if (Input.GetKeyDown((KeyCode) 13))
    {
      if (this.typing)
      {
        this.SendMessage(this.inputField.get_text());
      }
      else
      {
        this.ShowChat();
        ((Selectable) this.inputField).set_interactable(true);
        ((Selectable) this.inputField).Select();
        this.typing = true;
      }
    }
    if (this.typing && !this.inputField.get_isFocused())
      ((Selectable) this.inputField).Select();
    if (!Input.GetKeyDown((KeyCode) 27) || !this.typing)
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
    ((Graphic) this.overlay).CrossFadeAlpha(0.0f, 1f, true);
    ((Graphic) this.messages).CrossFadeAlpha(0.0f, 1f, true);
    ((Graphic) ((Component) this.inputField).GetComponent<Image>()).CrossFadeAlpha(0.0f, 1f, true);
    ((Graphic) ((Component) this.inputField).GetComponentInChildren<TextMeshProUGUI>()).CrossFadeAlpha(0.0f, 1f, true);
  }

  private void ShowChat()
  {
    ((Graphic) this.overlay).CrossFadeAlpha(1f, 0.2f, true);
    ((Graphic) this.messages).CrossFadeAlpha(1f, 0.2f, true);
    ((Graphic) ((Component) this.inputField).GetComponent<Image>()).CrossFadeAlpha(0.2f, 1f, true);
    ((Graphic) ((Component) this.inputField).GetComponentInChildren<TextMeshProUGUI>()).CrossFadeAlpha(0.2f, 1f, true);
  }

  public ChatBox() => base.\u002Ector();
}
