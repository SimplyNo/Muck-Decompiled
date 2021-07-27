// Decompiled with JetBrains decompiler
// Type: Dragon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Dragon : MonoBehaviour
{
  public RandomSfx wingFlap;
  public GameObject roar;
  public static Dragon Instance;

  private void Awake()
  {
    Dragon.Instance = this;
    this.transform.rotation = Quaternion.LookRotation(Vector3.up);
  }

  private void Start() => MusicController.Instance.FinalBoss();

  public void PlayWingFlap() => this.wingFlap.Randomize(0.0f);

  private void OnDestroy()
  {
    Debug.LogError((object) "Game is over lol");
    Object.Instantiate<GameObject>(this.roar, this.transform.position, Quaternion.identity);
    if (!LocalClient.serverOwner)
      return;
    GameManager.instance.GameOver(-3, 8f);
    ServerSend.GameOver(-3);
  }
}
