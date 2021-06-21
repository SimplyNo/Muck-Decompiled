// Decompiled with JetBrains decompiler
// Type: Settings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
  public Button backBtn;
  [Header("Game")]
  public MyBoolSetting camShake;
  public SliderSetting fov;
  public SliderSetting sens;
  public MyBoolSetting invertedHor;
  public MyBoolSetting invertedVer;
  public MyBoolSetting grass;
  public MyBoolSetting tutorial;
  [Header("Controls")]
  public ControlSetting forward;
  public ControlSetting backward;
  public ControlSetting left;
  public ControlSetting right;
  public ControlSetting jump;
  public ControlSetting sprint;
  public ControlSetting interact;
  public ControlSetting inventory;
  public ControlSetting map;
  public ControlSetting leftClick;
  public ControlSetting rightClick;
  [Header("Graphics")]
  public ScrollSettings shadowQuality;
  public ScrollSettings shadowResolution;
  public ScrollSettings shadowDistance;
  public ScrollSettings shadowCascades;
  public ScrollSettings textureQuality;
  public ScrollSettings antiAliasing;
  public MyBoolSetting softParticles;
  public ScrollSettings bloom;
  public MyBoolSetting motionBlur;
  public MyBoolSetting ao;
  [Header("Video")]
  public ResolutionSetting resolution;
  public MyBoolSetting fullscreen;
  public ScrollSettings fullscreenMode;
  public ScrollSettings vSync;
  public SliderSetting fpsLimit;
  [Header("Audio")]
  public SliderSetting volume;
  [Header("Audio")]
  public SliderSetting music;

  private void Start() => this.UpdateSave();

  private void UpdateSave()
  {
    this.camShake.SetSetting(SaveManager.Instance.state.cameraShake);
    // ISSUE: method pointer
    this.camShake.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateCamShake)));
    this.fov.SetSettings(SaveManager.Instance.state.fov);
    // ISSUE: method pointer
    this.fov.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateFov)));
    this.sens.SetSettings(this.FloatToInt(SaveManager.Instance.state.sensMultiplier));
    // ISSUE: method pointer
    this.sens.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateSens)));
    this.invertedHor.SetSetting(SaveManager.Instance.state.invertedMouseHor);
    // ISSUE: method pointer
    this.invertedHor.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateInverted)));
    this.invertedVer.SetSetting(SaveManager.Instance.state.invertedMouseVert);
    // ISSUE: method pointer
    this.invertedVer.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateInverted)));
    this.grass.SetSetting(SaveManager.Instance.state.grass);
    // ISSUE: method pointer
    this.grass.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateGrass)));
    this.tutorial.SetSetting(SaveManager.Instance.state.tutorial);
    // ISSUE: method pointer
    this.tutorial.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateTutorial)));
    this.forward.SetSetting(SaveManager.Instance.state.forward, "Forward");
    // ISSUE: method pointer
    this.forward.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateForwardKey)));
    this.backward.SetSetting(SaveManager.Instance.state.backwards, "Backward");
    // ISSUE: method pointer
    this.backward.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateBackwardKey)));
    this.left.SetSetting(SaveManager.Instance.state.left, "Left");
    // ISSUE: method pointer
    this.left.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateLeftKey)));
    this.right.SetSetting(SaveManager.Instance.state.right, "Right");
    // ISSUE: method pointer
    this.right.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateRightKey)));
    this.jump.SetSetting(SaveManager.Instance.state.jump, "Jump");
    // ISSUE: method pointer
    this.jump.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateJumpKey)));
    this.sprint.SetSetting(SaveManager.Instance.state.sprint, "Sprint");
    // ISSUE: method pointer
    this.sprint.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateSprintKey)));
    this.interact.SetSetting(SaveManager.Instance.state.interact, "Interact");
    // ISSUE: method pointer
    this.interact.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateInteractKey)));
    this.inventory.SetSetting(SaveManager.Instance.state.inventory, "Inventory");
    // ISSUE: method pointer
    this.inventory.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateInventoryKey)));
    this.map.SetSetting(SaveManager.Instance.state.map, "Map");
    // ISSUE: method pointer
    this.map.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateMapKey)));
    this.leftClick.SetSetting(SaveManager.Instance.state.leftClick, "Left Click / Attack");
    // ISSUE: method pointer
    this.leftClick.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateLeftClickKey)));
    this.rightClick.SetSetting(SaveManager.Instance.state.rightClick, "Right Click / Build");
    // ISSUE: method pointer
    this.rightClick.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateRightClickKey)));
    this.shadowQuality.SetSettings(Enum.GetNames(typeof (Settings.ShadowQuality)), SaveManager.Instance.state.shadowQuality);
    // ISSUE: method pointer
    this.shadowQuality.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateShadowQuality)));
    this.shadowResolution.SetSettings(Enum.GetNames(typeof (Settings.ShadowResolution)), SaveManager.Instance.state.shadowResolution);
    // ISSUE: method pointer
    this.shadowResolution.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateShadowResolution)));
    this.shadowDistance.SetSettings(Enum.GetNames(typeof (Settings.ShadowDistance)), SaveManager.Instance.state.shadowDistance);
    // ISSUE: method pointer
    this.shadowDistance.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateShadowDistance)));
    this.shadowCascades.SetSettings(Enum.GetNames(typeof (Settings.ShadowCascades)), SaveManager.Instance.state.shadowCascade);
    // ISSUE: method pointer
    this.shadowCascades.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateShadowCascades)));
    this.textureQuality.SetSettings(Enum.GetNames(typeof (Settings.TextureResolution)), SaveManager.Instance.state.textureQuality);
    // ISSUE: method pointer
    this.textureQuality.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateTextureRes)));
    this.antiAliasing.SetSettings(Enum.GetNames(typeof (Settings.AntiAliasing)), SaveManager.Instance.state.antiAliasing);
    // ISSUE: method pointer
    this.antiAliasing.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateAntiAliasing)));
    this.softParticles.SetSetting(SaveManager.Instance.state.softParticles);
    // ISSUE: method pointer
    this.softParticles.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateSoftParticles)));
    this.bloom.SetSettings(Enum.GetNames(typeof (Settings.Bloom)), SaveManager.Instance.state.bloom);
    // ISSUE: method pointer
    this.bloom.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateBloom)));
    this.motionBlur.SetSetting(SaveManager.Instance.state.motionBlur);
    // ISSUE: method pointer
    this.motionBlur.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateMotionBlur)));
    this.ao.SetSetting(SaveManager.Instance.state.ambientOcclusion);
    // ISSUE: method pointer
    this.ao.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateAO)));
    this.resolution.SetSettings(Screen.get_resolutions(), Screen.get_currentResolution());
    this.fullscreen.SetSetting(Screen.get_fullScreen());
    // ISSUE: method pointer
    this.fullscreen.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateFullscreen)));
    this.vSync.SetSettings(Enum.GetNames(typeof (Settings.VSync)), SaveManager.Instance.state.vSync);
    // ISSUE: method pointer
    this.vSync.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateVSync)));
    this.fullscreenMode.SetSettings(Enum.GetNames(typeof (FullScreenMode)), SaveManager.Instance.state.fullscreenMode);
    // ISSUE: method pointer
    this.fullscreenMode.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateFullscreenMode)));
    this.fpsLimit.SetSettings(SaveManager.Instance.state.fpsLimit);
    // ISSUE: method pointer
    this.fpsLimit.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateMaxFps)));
    this.volume.SetSettings(SaveManager.Instance.state.volume);
    // ISSUE: method pointer
    this.volume.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateVolume)));
    this.music.SetSettings(SaveManager.Instance.state.music);
    // ISSUE: method pointer
    this.music.onClick.AddListener(new UnityAction((object) this, __methodptr(UpdateMusic)));
  }

  private void UpdateCamShake() => CurrentSettings.Instance.UpdateCamShake(this.IntToBool(this.camShake.currentSetting));

  private void UpdateInverted() => CurrentSettings.Instance.UpdateInverted(this.IntToBool(this.invertedHor.currentSetting), this.IntToBool(this.invertedVer.currentSetting));

  private void UpdateGrass() => CurrentSettings.Instance.UpdateGrass(this.IntToBool(this.grass.currentSetting));

  private void UpdateTutorial() => CurrentSettings.Instance.UpdateTutorial(this.IntToBool(this.grass.currentSetting));

  private void UpdateSens() => CurrentSettings.Instance.UpdateSens(this.IntToFloat(this.sens.currentSetting));

  private void UpdateFov() => CurrentSettings.Instance.UpdateFov((float) this.fov.currentSetting);

  private void UpdateForwardKey()
  {
    SaveManager.Instance.state.forward = this.forward.currentKey;
    SaveManager.Instance.Save();
    InputManager.forward = this.forward.currentKey;
  }

  private void UpdateBackwardKey()
  {
    SaveManager.Instance.state.backwards = this.backward.currentKey;
    SaveManager.Instance.Save();
    InputManager.backwards = this.backward.currentKey;
  }

  private void UpdateLeftKey()
  {
    SaveManager.Instance.state.left = this.left.currentKey;
    SaveManager.Instance.Save();
    InputManager.left = this.left.currentKey;
  }

  private void UpdateRightKey()
  {
    SaveManager.Instance.state.right = this.right.currentKey;
    SaveManager.Instance.Save();
    InputManager.right = this.right.currentKey;
  }

  private void UpdateJumpKey()
  {
    SaveManager.Instance.state.jump = this.jump.currentKey;
    SaveManager.Instance.Save();
    InputManager.jump = this.jump.currentKey;
  }

  private void UpdateSprintKey()
  {
    SaveManager.Instance.state.sprint = this.sprint.currentKey;
    SaveManager.Instance.Save();
    InputManager.sprint = this.sprint.currentKey;
  }

  private void UpdateInteractKey()
  {
    SaveManager.Instance.state.interact = this.interact.currentKey;
    SaveManager.Instance.Save();
    InputManager.interact = this.interact.currentKey;
  }

  private void UpdateInventoryKey()
  {
    SaveManager.Instance.state.inventory = this.inventory.currentKey;
    SaveManager.Instance.Save();
    InputManager.inventory = this.inventory.currentKey;
  }

  private void UpdateMapKey()
  {
    SaveManager.Instance.state.map = this.map.currentKey;
    SaveManager.Instance.Save();
    InputManager.map = this.map.currentKey;
  }

  private void UpdateLeftClickKey()
  {
    SaveManager.Instance.state.leftClick = this.leftClick.currentKey;
    SaveManager.Instance.Save();
    InputManager.leftClick = this.leftClick.currentKey;
  }

  private void UpdateRightClickKey()
  {
    SaveManager.Instance.state.rightClick = this.rightClick.currentKey;
    SaveManager.Instance.Save();
    InputManager.rightClick = this.rightClick.currentKey;
  }

  private void UpdateShadowQuality() => CurrentSettings.Instance.UpdateShadowQuality(this.shadowQuality.currentSetting);

  private void UpdateShadowResolution() => CurrentSettings.Instance.UpdateShadowResolution(this.shadowResolution.currentSetting);

  private void UpdateShadowDistance() => CurrentSettings.Instance.UpdateShadowDistance(this.shadowDistance.currentSetting);

  private void UpdateShadowCascades() => CurrentSettings.Instance.UpdateShadowCascades(this.shadowCascades.currentSetting);

  private void UpdateTextureRes() => CurrentSettings.Instance.UpdateTextureQuality(this.textureQuality.currentSetting);

  private void UpdateAntiAliasing() => CurrentSettings.Instance.UpdateAntiAliasing(this.antiAliasing.currentSetting);

  private void UpdateSoftParticles() => CurrentSettings.Instance.UpdateSoftParticles(this.IntToBool(this.softParticles.currentSetting));

  private void UpdateBloom() => CurrentSettings.Instance.UpdateBloom(this.bloom.currentSetting);

  private void UpdateMotionBlur() => CurrentSettings.Instance.UpdateMotionBlur(this.IntToBool(this.motionBlur.currentSetting));

  private void UpdateAO() => CurrentSettings.Instance.UpdateAO(this.IntToBool(this.ao.currentSetting));

  private void UpdateFullscreen() => CurrentSettings.Instance.UpdateFullscreen(this.IntToBool(this.fullscreen.currentSetting));

  private void UpdateFullscreenMode() => CurrentSettings.Instance.UpdateFullscreenMode(this.fullscreenMode.currentSetting);

  private void UpdateVSync() => CurrentSettings.Instance.UpdateVSync(this.vSync.currentSetting);

  private void UpdateMaxFps() => CurrentSettings.Instance.UpdateMaxFps(this.fpsLimit.currentSetting);

  private void UpdateVolume() => CurrentSettings.Instance.UpdateVolume(this.volume.currentSetting);

  private void UpdateMusic() => CurrentSettings.Instance.UpdateMusic(this.music.currentSetting);

  private float IntToFloat(int i) => (float) i / 100f;

  private int FloatToInt(float f) => (int) ((double) f * 100.0);

  private int BoolToInt(bool b) => b ? 1 : 0;

  private bool IntToBool(int i) => i != 0;

  public void ResetSaveFile()
  {
    SaveManager.Instance.NewSave();
    SaveManager.Instance.Save();
    this.UpdateSave();
    CurrentSettings.Instance.UpdateSave();
  }

  public Settings() => base.\u002Ector();

  public enum BoolSetting
  {
    Off,
    On,
  }

  public enum VSync
  {
    Off,
    Always,
    Half,
  }

  public enum ShadowQuality
  {
    Off,
    Hard,
    Soft,
  }

  public enum ShadowResolution
  {
    Low,
    Medium,
    High,
    Ultra,
  }

  public enum ShadowDistance
  {
    Low,
    Medium,
    High,
    Ultra,
  }

  public enum ShadowCascades
  {
    None,
    Two,
    Four,
  }

  public enum TextureResolution
  {
    Low,
    Medium,
    High,
    Ultra,
  }

  public enum AntiAliasing
  {
    Off,
    x2,
    x4,
    x8,
  }

  public enum Bloom
  {
    Off,
    Fast,
    Fancy,
  }
}
