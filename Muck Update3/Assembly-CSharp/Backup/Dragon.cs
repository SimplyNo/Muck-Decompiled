// Decompiled with JetBrains decompiler
// Type: Dragon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 557671A5-6448-43F1-8017-7CE07FCBB682
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

  private void Start() => MusicController.Instance.StopSong(0.5f);

  public void PlayWingFlap() => this.wingFlap.Randomize(0.0f);

  private void OnDestroy()
  {
    Debug.LogError((object) "Game is over lol");
    Object.Instantiate<GameObject>(this.roar, this.transform.position, Quaternion.identity);
    if (!LocalClient.serverOwner)
      return;
    GameManager.instance.GameOver(-3, 8f);
    ServerSend.GameOver();
  }
}
