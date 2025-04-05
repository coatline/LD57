using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BOOM!", menuName = "Sound")]

public class SoundType : ScriptableObject
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] Vector2 pitchShiftRange;

    public Vector2 PitchShiftRange => pitchShiftRange;
    public AudioClip RandomSound => clips[Random.Range(0, clips.Length)];
}
