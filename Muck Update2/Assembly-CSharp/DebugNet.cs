// Decompiled with JetBrains decompiler
// Type: DebugNet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using Steamworks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class DebugNet : MonoBehaviour
{
  public TextMeshProUGUI fps;
  public GameObject console;
  private bool fpsOn;
  private bool speedOn;
  private bool pingOn;
  private bool bandwidthOn;
  private float deltaTime;
  public static List<string> r = new List<string>();
  public static DebugNet Instance;
  private float byteUp;
  private float byteDown;
  private float pSent;
  private float pReceived;

  private void Start()
  {
    DebugNet.Instance = this;
    ((Component) this).get_gameObject().SetActive(false);
    this.InvokeRepeating("BandWidth", 1f, 1f);
  }

  public void ToggleConsole() => ((Component) this).get_gameObject().SetActive(!((Component) this).get_gameObject().get_activeInHierarchy());

  private void Update() => this.Fps();

  private void Fps()
  {
    if (!this.fpsOn && !this.speedOn && (!this.pingOn && !this.bandwidthOn))
    {
      if (((Behaviour) this.fps).get_enabled())
        return;
      ((Component) this.fps).get_gameObject().SetActive(false);
    }
    else
    {
      if (!((Component) this.fps).get_gameObject().get_activeInHierarchy())
        ((Component) this.fps).get_gameObject().SetActive(true);
      string str1 = "";
      this.deltaTime += (float) (((double) Time.get_unscaledDeltaTime() - (double) this.deltaTime) * 0.100000001490116);
      float num1 = this.deltaTime * 1000f;
      float num2 = 1f / this.deltaTime;
      if (this.fpsOn)
        str1 += string.Format("{0:0.0} ms ({1:0.} fps)", (object) num1, (object) num2);
      if (this.speedOn)
      {
        Vector3 velocity = PlayerMovement.Instance.GetVelocity();
        string str2 = str1;
        Vector2 vector2 = new Vector2((float) velocity.x, (float) velocity.z);
        string str3 = string.Format("{0:F1}", (object) ((Vector2) ref vector2).get_magnitude());
        str1 = str2 + "\nm/s: " + str3;
      }
      if (this.pingOn)
      {
        int ping = NetStatus.GetPing();
        string str2 = "<color=";
        if (ping < 60)
          str2 += "\"green\"";
        else if (ping < 100)
          str2 += "\"yellow\"";
        else if (ping >= 100)
          str2 += "\"red\"";
        str1 += string.Format("\n{0}>ping: {1}ms <color=\"black\">", (object) str2, (object) NetStatus.GetPing());
      }
      if (this.bandwidthOn)
        str1 = str1 + string.Format("\nbyte up/s:    {0}", (object) this.byteUp) + string.Format("\nbyte down/s : {0}", (object) this.byteDown) + string.Format("\npacket up/s : {0}", (object) this.pSent) + string.Format("\npacket down/s : {0}", (object) this.pReceived);
      string str4 = str1 + "<size=70%>";
      foreach (string str2 in DebugNet.r)
        ;
      int num3 = 0;
      int num4 = 0;
      Scene activeScene = SceneManager.GetActiveScene();
      foreach (GameObject rootGameObject in ((Scene) ref activeScene).GetRootGameObjects())
      {
        ++num3;
        if (rootGameObject.get_activeInHierarchy())
          ++num4;
      }
      string str5 = str4 + string.Format("\ngos active: {0} | gos total {1}", (object) num4, (object) num3) + string.Format("\nserver ip:    {0}", (object) Server.ipAddress) + string.Format("\nresources hashmap size: {0}", (object) ResourceManager.Instance.list.Count) + string.Format("\nactive mobs: {0}", (object) MobManager.Instance.mobs.Count) + string.Format("\nactive mobs: {0} /  / {1}", (object) MobManager.Instance.GetActiveEnemies(), (object) GameLoop.currentMobCap) + string.Format("\nhp multiplier: {0}", (object) GameManager.instance.MobHpMultiplier()) + string.Format("\ndamage multiplier: {0}", (object) GameManager.instance.MobDamageMultiplier());
      uint num5 = Profiler.GetTotalAllocatedMemory() / 1048576U;
      uint num6 = Profiler.GetTotalReservedMemory() / 1048576U;
      int num7 = (int) (Profiler.GetTotalUnusedReservedMemory() / 1048576U);
      string str6 = str5 + string.Format("\nramTotal: {0}mb | {1}mb / {2}mb", (object) num5, (object) num6, (object) SystemInfo.get_systemMemorySize());
      SteamId serverHost = (object) LocalClient.instance.serverHost;
      Friend friend = new Friend(SteamId.op_Implicit((ulong) LocalClient.instance.serverHost.Value));
      string name = ((Friend) ref friend).get_Name();
      string str7 = string.Format("\nServer host: {0} ({1})", (object) serverHost, (object) name);
      ((TMP_Text) this.fps).set_text(str6 + str7 + string.Format("\nMy server id: {0}", (object) LocalClient.instance.myId) + string.Format("\nAm server owner: {0}", (object) LocalClient.serverOwner) + string.Format("Amount of mob zones: {0}", (object) MobZoneManager.Instance.zones.Count));
    }
  }

  private void BandWidth()
  {
    this.byteUp = (float) ClientSend.bytesSent;
    this.byteDown = (float) LocalClient.byteDown;
    this.pSent = (float) ClientSend.packetsSent;
    this.pReceived = (float) LocalClient.packetsReceived;
    ClientSend.bytesSent = 0;
    ClientSend.packetsSent = 0;
    LocalClient.byteDown = 0;
    LocalClient.packetsReceived = 0;
  }

  private void OpenConsole() => this.console.get_gameObject().SetActive(true);

  private void CloseConsole() => this.console.get_gameObject().SetActive(false);

  public DebugNet() => base.\u002Ector();
}
