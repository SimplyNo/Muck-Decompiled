// Decompiled with JetBrains decompiler
// Type: PPController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8DB17789-D6D5-48DE-86AB-E696A5FF6B2B
// Assembly location: D:\SteamLibrary\steamapps\common\Muck\Muck_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPController : MonoBehaviour
{
  private MotionBlur motionBlur;
  private UnityEngine.Rendering.PostProcessing.Bloom bloom;
  private AmbientOcclusion ao;
  private ChromaticAberration chromaticAberration;
  private PostProcessProfile profile;
  public static PPController Instance;

  private void Awake()
  {
    if ((bool) (Object) PPController.Instance)
    {
      Object.Destroy((Object) this.gameObject);
    }
    else
    {
      PPController.Instance = this;
      this.profile = this.GetComponent<PostProcessVolume>().profile;
      this.motionBlur = this.profile.GetSetting<MotionBlur>();
      this.bloom = this.profile.GetSetting<UnityEngine.Rendering.PostProcessing.Bloom>();
      this.ao = this.profile.GetSetting<AmbientOcclusion>();
      this.chromaticAberration = this.profile.GetSetting<ChromaticAberration>();
    }
  }

  public void SetMotionBlur(bool b) => this.motionBlur.enabled.value = b;

  public void SetBloom(int i)
  {
    switch (i)
    {
      case 0:
        this.bloom.enabled.value = false;
        break;
      case 1:
        this.bloom.enabled.value = true;
        this.bloom.fastMode.value = true;
        break;
      case 2:
        this.bloom.enabled.value = true;
        this.bloom.fastMode.value = false;
        break;
    }
  }

  public void SetAO(bool b)
  {
    this.ao.enabled.value = b;
    this.chromaticAberration.fastMode.value = !b;
  }

  public void SetChromaticAberration(float f)
  {
    if ((double) f <= 0.0)
    {
      this.chromaticAberration.enabled.value = false;
    }
    else
    {
      if (!(bool) (ParameterOverride<bool>) this.chromaticAberration.enabled)
        this.chromaticAberration.enabled.value = true;
      this.chromaticAberration.intensity.value = f;
    }
  }

  public void Reset() => this.SetChromaticAberration(0.0f);
}
