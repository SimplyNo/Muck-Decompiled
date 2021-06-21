// Decompiled with JetBrains decompiler
// Type: CurrentSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;

public class CurrentSettings : MonoBehaviour
{
  public static bool cameraShake;
  public static bool grass = true;
  public static bool invertedHor = false;
  public static bool invertedVer = false;
  public float sensMultiplier;
  public int fov;
  public bool tutorial;
  public float volume;
  public float music;
  public static CurrentSettings Instance;

  private void Awake()
  {
    if (Object.op_Implicit((Object) CurrentSettings.Instance))
    {
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
    else
    {
      CurrentSettings.Instance = this;
      Debug.LogError((object) "new settings instance");
    }
  }

  private void Start() => this.InitSettings();

  private void InitSettings() => this.UpdateSave();

  public void UpdateSave()
  {
    this.UpdateCamShake(SaveManager.Instance.state.cameraShake);
    this.UpdateFov((float) SaveManager.Instance.state.fov);
    this.UpdateSens(SaveManager.Instance.state.sensMultiplier);
    this.UpdateGrass(SaveManager.Instance.state.grass);
    this.UpdateTutorial(SaveManager.Instance.state.tutorial);
    this.UpdateShadowQuality(SaveManager.Instance.state.shadowQuality);
    this.UpdateShadowResolution(SaveManager.Instance.state.shadowResolution);
    this.UpdateShadowCascades(SaveManager.Instance.state.shadowCascade);
    this.UpdateShadowDistance(SaveManager.Instance.state.shadowDistance);
    this.UpdateTextureQuality(SaveManager.Instance.state.textureQuality);
    this.UpdateAntiAliasing(SaveManager.Instance.state.antiAliasing);
    this.UpdateSoftParticles(SaveManager.Instance.state.softParticles);
    this.UpdateBloom(SaveManager.Instance.state.bloom);
    this.UpdateMotionBlur(SaveManager.Instance.state.motionBlur);
    this.UpdateAO(SaveManager.Instance.state.ambientOcclusion);
    Vector2 resolution = SaveManager.Instance.state.resolution;
    int refreshRate = SaveManager.Instance.state.refreshRate;
    this.UpdateResolution((int) resolution.x, (int) resolution.y, refreshRate);
    this.UpdateFullscreen(SaveManager.Instance.state.fullscreen);
    this.UpdateVSync(SaveManager.Instance.state.vSync);
    this.UpdateFullscreenMode(SaveManager.Instance.state.fullscreenMode);
    this.UpdateMaxFps(SaveManager.Instance.state.fpsLimit);
    this.UpdateVolume(SaveManager.Instance.state.volume);
    this.UpdateMusic(SaveManager.Instance.state.music);
  }

  public void UpdateCamShake(bool b)
  {
    SaveManager.Instance.state.cameraShake = b;
    SaveManager.Instance.Save();
    CurrentSettings.cameraShake = b;
  }

  public void UpdateSens(float i)
  {
    MonoBehaviour.print((object) ("updates sens to: " + (object) i));
    SaveManager.Instance.state.sensMultiplier = i;
    SaveManager.Instance.Save();
    this.sensMultiplier = i;
    PlayerInput.sensMultiplier = i;
  }

  public void UpdateFov(float i)
  {
    int num = (int) i;
    Debug.LogError((object) ("updates fov to: " + (object) num));
    SaveManager.Instance.state.fov = num;
    SaveManager.Instance.Save();
    Debug.LogError((object) ("Save fov as: " + (object) num));
    this.fov = num;
    if (!Object.op_Implicit((Object) MoveCamera.Instance))
      return;
    MoveCamera.Instance.UpdateFov((float) this.fov);
  }

  public void UpdateInverted(bool hor, bool ver)
  {
    Debug.Log((object) ("Setting inverted to: " + hor.ToString() + ", ver: " + ver.ToString()));
    SaveManager.Instance.state.invertedMouseHor = hor;
    SaveManager.Instance.state.invertedMouseVert = ver;
    SaveManager.Instance.Save();
    CurrentSettings.invertedHor = hor;
    CurrentSettings.invertedVer = ver;
  }

  public void UpdateGrass(bool b)
  {
    Debug.Log((object) ("Setting grass to: " + b.ToString()));
    SaveManager.Instance.state.grass = b;
    SaveManager.Instance.Save();
    CurrentSettings.grass = b;
  }

  public void UpdateTutorial(bool b)
  {
    Debug.Log((object) ("Setting tutorial to: " + b.ToString()));
    SaveManager.Instance.state.tutorial = b;
    SaveManager.Instance.Save();
    this.tutorial = b;
  }

  public void UpdateShadowQuality(int i)
  {
    SaveManager.Instance.state.shadowQuality = i;
    SaveManager.Instance.Save();
    QualitySettings.set_shadows((ShadowQuality) i);
    MonoBehaviour.print((object) "updating shadow quality");
  }

  public void UpdateShadowResolution(int i)
  {
    SaveManager.Instance.state.shadowResolution = i;
    SaveManager.Instance.Save();
    QualitySettings.set_shadowResolution((ShadowResolution) i);
    MonoBehaviour.print((object) "updating shadow res");
  }

  public void UpdateShadowCascades(int i)
  {
    SaveManager.Instance.state.shadowCascade = i;
    SaveManager.Instance.Save();
    QualitySettings.set_shadowCascades(2 * i);
    MonoBehaviour.print((object) "updating shadow cascades");
  }

  public void UpdateShadowDistance(int i)
  {
    SaveManager.Instance.state.shadowDistance = i;
    SaveManager.Instance.Save();
    QualitySettings.set_shadowDistance((float) (i * 40));
    MonoBehaviour.print((object) "updating shadow distance");
  }

  public void UpdateTextureQuality(int i)
  {
    SaveManager.Instance.state.textureQuality = i;
    SaveManager.Instance.Save();
    QualitySettings.set_masterTextureLimit(3 - i);
    MonoBehaviour.print((object) "updating texture quality");
  }

  public void UpdateAntiAliasing(int i)
  {
    SaveManager.Instance.state.antiAliasing = i;
    SaveManager.Instance.Save();
    int num = 0;
    switch (i)
    {
      case 0:
        num = 0;
        break;
      case 1:
        num = 2;
        break;
      case 2:
        num = 4;
        break;
      case 3:
        num = 8;
        break;
    }
    QualitySettings.set_antiAliasing(num);
    MonoBehaviour.print((object) "updating AA");
  }

  public void UpdateSoftParticles(bool b)
  {
    SaveManager.Instance.state.softParticles = b;
    SaveManager.Instance.Save();
    QualitySettings.set_softParticles(b);
    MonoBehaviour.print((object) "updating soft particles");
  }

  public void UpdateBloom(int i)
  {
    SaveManager.Instance.state.bloom = i;
    SaveManager.Instance.Save();
    PPController.Instance.SetBloom(i);
    MonoBehaviour.print((object) "updating bloom");
  }

  public void UpdateMotionBlur(bool b)
  {
    SaveManager.Instance.state.motionBlur = b;
    SaveManager.Instance.Save();
    PPController.Instance.SetMotionBlur(b);
    MonoBehaviour.print((object) "updating motion blur");
  }

  public void UpdateAO(bool b)
  {
    SaveManager.Instance.state.ambientOcclusion = b;
    SaveManager.Instance.Save();
    PPController.Instance.SetAO(b);
    MonoBehaviour.print((object) "updating AO");
  }

  public void UpdateResolution(int width, int height, int refreshRate)
  {
    if (Vector2.op_Equality(SaveManager.Instance.state.resolution, Vector2.get_zero()))
    {
      Resolution currentResolution = Screen.get_currentResolution();
      width = ((Resolution) ref currentResolution).get_width();
      height = ((Resolution) ref currentResolution).get_height();
      refreshRate = ((Resolution) ref currentResolution).get_refreshRate();
      MonoBehaviour.print((object) "finding custom res");
    }
    Screen.SetResolution(width, height, SaveManager.Instance.state.fullscreen, refreshRate);
    SaveManager.Instance.state.resolution = new Vector2((float) width, (float) height);
    SaveManager.Instance.Save();
    MonoBehaviour.print((object) "Updated screen resoltion");
  }

  public void UpdateFullscreen(bool i)
  {
    SaveManager.Instance.state.fullscreen = i;
    SaveManager.Instance.Save();
    Screen.set_fullScreen(i);
    MonoBehaviour.print((object) "updated fullscreen");
  }

  public void UpdateFullscreenMode(int i)
  {
    SaveManager.Instance.state.fullscreenMode = i;
    SaveManager.Instance.Save();
    Screen.set_fullScreenMode((FullScreenMode) i);
    MonoBehaviour.print((object) "updated fullscreenmode");
  }

  public void UpdateVSync(int i)
  {
    SaveManager.Instance.state.vSync = i;
    SaveManager.Instance.Save();
    QualitySettings.set_vSyncCount(i);
    MonoBehaviour.print((object) "updated vsync");
  }

  public void UpdateMaxFps(int i)
  {
    SaveManager.Instance.state.fpsLimit = i;
    SaveManager.Instance.Save();
    Application.set_targetFrameRate(i);
    MonoBehaviour.print((object) "updated fps limit");
  }

  public void UpdateVolume(int i)
  {
    SaveManager.Instance.state.volume = i;
    SaveManager.Instance.Save();
    AudioListener.set_volume((float) i / 10f);
    MonoBehaviour.print((object) "updated volume");
  }

  public void UpdateMusic(int i)
  {
    SaveManager.Instance.state.music = i;
    SaveManager.Instance.Save();
    MusicController.Instance.SetVolume((float) i / 10f);
    MonoBehaviour.print((object) "updated music");
  }

  public CurrentSettings() => base.\u002Ector();
}
