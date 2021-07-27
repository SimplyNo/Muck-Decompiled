// Decompiled with JetBrains decompiler
// Type: Commands
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Commands : MonoBehaviour
{
  public TMP_InputField inputField;
  public TextMeshProUGUI suggestText;
  private string suggestedText;

  private void Update()
  {
    this.PredictCommands();
    this.PlayerInput();
    this.suggestText.text = this.suggestedText;
  }

  private void PlayerInput()
  {
    if (!Input.GetKeyDown(KeyCode.Tab))
      return;
    this.FillCommand();
  }

  private void FillCommand()
  {
    if (!ChatBox.Instance.typing || this.suggestedText == "")
      return;
    this.inputField.text = this.suggestedText;
    this.inputField.stringPosition = this.inputField.text.Length;
  }

  private void PredictCommands()
  {
    if (!ChatBox.Instance.typing)
    {
      if (!(this.suggestText.text != ""))
        return;
      this.suggestedText = "";
      this.suggestText.text = "";
    }
    else
    {
      this.suggestedText = "";
      string text = this.inputField.text;
      if (text.Length < 1)
        return;
      string str1 = ((IEnumerable<string>) text.Split(' ')).Last<string>();
      if (str1.Length < 1)
        return;
      string str2 = str1[0].ToString() ?? "";
      string str3 = str1.Remove(0, 1);
      if (str2 == "/")
      {
        foreach (string command in ChatBox.Instance.commands)
        {
          if (command.StartsWith(str3))
          {
            this.suggestedText = text;
            int num = command.Length - str3.Length;
            this.suggestedText += command.Substring(command.Length - num);
            return;
          }
        }
      }
      string[] strArray = text.Split();
      if (strArray.Length < 2)
        return;
      int startIndex = text.IndexOf(" ", StringComparison.Ordinal) + 1;
      string str4 = text.Substring(startIndex);
      if (!(strArray[0] == "/kick"))
        return;
      foreach (Client client in Server.clients.Values)
      {
        if (client != null && client.player != null && client.player.username.ToLower().Contains(str4.ToLower()))
        {
          this.suggestedText = strArray[0] + " ";
          this.suggestedText += client.player.username;
          break;
        }
      }
    }
  }
}
