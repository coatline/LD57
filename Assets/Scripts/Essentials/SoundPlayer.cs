using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : Singleton<SoundPlayer>
{
    [SerializeField] AudioSource audioSourcePrefab;

    List<AudioSource> usableSoundSources;
    GameObject parent;

    protected override void Awake()
    {
        base.Awake();
        usableSoundSources = new List<AudioSource>();
    }

    public void PlaySound(SoundType sound, Vector3 soundPosition, float volume = 1, float spatialBlend = 1)
    {
        if (parent == null)
            parent = new GameObject("Sounds");

        PlayClip(sound.RandomSound, TryGetAudioSource(), soundPosition, volume, spatialBlend, 1 + Random.Range(sound.PitchShiftRange.x, sound.PitchShiftRange.y));
    }

    public void PlaySound(string sound, Vector3 soundPosition, float volume = 1, float spatialBlend = 1) { PlaySound(DataLibrary.I.Sounds[sound], soundPosition, volume, spatialBlend); }

    public void PlayAudioClip(AudioClip clip, Vector3 soundPosition, float volume = 1, float spatialBlend = 1, float pitch = 1) => PlayClip(clip, TryGetAudioSource(), soundPosition, volume, spatialBlend, pitch);

    void PlayClip(AudioClip clip, AudioSource audioSource, Vector3 soundPosition, float volume, float spatialBlend, float pitch)
    {
        audioSource.transform.position = soundPosition;
        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);

        StartCoroutine(DelayUseAgain(audioSource, clip.length));
    }

    AudioSource TryGetAudioSource()
    {
        if (usableSoundSources.Count == 0)
            return NewAudioSource();
        else
        {
            // Removing from the end of the list is faster
            int index = usableSoundSources.Count - 1;
            AudioSource source = usableSoundSources[index];
            usableSoundSources.RemoveAt(index);
            return source;
        }
    }

    AudioSource NewAudioSource()
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform);
        //audioSource.maxDistance = 100f;
        //audioSource.spatialBlend = 1f;
        //audioSource.rolloffMode = AudioRolloffMode.Linear;
        //audioSource.dopplerLevel = 0.0f;

        return audioSource;
    }

    IEnumerator DelayUseAgain(AudioSource source, float soundLength)
    {
        yield return new WaitForSeconds(soundLength);

        if (source.gameObject != null)
            usableSoundSources.Add(source);
    }
}