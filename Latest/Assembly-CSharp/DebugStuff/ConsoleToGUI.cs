// Decompiled with JetBrains decompiler
// Type: DebugStuff.ConsoleToGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

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
      if (Object.op_Implicit((Object) ConsoleToGUI.Instance))
      {
        Object.Destroy((Object) ((Component) this).get_gameObject());
      }
      else
      {
        ConsoleToGUI.Instance = this;
        Object.DontDestroyOnLoad((Object) ((Component) this).get_gameObject());
      }
    }

    private void OnEnable() => Application.add_logMessageReceived(new Application.LogCallback((object) this, __methodptr(Log)));

    private void OnDisable() => Application.remove_logMessageReceived(new Application.LogCallback((object) this, __methodptr(Log)));

    public void Log(string logString, string stackTrace, LogType type)
    {
      this.output = logString;
      this.stack = stackTrace;
      ConsoleToGUI.myLog = this.output + "\n" + ConsoleToGUI.myLog;
      if (ConsoleToGUI.myLog.Length > 300)
        ConsoleToGUI.myLog = ConsoleToGUI.myLog.Substring(0, 200);
      ((TMP_Text) this.debugText).set_text(ConsoleToGUI.myLog);
    }

    private void OnGUI()
    {
    }

    public ConsoleToGUI() => base.\u002Ector();
  }
}
