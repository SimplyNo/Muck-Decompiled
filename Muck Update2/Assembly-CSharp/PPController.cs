// Decompiled with JetBrains decompiler
// Type: PPController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 68ECCA8E-CF88-4CE2-9D74-1A5BFC0637BB
// Assembly location: D:\Repo\Muck Update2\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPController : MonoBehaviour
{
  private MotionBlur motionBlur;
  private Bloom bloom;
  private AmbientOcclusion ao;
  private ChromaticAberration chromaticAberration;
  private PostProcessProfile profile;
  public static PPController Instance;

  private void Awake()
  {
    if (Object.op_Implicit((Object) PPController.Instance))
    {
      Object.Destroy((Object) ((Component) this).get_gameObject());
    }
    else
    {
      PPController.Instance = this;
      this.profile = ((PostProcessVolume) ((Component) this).GetComponent<PostProcessVolume>()).get_profile();
      this.motionBlur = (MotionBlur) this.profile.GetSetting<MotionBlur>();
      this.bloom = (Bloom) this.profile.GetSetting<Bloom>();
      this.ao = (AmbientOcclusion) this.profile.GetSetting<AmbientOcclusion>();
      this.chromaticAberration = (ChromaticAberration) this.profile.GetSetting<ChromaticAberration>();
    }
  }

  public void SetMotionBlur(bool b) => ((ParameterOverride<bool>) ((PostProcessEffectSettings) this.motionBlur).enabled).value = (__Null) (b ? 1 : 0);

  public void SetBloom(int i)
  {
    switch (i)
    {
      case 0:
        ((ParameterOverride<bool>) ((PostProcessEffectSettings) this.bloom).enabled).value = (__Null) 0;
        break;
      case 1:
        ((ParameterOverride<bool>) ((PostProcessEffectSettings) this.bloom).enabled).value = (__Null) 1;
        ((ParameterOverride<bool>) this.bloom.fastMode).value = (__Null) 1;
        break;
      case 2:
        ((ParameterOverride<bool>) ((PostProcessEffectSettings) this.bloom).enabled).value = (__Null) 1;
        ((ParameterOverride<bool>) this.bloom.fastMode).value = (__Null) 0;
        break;
    }
  }

  public void SetAO(bool b)
  {
    ((ParameterOverride<bool>) ((PostProcessEffectSettings) this.ao).enabled).value = (__Null) (b ? 1 : 0);
    ((ParameterOverride<bool>) this.chromaticAberration.fastMode).value = (__Null) (!b ? 1 : 0);
  }

  public void SetChromaticAberration(float f)
  {
    if ((double) f <= 0.0)
    {
      ((ParameterOverride<bool>) ((PostProcessEffectSettings) this.chromaticAberration).enabled).value = (__Null) 0;
    }
    else
    {
      if (!ParameterOverride<bool>.op_Implicit((ParameterOverride<bool>) ((PostProcessEffectSettings) this.chromaticAberration).enabled))
        ((ParameterOverride<bool>) ((PostProcessEffectSettings) this.chromaticAberration).enabled).value = (__Null) 1;
      ((ParameterOverride<float>) this.chromaticAberration.intensity).value = (__Null) (double) f;
    }
  }

  public void Reset() => this.SetChromaticAberration(0.0f);

  public PPController() => base.\u002Ector();
}
