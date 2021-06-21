// Decompiled with JetBrains decompiler
// Type: ServerCommunication
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class ServerCommunication : MonoBehaviour
{
  public Transform root;
  public Transform cam;
  public PlayerStatus playerStatus;
  private int lastSentHp;
  private float hpThreshold;
  private float posThreshold;
  private float rotThreshold;
  private Vector3 lastSentPosition;
  private float lastSentRotationY;
  private float lastSentRotationX;
  private float lastSentXZ;
  private float lastSentBlendX;
  private float lastSentBlendY;
  private static readonly float updatesPerSecond = 12f;
  private static readonly float slowUpdatesPerSecond = 8f;
  private static readonly float slowerUpdatesPerSecond = 2f;
  private float updateFrequency;
  private float slowUpdateFrequency;
  private float slowerUpdateFrequency;

  private void Awake()
  {
    this.InvokeRepeating("QuickUpdate", this.updateFrequency, this.updateFrequency);
    this.InvokeRepeating("SlowUpdate", this.slowUpdateFrequency, this.slowUpdateFrequency);
    this.InvokeRepeating("SlowerUpdate", this.slowerUpdateFrequency, this.slowerUpdateFrequency);
  }

  private void QuickUpdate()
  {
    if ((double) Vector3.Distance(this.root.get_position(), this.lastSentPosition) <= (double) this.posThreshold)
      return;
    ClientSend.PlayerPosition(this.root.get_position());
    this.lastSentPosition = this.root.get_position();
  }

  private void SlowUpdate()
  {
    float y = (float) this.cam.get_eulerAngles().y;
    float x = (float) this.cam.get_eulerAngles().x;
    if ((double) x >= 270.0)
      x -= 360f;
    float num = Mathf.Abs(this.lastSentRotationY - y);
    if ((double) Mathf.Abs(this.lastSentRotationX - x) <= (double) this.rotThreshold && (double) num <= (double) this.rotThreshold)
      return;
    ClientSend.PlayerRotation(y, x);
    this.lastSentRotationY = y;
    this.lastSentRotationX = x;
  }

  private void SlowerUpdate()
  {
    int num = Mathf.Abs(this.playerStatus.HpAndShield() - this.lastSentHp);
    if (num == 0)
      return;
    MonoBehaviour.print((object) "nope");
    if ((double) num <= (double) this.hpThreshold && !this.playerStatus.IsFullyHealed())
      return;
    MonoBehaviour.print((object) "sent update");
    ClientSend.PlayerHp(this.playerStatus.HpAndShield(), this.playerStatus.MaxHpAndShield());
    this.lastSentHp = this.playerStatus.HpAndShield();
  }

  public ServerCommunication() => base.\u002Ector();
}
