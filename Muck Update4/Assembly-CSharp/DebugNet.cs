// Decompiled with JetBrains decompiler
// Type: DebugNet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

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
  private bool fpsOn = true;
  private bool speedOn = true;
  private bool pingOn = true;
  private bool bandwidthOn = true;
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
    this.gameObject.SetActive(false);
    this.InvokeRepeating("BandWidth", 1f, 1f);
  }

  public void ToggleConsole() => this.gameObject.SetActive(!this.gameObject.activeInHierarchy);

  private void Update() => this.Fps();

  private void Fps()
  {
    if (!this.fpsOn && !this.speedOn && (!this.pingOn && !this.bandwidthOn))
    {
      if (this.fps.enabled)
        return;
      this.fps.gameObject.SetActive(false);
    }
    else
    {
      if (!this.fps.gameObject.activeInHierarchy)
        this.fps.gameObject.SetActive(true);
      string str1 = "";
      this.deltaTime += (float) (((double) Time.unscaledDeltaTime - (double) this.deltaTime) * 0.100000001490116);
      float num1 = this.deltaTime * 1000f;
      float num2 = 1f / this.deltaTime;
      if (this.fpsOn)
        str1 += string.Format("{0:0.0} ms ({1:0.} fps)", (object) num1, (object) num2);
      if (this.speedOn)
      {
        Vector3 velocity = PlayerMovement.Instance.GetVelocity();
        str1 = str1 + "\nm/s: " + string.Format("{0:F1}", (object) new Vector2(velocity.x, velocity.z).magnitude);
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
      string str3 = str1 + "<size=70%>";
      foreach (string str2 in DebugNet.r)
        ;
      int num3 = 0;
      int num4 = 0;
      foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
      {
        ++num3;
        if (rootGameObject.activeInHierarchy)
          ++num4;
      }
      string str4 = str3 + string.Format("\ngos active: {0} | gos total {1}", (object) num4, (object) num3) + string.Format("\nserver ip:    {0}", (object) Server.ipAddress) + string.Format("\nresources hashmap size: {0}", (object) ResourceManager.Instance.list.Count) + string.Format("\nactive mobs: {0}", (object) MobManager.Instance.mobs.Count) + string.Format("\nactive mobs: {0} /  / {1}", (object) MobManager.Instance.GetActiveEnemies(), (object) GameLoop.currentMobCap) + string.Format("\nhp multiplier: {0}", (object) GameManager.instance.MobHpMultiplier()) + string.Format("\ndamage multiplier: {0}", (object) GameManager.instance.MobDamageMultiplier());
      uint num5 = Profiler.GetTotalAllocatedMemory() / 1048576U;
      uint num6 = Profiler.GetTotalReservedMemory() / 1048576U;
      int num7 = (int) (Profiler.GetTotalUnusedReservedMemory() / 1048576U);
      this.fps.text = str4 + string.Format("\nramTotal: {0}mb | {1}mb / {2}mb", (object) num5, (object) num6, (object) SystemInfo.systemMemorySize) + string.Format("\nServer host: {0} ({1})", (object) LocalClient.instance.serverHost, (object) new Friend((SteamId) LocalClient.instance.serverHost.Value).Name) + string.Format("\nMy server id: {0}", (object) LocalClient.instance.myId) + string.Format("\nAm server owner: {0}", (object) LocalClient.serverOwner) + string.Format("Amount of mob zones: {0}", (object) MobZoneManager.Instance.zones.Count) + string.Format("\nOnly rock: {0}", (object) GameManager.instance.onlyRock) + string.Format("\nDamage taken by any players: {0}", (object) GameManager.instance.damageTaken) + string.Format("\nAny powerups picked up by any players: {0}", (object) GameManager.instance.powerupsPickedup);
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

  private void OpenConsole() => this.console.gameObject.SetActive(true);

  private void CloseConsole() => this.console.gameObject.SetActive(false);
}
