using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]

[VolumeComponentMenu("Custom/SphereVolumeComponent")]
public class SphereVolumeComponent : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter intensity = new ClampedFloatParameter(value: 0, min: 0, max: 1, overrideState: true);

    public bool IsActive() => intensity.value > 0;
}
