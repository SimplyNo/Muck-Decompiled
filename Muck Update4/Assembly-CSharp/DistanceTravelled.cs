// Decompiled with JetBrains decompiler
// Type: DistanceTravelled
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DistanceTravelled : MonoBehaviour
{
  public float groundTravelled;
  public float waterTravelled;
  public PlayerMovement playerMovement;
  public Rigidbody rb;
  public Vector3 lastPos;
  private float interval = 5f;

  private void Start()
  {
    this.lastPos = this.rb.position;
    this.InvokeRepeating("SlowUpdate", this.interval, this.interval);
  }

  private void SlowUpdate()
  {
    int groundTravelled = (int) this.groundTravelled;
    int waterTravelled = (int) this.waterTravelled;
    if ((bool) (Object) AchievementManager.Instance)
      AchievementManager.Instance.MoveDistance(groundTravelled, waterTravelled);
    this.groundTravelled = 0.0f;
    this.waterTravelled = 0.0f;
  }

  private void FixedUpdate()
  {
    float num = Vector3.Distance(VectorExtensions.XZVector(this.rb.position), VectorExtensions.XZVector(this.lastPos));
    if (this.playerMovement.IsUnderWater())
      this.waterTravelled += num;
    else
      this.groundTravelled += num;
    this.lastPos = this.rb.transform.position;
  }
}
