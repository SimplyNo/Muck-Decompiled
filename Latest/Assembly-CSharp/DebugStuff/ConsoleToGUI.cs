// Decompiled with JetBrains decompiler
// Type: DebugStuff.ConsoleToGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BACBFE5D-6724-4F02-B6BB-D6D37EC5478A
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

namespace DebugStuff
{
  public class ConsoleToGUI : MonoBehaviour
  {
    public TextMeshProUGUI debugText;
    private static string myLog = "";
    private string output;
    private string stack;
    public static ConsoleToGUI Instance;

    private void Awake()
    {
      if ((bool) (Object) ConsoleToGUI.Instance)
      {
        Object.Destroy((Object) this.gameObject);
      }
      else
      {
        ConsoleToGUI.Instance = this;
        Object.DontDestroyOnLoad((Object) this.gameObject);
      }
    }

    private void OnEnable() => Application.logMessageReceived += new Application.LogCallback(this.Log);

    private void OnDisable() => Application.logMessageReceived -= new Application.LogCallback(this.Log);

    public void Log(string logString, string stackTrace, LogType type)
    {
      this.output = logString;
      this.stack = stackTrace;
      ConsoleToGUI.myLog = this.output + "\n" + ConsoleToGUI.myLog;
      if (ConsoleToGUI.myLog.Length > 300)
        ConsoleToGUI.myLog = ConsoleToGUI.myLog.Substring(0, 200);
      this.debugText.text = ConsoleToGUI.myLog;
    }

    private void OnGUI()
    {
    }
  }
}
