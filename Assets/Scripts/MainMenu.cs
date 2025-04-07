using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] SoundType playSound;
    [SerializeField] AudioSource music;

    bool musicFade = false;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Play()
    {
        SoundPlayer.I.PlaySound(playSound, transform.position, 1, 0);
        SceneFader.I.LoadNewScene("Intro", 2f);
        musicFade = true;
    }

    private void Update()
    {
        if (musicFade)
            music.volume -= Time.deltaTime;
    }

    public void LoadMenu()
    {
        SoundPlayer.I.PlaySound(playSound, transform.position, 1, 0);
        SceneFader.I.LoadNewScene("Menu");
    }
}
