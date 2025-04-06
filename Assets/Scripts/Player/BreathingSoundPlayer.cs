using UnityEngine;

public class BreathingSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] SoundType normalBreathingSound;
    [SerializeField] float normalBreathInterval;
    [SerializeField] AudioClip runningBreathingAudio;
    [SerializeField] AudioClip recoveryBreathingAudio;

    IntervalTimer normalBreathTimer;
    bool recovering;

    private void Start()
    {
        playerMovement.SprintingChanged += PlayerMovement_SprintingChanged;
        playerMovement.StaminaFull += PlayerMovement_StaminaFull;
    }

    private void PlayerMovement_StaminaFull()
    {
        audioSource.Stop();
    }

    private void PlayerMovement_SprintingChanged(bool isSprinting)
    {
        if (isSprinting)
        {
            audioSource.loop = true;
            audioSource.clip = runningBreathingAudio;
            audioSource.Play();
        }
        else
        {
            audioSource.loop = false;
            audioSource.clip = recoveryBreathingAudio;
            audioSource.Play();
        }
    }
}
